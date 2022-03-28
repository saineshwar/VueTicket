using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TicketCore.Models.Tickets;

namespace TicketCore.ViewModels.Tickets
{
    public class EditTicketReplyViewModel
    {
        public long? TicketId { get; set; }
        public long? TicketReplyDetailsId { get; set; }
        public long? TicketReplyId { get; set; }

        [Required(ErrorMessage = "Please Enter Description.")]
        [DisplayName("Message")]
        public string Message { get; set; }
        public List<ReplyAttachmentModel> ListofAttachments { get; set; }

        [DisplayName("Note")]
        public string Note { get; set; }
    }
}