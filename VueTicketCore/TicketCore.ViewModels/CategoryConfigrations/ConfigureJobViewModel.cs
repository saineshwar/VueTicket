using System.ComponentModel.DataAnnotations;

namespace TicketCore.ViewModels.CategoryConfigrations
{
    public class ConfigureJobViewModel
    {

        [Display(Name = "Assign Tickets Job")]
        public bool AssignTicketsJob { get; set; }

        [Display(Name = "Ticket Overdue Job")]
        public bool TicketOverdueJob { get; set; }

        [Display(Name = "Overdue Notification Job")]
        public bool OverdueNotificationJob { get; set; }

        [Display(Name = "Overdue EveryRespons Job")]
        public bool OverdueEveryResponsJob { get; set; }

        [Display(Name = "Auto Escalation Job Stage1")]
        public bool AutoEscalationJobStage1 { get; set; }

        [Display(Name = "Auto Escalation Job Stage2")]
        public bool AutoEscalationJobStage2 { get; set; }

        [Display(Name = "Auto Close Tickets Job")]
        public bool AutoCloseTicketsJob { get; set; }
    }
}