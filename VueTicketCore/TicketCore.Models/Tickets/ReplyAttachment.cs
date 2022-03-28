using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketCore.Models.Tickets
{
    [Table("ReplyAttachment")]
    public class ReplyAttachmentModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ReplyAttachmentId { get; set; }
        public string OriginalAttachmentName { get; set; }
        public string GenerateAttachmentName { get; set; }
        public string AttachmentType { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public long? CreatedBy { get; set; }
        public long? ModifiedBy { get; set; }
        public long? TicketId { get; set; }
        public long? TicketReplyId { get; set; }
        public string BucketName { get; set; }
        public string DirectoryName { get; set; }
    }

    [Table("ReplyAttachmentDetails")]
    public class ReplyAttachmentDetailsModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ReplyAttachmentDetailsId { get; set; }
        [MaxLength]
        public string AttachmentBase64 { get; set; }
        public long? TicketId { get; set; }
        public long ReplyAttachmentId { get; set; }
        public long? CreatedBy { get; set; }
        public long? ModifiedBy { get; set; }
    }
}