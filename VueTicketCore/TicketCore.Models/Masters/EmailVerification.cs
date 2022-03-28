using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketCore.Models.Masters
{
    [Table("EmailVerification")]
    public class EmailVerificationModel
    {
        [Key]
        public long EmailVerificationId { get; set; }
        public string EmailId { get; set; }
        public long UserId { get; set; }
        public bool Verified { get; set; }
        public DateTime? VerifiedDate { get; set; }
        public string VerificationCode { get; set; }
        public DateTime CreateDate { get; set; }
    }
}