using System;
using System.Collections.Generic;
using StakePlaceEntities;
namespace stake_place_web.Entities.Ticket
{
    public class TicketRequest
    {
        public List<int?> MatchCodes { get; set; }
        public List<string> UserLevels { get; set; }
        public string Amount { get; set; }
        public string Ip { get; set; }
        public string Account { get; set; }
        public Views ViewLines { get; set; }
        public Markets MarketLines { get; set; }
        public Records RecordLines { get; set; }
        public Sports SportLines { get; set; }
        public Transactions TransactionLines { get; set; }
        public int VipLines { get; set; }
        public int SpecialLines { get; set; }
        public Tickets TicketLines { get; set; }
        public Status StatusLines { get; set; }
        public TicketRequest ()
        {
            UserLevels = new List<string> ();
        }
    }
}