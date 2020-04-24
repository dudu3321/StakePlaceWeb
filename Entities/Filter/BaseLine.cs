using System;
using System.Collections.Generic;
using DataStruct.Cache.Classes;
using StakePlaceEntities;

namespace stake_place_web.Entities.Filter
{
    public class ViewLine : BaseLine<Views>
    {
        public ViewLine (Views value, string description) : base (value, description) { }

        public static List<ViewLine> GetAllLines => new List<ViewLine>
        {
            new ViewLine (Views.OnlyMOMatches, "MO matches"),
            new ViewLine (Views.AllMatches, "All matches")
        };

        public static List<ViewLine> GetMOOnlyLines => new List<ViewLine>
        {
            new ViewLine (Views.OnlyMOMatches, "MO matches")
        };
    }

    public class MarketLine : BaseLine<Markets>
    {
        public MarketLine (Markets value, string description) : base (value, description) { }

        public static List<MarketLine> GetAllLines => new List<MarketLine>
        {
            new MarketLine (Markets.All, "All markets"),
            new MarketLine (Markets.Early, "Early"),
            new MarketLine (Markets.Today, "Today"),
            new MarketLine (Markets.Running, "Running"),
            new MarketLine (Markets.RunningAndToday, "Running+Today"),
        };
    }

    public class RecordLine : BaseLine<Records>
    {
        public RecordLine (Records value, string description) : base (value, description) { }

        public static List<RecordLine> GetAllLines => new List<RecordLine>
        {
            new RecordLine (Records.FiftyLines, "50 tickets"),
            new RecordLine (Records.OneHundredLines, "100 tickets"),
            new RecordLine (Records.TwoHundredLines, "200 tickets"),
            new RecordLine (Records.FiveHundredLines, "500 tickets"),
            new RecordLine (Records.OneThousandLines, "1000 tickets"),
            new RecordLine (Records.TwoThousandLines, "2000 tickets")
        };

        public static List<RecordLine> GetMOOnlyLines => new List<RecordLine>
        {
            new RecordLine (Records.FiftyLines, "50 tickets"),
            new RecordLine (Records.OneHundredLines, "100 tickets"),
            new RecordLine (Records.TwoHundredLines, "200 tickets"),
            new RecordLine (Records.FiveHundredLines, "500 tickets")
        };
    }

    public class SportLine : BaseLine<Sports>
    {
        public SportLine (Sports value, string description) : base (value, description) { }

        public static List<SportLine> GetAllLines => new List<SportLine>
        {
            new SportLine (Sports.All, "All sports"),
            new SportLine (Sports.Soccer, "Soccer"),
            new SportLine (Sports.Others, "Others"),
            new SportLine (Sports.Specials3D4D, "3D/4D Specials"),
            new SportLine (Sports.Specials1D2D, "1D/2D Specials"),
            new SportLine (Sports.Basketball, "Basketball"),
            new SportLine (Sports.Futsal, "Futsal"),
            new SportLine (Sports.BeachSoccer, "Beach Soccer"),
            new SportLine (Sports.AmericanFootball, "American Football"),
            new SportLine (Sports.Baseball, "Baseball"),
            new SportLine (Sports.IceHockey, "Ice Hockey"),
            new SportLine (Sports.Tennis, "Tennis"),
            new SportLine (Sports.FinancialBets, "Financial Bets"),
            new SportLine (Sports.Badminton, "Badminton"),
            new SportLine (Sports.Golf, "Golf"),
            new SportLine (Sports.Cricket, "Cricket"),
            new SportLine (Sports.Volleyball, "Volleyball"),
            new SportLine (Sports.Handball, "Handball"),
            new SportLine (Sports.Pool, "Pool"),
            new SportLine (Sports.Billiard, "Billiard"),
            new SportLine (Sports.Snooker, "Snooker"),
            new SportLine (Sports.Rugby, "Rugby"),
            new SportLine (Sports.MotoGp, "Moto GP"),
            new SportLine (Sports.Darts, "Darts"),
            new SportLine (Sports.Boxing, "Boxing"),
            new SportLine (Sports.MuayThai, "Muay Thai"),
            new SportLine (Sports.Athletics, "Athletics"),
            new SportLine (Sports.ESports, "E-Sports"),
            new SportLine (Sports.Archery, "Archery"),
            new SportLine (Sports.Chess, "Chess"),
            new SportLine (Sports.Diving, "Diving"),
            new SportLine (Sports.Equestrian, "Equestrian"),
            new SportLine (Sports.Entertainment, "Entertainment"),
            new SportLine (Sports.Canoeing, "Canoeing"),
            new SportLine (Sports.CombatSports, "Combat Sports"),
            new SportLine (Sports.Cycling, "Cycling"),
            new SportLine (Sports.Hockey, "Hockey"),
            new SportLine (Sports.Gymnastics, "Gymnastics"),
            new SportLine (Sports.FloorBall, "Floor Ball"),
            new SportLine (Sports.Novelties, "Novelties"),
            new SportLine (Sports.Olympic, "Olympic"),
            new SportLine (Sports.Politics, "Politics"),
            new SportLine (Sports.Squash, "Squash"),
            new SportLine (Sports.Swimming, "Swimming"),
            new SportLine (Sports.RugbyUnion, "Rugby Union"),
            new SportLine (Sports.TableTennis, "Table Tennis"),
            new SportLine (Sports.Weightlifting, "Weightlifting"),
            new SportLine (Sports.WinterSports, "Winter Sports"),
            new SportLine (Sports.WaterPolo, "Water Polo"),
            new SportLine (Sports.Speedway, "Speedway")
        };
    }

