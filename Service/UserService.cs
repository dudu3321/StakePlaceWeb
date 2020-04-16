using System.Collections.Generic;

namespace stake_place_web.Service
{
    public interface IUserService
    {
        List<string> GetUserLevels();
    }

    public class UserService : IUserService
    {
        public List<string> GetUserLevels()
        {
            var result = new List<string>();
            return result;
        }
    }
}