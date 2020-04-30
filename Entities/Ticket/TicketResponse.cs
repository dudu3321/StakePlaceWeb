using System.Collections.Generic;

namespace stake_place_web.Entities.Ticket
{
    public class TicketResponse
    {
        public TicketResponse(int added, List<StakePlaceTicket> stakePlaceTickets)
        {
            Added = added;
            StakePlaceTickets = stakePlaceTickets;
        }

        public int Added { get; set; }
        public List<StakePlaceTicket> StakePlaceTickets { get; set; }
    }
}