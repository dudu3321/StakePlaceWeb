using System;
using Microsoft.AspNetCore.Mvc;
using stake_place_web.Entities.Filter;
using stake_place_web.Service;
using StakePlaceEntities;
namespace stake_place_web.Controllers
{
    [ApiController]
    [Route ("[controller]")]
    public class FilterController
    {
        private readonly IFilterService _filtersService;
        FilterController(IFilterService filtersService){
            _filtersService = filtersService;
        }
        
        [HttpGet]
        public FilterResponse GET ()
        {
            return _filtersService.GetFiltersData();
        }
    }
}