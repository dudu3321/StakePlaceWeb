using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using DataStruct.MatchEntities.Entity;
using StakePlaceEntities;

namespace stake_place_web.Entities.Ticket
{
    public class StakePlaceTicket
    {
        public static readonly List<string> RejectedStatus = new List<string> { "R", "J", "Y", "G", "E" };
        public static readonly List<string> PendingStatus = new List<string> { "D", "X" };
        public static readonly List<string> AcceptedStatus = new List<string> { "N", "A", "S" };
        public static readonly List<string> TransactionsHdpAndOu = new List<string> { "OU", "HDP" };

        public int Id { get; set; }

        public bool IsStock { get; set; }
        
        public string Account { get; set; }
        public Color AccountForeColor { get; set; }
        public Color AccountBackColor { get; set; }

        public string League { get; set; }
        public Color LeagueColor { get; set; }

        public string Home { get; set; }
        public Color HomeColor { get; set; }

        public string Away { get; set; }
        public Color AwayColor { get; set; }

        public string TransType { get; set; }
        public Color TransTypeColor { get; set; }

        public string Run { get; set; }

        public string Hdp { get; set; }
        public Color HdpColor { get; set; }

        public string MmrOdds { get; set; }
        public Color MmrOddsColor { get; set; }

        public string Odds { get; set; }
        public Color OddsColor { get; set; }

        public string MyOdds { get; set; }
        public Color MyOddsColor { get; set; }

        private bool _isPending;
        public bool IsPending => _isPending;

        private string _dangerStatus = string.Empty;

        public string DangerStatus
        {
            get { return _dangerStatus; }
            set
            {
                if (RejectedStatus.IndexOf(value) >= 0)
                {
                    DangerStatusColor = Color.Red;
                    _isPending = false;
                }
                else if (PendingStatus.IndexOf(value) >= 0)
                {
                    DangerStatusColor = Color.Orange;
                    _isPending = true;
                }
                else if (AcceptedStatus.IndexOf(value) >= 0)
                {
                    DangerStatusColor = Color.Green;
                    _isPending = false;
                }
                _dangerStatus = value;
            }
        }

        public Color DangerStatusColor { get; set; }

        public decimal AmountForFilter => RejectedStatus.IndexOf(DangerStatus) >= 0 ? 0 : _amount;
        private readonly decimal _amount;
        public string Amount 
        {
            get
            {
                if (_hideAmount) return HIDDEN_VALUE;
                // return RejectedStatus.IndexOf(DangerStatus) >= 0 ? "0" : $"{_amount.ToString("N0")}";
                return $"{_amount:n0}";
            }
        }

        public string TransDate { get; set; }
        public string BetTime { get; set; }
        public string BetDate { get; set; }
        public string BetIp { get; set; }

