using TicketCore.Models.Masters;
using TicketCore.Models.Verification;

namespace TicketCore.Data.EmailVerification.Queries
{
    public interface IVerificationQueries
    {

        string GetResetGeneratedTokenbyUnq(int? unq);
        EmailVerificationModel GetEmailVerificationCodeGeneratedToken(long userid);
    }
}