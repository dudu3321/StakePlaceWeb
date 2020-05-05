using System;
using System.Collections.Generic;
using System.IO;
using CMDKClass;
using CommunalClass;
using DataStruct.AdminOperator;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using stake_place_web.Entities;
using stake_place_web.Entities.Login;
using stake_place_web.Enums;
using stake_place_web.Hubs;
using StakePlaceEntities;
using StakePlaceEntities.Dao.MongoDb;

namespace stake_place_web.Service
{
    public interface ILoginService
    {
        Dictionary<string, string> userConnectionId
        {
            get;
            set;
        }
        void DoLogin (string moLogin, string password, string conneciontId);
    }

    public class LoginService : ILoginService
    {
        private readonly IConfiguration _config;
        private readonly TCPClient2 _tcpClient;
        private readonly MiniUserV2Dao _miniUserV2Dao;
        private readonly MiniUserMatchIdsV2Dao _miniUserMatchIdsV2Dao;
        private readonly IHubContext<UserConnectionHub> _hubContext;
        public Dictionary<string, string> userConnectionId { get; set; }
        private readonly DefaultContractResolver contractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy () };
        public LoginService (IConfiguration config, IHubContext<UserConnectionHub> hubContext)
        {
            _config = config;
            _hubContext = hubContext;

            var moIp = _config["MOServerIp"];
            var moPort = _config["MOServerLoginPort"];

            _tcpClient = new TCPClient2 (moIp, int.Parse (moPort));
            _tcpClient.OnReceiveEvent += TcpClientOnReceiveEvent;
            _tcpClient.Connecting ();

            var connectionString = _config["MongoTicketsStatusConnectionString"];
            var mongoDbId = $"{Environment.UserName}@{Environment.MachineName}@{Helpers.GetLocalIPAddress()}";
            var APPLICATION_NAME = _config["ApplicationName"];

            _miniUserV2Dao = MiniUserV2Dao.CreateInstance (connectionString,
                $"{mongoDbId}@MiniUserV2Dao@MOClient@{APPLICATION_NAME}");
            _miniUserMatchIdsV2Dao = MiniUserMatchIdsV2Dao.CreateInstance (connectionString,
                $"{mongoDbId}@MiniUserMatchIdsV2Dao@MOClient@{APPLICATION_NAME}");

            userConnectionId = new Dictionary<string, string> ();
        }

        public void DoLogin (string moLogin, string password, string conneciontId)
        {
            var encryptedPassword = stringCoding.GetMD5 (password);
            var machineName = Environment.MachineName;
            var userName = Environment.UserName;
            var loginType = (int) MoLoginType.Stakeplace;
            var @params = $"{moLogin}#{password}#{encryptedPassword}#{machineName}#{userName}#{loginType}";

            userConnectionId.TryAdd (moLogin, conneciontId);

            using (var stream = StreamConvert.StringToStream (@params, false))
            {
                _tcpClient.SendData (2, stream);
            }
        }

        private async void ReceivedInvoke (MoLoginResponse moLoginResponse, MoLoginStatus status, string title, string message)
        {
            var conneciontId = userConnectionId[moLoginResponse.MoLogin];
            var method = string.Empty;
            moLoginResponse.MoLoginStatus = status;
            moLoginResponse.Title = title;
            moLoginResponse.Message = message;
            switch (status)
            {
                case MoLoginStatus.KickOut:
                    method = "userLogout";
                    break;
                default:
                    method = "userLogin";
                    break;
            }

            await _hubContext
                .Clients
                .Client (conneciontId)
                .SendAsync (
                    method,
                    JsonConvert.SerializeObject (moLoginResponse, new JsonSerializerSettings { ContractResolver = contractResolver })
                );
        }
        #region Private Methods

