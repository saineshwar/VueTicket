using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using TicketCore.Models.Knowledgebase;
using TicketCore.ViewModels.Knowledgebase;

namespace TicketCore.Data.Knowledgebase.Queries
{
    public interface IKnowledgebaseQueries
    {
        IQueryable<KnowledgebaseGridViewModel> ShowAllKnowledgebase(string sortColumn, string sortColumnDir,
            string search);

        EditKnowledgebaseViewModel GetKnowledgeBaseDetailsbyId(long KnowledgebaseId);
        List<KnowledgebaseAttachments> GetListAttachmentsByKnowledgebaseId(long? KnowledgebaseId);
        bool KnowledgebaseAttachmentsExistbyknowledgebaseId(long? knowledgebaseId);
        KnowledgebaseAttachments GetAttachments(long knowledgebaseId, long knowledgebaseAttachmentsId);

        KnowledgebaseAttachmentsDetails GetAttachmentDetailsByAttachmentId(long knowledgebaseId,
            long knowledgebaseAttachmentsId);

        List<KnowledgeSearch> SearchKnowledgebasebydepartmentId(string departmentId);
        List<KnowledgeSearch> SearchKnowledgebase(string search);
        KnowledgebaseArticleViewModel GetKnowledgebaseDetailsForArticle(long knowledgebaseId);
        List<AllKnowledgebaseViewModel> AllKnowledgebase();
    }
}