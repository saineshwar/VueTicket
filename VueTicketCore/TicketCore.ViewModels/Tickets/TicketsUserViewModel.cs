using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TicketCore.ViewModels.Tickets
{
    public class TicketsUserViewModel
    {
        public long TicketId { get; set; }

        [MaxLength(20)]
        [DisplayName("TrackingId")]
        public string TrackingId { get; set; }

        [Required(ErrorMessage = "Please Select Priority.")]
        [DisplayName("Priority")]
        public int? PriorityId { get; set; }
        public List<SelectListItem> ListofPriority { get; set; }

        [DisplayName("Department")]
        [Required(ErrorMessage = "Please Select Department.")]
        public int? DepartmentId { get; set; }
        public List<SelectListItem> ListofDepartment { get; set; }

        [DisplayName("Subject")]
        [Required(ErrorMessage = "Please Enter Subject.")]
        [MaxLength(200, ErrorMessage = "Only 200 Characters Allowed")]
        public string Subject { get; set; }

        [MaxLength(2000)]
        [DisplayName("Message")]
        [Required(ErrorMessage = "Please Enter Message.")]
        public string Message { get; set; }
      
    }

    public class TicketCommonViewModel
    {
        public long? HiddenUserId { get; set; }

        [DisplayName("Requester")]
        [Required(ErrorMessage = "Please Select Requester Name.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please Select Priority.")]
        [DisplayName("Priority")]
        public int? PriorityId { get; set; }
        public List<SelectListItem> ListofPriority { get; set; }

        [DisplayName("Department")]
        [Required(ErrorMessage = "Please Select Department.")]
        public int? DepartmentId { get; set; }
        public List<SelectListItem> ListofDepartment { get; set; }

        [MaxLength(200,ErrorMessage = "Only 200 Characters Allowed")]
        [Required(ErrorMessage = "Please Enter Subject.")]
        [DisplayName("Subject")]
        public string Subject { get; set; }

        [MaxLength(2000)]
        [Required(ErrorMessage = "Please Enter Message.")]
        [DisplayName("Message")]
        public string Message { get; set; }

    }
}