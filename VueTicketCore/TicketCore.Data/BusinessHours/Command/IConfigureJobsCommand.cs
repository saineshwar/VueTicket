using TicketCore.Models.CategoryConfigrations;

namespace TicketCore.Data.BusinessHours.Command
{
    public interface IConfigureJobsCommand
    {
        bool Save(ConfigureJobModel ConfigureJobModel);
        int Update(ConfigureJobModel ConfigureJobModel);
    }
}