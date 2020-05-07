using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using stake_place_web.Entities.Filter;
using stake_place_web.Entities.Ticket;
using stake_place_web.Service;

namespace stake_place_web.Controllers
{
    [ApiController]
    [Route ("[controller]")]
    public class TicketController
    {
        private readonly ITicketService _ticketService;
        public TicketController (ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpPost]
        public TicketResponse Post ([FromBody] TicketRequest request)
        {
            var ticketParameters = _ticketService.GetTicketParameters (request);
            return _ticketService.GetTicketResponse (request.ConnectionId, _ticketService.GetTickets (ticketParameters));
        } 
    }
}