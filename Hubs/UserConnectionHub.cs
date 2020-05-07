using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using stake_place_web.Entities.Login;
using stake_place_web.Service;

namespace stake_place_web.Hubs
{
    public class UserConnectionHub : Hub
    {
        private readonly ILoginService _loginService;
        public UserConnectionHub(ILoginService loginService)
        {
            _loginService = loginService;
        }

        public override Task OnDisconnectedAsync(Exception exception){
            _loginService.userConnectionId.Remove(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        public void UserLogin(MoLoginRequest request){
            _loginService.DoLogin(request);
        }
    }
}