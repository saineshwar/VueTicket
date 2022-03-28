using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketCore.Models.Knowledgebase
{
    [Table("KnowledgebaseAttachmentsDetails")]
    public class KnowledgebaseAttachmentsDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long KnowledgebaseAttachmentsDetailsId { get; set; }
        public long KnowledgebaseId { get; set; }
        public long KnowledgebaseAttachmentsId { get; set; }
        [MaxLength]
        public string AttachmentBase64 { get; set; }
        public long? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
      
    }
}