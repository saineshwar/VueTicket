using System.ComponentModel.DataAnnotations;

namespace TicketCore.ViewModels.Notices
{
    public class CreateNoticeViewModel
    {
        [MinLength(5, ErrorMessage = "Minimum Username must be 5 in characters")]
        [Required(ErrorMessage = "Enter Notice Title")]
        [Display(Name = "Notice Title")]
        public string NoticeTitle { get; set; }

        [Required(ErrorMessage = "Select Notice Start Date")]
        [Display(Name = "Notice Start Date")]
        public string NoticeStart { get; set; }

        [Required(ErrorMessage = "Select Notice End Date")]
        [Display(Name = "Notice End Date")]
        public string NoticeEnd { get; set; }

        [Required(ErrorMessage = "Enter Notice Body")]
        [Display(Name = "Notice Body")]
        public string NoticeBody { get; set; }
    }


    public class EditNoticeViewModel
    {
        public int NoticeId { get; set; }

        [MinLength(5, ErrorMessage = "Minimum Username must be 5 in characters")]
        [Required(ErrorMessage = "Enter Notice Title")]
        [Display(Name = "Notice Title")]
        public string NoticeTitle { get; set; }

        
        [Display(Name = "Notice Start Date")]
        public string NoticeStart { get; set; }

       
        [Display(Name = "Notice End Date")]
        public string NoticeEnd { get; set; }

        [Required(ErrorMessage = "Enter Notice Body")]
        [Display(Name = "Notice Body")]
        public string NoticeBody { get; set; }
        public bool Status { get; set; }
    }

    public class RequestDeleteNotice
    {
        public int NoticeId { get; set; }
    }
}