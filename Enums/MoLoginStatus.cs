namespace stake_place_web.Enums
{
    public enum MoLoginStatus 
    {
        Undefined = 0,
        Success = 1,
        UserNotExists = 2,
        LoginAreaLimit = 3,
        WrongPassword = 4,
        AccountInactive = 5,
        InvalidArguments = 6,
        KickOut = -2,
        NoResponse = -3,
        Error = 7
    }
}