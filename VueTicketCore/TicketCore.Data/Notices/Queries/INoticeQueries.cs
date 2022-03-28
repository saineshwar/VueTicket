using System.Linq;
using TicketCore.Models.Notices;
using TicketCore.ViewModels.Notices;

namespace TicketCore.Data.Notices.Queries
{
    public interface INoticeQueries
    {
        NoticeDisplayViewModel ShowNotice();
        bool ValidateNotice(string fromdatetime);
        IQueryable<NoticeGrid> ShowAllNotice(string sortColumn, string sortColumnDir, string search);
        EditNoticeViewModel GetNoticeDetailsForEdit(int? noticeId);
        Notice GetNoticeByNoticeId(int? noticeId);
        NoticeDetails GetNoticeDetailsByNoticeId(int? noticeId);
    }
}