using TicketCore.Models.Notices;

namespace TicketCore.Data.Notices.Command
{

    public interface INoticeCommand
    {
        int AddNotice(Notice notice, NoticeDetails noticeDetails);
        int UpdateNotice(Notice notice, NoticeDetails noticeDetails);
        int DeleteNotice(Notice notice, NoticeDetails noticeDetails);
    }
}