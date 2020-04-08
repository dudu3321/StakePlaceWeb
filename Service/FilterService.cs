using System;
using System.Collections.Generic;
using StakePlaceEntities;
using StakePlaceEntities.Dao.MongoDb;
using stake_place_web.Entities.Filter;
namespace stake_place_web.Service
{
    public interface IFilterService
    {
        FilterResponse GetFiltersData();
    }

    public class FilterService : IFilterService
    {
        private MiniVipTypeV2Dao _miniVipTypeV2Dao;
        private MiniSpecialLanguageV2Dao _miniSpecialLanguageV2Dao;
        private MiniUserMatchIdsV2Dao _miniUserMatchIdsV2Dao;

        public FilterResponse GetFiltersData()
        {
            var filterResponse = new FilterResponse();
            var miniVipTypes = GetVipTypes();
            if (miniVipTypes.Count > 0)
            {
                VipLine.AddRange(filterResponse.vipLines, miniVipTypes);
            }
            var miniSpecials = GetSpecials();
            if (miniSpecials.Count > 0)
            {
                SpecialLine.AddRange(filterResponse.specialLines, miniSpecials);
            }

            return filterResponse;
        }

        private List<MiniVipTypeV2> GetVipTypes()
        {
            try
            {
                return _miniVipTypeV2Dao.QueryAll(false);
            }
            catch (Exception ex)
            { }
            return new List<MiniVipTypeV2>();
        }

        private List<MiniSpecialLanguageV2> GetSpecials()
        {
            try
            {
                return _miniSpecialLanguageV2Dao.QueryAll(false);
            }
            catch (Exception ex)
            { }
            return new List<MiniSpecialLanguageV2>();
        }
    }
}