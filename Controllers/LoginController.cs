using System;
using Microsoft.AspNetCore.Mvc;
using stake_place_web.Entities.Login;
using stake_place_web.Enums;
using stake_place_web.Service;

namespace stake_place_web.Controllers
{
    [ApiController]
    [Route ("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public LoginController (ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost]
        public MoLoginResponse POST ([FromBody] MoLoginRequest request)
        {
            var response = new MoLoginResponse ();
            try
            {
                _loginService.Login (request.MoLogin, request.Password);

                while (true)
                {
                    response = _loginService.pendingMoLogin[request.MoLogin];
                    if (response.UpdateFinished)
                    {
                        break;
                    }
                    if ((DateTime.Now - response.CreateTime).TotalSeconds > 2)
                    {
                        _loginService.ReceivedInvoke (MoLoginStatus.Error, request.MoLogin, "", "Mo Service response timeout.");
                    }
                }
            }
            catch (Exception ex)
            {
                response.MoLoginStatus = MoLoginStatus.Error;
                response.Title = "Exception Error!";
                response.Message = ex.ToString();
            }
            return response;
        }
    }
}