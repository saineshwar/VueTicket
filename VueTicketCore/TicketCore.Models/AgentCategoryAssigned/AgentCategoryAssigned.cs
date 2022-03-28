using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketCore.Models.AgentCategoryAssigned
{
    [Table("AgentDepartmentAssigned")]
    public class AgentDepartmentAssigned
    {
        [Key]
        public int AgentDepartmentId { get; set; }
        public int DepartmentId { get; set; }
        public long UserId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}