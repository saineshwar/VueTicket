using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketCore.Models.Profiles
{
    [Table("ProfileImageStatus")]
    public class ProfileImageStatus
    {
        [Key]
        public long ProfileImageStatusId { get; set; }
        public bool Isuploaded { get; set; }
        public long? UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public long ProfileImageId { get; set; }

    }
}