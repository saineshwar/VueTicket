using System;
using System.ComponentModel.DataAnnotations;

namespace TicketCore.ViewModels.Notices
{
    public class NoticeDisplayViewModel
    {
        public int NoticeId { get; set; }

        [Display(Name = "Notice Title")]
        public string NoticeTitle { get; set; }

        [Display(Name = "Notice Start Date")]
        public DateTime? NoticeStart { get; set; }

        [Display(Name = "Notice End Date")]
        public DateTime? NoticeEnd { get; set; }

        public string CreatedOn { get; set; }

        [Display(Name = "Notice Body")]
        public string NoticeBody { get; set; }
    }
}