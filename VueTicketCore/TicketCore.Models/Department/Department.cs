using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketCore.Models.Department
{
    [Table("Department")]
    public class Departments
    {
        [Key]
        public int DepartmentId { get; set; }


        [DisplayName("Department Name")]
        [Required(ErrorMessage = "Enter Department Name")]
        public string DepartmentName { get; set; }

        [Required(ErrorMessage = "Required Status")]
        [Display(Name = "Status")]
        public bool Status { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? UserId { get; set; }

        [Required(ErrorMessage = "Enter Department Code")]
        [DisplayName("Department Code")]
        [StringLength(1, MinimumLength = 1, ErrorMessage = "Code for Category Must be of only 1 words")]
        public string Code { get; set; }

        [DisplayName("Department Description")]
        public string DepartmentDescription { get; set; }

    }
}