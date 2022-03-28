using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketCore.Models.Notices
{
    [Table("NoticeDetails")]
    public class NoticeDetails
    {
        [Key]
        public int NoticeDetailsId { get; set; }
        public string NoticeBody { get; set; }
        public int? NoticeId { get; set; }
        public Notice Notice { get; set; }
    }
}