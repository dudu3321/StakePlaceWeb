using System.Collections.Generic;

namespace stake_place_web.Entities
{
    public class FieldsAcl
    {
        public readonly bool HideDates;
        public readonly bool HideRefNo;
        public readonly bool HideAmount;
        public readonly bool HideMemberInfo;

        public FieldsAcl(List<string> userLevels)
        {
            HideMemberInfo = !userLevels.Contains("4");
            HideRefNo = !userLevels.Contains("5");
            HideAmount = !userLevels.Contains("6");
            HideDates = !userLevels.Contains("7");
        }
    }
}