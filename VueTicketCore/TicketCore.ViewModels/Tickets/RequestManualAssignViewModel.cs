namespace TicketCore.ViewModels.Tickets
{
    public class RequestManualAssignViewModel
    {
        public long? TicketId { get; set; }
        public long? CreatedUserId { get; set; }
        public long? AssignedtoUserId { get; set; }
        public int? PriorityId { get; set; }
        public int? DepartmentId { get; set; }
        public long? AssignedbyUserId { get; set; }
        
    }
}