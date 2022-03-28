using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketCore.Models.Knowledgebase
{
    [Table("KnowledgebaseAttachments")]
    public class KnowledgebaseAttachments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long KnowledgebaseAttachmentsId { get; set; }
        public string OriginalAttachmentName { get; set; }
        public string GenerateAttachmentName { get; set; }
        public string AttachmentType { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public long? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public long? KnowledgebaseId { get; set; }
        public string BucketName { get; set; }
        public string DirectoryName { get; set; }
    }
}