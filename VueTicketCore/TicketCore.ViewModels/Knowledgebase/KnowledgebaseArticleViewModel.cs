using System.Collections.Generic;
using System.ComponentModel;
using TicketCore.Models.Knowledgebase;

namespace TicketCore.ViewModels.Knowledgebase
{
    public class KnowledgebaseArticleViewModel
    {
        public long? KnowledgebaseId { get; set; }

        [DisplayName("Subject")]
        public string Subject { get; set; }

        [DisplayName("Department")]
        public string DepartmentName { get; set; }

        [DisplayName("Type")]
        public string KnowledgebaseTypeName { get; set; }

        [DisplayName("Content")]
        public string Contents { get; set; }
        [DisplayName("Keywords")]
        public string Keywords { get; set; }

        [DisplayName("Status")]
        public bool Status { get; set; }
        public List<KnowledgebaseAttachments> ListofAttachments { get; set; }
    }
}