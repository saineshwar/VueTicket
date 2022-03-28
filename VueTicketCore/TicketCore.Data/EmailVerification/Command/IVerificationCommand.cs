using TicketCore.Models.Verification;
using TicketCore.ViewModels.Usermaster;
using ResetPasswordVerification = TicketCore.Models.Verification.ResetPasswordVerification;

namespace TicketCore.Data.EmailVerification.Command
{
    public interface IVerificationCommand
    {
       
        string InsertResetPasswordVerificationToken(ResetPasswordVerification resetPassword);
        string UpdatePasswordandVerificationStatus(UpdateResetPasswordVerification resetPassword);
        int InsertEmailVerification(long? userId, string emailId, string verificationCode);
        bool UpdatedVerificationCode(long emailVerificationId);
    }
}