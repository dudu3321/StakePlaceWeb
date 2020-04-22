using System;
using System.Collections.Generic;
using DataStruct.MatchEntities.Entity;
using MongoDB.Bson;
using MongoDB.Driver;
using StakePlaceEntities;

namespace stake_place_web.Entities.Ticket
{
    public class TicketsQueryParameters
    {
        public static readonly List<string> RejectedStatusList = new List<string> { "R", "J", "Y", "G", "E" };
        public static readonly List<string> PendingAndAcceptedStatusList = new List<string> { "D", "X", "N", "A", "S" };
        public static readonly List<string> PendingStatusList = new List<string> { "D", "X" };
        public static readonly List<string> AcceptedStatusList = new List<string> { "N", "A", "S" };

        public DateTime? LastQueryTime { get; set; }
        public Views View { get; set; }
        public Markets Market { get; set; }
        public Records Record { get; set; }
        public int MaxRecords { get; set; }
        public decimal Amount { get; set; }
        public bool CheckAmount => Amount > 0;
        public string Account { get; set; }
        public bool CheckAccount => !string.IsNullOrWhiteSpace (Account);
        public string Ip { get; set; }
        public bool CheckIp => !string.IsNullOrWhiteSpace (Ip);
        public Sports Sport { get; set; }
        public Transactions Transaction { get; set; }
        public int VipType { get; set; }
        public int Special { get; set; }
        public Tickets Ticket { get; set; }
        public Status Status { get; set; }
        public List<int?> MatchCodes { get; set; }

        public int LastSocTransIdFound { get; set; } = 0;
        public int LastSocTransTradeInIdFound { get; set; } = 0;

        private readonly List<FilterDefinition<MiniTicketV2>> _filters;

        private static readonly List<int> _normalSpecialIds = new List<int>
        {
            1000,
            1002,
            1003,
            1004
        };

        public int WorkingDate
        {
            get
            {
                var now = DateTime.Now;
                var realWorkingDate = now;
                if (now.Hour < 12) realWorkingDate = now.AddDays (-1);
                var workingDate = int.Parse (realWorkingDate.ToString ("yyyyMMdd"));
                return workingDate;
            }
        }

        public TicketsQueryParameters ()
        {
            View = Views.AllMatches;
            Market = Markets.All;
            Record = Records.FiftyLines;
            MaxRecords = 50;
            Amount = 0;
            Account = string.Empty;
            Ip = string.Empty;
            Sport = Sports.All;
            Transaction = Transactions.All;
            VipType = 0;
            Special = 0;
            Ticket = Tickets.All;
            Status = Status.All;
            MatchCodes = new List<int?> ();

            _filters = new List<FilterDefinition<MiniTicketV2>> ();
        }

        public List<FilterDefinition<MiniTicketV2>> GetMongoDbFilters ()
        {
            _filters.Clear ();

            // SetIdsConditions();
            SetUpdateTimeConditions ();
            SetViewConditions ();
            SetMarketConditions ();
            SetAmountConditions ();
            SetAccountConditions ();
            SetIpConditions ();
            SetSportConditions ();
            SetTransactionConditions ();
            SetVipTypeConditions ();
            SetSpecialConditions ();
            SetStatusConditions ();
            SetTicketConditions ();

            return _filters;
        }

        private void SetUpdateTimeConditions ()
        {
            if (LastQueryTime != null)
            {
                _filters.Add (Builders<MiniTicketV2>
                    .Filter
                    .Gt ("Date", new [] { LastQueryTime })
                );
            }
        }

        private void SetIdsConditions ()
        {
            _filters.Add (Builders<MiniTicketV2>
                .Filter
                .Where (v =>
                    v.SocTransId > LastSocTransIdFound && v.IsStock ||
                    v.SocTransTradeInId > LastSocTransTradeInIdFound && v.IsStock == false
                ));
        }

        private void SetTicketConditions ()
        {
            switch (Ticket)
            {
                case Tickets.Normal:
                    _filters.Add (Builders<MiniTicketV2>.Filter.Eq (v => v.IsStock, true));
                    break;
                case Tickets.TradeIn:
                    _filters.Add (Builders<MiniTicketV2>.Filter.Eq (v => v.IsStock, false));
                    break;
            }
        }

