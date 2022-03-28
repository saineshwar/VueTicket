using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketCore.Models.Profiles
{
    [Table("Signatures")]
    public class Signatures
    {
        [Key]
        public int SignatureId { get; set; }
        public string Signature { get; set; }
        public long UserId { get; set; }
    }
}