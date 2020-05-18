using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using stake_place_web.Entities.Login;
using stake_place_web.Service;

namespace stake_place_web.Hubs
{
    public class TicketConnectionHub : Hub
    {
        private readonly ITicketService _ticketService;
        public TicketConnectionHub(ITicketService ticketService, ILoginService loginService)
        {
            _ticketService = ticketService;
        }

        public override Task OnDisconnectedAsync(Exception exception){
            Console.WriteLine($"[LOG] disconnect ConnectionId:{Context.ConnectionId}");
            _ticketService.onConnectUserParams.Remove(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}