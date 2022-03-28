using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TicketCore.ViewModels.Tickets
{
    public class TicketReplyViewModel
    {
        [DisplayName("TicketId")]
        public long? TicketId { get; set; }

        [StringLength(700)]
        [Required(ErrorMessage = "Please Enter Message.")]
        [DisplayName("Message")]
        public string Message { get; set; }

        [DisplayName("Note")]
        public string Note { get; set; }

        [Display(Name = "Status")]
        public int? StatusId { get; set; }
        public List<SelectListItem> ListofStatus { get; set; }
    }


    public class TicketUserReplyViewModel
    {
        [DisplayName("TicketId")]
        public long? TicketId { get; set; }

        [StringLength(700)]
        [Required(ErrorMessage = "Please Enter Message.")]
        [DisplayName("Message")]
        public string Message { get; set; }

        [DisplayName("Note")]
        public string Note { get; set; }

    }
}