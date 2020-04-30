using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using stake_place_web.Service;

namespace stake_place_web.Hubs
{
    public class MainFormResultHub : Hub
    {
        private ITicketService _ticketService;
        public MainFormResultHub(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        public override Task OnDisconnectedAsync(Exception exception){
            _ticketService.onConnectUserParams.Remove(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}