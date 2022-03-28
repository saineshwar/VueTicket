using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketCore.Models.Tickets
{
    [Table("TicketReply")]
    public class TicketReplyModel
    {
        [Key] 
        public long TicketReplyId { get; set; }
        public long? TicketId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string CreatedDateDisplay { get; set; }
        public bool DeleteStatus { get; set; }
        public long? CreatedBy { get; set; }
        public long? ModifiedBy { get; set; }
        public long? SystemUser { get; set; }
        public long? TicketUser { get; set; }
        
            
    }

    [Table("TicketReplyDetails")]
    public class TicketReplyDetailsModel
    {
        [Key]
        public long TicketReplyDetailsId { get; set; }
        public string Message { get; set; }
        public long? TicketReplyId { get; set; }
        public string Note { get; set; }
        
    }
}