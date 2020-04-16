using System;
using System.Collections.Generic;
using System.IO;
using CMDKClass;
using CommunalClass;
using DataStruct.AdminOperator;
using Microsoft.Extensions.Configuration;
using stake_place_web.Entities;
using stake_place_web.Entities.Login;
using stake_place_web.Enums;
using StakePlaceEntities;
using StakePlaceEntities.Dao.MongoDb;

namespace stake_place_web.Service
{
    public interface ILoginService
    {
        Dictionary<string, MoLoginResponse> pendingMoLogin
        {
            get;
            set;
        }
        void GetMeta (string moLogin, MoLoginResponse response);
        void Login (string moLogin, string password);
        void ReceivedInvoke (MoLoginStatus status, string moLogin, string title, string message);
        void Start ();
    }

    public class LoginService : ILoginService
    {
        private IConfiguration _config;
        private readonly TCPClient2 _tcpClient;
        private MiniUserV2Dao _miniUserV2Dao;
        private MiniUserMatchIdsV2Dao _miniUserMatchIdsV2Dao;
        public Dictionary<string, MoLoginResponse> pendingMoLogin
        {
            get;
            set;
        }
        public LoginService (IConfiguration config)
        {
            _config = config;

            var moIp = _config["MOServerIp"];
            var moPort = _config["MOServerLoginPort"];

            _tcpClient = new TCPClient2 (moIp, int.Parse (moPort));
            _tcpClient.OnReceiveEvent += TcpClientOnReceiveEvent;
            Start ();
        }

        public void Start ()
        {
            _tcpClient.Connecting ();

            var connectionString = _config["MongoTicketsStatusConnectionString"];
            var mongoDbId = $"{Environment.UserName}@{Environment.MachineName}@{Helpers.GetLocalIPAddress()}";
            var APPLICATION_NAME = _config["ApplicationName"];
            
            _miniUserV2Dao = MiniUserV2Dao.CreateInstance (connectionString,
                $"{mongoDbId}@MiniUserV2Dao@MOClient@{APPLICATION_NAME}");
            _miniUserMatchIdsV2Dao = MiniUserMatchIdsV2Dao.CreateInstance (connectionString,
                $"{mongoDbId}@MiniUserMatchIdsV2Dao@MOClient@{APPLICATION_NAME}");

            pendingMoLogin = new Dictionary<string, MoLoginResponse> ();
        }

        public void Login (string moLogin, string password)
        {
            var encryptedPassword = stringCoding.GetMD5 (password);
            var machineName = Environment.MachineName;
            var userName = Environment.UserName;
            var loginType = (int) MoLoginType.Stakeplace;
            var @params = $"{moLogin}#{password}#{encryptedPassword}#{machineName}#{userName}#{loginType}";

            using (var stream = StreamConvert.StringToStream (@params, false))
            {
                _tcpClient.SendData (2, stream);
            }
            pendingMoLogin.Add (moLogin, new MoLoginResponse ()
            {
                MoLogin = moLogin,
                    EncryptedPassword = encryptedPassword,
                    UpdateFinished = false,
                    CreateTime = DateTime.Now
            });
        }

        public void ReceivedInvoke (MoLoginStatus status, string moLogin, string title, string message)
        {
            pendingMoLogin[moLogin].MoLoginStatus = status;
            pendingMoLogin[moLogin].Title = title;
            pendingMoLogin[moLogin].Message = message;
            pendingMoLogin[moLogin].UpdateFinished = true;
        }

        #region Private Methods