        private void SetStatusConditions ()
        {
            switch (Status)
            {
                case Status.All:
                    break;
                case Status.PendingAndAccepted:
                    _filters.Add (Builders<MiniTicketV2>.Filter.In (v => v.DangerStatus, PendingAndAcceptedStatusList));
                    break;
                case Status.Accepted:
                    _filters.Add (Builders<MiniTicketV2>.Filter.In (v => v.DangerStatus, AcceptedStatusList));
                    break;
                case Status.Rejected:
                    _filters.Add (Builders<MiniTicketV2>.Filter.In (v => v.DangerStatus, RejectedStatusList));
                    break;
                case Status.Pending:
                    _filters.Add (Builders<MiniTicketV2>.Filter.In (v => v.DangerStatus, PendingStatusList));
                    break;
            }
        }

        private void SetSpecialConditions ()
        {
            switch (Special)
            {
                case 0: // All
                    break;
                case -1: // All specials
                    _filters.Add (Builders<MiniTicketV2>
                        .Filter
                        .Eq (v => v.LeagueType, LeagueType.Special));
                    /*.Where
                    (v =>
                        (
                            v.SpecialIds.Count > 0 &&
                            !v.SpecialIds.Any(specialId => _normalSpecialIds.Contains(specialId))
                        ) ||
                        v.LeagueType == LeagueType.Special
                    ));*/
                    break;
                case -2: // All normals
                    _filters.Add (Builders<MiniTicketV2>
                        .Filter
                        .Ne (v => v.LeagueType, LeagueType.Special));
                    /*.Where
                    (v =>
                        (
                            v.SpecialIds.Count == 0 ||
                            v.SpecialIds.Any(specialId => _normalSpecialIds.Contains(specialId))
                        ) &&
                        v.LeagueType != LeagueType.Special
                    ));*/
                    break;
                default:
                    // var specialStr = Special.ToString();
                    // _filters.Add(Builders<MiniTicketV2>.Filter.Regex(v => v.SpecialId, new BsonRegularExpression(specialStr, "g")));
                    _filters.Add (Builders<MiniTicketV2>
                        .Filter.Where (v => v.SpecialIds.Contains (Special)));
                    break;
            }
        }

        private void SetVipTypeConditions ()
        {
            switch (VipType)
            {
                case 0: // All
                    break;
                case -1: // All VIP Types
                    _filters.Add (Builders<MiniTicketV2>.Filter.Gt (v => v.VipType, 0));
                    break;
                default: // Others
                    _filters.Add (Builders<MiniTicketV2>.Filter.Eq (v => v.VipType, VipType));
                    break;
            }
        }

        private void SetTransactionConditions ()
        {
            var transactionTypes = new List<string> { "OU", "HDP" };
            switch (Transaction)
            {
                case Transactions.HdpAndOu:
                    _filters.Add (Builders<MiniTicketV2>.Filter.In (v => v.TransType, transactionTypes));
                    break;
                case Transactions.Parlays:
                    _filters.Add (Builders<MiniTicketV2>.Filter.Eq (v => v.TransType, "PAR"));
                    break;
                case Transactions.Others: // Others (all except HDP/OU)
                    _filters.Add (Builders<MiniTicketV2>.Filter.Nin (v => v.TransType, transactionTypes));
                    break;
                case Transactions.Others_Nin_HdpOuPar: // Others (all except HDP/OU/PAR)
                    transactionTypes.Add ("PAR");
                    _filters.Add (Builders<MiniTicketV2>.Filter.Nin (v => v.TransType, transactionTypes));
                    break;
            }
        }

