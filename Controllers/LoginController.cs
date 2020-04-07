using System.Xml.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using stake_place_web.Service;
using stake_place_web.Entities.Login;
using stake_place_web.Enums;

namespace stake_place_web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
             _loginService = loginService;
        }

        [HttpPost]
        public MoLoginResponse POST([FromBody]MoLoginRequest request)
        {
            _loginService.Login (request.MoLogin, request.Password);
            var response = new MoLoginResponse ();
            while (true) {
                response = _loginService.pendingMoLogin[request.MoLogin];
                if (response.UpdateFinished) {
                    break;
                }
                if((DateTime.Now - response.CreateTime).TotalSeconds > 2){
                   _loginService.ReceivedInvoke(MoLoginStatus.Error, request.MoLogin, "", "Mo Service response timeout.");
                }
            } 
            return response;
        }
    }
}
