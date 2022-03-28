using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketCore.Models.Profiles
{
    [Table("ProfileImage")]
    public class ProfileImage
    {
        [Key]
        public long ProfileImageId { get; set; }
        public string ProfileImageBase64String { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? UserId { get; set; }
    }
}