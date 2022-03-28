using TicketCore.Models.Profiles;
using TicketCore.ViewModels.Usermaster;

namespace TicketCore.Data.Profiles.Queries
{
    public interface IProfileQueries
    {
        UsermasterEditView GetprofileById(long userId);
        UserProfileView GetUserprofileById(long userId);
        Signatures GetSignatureDetails(long userId);
        bool IsProfileImageExists(long userId);
        string GetProfileImageBase64String(long userId);
        string GetSignature(long userId);
        bool CheckSignatureAlreadyExists(long userId);
    }
}