        private void GetMeta (string moLogin, MoLoginResponse moLoginResponse)
        {
            MiniUserV2 miniUser;
            if (_miniUserV2Dao.TryQuery (moLogin, out miniUser))
            {
                if (miniUser.LockStatus == 0)
                {
                    if (miniUser.Md5Password == moLoginResponse.EncryptedPassword)
                    {
                        if (miniUser.IsStakeplaceLogin)
                        {
                            if (moLogin.StartsWith ("pascal", StringComparison.OrdinalIgnoreCase))
                            {
                                moLoginResponse.View = Views.AllMatches;
                            }
                            else
                            {
                                moLoginResponse.View = miniUser.CanViewOnlyMOMatches ? Views.OnlyMOMatches : Views.AllMatches;
                            }
                            MiniUserMatchIdsV2 miniUserMatchIds;
                            if (_miniUserMatchIdsV2Dao.TryQueryOne (moLogin, out miniUserMatchIds))
                            {
                                moLoginResponse.MatchCodes.Clear ();
                                moLoginResponse.MatchCodes.AddRange (miniUserMatchIds.MatchIds);
                            }
                            ReceivedInvoke (moLoginResponse, MoLoginStatus.Success, "", "");
                        }
                        else
                        {
                            ReceivedInvoke (moLoginResponse, MoLoginStatus.Error, "StakePlaceClient Credentials errors",
                                "You are not allowed to use StakePlace.");
                        }
                    }
                    else
                    {
                        ReceivedInvoke (moLoginResponse, MoLoginStatus.Error, "StakePlaceClient Credentials errors",
                            "Your password is incorrect.");
                    }
                }
                else
                {
                    ReceivedInvoke (moLoginResponse, MoLoginStatus.Error, "StakePlaceClient Credentials errors",
                        "Your account has been closed/locked.");
                }
            }
            else
            {
                ReceivedInvoke (moLoginResponse, MoLoginStatus.Error, "StakePlaceClient Credentials errors",
                    "Your login is unknown.");
            }
        }

        #region MOClient TCPClient Events Methods

        private void TcpClientOnReceiveEvent (object sender, int tid, Stream data)
        {
            try
            {
                var quitMesage = string.Empty;
                var response = StreamConvert.StreamToString (data, false, true);
                string[] loginResult = response.Split ('#');
                var moLogin = loginResult[0];
                var moLoginResponse = new MoLoginResponse ();
                moLoginResponse.MoLogin = moLogin;
                var command = (MoLoginStatus) tid;
                switch (command)
                {
                    case MoLoginStatus.Success:
                        {
                            if (loginResult.Length < 4)
                            {
                                ReceivedInvoke (moLoginResponse, MoLoginStatus.Error, "MOService Error Message",
                                    "An error happened during login. Error message below " +
                                    $"{Environment.NewLine} msg={response}");
                                break;
                            }

                            var userLevels = loginResult[1].Split (new []
                            {
                                ','
                            }, StringSplitOptions.RemoveEmptyEntries);

                            if (userLevels.Length > 0)
                            {
                                moLoginResponse.UserLevels.Clear ();
                                moLoginResponse.UserLevels.AddRange (userLevels);
                                GetMeta (moLogin, moLoginResponse);
                                ReceivedInvoke (moLoginResponse, command, "", "");
                                break;
                            }

                            var errorMessage = loginResult[2];
                            ReceivedInvoke (moLoginResponse, command, "MOService Error Message",
                                "An error happened during login. Error message below " +
                                $"{Environment.NewLine} msg={errorMessage}");
                            break;
                        }
                    case MoLoginStatus.WrongPassword:
                        ReceivedInvoke (moLoginResponse, command, "MOService Credentials errors",
                            "Your password is incorrect.");
                        break;
                    case MoLoginStatus.AccountInactive:
                        ReceivedInvoke (moLoginResponse, command, "MOService Credentials errors",
                            "Your account has been closed/locked.");
                        break;
                    case MoLoginStatus.LoginAreaLimit:
                        ReceivedInvoke (moLoginResponse, command, "MOService Credentials errors",
                            $"You are not authorized to login. tid={tid}");
                        break;
                    case MoLoginStatus.UserNotExists:
                        ReceivedInvoke (moLoginResponse, command, "MOService Credentials errors",
                            "Your login is unknown.");
                        break;
                    case MoLoginStatus.InvalidArguments:
                        ReceivedInvoke (moLoginResponse, command, "MOService Credentials errors",
                            "The login request to MOService was not well formatted. " +
                            "Please contact IT NOC");
                        break;
                    case MoLoginStatus.KickOut:
                        ReceivedInvoke (moLoginResponse, command, "", "You have been logged out by the system due to multiple login. " +
                            "This session will be terminated!");
                        break;
                    case MoLoginStatus.NoResponse:
                        ReceivedInvoke (moLoginResponse, command, "MOService Notification", "The server is not responding, please try again later!");
                        break;
                    default:
                        ReceivedInvoke (moLoginResponse, command, "MOService Credentials errors",
                            "Unmanaged/Undefined login response. Please contact IT NOC. " +
                            $"tid={command}");
                        break;
                }
            }
            catch (Exception ex) { }
        }

        #endregion

        #endregion
    }
}