using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketCore.Models.Tickets
{
    [Table("Attachments")]
    public class AttachmentsModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AttachmentId { get; set; }
        public string OriginalAttachmentName { get; set; }
        public string GenerateAttachmentName { get; set; }
        public string AttachmentType { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public long? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public long? TicketId { get; set; }
        public string BucketName { get; set; }
        public string DirectoryName { get; set; }
    }


    [Table("AttachmentDetails")]
    public class AttachmentDetailsModel
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AttachmentDetailsId { get; set; }
        [MaxLength]
        public string AttachmentBase64 { get; set; }
        public long? TicketId { get; set; }
        public long? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public long AttachmentId { get; set; }

    }
}