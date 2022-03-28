using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketCore.Models.Knowledgebase
{
    [Table("KnowledgebaseType")]
    public class KnowledgebaseType
    {
        [Key]
        public int KnowledgebaseTypeId { get; set; }
        public string KnowledgebaseTypeName { get; set; }
    }
}