using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using TicketCore.Models.Knowledgebase;

namespace TicketCore.ViewModels.Knowledgebase
{
    public class EditKnowledgebaseViewModel
    {
        public long KnowledgebaseId { get; set; }
        public long KnowledgebaseDetailsId { get; set; }
        

        [DisplayName("Subject")]
        [Required(ErrorMessage = "Please Enter Subject")]
        public string Subject { get; set; }

        [DisplayName("Category")]
        [Required(ErrorMessage = "Please Select Department")]
        public int? DepartmentId { get; set; }
        public List<SelectListItem> ListofDepartment { get; set; }

        [DisplayName("Type")]
        [Required(ErrorMessage = "Please Select Type")]
        public int? KnowledgebaseTypeId { get; set; }

        public List<SelectListItem> ListofKnowledgebaseType { get; set; }

        [DisplayName("Contents")]
        [Required(ErrorMessage = "Please Enter Contents")]
        [MaxLength(2000)]
        public string Contents { get; set; }

        [MaxLength(500)]
        [DisplayName("Keywords")]
        [Required(ErrorMessage = "Please Enter Keywords")]
        public string Keywords { get; set; }

        [DisplayName("Status")]
        public bool Status { get; set; }
        public List<KnowledgebaseAttachments> ListofAttachments { get; set; }
    }
}