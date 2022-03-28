using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using TicketCore.Models.Tickets;

namespace TicketCore.ViewModels.Tickets
{
    public class DisplayTicketViewModel
    {
        public TicketDetailsViewModel TicketDetailViewModel { get; set; }
        public TicketReplyViewModel TicketReply { get; set; }
        public ViewTicketReplyMainModel ViewMainModel { get; set; }
        public ExpressChangesTicketViewModel ExpressChangesViewModel { get; set; }
        public List<AttachmentsModel> ListofAttachments { get; set; }
        public EscalatedUserViewModel EscalatedUser { get; set; }

    }

    public class TicketDetailsViewModel
    {
        [DisplayName("TicketId")]
        public long TicketId { get; set; }

        [DisplayName("TrackingId")]
        public string TrackingId { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Email")]
        public string EmailId { get; set; }

        [DisplayName("Priority")]
        public string PriorityName { get; set; }

        [DisplayName("Department")]
        public string DepartmentName { get; set; }

        [DisplayName("MobileNo")]
        public string MobileNo { get; set; }

        [DisplayName("CreatedOn")]
        public string CreatedDate { get; set; }
        public long? UserId { get; set; }
        public bool StatusAssigned { get; set; }
        public long TicketDetailsId { get; set; }

        [DisplayName("Subject")]
        public string Subject { get; set; }

        [DisplayName("Message")]
        public string Message { get; set; }
        public int TicketLockStatus { get; set; }
        public string RoleName { get; set; }
        public bool? DeleteStatus { get; set; }
        public bool EscalationStatus { get; set; }
        public DateTime? FirstResponseDue { get; set; }
        public bool FirstResponseStatus { get; set; }
        public DateTime? ResolutionDue { get; set; }
        public bool ResolutionStatus { get; set; }
        public bool EveryResponseStatus { get; set; }
        public bool EscalationStage1Status { get; set; }
        public bool EscalationStage2Status { get; set; }
        public DateTime? EscalationDate1 { get; set; }
        public DateTime? EscalationDate2 { get; set; }

        public int? PriorityId { get; set; }
        public int? StatusId { get; set; }
        
        [DisplayName("Status")]
        public string StatusName { get; set; }

        [DisplayName("Assignedto")]
        public string AssignedTicketUser { get; set; }
        public int DepartmentId { get; set; }
    }

    public class ExpressChangesTicketViewModel
    {
        [DisplayName("ChangePriority")]
        public int? PriorityId { get; set; }
        public List<SelectListItem> ListofPriority { get; set; }

        [DisplayName("ChangeStatus")]
        public int? StatusId { get; set; }
        public List<SelectListItem> ListofStatus { get; set; }

        [Display(Name = "Assignedto")]
        public string AssignedTo { get; set; }

        [Display(Name = "Priority")]
        public string Priority { get; set; }

        [Display(Name = "Assignedto")]
        [Required(ErrorMessage = "Assigned To is Required")]
        public int? AssignedToId { get; set; }
        public List<SelectListItem> ListofUsers { get; set; }
    }

}