        private void SetSportConditions ()
        {
            switch (Sport)
            {
                case Sports.Others:
                    _filters.Add (Builders<MiniTicketV2>.Filter.Ne (v => v.SportType, "S"));
                    break;
                case Sports.Soccer:
                    _filters.Add (Builders<MiniTicketV2>.Filter.Eq (v => v.SportType, "S"));
                    break;
                case Sports.Specials3D4D:
                    _filters.Add (Builders<MiniTicketV2>.Filter.Eq (v => v.SportType, "34D"));
                    break;
                case Sports.Specials1D2D:
                    _filters.Add (Builders<MiniTicketV2>.Filter.Eq (v => v.SportType, "12D"));
                    break;
                case Sports.Basketball:
                    _filters.Add (Builders<MiniTicketV2>.Filter.Eq (v => v.SportType, "BB"));
                    break;
                case Sports.Futsal:
                    _filters.Add (Builders<MiniTicketV2>.Filter.Eq (v => v.SportType, "FS"));
                    break;
                case Sports.BeachSoccer:
                    _filters.Add (Builders<MiniTicketV2>.Filter.Eq (v => v.SportType, "BC"));
                    break;
                case Sports.AmericanFootball:
                    _filters.Add (Builders<MiniTicketV2>.Filter.Eq (v => v.SportType, "UF"));
                    break;
                case Sports.Baseball:
                    _filters.Add (Builders<MiniTicketV2>.Filter.Eq (v => v.SportType, "BE"));
                    break;
                case Sports.IceHockey:
                    _filters.Add (Builders<MiniTicketV2>.Filter.Eq (v => v.SportType, "IH"));
                    break;
                case Sports.Tennis:
                    _filters.Add (Builders<MiniTicketV2>.Filter.Eq (v => v.SportType, "TN"));
                    break;
                case Sports.FinancialBets:
                    _filters.Add (Builders<MiniTicketV2>.Filter.Eq (v => v.SportType, "FB"));
                    break;
                case Sports.Badminton:
                    _filters.Add (Builders<MiniTicketV2>.Filter.Eq (v => v.SportType, "BA"));
                    break;
                case Sports.Golf:
                    _filters.Add (Builders<MiniTicketV2>.Filter.Eq (v => v.SportType, "GF"));
                    break;
                case Sports.Cricket:
                    _filters.Add (Builders<MiniTicketV2>.Filter.Eq (v => v.SportType, "CK"));
                    break;
                case Sports.Volleyball:
                    _filters.Add (Builders<MiniTicketV2>.Filter.Eq (v => v.SportType, "VB"));
                    break;
                case Sports.Handball:
                    _filters.Add (Builders<MiniTicketV2>.Filter.Eq (v => v.SportType, "HB"));
                    break;
                case Sports.Pool:
                    _filters.Add (Builders<MiniTicketV2>.Filter.Eq (v => v.SportType, "PL"));
                    break;
                case Sports.Billiard:
                    _filters.Add (Builders<MiniTicketV2>.Filter.Eq (v => v.SportType, "BL"));
                    break;
                case Sports.Snooker:
                    _filters.Add (Builders<MiniTicketV2>.Filter.Eq (v => v.SportType, "NS"));
                    break;
                case Sports.Rugby:
                    _filters.Add (Builders<MiniTicketV2>.Filter.Eq (v => v.SportType, "RB"));
                    break;
                case Sports.MotoGp:
                    _filters.Add (Builders<MiniTicketV2>.Filter.Eq (v => v.SportType, "GP"));
                    break;
                case Sports.Darts:
                    _filters.Add (Builders<MiniTicketV2>.Filter.Eq (v => v.SportType, "DT"));
                    break;
                case Sports.Boxing:
                    _filters.Add (Builders<MiniTicketV2>.Filter.Eq (v => v.SportType, "BX"));
                    break;
                case Sports.MuayThai:
                    _filters.Add (Builders<MiniTicketV2>.Filter.Eq (v => v.SportType, "MT"));
                    break;
                case Sports.Athletics:
                    _filters.Add (Builders<MiniTicketV2>.Filter.Eq (v => v.SportType, "AT"));
                    break;
                case Sports.ESports:
                    _filters.Add (Builders<MiniTicketV2>.Filter.Eq (v => v.SportType, "ES"));
                    break;
                case Sports.Archery:
                    _filters.Add (Builders<MiniTicketV2>.Filter.Eq (v => v.SportType, "AR"));
                    break;
                case Sports.Chess:
                    _filters.Add (Builders<MiniTicketV2>.Filter.Eq (v => v.SportType, "CH"));
                    break;
                case Sports.Diving:
                    _filters.Add (Builders<MiniTicketV2>.Filter.Eq (v => v.SportType, "DV"));
                    break;
                case Sports.Equestrian:
                    _filters.Add (Builders<MiniTicketV2>.Filter.Eq (v => v.SportType, "EQ"));
                    break;
                case Sports.Entertainment:
                    _filters.Add (Builders<MiniTicketV2>.Filter.Eq (v => v.SportType, "ET"));
                    break;
                case Sports.Canoeing:
                    _filters.Add (Builders<MiniTicketV2>.Filter.Eq (v => v.SportType, "CN"));
                    break;
                case Sports.CombatSports:
                    _filters.Add (Builders<MiniTicketV2>.Filter.Eq (v => v.SportType, "CS"));
                    break;
                case Sports.Cycling:
                    _filters.Add (Builders<MiniTicketV2>.Filter.Eq (v => v.SportType, "CY"));
                    break;
                case Sports.Hockey:
                    _filters.Add (Builders<MiniTicketV2>.Filter.Eq (v => v.SportType, "HK"));
                    break;
                case Sports.Gymnastics:
                    _filters.Add (Builders<MiniTicketV2>.Filter.Eq (v => v.SportType, "GM"));
                    break;
                case Sports.FloorBall:
                    _filters.Add (Builders<MiniTicketV2>.Filter.Eq (v => v.SportType, "FL"));
                    break;
                case Sports.Novelties:
                    _filters.Add (Builders<MiniTicketV2>.Filter.Eq (v => v.SportType, "NT"));
                    break;
                case Sports.Olympic:
                    _filters.Add (Builders<MiniTicketV2>.Filter.Eq (v => v.SportType, "OL"));
                    break;
                case Sports.Politics:
                    _filters.Add (Builders<MiniTicketV2>.Filter.Eq (v => v.SportType, "PO"));
                    break;
                case Sports.Squash:
                    _filters.Add (Builders<MiniTicketV2>.Filter.Eq (v => v.SportType, "QQ"));
                    break;
                case Sports.Swimming:
                    _filters.Add (Builders<MiniTicketV2>.Filter.Eq (v => v.SportType, "MN"));
                    break;
                case Sports.RugbyUnion:
                    _filters.Add (Builders<MiniTicketV2>.Filter.Eq (v => v.SportType, "RU"));
                    break;
                case Sports.TableTennis:
                    _filters.Add (Builders<MiniTicketV2>.Filter.Eq (v => v.SportType, "TT"));
                    break;
                case Sports.Weightlifting:
                    _filters.Add (Builders<MiniTicketV2>.Filter.Eq (v => v.SportType, "WG"));
                    break;
                case Sports.WinterSports:
                    _filters.Add (Builders<MiniTicketV2>.Filter.Eq (v => v.SportType, "WI"));
                    break;
                case Sports.WaterPolo:
                    _filters.Add (Builders<MiniTicketV2>.Filter.Eq (v => v.SportType, "WP"));
                    break;
                case Sports.Speedway:
                    _filters.Add (Builders<MiniTicketV2>.Filter.Eq (v => v.SportType, "WS"));
                    break;
            }
        }