    public class TransactionLine : BaseLine<Transactions>
    {
        public TransactionLine (Transactions value, string description) : base (value, description) { }

        public static List<TransactionLine> GetAllLines => new List<TransactionLine>
        {
            new TransactionLine (Transactions.All, "All types"),
            new TransactionLine (Transactions.HdpAndOu, "HDP+OU"),
            new TransactionLine (Transactions.Parlays, "PAR"),
            new TransactionLine (Transactions.Others, "Others (all except HDP/OU)"),
            new TransactionLine (Transactions.Others_Nin_HdpOuPar, "Others (all except HDP/OU/PAR)"),
        };
    }

    public class TicketLine : BaseLine<Tickets>
    {
        public TicketLine (Tickets value, string description) : base (value, description) { }

        public static List<TicketLine> GetAllLines => new List<TicketLine>
        {
            new TicketLine (Tickets.All, "All tickets"),
            new TicketLine (Tickets.Normal, "Normal"),
            new TicketLine (Tickets.TradeIn, "TradeIn"),
        };
    }

    public class StatusLine : BaseLine<Status>
    {
        public StatusLine (Status value, string description) : base (value, description) { }

        public static List<StatusLine> GetAllLines => new List<StatusLine>
        {
            new StatusLine (Status.All, "All status"),
            new StatusLine (Status.Pending, "Pending"),
            new StatusLine (Status.Accepted, "Accepted"),
            new StatusLine (Status.PendingAndAccepted, "Pending+Accepted"),
            new StatusLine (Status.Rejected, "Rejected"),
        };
    }

    public class VipLine : BaseLine<int>
    {
        public VipLine (int value, string description) : base (value, description) { }

        public static List<VipLine> GetAllLines => new List<VipLine>
        {
            new VipLine (0, "All members"),
            new VipLine (-1, "All VIPs"),
        };

        public static void AddRange (List<VipLine> vipTypes, List<MiniVipTypeV2> vipTypesToAdd)
        {
            foreach (var vipType in vipTypesToAdd)
            {
                var vipTypeId = vipType.Id;
                var value = vipType.Description;
                vipTypes.Add (new VipLine (vipTypeId, value));
            }
        }

        [Obsolete ("", true)]
        public static void AddRange (List<VipLine> vipTypes, List<MiniVipType> vipTypesToAdd)
        {
            foreach (var vipType in vipTypesToAdd)
            {
                var vipTypeId = vipType.VIPType;
                var value = vipType.VIPDesc;
                vipTypes.Add (new VipLine (vipTypeId, value));
            }
        }
    }

    public class SpecialLine : BaseLine<int>
    {
        public SpecialLine (int value, string description) : base (value, description) { }

        public static List<SpecialLine> GetAllLines => new List<SpecialLine>
        {
            new SpecialLine (0, "All leagues"),
            new SpecialLine (-1, "All (specials)"),
            new SpecialLine (-2, "All (normals)"),
        };

        public static void AddRange (List<SpecialLine> specials, List<MiniSpecialLanguageV2> specialsToAdd)
        {
            foreach (var specialLanguage in specialsToAdd)
            {
                var specialId = specialLanguage.Id;
                var special = specialLanguage.MODesc;
                specials.Add (new SpecialLine (specialId, special));
            }
        }

        [Obsolete ("", true)]
        public static void AddRange (List<SpecialLine> specials, List<SpecialLanguages> specialsToAdd)
        {
            foreach (var specialLanguage in specialsToAdd)
            {
                var specialId = specialLanguage.SpecialId;
                var language = specialLanguage.Languages["en_US"];
                var special = language.SpecialMODesc;
                specials.Add (new SpecialLine (specialId, special));
            }
        }
    }

    [Obsolete ("", true)]
    public enum TicketRequests
    {
        OneThousand,
        OneThousandFiveHundreds,
        TwoThousands,
        TwoThousandsFiveHundreds,
        FiveThousands
    }

    [Obsolete ("", true)]
    public class TicketRequestsLine : BaseLine<TicketRequests>
    {
        private TicketRequestsLine (TicketRequests value, string description) : base (value, description) { }

        public static List<TicketRequestsLine> GetAllLines => new List<TicketRequestsLine>
        {
            new TicketRequestsLine (TicketRequests.OneThousand, "1.0 sec."),
            new TicketRequestsLine (TicketRequests.OneThousandFiveHundreds, "1.5 sec."),
            new TicketRequestsLine (TicketRequests.TwoThousands, "2.0 sec."),
            new TicketRequestsLine (TicketRequests.TwoThousandsFiveHundreds, "2.5 sec."),
            new TicketRequestsLine (TicketRequests.FiveThousands, "5.0 sec.")
        };
    }

}