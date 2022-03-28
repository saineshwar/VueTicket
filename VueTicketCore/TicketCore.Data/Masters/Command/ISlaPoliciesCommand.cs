using TicketCore.Models.Masters;

namespace TicketCore.Data.Masters.Command
{
    public interface ISlaPoliciesCommand
    {
        int? Add(SlaPolicies slaPolicies);
        int Delete(int? slaPoliciesId);
        int? AddReminder(SlaPoliciesReminder slaPoliciesReminder);
    }
}