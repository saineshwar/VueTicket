using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.VisualBasic;
using TicketCore.ViewModels.Audit;

namespace TicketCore.ViewModels.Usermaster
{
    public class UserProfileViewModel
    {
        

        [DisplayName("FullName")]
        public string FullName { get; set; }

        [DisplayName("RoleName")]
        public string RoleName { get; set; }

        [DisplayName("Email")]
        public string EmailId { get; set; }

        [DisplayName("MobileNo")]
        public string MobileNo { get; set; }

        [DisplayName("Gender")]
        public string Gender { get; set; }
        public DateTime? FirstLoginDate { get; set; }
    }

    public class ProfileViewModel
    {
        public UserProfileViewModel Profile { get; set; }

        public List<AuditViewModel> ListofActivites { get; set; }
    }
}