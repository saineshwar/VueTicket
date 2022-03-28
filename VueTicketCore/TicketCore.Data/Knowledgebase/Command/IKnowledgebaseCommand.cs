using System.Collections.Generic;
using System.Threading.Tasks;
using TicketCore.Models.Knowledgebase;
using TicketCore.ViewModels.Knowledgebase;
using TicketCore.ViewModels.Tickets;

namespace TicketCore.Data.Knowledgebase.Command
{
    public interface IKnowledgebaseCommand
    {
        Task<bool> AddKnowledgebase(
            KnowledgebaseModel knowledgebase,
            List<KnowledgebaseAttachmentViewModel> listknowledgebaseAttachments,
            KnowledgebaseDetails knowledgebaseDetails
        );

        Task<bool> Update(
            KnowledgebaseModel knowledgebase,
            List<KnowledgebaseAttachmentViewModel> listknowledgebaseAttachments,
            KnowledgebaseDetails knowledgebaseDetails
        );

        Task<bool> DeleteAttachmentByAttachmentId(KnowledgebaseAttachments attachmentsModel,
            KnowledgebaseAttachmentsDetails attachmentDetailsModel);
        Task<bool> UpdateStatus(RequestKnowledgebaseChange requestKnowledgebase, long modifiedBy);
    }
}