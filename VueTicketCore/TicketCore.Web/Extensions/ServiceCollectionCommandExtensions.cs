using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TicketCore.Core;
using TicketCore.Data.ApplicationLog.Command;
using TicketCore.Data.AssignmentLoad.Command;
using TicketCore.Data.Audit.Command;
using TicketCore.Data.BusinessHours.Command;
using TicketCore.Data.Department.Command;
using TicketCore.Data.EmailVerification.Command;
using TicketCore.Data.Knowledgebase.Command;
using TicketCore.Data.Masters.Command;
using TicketCore.Data.MenuCategorys.Command;
using TicketCore.Data.MenuMasters.Command;
using TicketCore.Data.Notices.Command;
using TicketCore.Data.Notifications.Command;
using TicketCore.Data.Profiles.Command;
using TicketCore.Data.Rolemasters.Command;
using TicketCore.Data.SmtpEmailSetting.Command;
using TicketCore.Data.Tickets.Command;
using TicketCore.Data.Usermaster.Command;
using TicketCore.Services.MailHelper;

namespace TicketCore.Web.Extensions
{
    public static partial class ServiceCollectionCommandExtensions
    {
        public static IServiceCollection AddCommandServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IRoleCommand, RoleCommand>();
            services.AddScoped<IVerificationCommand, VerificationCommand>();
            services.AddScoped<IVerificationCommand, VerificationCommand>();
            services.AddScoped<IProfileCommand, ProfileCommand>();
            services.AddScoped<IUserMasterCommand, UserMasterCommand>();
            services.AddScoped<IMenuCategoryCommand, MenuCategoryCommand>();
            services.AddScoped<IMenuMasterCommand, MenuMasterCommand>();
            services.AddScoped<ISubMenuMasterCommand, SubMenuMasterCommand>();
            services.AddScoped<IOrderingCommand, OrderingCommand>();
            services.AddScoped<IDepartmentCommand, DepartmentCommand>();
            services.AddScoped<ISmtpSettingsCommand, SmtpSettingsCommand>();
            services.AddScoped<IGeneralSettingsCommand, GeneralSettingsCommand>();
            services.AddScoped<IEmailLogCommand, EmailLogCommand>();
            services.AddScoped<IBusinessHoursCommand, BusinessHoursCommand>();
            services.AddScoped<IDepartmentConfigrationCommand, DepartmentConfigrationCommand>();
            services.AddScoped<IHolidayCommand, HolidayCommand>();
            services.AddScoped<ISlaPoliciesCommand, SlaPoliciesCommand>();
            services.AddScoped<IAssignmentLoadCommand, AssignmentLoadCommand>();
            services.AddScoped<ITicketHistoryHelper, TicketHistoryHelper>();
            services.AddScoped<IGenerateTicketNo, GenerateTicketNo>();
            services.AddScoped<ITicketCommand, TicketCommand>();
            services.AddScoped<ITicketHistoryCommand, TicketHistoryCommand>();
            services.AddScoped<IKnowledgebaseCommand, KnowledgebaseCommand>();
            services.AddScoped<ITicketNotificationCommand, TicketNotificationCommand>();
            services.AddScoped<IVerificationCommand, VerificationCommand>();
            services.AddScoped<IAuditCommand, AuditCommand>();
            services.AddScoped<IApplicationMailingService, ApplicationMailingService>();
            services.AddScoped<IConfigureJobsCommand, ConfigureJobsCommand>();
            services.AddScoped<INoticeCommand, NoticeCommand>();
            

            return services;
        }
    }
}