        private void SetIpConditions ()
        {
            if (!string.IsNullOrWhiteSpace (Ip))
            {
                _filters.Add (Builders<MiniTicketV2>.Filter
                    .Regex (v => v.BetIp, new BsonRegularExpression ($"^{Ip}")));
            }
        }

        private void SetAccountConditions ()
        {
            if (!string.IsNullOrWhiteSpace (Account))
            {
                var accountLow = Account.ToLower ();
                _filters.Add (Builders<MiniTicketV2>.Filter
                    .Regex (v => v.UserName, new BsonRegularExpression ($"^{accountLow}")));
            }
        }

        private void SetAmountConditions ()
        {
            if (Amount > 0)
            {
                _filters.Add (Builders<MiniTicketV2>.Filter.Gte (v => v.Amount, Amount));
            }
        }

        private void SetViewConditions ()
        {
            if (View == Views.OnlyMOMatches)
            {
                _filters.Add (Builders<MiniTicketV2>.Filter.In (v => v.MatchId, MatchCodes));
            }
        }

        private void SetMarketConditions ()
        {
            var workingDate = WorkingDate;
            switch (Market)
            {
                case Markets.All:
                    break;
                case Markets.Early:
                    _filters.Add (Builders<MiniTicketV2>.Filter.Where (v => !v.IsRun && v.WorkingDate > workingDate));
                    break;
                case Markets.Today:
                    _filters.Add (Builders<MiniTicketV2>.Filter.Where (v => v.WorkingDate == workingDate && v.IsRun == false));
                    break;
                case Markets.Running:
                    _filters.Add (Builders<MiniTicketV2>.Filter.Where (v => v.IsRun));
                    break;
                case Markets.RunningAndToday:
                    _filters.Add (Builders<MiniTicketV2>.Filter.Where (v => v.IsRun || v.WorkingDate == workingDate));
                    break;
            }
        }
    }
}