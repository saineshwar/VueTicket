using System;

namespace TicketCore.ViewModels.Tickets
{
    public class EscalatedUserViewModel
    {
        public string UsernameStage1 { get; set; }
        public string UsernameStage2 { get; set; }
        public DateTime? EscalationDate1 { get; set; }
        public DateTime? EscalationDate2 { get; set; }
        public bool EscalationStage1Status { get; set; }
        public bool EscalationStage2Status { get; set; }
    }
}