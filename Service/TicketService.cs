using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using stake_place_web.Entities;
using stake_place_web.Entities.Ticket;
using StakePlaceEntities;
using StakePlaceEntities.Dao.MongoDb;

namespace stake_place_web.Service
{
    public interface ITicketService
    {
        TicketsQueryParameters GetTicketParameters (TicketRequest request);
        List<StakePlaceTicket> GetTickets (TicketsQueryParameters _ticketsQueryParameters, List<string> userLevels);
    }

    public class TicketService : ITicketService
    {
        MiniTicketV2Dao _miniTicketV2Dao;
        IConfiguration _config;
        public TicketService (IConfiguration config)
        {
            _config = config;

            var APPLICATION_NAME = _config["ApplicationName"];
            var connectionString = _config["MongoTicketsStatusConnectionString"];
            var mongoDbId = $"{Environment.UserName}@{Environment.MachineName}@{Helpers.GetLocalIPAddress()}";

            _miniTicketV2Dao = MiniTicketV2Dao.CreateInstance(connectionString,
                $"{mongoDbId}@MiniTicketV2Dao@TicketsBll@{APPLICATION_NAME}");
        }

        public TicketsQueryParameters GetTicketParameters (TicketRequest request)
        {

            var result = new TicketsQueryParameters ();

            #region LastQueryTime

            try
            {
                result.LastQueryTime = request.LastQueryTime;
            }
            catch (Exception) { }

            #endregion

            #region View

            result.View = Views.OnlyMOMatches;
            try
            {
                result.View = request.View;
            }
            catch (Exception)
            {
                // XtraMessageBox.Show("Could not identify View value. Default value will be used (MO Matches only) " +
                //     "Please contact IT NOC.", "StakePlace Client Error Message",
                //     MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }

            #endregion

            #region Market

            result.Market = Markets.All;
            try
            {
                result.Market = request.Market;
            }
            catch (Exception)
            {
                // XtraMessageBox.Show("Could not identify Market value. Default value will be used (All) " +
                //     "Please contact IT NOC.", "StakePlace Client Error Message",
                //     MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }

            #endregion

            #region Record

            result.Record = Records.FiftyLines;
            try
            {
                result.Record = request.Record;
            }
            catch (Exception)
            {
                // XtraMessageBox.Show("Could not identify Records value. Default value will be used (50) " +
                //     "Please contact IT NOC.", "StakePlace Client Error Message",
                //     MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }
            switch (result.Record)
            {
                case Records.FiftyLines:
                    result.MaxRecords = 50;
                    break;
                case Records.OneHundredLines:
                    result.MaxRecords = 100;
                    break;
                case Records.TwoHundredLines:
                    result.MaxRecords = 200;
                    break;
                case Records.FiveHundredLines:
                    result.MaxRecords = 500;
                    break;
                case Records.OneThousandLines:
                    result.MaxRecords = 1000;
                    break;
                case Records.TwoThousandLines:
                    result.MaxRecords = 2000;
                    break;
            }

            #endregion

            #region Amount

            result.Amount = 0m;
            if (!string.IsNullOrWhiteSpace (request.Amount))
            {
                try
                {
                    result.Amount = decimal.Parse (request.Amount);
                }
                catch (Exception)
                {
                    // XtraMessageBox.Show("Could not identify Amount value. Default value will be used (0) " +
                    //     "Please contact IT NOC.", "StakePlace Client Error Message",
                    //     MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                }
            }

            #endregion

            #region Account / IP 

            result.Account = request.Account;
            result.Ip = request.Ip;

            #endregion

            #region Sport

            result.Sport = Sports.All;
            try
            {
                result.Sport = request.Sport;
            }
            catch (Exception)
            {
                // XtraMessageBox.Show("Could not identify Sport value. Default value will be used (All) " +
                //     "Please contact IT NOC.", "StakePlace Client Error Message",
                //     MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }

            #endregion

            #region Transaction Type

            result.Transaction = Transactions.All;
            try
            {
                result.Transaction = request.Transaction;
            }
            catch (Exception)
            {
                // XtraMessageBox.Show("Could not identify Transaction Type value. Default value will be used (All) " +
                //     "Please contact IT NOC.", "StakePlace Client Error Message",
                //     MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }

            #endregion

            #region VIP Type

            result.VipType = 0;
            try
            {
                result.VipType = request.Vip;
            }
            catch (Exception)
            {
                // XtraMessageBox.Show("Could not identify VIP Type value. Default value will be used (All) " +
                //     "Please contact IT NOC.", "StakePlace Client Error Message",
                //     MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }

            #endregion

            #region Special

            result.Special = 0;
            try
            {
                result.Special = request.Special;
            }
            catch (Exception)
            {
                // XtraMessageBox.Show("Could not identify Special value. Default value will be used (All) " +
                //     "Please contact IT NOC.", "StakePlace Client Error Message",
                //     MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }

            #endregion

            #region Ticket Type

            result.Ticket = Tickets.All;
            try
            {
                result.Ticket = request.Ticket;
            }
            catch (Exception)
            {
                // XtraMessageBox.Show("Could not identify Ticket value. Default value will be used (All) " +
                //     "Please contact IT NOC.", "StakePlace Client Error Message",
                //     MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }

            #endregion

            #region Status

            result.Status = Status.All;
            try
            {
                result.Status = request.Status;
            }
            catch (Exception)
            {
                // XtraMessageBox.Show("Could not identify Status value. Default value will be used (All) " +
                //     "Please contact IT NOC.", "StakePlace Client Error Message",
                //     MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }

            #endregion

            return result;
        }

        public List<StakePlaceTicket> GetTickets (TicketsQueryParameters _ticketsQueryParameters, List<string> userLevels)
        {
            var stakePlaceTickets = new List<StakePlaceTicket> ();
            var fieldsAcl = new FieldsAcl (userLevels);
            var miniTickets = QueryTickets (_ticketsQueryParameters);
            foreach (var miniTicket in miniTickets)
            {
                var stakePlaceTicket = new StakePlaceTicket (miniTicket, fieldsAcl);
                stakePlaceTickets.Add (stakePlaceTicket);
            }
            return stakePlaceTickets;
        }

        private IEnumerable<MiniTicketV2> QueryTickets (TicketsQueryParameters ticketsQueryParameters)
        {
            var filters = ticketsQueryParameters.GetMongoDbFilters ();
            var filter = filters.Count > 0 ?
                Builders<MiniTicketV2>.Filter.And (filters) :
                Builders<MiniTicketV2>.Filter.Empty;
            var sort = Builders<MiniTicketV2>.Sort.Combine (new List<SortDefinition<MiniTicketV2>> ()
            {
                Builders<MiniTicketV2>.Sort.Descending (v => v.TransDate),
                    Builders<MiniTicketV2>.Sort.Descending (v => v.Id),
            });
            var projection = Builders<MiniTicketV2>.Projection
                // .Exclude(v => v.Admin)
                .Exclude (v => v.Member)
                .Exclude (v => v.LeagueId)
                .Exclude (v => v.HomeId)
                .Exclude (v => v.AwayId)
                .Exclude (v => v.MatchGroupId);
            using (var cursor = _miniTicketV2Dao.Collection
                .Find (filter, new FindOptions { BatchSize = 50 })
                .Project<MiniTicketV2> (projection)
                .Sort (sort)
                .Limit (ticketsQueryParameters.MaxRecords)
                .ToCursor (new CancellationTokenSource().Token))
            {
                while (cursor.MoveNext ())
                {
                    var values = cursor.Current;
                    foreach (var value in values)
                    {
                        yield return value;
                    }
                }
            }
        }
    }
}