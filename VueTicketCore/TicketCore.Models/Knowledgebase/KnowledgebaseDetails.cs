using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketCore.Models.Knowledgebase
{
    [Table("KnowledgebaseDetails")]
    public class KnowledgebaseDetails
    {
        [Key]
        public long KnowledgebaseDetailsId { get; set; }
        [MaxLength(3000)]
        public string Contents { get; set; }
        [MaxLength(500)]
        public string Keywords { get; set; }
        public long KnowledgebaseId { get; set; }
    }
}