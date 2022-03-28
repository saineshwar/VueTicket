using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using TicketCore.Models.Tickets;

namespace TicketCore.ViewModels.Tickets
{
    public class EditTicketViewModel
    {
        [DisplayName("TicketId")]
        public long TicketId { get; set; }

        [DisplayName("TrackingId")]
        [MaxLength(20)]
        public string TrackingId { get; set; }

        [Required(ErrorMessage = "Please Select Priority.")]
        [DisplayName("Priority")]
        public int? PriorityId { get; set; }
        public List<SelectListItem> ListofPriority { get; set; }

        [DisplayName("Department")]
        [Required(ErrorMessage = "Please Select Department.")]
        public int? DepartmentId { get; set; }
        public List<SelectListItem> ListofDepartment { get; set; }

        public DateTime? CreatedDate { get; set; }
        public long? TicketDetailsId { get; set; }

        [Required(ErrorMessage = "Please Enter Subject.")]
        [DisplayName("Subject")]
        public string Subject { get; set; }

        [MaxLength(2000)]
        [Required(ErrorMessage = "Please Enter Message.")]
        [DisplayName("Message")]
        public string Message { get; set; }
        public bool HasAttachment { get; set; }
        public List<AttachmentsModel> ListofAttachments { get; set; }
        public long? CreatedBy { get; set; }
    }
}