        public void GetMeta (string moLogin, MoLoginResponse response)
        {
            MiniUserV2 miniUser;
            var moLoginResponse = pendingMoLogin[moLogin];
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
                                response.View = Views.AllMatches;
                            }
                            else
                            {
                                response.View = miniUser.CanViewOnlyMOMatches ? Views.OnlyMOMatches : Views.AllMatches;
                            }
                            MiniUserMatchIdsV2 miniUserMatchIds;
                            if (_miniUserMatchIdsV2Dao.TryQueryOne (moLogin, out miniUserMatchIds))
                            {
                                moLoginResponse.MatchCodes.Clear ();
                                moLoginResponse.MatchCodes.AddRange (miniUserMatchIds.MatchIds);
                            }
                            ReceivedInvoke (MoLoginStatus.Success, moLogin, "", "");
                        }
                        else
                        {
                            ReceivedInvoke (MoLoginStatus.Error, moLogin, "StakePlaceClient Credentials errors",
                                "You are not allowed to use StakePlace.");
                        }
                    }
                    else
                    {
                        ReceivedInvoke (MoLoginStatus.Error, moLogin, "StakePlaceClient Credentials errors",
                            "Your password is incorrect.");
                    }
                }
                else
                {
                    ReceivedInvoke (MoLoginStatus.Error, moLogin, "StakePlaceClient Credentials errors",
                        "Your account has been closed/locked.");
                }
            }
            else
            {
                ReceivedInvoke (MoLoginStatus.Error, moLogin, "StakePlaceClient Credentials errors",
                    "Your login is unknown.");
            }
        }

        #region MOClient TCPClient Events Methods

        private void TcpClientOnReceiveEvent (object sender, int tid, Stream data)
        {
            var quitMesage = string.Empty;
            var response = StreamConvert.StreamToString (data, false, true);
            string[] loginResult = response.Split ('#');
            var moLogin = loginResult[0];
            var moLoginResponse = pendingMoLogin[moLogin];
            var command = (MoLoginStatus) tid;
            switch (command)
            {
                case MoLoginStatus.Success:
                    {
                        if (loginResult.Length < 4)
                        {
                            ReceivedInvoke (MoLoginStatus.Error, moLogin, "MOService Error Message",
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
                            ReceivedInvoke (command, moLogin, "", "");
                            moLoginResponse.UserLevels.Clear ();
                            moLoginResponse.UserLevels.AddRange (userLevels);
                            GetMeta (moLogin, moLoginResponse);
                            break;
                        }

                        var errorMessage = loginResult[2];
                        ReceivedInvoke (command, moLogin, "MOService Error Message",
                            "An error happened during login. Error message below " +
                            $"{Environment.NewLine} msg={errorMessage}");
                        break;
                    }
                case MoLoginStatus.WrongPassword:
                    ReceivedInvoke (command, moLogin, "MOService Credentials errors",
                        "Your password is incorrect.");
                    break;
                case MoLoginStatus.AccountInactive:
                    ReceivedInvoke (command, moLogin, "MOService Credentials errors",
                        "Your account has been closed/locked.");
                    break;
                case MoLoginStatus.LoginAreaLimit:
                    ReceivedInvoke (command, moLogin, "MOService Credentials errors",
                        $"You are not authorized to login. tid={tid}");
                    break;
                case MoLoginStatus.UserNotExists:
                    ReceivedInvoke (command, moLogin, "MOService Credentials errors",
                        "Your login is unknown.");
                    break;
                case MoLoginStatus.InvalidArguments:
                    ReceivedInvoke (command, moLogin, "MOService Credentials errors",
                        "The login request to MOService was not well formatted. " +
                        "Please contact IT NOC");
                    break;
                case MoLoginStatus.KickOut:
                    ReceivedInvoke (command, moLogin, "", "You have been logged out by the system due to multiple login. " +
                        "This session will be terminated!");
                    break;
                case MoLoginStatus.NoResponse:
                    ReceivedInvoke (command, moLogin, "MOService Notification", "The server is not responding, please try again later!");
                    break;
                default:
                    ReceivedInvoke (command, moLogin, "MOService Credentials errors",
                        "Unmanaged/Undefined login response. Please contact IT NOC. " +
                        $"tid={command}");
                    break;
            }
        }

        #endregion

        #endregion
    }
}