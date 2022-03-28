namespace TicketCore.ViewModels.Masters
{
    public class SlaPoliciesReminderShowViewModel
    {
        public int SlaPoliciesReminderId { get; set; }
        public int? FirstResponseHour { get; set; }
        public int? FirstResponseMins { get; set; }

        public int? NextResponseHour { get; set; }
        public int? NextResponseMins { get; set; }

        public int? ResolutionResponseHour { get; set; }
        public int? ResolutionResponseMins { get; set; }

        public string BusinessHours { get; set; }
    }
}