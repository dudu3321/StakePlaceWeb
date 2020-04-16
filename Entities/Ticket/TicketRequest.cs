using StakePlaceEntities;
using System;
using System.Collections.Generic;
namespace stake_place_web.Entities.Ticket
{
    public class TicketRequest
    {
        public List<string> UserLevels {get;set;}
        public string Amount { get; set; }
        public string Ip { get; set; }
        public string Account { get; set; }
        public Views View { get; set; }
        public Markets Market { get; set; }
        public Records Record { get; set; }
        public Sports Sport { get; set; }
        public Transactions Transaction { get; set; }
        public int Vip { get; set; }
        public int Special { get; set; }
        public Tickets Ticket { get; set; }
        public Status Status { get; set; }
        public DateTime? LastQueryTime {get; set;}
        public TicketRequest(){
            UserLevels = new List<string>();
        }
    }
}