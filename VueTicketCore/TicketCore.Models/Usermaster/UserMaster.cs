using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketCore.Models.Usermaster
{
    [Table("Usermaster")]
    public class UserMaster
    {
        [Key]
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public string MobileNo { get; set; }
        public string Gender { get; set; }
        public DateTime? CreatedOn { get; set; } = DateTime.Now;
        public bool Status { get; set; }
        public long? CreatedBy { get; set; }
        public bool IsFirstLogin { get; set; } = false;
        public DateTime? ModifiedOn { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? IsFirstLoginDate { get; set; } 
        
    }
}