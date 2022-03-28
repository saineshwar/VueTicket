using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketCore.Models.CategoryConfigrations
{
    [Table("DepartmentConfigration")]
    public class DepartmentConfigration
    {
        [Key]
        public int DepartmentConfigrationId { get; set; }
        public int? DepartmentId { get; set; }
        public long AgentAdminUserId { get; set; }
        public int? BusinessHoursId { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public bool Status { get; set; }
        public DateTime? Modifiedon { get; set; }
        public long HodUserId { get; set; }
        public long? CreatedBy { get; set; }
        public long? ModifiedBy { get; set; }
    }
}