        private string _operated = string.Empty;
        public string Operated
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_operated)) return string.Empty;
                return _hideMemberInfo ? MIDDLE_HIDDEN_VALUE : _operated;
            }
            set { _operated = value; }
        }

        private string _updated = string.Empty;
        public string Updated
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_updated)) return string.Empty;
                return _hideMemberInfo ? MIDDLE_HIDDEN_VALUE : _updated;
            }
            set { _updated = value; }
        }

        public string RefNo { get; set; }
        public Color RefNoColor { get; set; }

        public int MatchCode { get; set; }

        public int SocTransId { get; set; }

        // private const string HIDDEN_VALUE = "**";
        private const string HIDDEN_VALUE = "@@";
        // private const string MIDDLE_HIDDEN_VALUE = "****";
        private const string MIDDLE_HIDDEN_VALUE = "@@@@";
        // private const string LONG_HIDDEN_VALUE = "********";
        private const string LONG_HIDDEN_VALUE = "@@@@@@@@";
        private readonly bool _isRun;
        private readonly int _workingDate;
        private readonly string _sportType;
        private readonly string _transType;
        private readonly int _vipType;
        [Obsolete("", true)]
        private readonly string _specialId;
        private readonly List<int> _specialIds;
        private readonly LeagueType _leagueType;

        private readonly bool _hideDates;
        private readonly bool _hideRefNo;
        private readonly bool _hideAmount;
        private readonly bool _hideMemberInfo;
        private static readonly List<int> _baseSpecialIds = new List<int> {1000};

        private readonly bool _isFullTime;

        public StakePlaceTicket(MiniTicketV2 ticket, FieldsAcl fieldsAcl)
        {
            _hideDates = fieldsAcl.HideDates;
            _hideRefNo = fieldsAcl.HideRefNo;
            _hideAmount = fieldsAcl.HideAmount;
            _hideMemberInfo = fieldsAcl.HideMemberInfo;

            _isFullTime = ticket.IsFullTime;

            _sportType = ticket.SportType;
            _workingDate = ticket.WorkingDate;
            _isRun = ticket.IsRun;
            _vipType = ticket.VipType;
            // _specialId = ticket.SpecialId;
            _specialIds = ticket.SpecialIds;
            _leagueType = ticket.LeagueType;
            var oldTransType = ticket.TransType;
            _transType = GetTransTypeValue(oldTransType);

            #region Displayed properties

            IsStock = ticket.IsStock;
            Id = IsStock ? ticket.SocTransId : ticket.SocTransTradeInId;
            SocTransId = ticket.SocTransId;

            Account = ticket.UserName;
            SetAccountColors(ticket.VipType);
            // AccountBackColor = GetAccountColor(ticket.VipType);

            var isFullTime = ticket.IsFullTime;
            var isBetHome = ticket.IsBetHome;
            var isHomeGive = ticket.IsHomeGive;

            TransType = $"{_transType}{(IsStock ? string.Empty : " (TI)")}";
            TransTypeColor = GetTransTypeColor(_transType);
            
            League = ticket.LeagueName;

            if (isFullTime)
            {
                LeagueColor = Color.Black;
                if (_transType.Equals("HDP") || _transType.Equals("OU") || _transType.Equals("OE"))
                {
                    if (isBetHome)
                    {
                        Home = $"*{ticket.HomeName}";
                        Away = $"{ticket.AwayName}";
                    }
                    else
                    {
                        Home = $"{ticket.HomeName}";
                        Away = $"*{ticket.AwayName}";
                    }
                }
                else
                {
                    Home = $"{ticket.HomeName}";
                    Away = $"{ticket.AwayName}";
                }
            }
            else
            {
                LeagueColor = Color.Red;
                if (_transType.Equals("HDP") || _transType.Equals("OU") || _transType.Equals("OE"))
                {
                    if (isBetHome)
                    {
                        Home = $"*1h-{ticket.HomeName}";
                        Away = $"1h-{ticket.AwayName}";
                    }
                    else
                    {
                        Home = $"1h-{ticket.HomeName}";
                        Away = $"*1h-{ticket.AwayName}";
                    }
                }
                else
                {
                    Home = $"1h-{ticket.HomeName}";
                    Away = $"1h-{ticket.AwayName}";
                }
            }

            var specialIdsWithoutBase = _specialIds.Except(_baseSpecialIds);
            // var specialsWithoutBase = _specialId.Replace(",", "").Replace("1000", "");
            // if (!string.IsNullOrWhiteSpace(specialsWithoutBase) || _leagueType == LeagueType.Special)
            if (specialIdsWithoutBase.Any() || _leagueType == LeagueType.Special)
            {
                LeagueColor = Color.Purple;
            }

            switch (_transType)
            {
                case "HDP":
                    {
                        if (isHomeGive)
                        {
                            if (isBetHome)
                            {
                                HomeColor = TransTypeColor;
                            }
                            else
                            {
                                HomeColor = TransTypeColor;
                            }
                        }
                        else
                        {
                            if (isBetHome)
                            {
                                AwayColor = TransTypeColor;
                            }
                            else
                            {
                                AwayColor = TransTypeColor;
                            }
                        }
                        break;
                    }
                case "OU":
                    {
                        if (isBetHome)
                        {
                            HomeColor = TransTypeColor;
                        }
                        else
                        {
                            AwayColor = TransTypeColor;
                        }
                        break;
                    }
                case "OE":
                    {
                        if (isBetHome)
                        {
                            HomeColor = TransTypeColor;
                        }
                        else
                        {
                            AwayColor = TransTypeColor;
                        }
                        break;
                    }
                default:
                    HomeColor = TransTypeColor;
                    AwayColor = TransTypeColor;
                    break;
            }

            Run = _isRun ? $"{ticket.HomeScore}-{ticket.AwayScore}" : string.Empty;

            var hdp = ticket.Hdp;
            var hdpAttributes = GetHdpAttributes(oldTransType, _transType, hdp, isHomeGive, isBetHome);
            Hdp = hdpAttributes.Item1;
            HdpColor = hdpAttributes.Item2;

            var oddType = ticket.OddType;
            var percent = ticket.Percent;
            var mmrAttributes = GetMmrAttributes(oldTransType, oddType, percent, isHomeGive, isBetHome);
            MmrOdds = mmrAttributes.Item1;
            MmrOddsColor = mmrAttributes.Item2;

            var odds = ticket.Odds;
            /*if (IsStock) Odds = _transType.Equals("PAR") ? $"{odds:0.##}" : $"{odds}";
            else Odds = $"{odds:0.##}";*/
            if (IsStock)
            {
                switch (oldTransType)
                {
                    case "HDP":
                    case "OU":
                    case "3D":
                    case "OE":
                    case "4D":
                        Odds = $"{odds * 10:0.##}";
                        break;
                    default:
                        Odds = $"{odds:0.##}";
                        break;
                }
                
            }
            else
            {
                Odds = $"{Math.Round(odds, 0, MidpointRounding.AwayFromZero):0}";
            }
            OddsColor = odds > 0 ? Color.Blue : odds < 0 ? Color.Red : Color.Black;

            // var myOdds = IsStock ? (odds * 10) : odds;
            // MyOdds = IsStock ? $"{(odds * 10):0.#}" : $"{odds:0.##}";
            if ((TransactionsHdpAndOu.Contains(_transType) || _transType.Equals("OE")) && IsStock)
            {
                var myOdds = ticket.MyOdds;
                MyOdds = $"{(myOdds * 10):0.##}";
                // MyOddsColor = myOdds > 0 ? Color.Blue : myOdds < 0 ? Color.Red : Color.Black;
                MyOddsColor = myOdds > 0 ? Color.Blue : myOdds < 0 ? Color.Red : Color.Black;
            }
            else
            {
                MyOdds = string.Empty;
                MyOddsColor = Color.Black;
            }

            DangerStatus = ticket.DangerStatus;
            // _amount = ticket.Amount;
            _amount = ticket.Admin.Stock;

            var transDate = ticket.TransDate;
            TransDate = transDate;
            BetTime = transDate.Substring(11);

            var betDate = transDate.Substring(5, 10);
            BetDate = $"{betDate[3]}{betDate[4]}/{betDate[0]}{betDate[1]}";

            BetIp = ticket.BetIp;

            RefNo = ticket.RefNo;
            if (IsStock)
            {
                RefNoColor = Color.Black;
            }
            else
            {
                RefNoColor = Color.BlueViolet;
            }

            Operated = ticket.CreateName;

            Updated = ticket.UpdateName;

            MatchCode = ticket.MatchId;

            #endregion

            #region Hide fields

            if (_hideMemberInfo)
            {
                Account = LONG_HIDDEN_VALUE;
                // AccountColor = Color.Black;

                BetIp = LONG_HIDDEN_VALUE;
            }

            if (_hideRefNo)
            {
                RefNo = LONG_HIDDEN_VALUE;
            }

            if (_hideDates)
            {
                BetDate = HIDDEN_VALUE;
                BetTime = HIDDEN_VALUE;
            }
            
            #endregion
        }

        #region Private Methods for values and colors calculation

        private static Tuple<string, Color> GetMmrAttributes(string transType, string oddType,
            decimal percent, bool isHomeGive, bool isBetHome)
        {
            if (string.IsNullOrWhiteSpace(oddType)) return new Tuple<string, Color>("--", Color.Black);
            if (oddType.EndsWith("MR") == false) return new Tuple<string, Color>("--", Color.Black);
            try
            {
                // var mmrOdds = int.Parse(percent);
                var mmrOdds = percent;
                switch (transType)
                {
                    case "OU":
                        {
                            var value = (isBetHome ? 1 : -1) * mmrOdds;
                            if (value > 0) return new Tuple<string, Color>($"{value}", Color.Blue);
                            return new Tuple<string, Color>($"{value}", Color.Red);
                        }
                    case "HDP":
                        {
                            if (isHomeGive)
                            {
                                var value = (isBetHome ? 1 : -1) * mmrOdds;
                                if (value > 0) return new Tuple<string, Color>($"{value}", Color.Blue);
                                return new Tuple<string, Color>($"{value}", Color.Red);
                            }
                            else
                            {
                                var value = (isBetHome ? -1 : 1) * mmrOdds;
                                if (value > 0) return new Tuple<string, Color>($"{value}", Color.Blue);
                                return new Tuple<string, Color>($"{value}", Color.Red);
                            }
                        }
                }
            }
            catch (Exception)
            {
            }
            return new Tuple<string, Color>("--", Color.Black);
        }

        private static Tuple<string, Color> GetHdpAttributes(string oldTransType, string newTransType, decimal hdp, bool isHomeGive, bool isBetHome)
        {
            var hdpValue = string.Empty;
            var hdpStyle = Color.Black;
            switch (oldTransType)
            {
                case "TG":
                    {
                        if (hdp == 1) hdpValue = "0-1";
                        else if (hdp == 23) hdpValue = "2-3";
                        else if (hdp == 46) hdpValue = "4-6";
                        else if (hdp == 70) hdpValue = "7&Over";
                        break;
                    }
                case "CS":
                    {
                        if (hdp == -99) hdpValue = "AOS";
                        else
                        {
                            if (hdp == 0) hdpValue = "0-0";
                            else if (hdp >= 1 && hdp <= 4) hdpValue = $"0-{hdp:0}";
                            else
                            {
                                var hdpStr = $"{hdp}";
                                hdpValue = $"{hdpStr.Substring(0, 1)}-{hdpStr.Substring(1, 1)}";
                            }
                        }
                        break;
                    }
                case "OE":
                    {
                        hdpValue = isBetHome ? "Odd" : "Even";
                        break;
                    }
                case "FLG":
                    {
                        if (hdp == 1) hdpValue = "HF";
                        else if (hdp == 2) hdpValue = "AF";
                        else if (hdp == 3) hdpValue = "HL";
                        else if (hdp == 4) hdpValue = "AL";
                        else if (hdp == 5) hdpValue = "NG";
                        break;
                    }
                case "OUT":
                    break;
                case "HFT":
                    {
                        if (hdp == 1) hdpValue = "HH";
                        else if (hdp == 2) hdpValue = "HA";
                        else if (hdp == 3) hdpValue = "HD";
                        else if (hdp == 4) hdpValue = "AH";
                        else if (hdp == 5) hdpValue = "AA";
                        else if (hdp == 6) hdpValue = "AD";
                        else if (hdp == 7) hdpValue = "DH";
                        else if (hdp == 8) hdpValue = "DA";
                        else if (hdp == 9) hdpValue = "DD";
                        break;
                    }
                case "PAR":
                case "4D":
                case "OU":
                    {
                        hdpValue = $"{hdp:0.##}";
                        break;
                    }
                case "1D":
                case "2D":
                    {
                        hdpValue = $"{hdp}";
                        break;
                    }
                case "3D":
                case "HDP":
                    {
                        if (hdp == 0)
                        {
                            hdpValue = $"{hdp:0.##}";
                        }
                        else
                        {
                            if (isHomeGive)
                            {
                                if (isBetHome)
                                {
                                    hdpValue = $"-{hdp:0.##}";
                                    hdpStyle = Color.Red;
                                }
                                else
                                {
                                    hdpValue = $"+{hdp:0.##}";
                                    hdpStyle = Color.Blue;
                                }
                            }
                            else
                            {
                                if (isBetHome)
                                {
                                    hdpValue = $"+{hdp:0.##}";
                                    hdpStyle = Color.Blue;
                                }
                                else
                                {
                                    hdpValue = $"-{hdp:0.##}";
                                    hdpStyle = Color.Red;
                                }
                            }
                        }
                        break;
                    }
                default:
                    {
                        hdpValue = oldTransType;
                        break;
                    }
            }

            return new Tuple<string, Color>(hdpValue, hdpStyle);
        }

        private static Color GetTransTypeColor(string transType)
        {
            switch (transType)
            {
                case "HDP": return Color.Red;
                case "OU": return Color.Blue;
                case "1D":
                case "2D":
                case "PAR":
                    return Color.Black;
                case "FLG": return Color.Purple;
                case "HFT": return Color.Brown;
                case "TG": return Color.CornflowerBlue;
                case "CS": return Color.Orange;
                case "OE": return Color.Green;
                case "1X2": return Color.DeepPink;
                case "DC": return Color.DarkViolet;
                case "OUT": return Color.Gray;
                default: return Color.White;
            }
        }

        private static string GetTransTypeValue(string oldTransType)
        {
            switch (oldTransType)
            {
                case "1":
                case "2":
                case "X":
                    return "1X2";
                case "3D":
                    return "HDP";
                case "4D":
                    return "OU";
                case "1X":
                case "12":
                case "X2":
                    return "DC";
                default:
                    return oldTransType;
            }
        }

        [Obsolete("CREDIT-1176: Please use AccountSetColors (2019/05/21)", true)]
        private static Color GetAccountColor(int vipType)
        {
            switch (vipType)
            {
                case 1:
                    return Color.Red;
                case 2:
                case 3:
                case 4:
                    return Color.Brown;
                case 5:
                case 6:
                case 7:
                    return Color.Orange;
                case 8:
                    // return Color.Yellow;
                    return ColorTranslator.FromHtml("#ffd200");
                case 9:
                    // return Color.Green;
                    return ColorTranslator.FromHtml("#008806");
                case 10:
                    // return Color.Purple;
                    return ColorTranslator.FromHtml("#A201B0");
                case 11:
                    // return Color.LightBlue;
                    return ColorTranslator.FromHtml("#00BAB3");
                case 12:
                    // return Color.Gray;
                    return ColorTranslator.FromHtml("#808080");
                case 13:
                    // return Color.LightGreen;
                    return ColorTranslator.FromHtml("#CCEE64");
                case 14:
                    // return Color.Cyan;
                    return ColorTranslator.FromHtml("#00ffff");
                case 15:
                    // return Color.Magenta;
                    return ColorTranslator.FromHtml("#ff00ff");
                case 16:
                    return Color.Blue;
                case 17:
                case 18:
                    // return Color.Magenta;
                    return ColorTranslator.FromHtml("#ff00ff");
                case 19:
                    return Color.Blue;
                case 20:
                    // return Color.DarkGreen;
                    return ColorTranslator.FromHtml("#007b9f");
                case 21:
                    // return Color.GreenYellow;
                    return ColorTranslator.FromHtml("#00ff54");
                case 22:
                    return Color.SteelBlue;
                case 23:
                    return Color.Brown;
                case 24:
                    return Color.Blue;
                case 25:
                    return Color.Brown;
                case 26:
                case 27:
                case 28:
                    // return Color.Maroon;
                    return ColorTranslator.FromHtml("#800000");
                default:
                    return Color.White;
            }
        }

        private void SetAccountColors(int vipType)
        {
            switch (vipType)
            {
                case 1:
                    AccountForeColor = Color.White;
                    AccountBackColor = Color.Red;
                    break;
                case 2:
                case 3:
                case 4:
                    AccountForeColor = Color.White;
                    AccountBackColor = Color.Brown;
                    break;
                case 5:
                case 6:
                case 7:
                    AccountForeColor = Color.Black;
                    AccountBackColor = Color.Orange;
                    break;
                case 8:
                    AccountForeColor = Color.Black;
                    // return Color.Yellow;
                    AccountBackColor = ColorTranslator.FromHtml("#ffd200");
                    break;
                case 9:
                    AccountForeColor = Color.White;
                    // return Color.Green;
                    AccountBackColor = ColorTranslator.FromHtml("#008806");
                    break;
                case 10:
                    AccountForeColor = Color.White;
                    // return Color.Purple;
                    AccountBackColor = ColorTranslator.FromHtml("#A201B0");
                    break;
                case 11:
                    AccountForeColor = Color.Black;
                    // return Color.LightBlue;
                    AccountBackColor = ColorTranslator.FromHtml("#00BAB3");
                    break;
                case 12:
                    AccountForeColor = Color.White;
                    // return Color.Gray;
                    AccountBackColor = ColorTranslator.FromHtml("#808080");
                    break;
                case 13:
                    AccountForeColor = Color.Black;
                    // return Color.LightGreen;
                    AccountBackColor = ColorTranslator.FromHtml("#CCEE64");
                    break;
                case 14:
                    AccountForeColor = Color.Black;
                    // return Color.Cyan;
                    AccountBackColor = ColorTranslator.FromHtml("#00ffff");
                    break;
                case 15:
                    AccountForeColor = Color.Black;
                    // return Color.Magenta;
                    AccountBackColor = ColorTranslator.FromHtml("#ff00ff");
                    break;
                case 16:
                    AccountForeColor = Color.White;
                    AccountBackColor = Color.Blue;
                    break;
                case 17:
                case 18:
                    AccountForeColor = Color.Black;
                    // return Color.Magenta;
                    AccountBackColor = ColorTranslator.FromHtml("#ff00ff");
                    break;
                case 19:
                    AccountForeColor = Color.White;
                    AccountBackColor = Color.Blue;
                    break;
                case 20:
                    AccountForeColor = Color.Black;
                    // return Color.DarkGreen;
                    AccountBackColor = ColorTranslator.FromHtml("#007b9f");
                    break;
                case 21:
                    // return Color.GreenYellow;
                    AccountBackColor = ColorTranslator.FromHtml("#00ff54");
                    break;
                case 22:
                    AccountForeColor = Color.Black;
                    AccountBackColor = Color.SteelBlue;
                    break;
                case 23:
                    AccountForeColor = Color.White;
                    AccountBackColor = Color.Brown;
                    break;
                case 24:
                    AccountForeColor = Color.White;
                    AccountBackColor = Color.Blue;
                    break;
                case 25:
                    AccountForeColor = Color.White;
                    AccountBackColor = Color.Brown;
                    break;
                case 26:
                case 27:
                case 28:
                    AccountForeColor = Color.White;
                    // return Color.Maroon;
                    AccountBackColor = ColorTranslator.FromHtml("#800000");
                    break;
                default:
                    AccountForeColor = Color.Black;
                    AccountBackColor = Color.White;
                    break;
            }
        }

        #endregion
    }
}