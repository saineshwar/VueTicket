using TicketCore.Models.Usermaster;

namespace TicketCore.Data.Usermaster.Queries
{
    public interface IUserTokensQueries
    {
        UserTokens GetUserSaltbyUserid(long userId);
    }
}