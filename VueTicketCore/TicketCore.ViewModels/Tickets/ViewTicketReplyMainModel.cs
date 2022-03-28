using System.Collections.Generic;
using TicketCore.Models.Tickets;

namespace TicketCore.ViewModels.Tickets
{
    public class ViewTicketReplyMainModel
    {
        public List<ViewTicketReplyHistoryModel> ListofTicketreply { get; set; }
        public List<ReplyAttachmentModel> ListofReplyAttachment { get; set; }
    }
}