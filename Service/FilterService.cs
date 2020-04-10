using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using stake_place_web.Entities.Filter;
using StakePlaceEntities;
using StakePlaceEntities.Dao.MongoDb;
namespace stake_place_web.Service
{
    public interface IFilterService
    {
        FilterResponse GetFiltersData ();
    }

    public class FilterService : IFilterService
    {
        private IConfiguration _config;
        private static string mongoDbId = $"{Environment.UserName}@{Environment.MachineName}";
        private MiniVipTypeV2Dao _miniVipTypeV2Dao;
        private MiniSpecialLanguageV2Dao _miniSpecialLanguageV2Dao;

        public FilterService (IConfiguration config)
        {
            try
            {
                _config = config;
                var applicationName = _config["ApplicationName"];
                var connectionString = _config["MongoTicketsStatusConnectionString"];

                _miniVipTypeV2Dao = MiniVipTypeV2Dao.CreateInstance (connectionString,
                    $"{mongoDbId}@MiniVipTypeV2Dao@OthersBll@{applicationName}"
                );

                _miniSpecialLanguageV2Dao = MiniSpecialLanguageV2Dao.CreateInstance (connectionString,
                    $"{mongoDbId}@MiniSpecialLanguageV2Dao@OthersBll@{applicationName}"
                );
            }
            catch (Exception ex)
            {

            }
        }

        public FilterResponse GetFiltersData ()
        {
            var filterResponse = new FilterResponse ();
            var miniVipTypes = GetVipTypes ();
            if (miniVipTypes.Count > 0)
            {
                VipLine.AddRange (filterResponse.vipLines, miniVipTypes);
            }
            var miniSpecials = GetSpecials ();
            if (miniSpecials.Count > 0)
            {
                SpecialLine.AddRange (filterResponse.specialLines, miniSpecials);
            }

            return filterResponse;
        }

        private List<MiniVipTypeV2> GetVipTypes ()
        {
            try
            {
                return _miniVipTypeV2Dao.QueryAll (false);
            }
            catch (Exception ex) { }
            return new List<MiniVipTypeV2> ();
        }

        private List<MiniSpecialLanguageV2> GetSpecials ()
        {
            try
            {
                return _miniSpecialLanguageV2Dao.QueryAll (false);
            }
            catch (Exception ex) { }
            return new List<MiniSpecialLanguageV2> ();
        }
    }
}