
using TicketCore.Models.Notices;
using Microsoft.EntityFrameworkCore;

namespace TicketCore.Data.Notices.Command
{
    public class NoticeCommand : INoticeCommand
    {
        private readonly VueTicketDbContext _vueTicketDbContext;
        public NoticeCommand(VueTicketDbContext vueTicketDbContext)
        {
            _vueTicketDbContext = vueTicketDbContext;
        }

        public int AddNotice(Notice notice, NoticeDetails noticeDetails)
        {
            _vueTicketDbContext.Notice.Add(notice);
            _vueTicketDbContext.NoticeDetails.Add(noticeDetails);
            return _vueTicketDbContext.SaveChanges();
        }

        public int UpdateNotice(Notice notice, NoticeDetails noticeDetails)
        {
            _vueTicketDbContext.Entry(notice).State = EntityState.Modified;
            _vueTicketDbContext.Entry(noticeDetails).State = EntityState.Modified;
            return _vueTicketDbContext.SaveChanges();
        }

        public int DeleteNotice(Notice notice, NoticeDetails noticeDetails)
        {
            _vueTicketDbContext.Entry(notice).State = EntityState.Deleted;
            _vueTicketDbContext.Entry(notice).State = EntityState.Deleted;
            return _vueTicketDbContext.SaveChanges();
        }
    }
}