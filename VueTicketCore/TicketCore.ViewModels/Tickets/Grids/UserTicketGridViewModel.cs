using System;

namespace TicketCore.ViewModels.Tickets.Grids
{
    public class UserTicketGridViewModel
    {
        public int RowNum { get; set; }
        public long TicketId { get; set; }
        public string TrackingId { get; set; }
        public string Subject { get; set; }
        public string DepartmentName { get; set; }
        public string Name { get; set; }
        public string Priority { get; set; }
        public int? PriorityId { get; set; }
        public int? StatusId { get; set; }
        public string Status { get; set; }
        public DateTime? FirstResponseDue { get; set; }
        public bool? FirstResponseStatus { get; set; }
        public DateTime? ResolutionDue { get; set; }
        public bool? ResolutionStatus { get; set; }
        public bool? EveryResponseStatus { get; set; }
        public bool? EscalationStage1Status { get; set; }
        public bool? EscalationStage2Status { get; set; }
        public DateTime? TicketAssignedDate { get; set; }
        public DateTime? TicketUpdatedDate { get; set; }
        public bool DeleteStatus { get; set; }
        public string StatusInfo { get; set; }
        public string AssignedAgent { get; set; }

    }
}