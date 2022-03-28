using TicketCore.Models.CategoryConfigrations;
using TicketCore.ViewModels.CategoryConfigrations;

namespace TicketCore.Data.BusinessHours.Queries
{
    public interface IConfigureJobsQueries
    {
        ConfigureJobModel GetConfigureJobDetails();
        int? GetConfigureJobCount();
        ConfigureJobViewModel GetConfigureJobDetailsViewModel();
    }
}