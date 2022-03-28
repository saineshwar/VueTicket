using TicketCore.Models.Profiles;
using TicketCore.ViewModels.Usermaster;

namespace TicketCore.Data.Profiles.Command
{
    public interface IProfileCommand
    {
        int UpdateUserMasterDetails(long userId, UsermasterEditView usermasterEditView);
        void UpdateProfileImage(ProfileImage profileImage);
        int DeleteProfileImage(long userId);
        int UpdateSignature(Signatures signatures);
        int DeleteSignature(Signatures signatures);
    }
}