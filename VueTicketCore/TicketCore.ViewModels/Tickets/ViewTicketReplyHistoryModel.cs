using System.ComponentModel;

namespace TicketCore.ViewModels.Tickets
{
    public class ViewTicketReplyHistoryModel
    {
        public long? TicketReplyId { get; set; }
        public string RepliedUserName { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Message")]
        public string Message { get; set; }

        public string CreatedDateDisplay { get; set; }

        [DisplayName("TrackingId")]
        public string TrackingId { get; set; }
        public string Viewcolor { get; set; }

        [DisplayName("TicketId")]
        public long TicketId { get; set; }


        public bool DeleteStatus { get; set; }
        public short RoleId { get; set; }

        [DisplayName("Note")]
        public string Note { get; set; }
        public long? SystemUser { get; set; }
        public long? TicketUser { get; set; }
    }
}