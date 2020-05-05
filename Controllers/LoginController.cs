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
    }
}