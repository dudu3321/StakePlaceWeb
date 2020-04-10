using System;
using System.Collections.Generic;
using stake_place_web.Enums;
using StakePlaceEntities;

namespace stake_place_web.Entities.Login
{
    public class MoLoginResponse
    {
        public MoLoginResponse ()
        {
            MatchCodes = new List<int> ();
            UserLevels = new List<string> ();
        }
        public string MoLogin { get; set; }
        public string EncryptedPassword { get; set; }
        public MoLoginStatus MoLoginStatus { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public List<int> MatchCodes { get; set; }
        public List<string> UserLevels { get; set; }
        public Views View { get; set; }
        public bool UpdateFinished { get; set; }
        public DateTime CreateTime { get; set; }
    }
}