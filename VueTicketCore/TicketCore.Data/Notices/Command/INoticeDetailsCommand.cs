using TicketCore.Models.Notices;

namespace TicketCore.Data.Notices.Command
{
    public interface INoticeDetailsCommand
    {
        void AddNoticeDetails(NoticeDetails noticeDetails);
        void UpdateNoticeDetails(NoticeDetails noticeDetails);
        void DeleteNoticeDetails(NoticeDetails noticeDetails);
    }
}