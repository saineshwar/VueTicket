using System.Linq;
using TicketCore.Models.Usermaster;

namespace TicketCore.Data.Usermaster.Queries
{
    public class UserTokensQueries : IUserTokensQueries
    {
        private readonly VueTicketDbContext _vueTicketDbContext;
        public UserTokensQueries(VueTicketDbContext vueTicketDbContext)
        {
            _vueTicketDbContext = vueTicketDbContext;
        }
        public UserTokens GetUserSaltbyUserid(long userId)
        {
            var usertoken = (from tempuser in _vueTicketDbContext.UserTokens
                             where tempuser.UserId == userId
                             select tempuser).FirstOrDefault();

            return usertoken;
        }
    }
}