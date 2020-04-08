using System;
using Microsoft.AspNetCore.Mvc;
using StakePlaceEntities;

namespace stake_place_web.Controllers {
    [ApiController]
    [Route ("[controller]")]
    public class FilterController {
        [HttpGet]
        public void GET ([FromBody] Views view) {
            
        }
    }
}