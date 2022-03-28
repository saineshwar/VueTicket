using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketCore.Models.Knowledgebase
{
    [Table("Knowledgebase")]
    public class KnowledgebaseModel
    {
        [Key]
        public long KnowledgebaseId { get; set; }
        [MaxLength(100)]
        public string Subject { get; set; }
        public int? KnowledgebaseTypeId { get; set; }
        public bool Status { get; set; }
        public int? DepartmentId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public long? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
    }
}