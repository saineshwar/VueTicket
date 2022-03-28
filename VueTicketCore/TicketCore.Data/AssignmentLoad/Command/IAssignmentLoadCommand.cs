using TicketCore.Models.AssignmentLoad;

namespace TicketCore.Data.AssignmentLoad.Command
{
    public interface IAssignmentLoadCommand
    {
        int? AddDefaultTicketCount(DefaultTicketSettings defaultTicket);
    }
}