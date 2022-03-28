namespace TicketCore.Core
{
    public interface IGenerateTicketNo
    {
        string ApplicationNo(out int runningTicketno);
    }
}