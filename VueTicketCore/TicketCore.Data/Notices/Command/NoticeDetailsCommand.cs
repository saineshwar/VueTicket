
using Microsoft.EntityFrameworkCore;
using TicketCore.Data;
using TicketCore.Data.Notices.Command;
using TicketCore.Models.Notices;

namespace TicketCore.Data.Notices.Command
{
    public class NoticeDetailsCommand : INoticeDetailsCommand
    {
        private readonly VueTicketDbContext _vueTicketDbContext;
        public NoticeDetailsCommand(VueTicketDbContext vueTicketDbContext)
        {
            _vueTicketDbContext = vueTicketDbContext;
        }

        public void AddNoticeDetails(NoticeDetails noticeDetails)
        {
            _vueTicketDbContext.NoticeDetails.Add(noticeDetails);
        }

        public void UpdateNoticeDetails(NoticeDetails noticeDetails)
        {
            _vueTicketDbContext.Entry(noticeDetails).State = EntityState.Modified;
        }

        public void DeleteNoticeDetails(NoticeDetails noticeDetails)
        {
            _vueTicketDbContext.Entry(noticeDetails).State = EntityState.Deleted;
        }
    }
}