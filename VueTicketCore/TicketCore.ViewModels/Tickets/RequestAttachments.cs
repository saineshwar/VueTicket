using System.ComponentModel.DataAnnotations;

namespace TicketCore.ViewModels.Tickets
{
    public class RequestAttachments
    {
        public long TicketId { get; set; }
        public long AttachmentsId { get; set; }
    }

    public class RequestReplyAttachments
    {
        public long TicketId { get; set; }
        public long AttachmentsId { get; set; }
        public long TicketReplyId { get; set; }
    }

    public class RequestChangePriority
    {
        [Required(ErrorMessage = "Required TicketId")]
        public long TicketId { get; set; }
        [Required(ErrorMessage = "Required PriorityId")]
        public short PriorityId { get; set; }
    }

    public class RequestKnowledgebaseAttachments
    {
        public long KnowledgebaseId { get; set; }
        public long AttachmentsId { get; set; }
    }

    public class RequestKnowledgebaseChange
    {
        public long KnowledgebaseId { get; set; }
        public bool Status { get; set; }
    }

    public class RequestTicketDetails
    {
        public long TicketId { get; set; }
    }

}