using System;

namespace TicketCore.ViewModels.Knowledgebase
{
    public class KnowledgebaseGridViewModel
    {
        public long KnowledgebaseId { get; set; }
        public string Subject { get; set; }
        public string DepartmentName { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Status { get; set; }
        public string KnowledgebaseTypeName { get; set; }
        public int? DepartmentId { get; set; }
    }
}