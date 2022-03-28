namespace TicketCore.ViewModels.Tickets
{
    public class TicketStatusResponse
    {
        public bool FirstResponseStatus { get; set; }
        public bool EveryResponseStatus { get; set; }
        public bool ResolutionStatus { get; set; }
        public bool EscalationStage1Status { get; set; }
        public bool EscalationStage2Status { get; set; }

    }
}