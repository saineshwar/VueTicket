using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TicketCore.Data.AssignmentLoad.Queries;
using TicketCore.Data.Audit.Queries;
using TicketCore.Data.BusinessHours.Queries;
using TicketCore.Data.Dashboard.Queries;
using TicketCore.Data.Department.Queries;
using TicketCore.Data.EmailVerification.Queries;
using TicketCore.Data.Knowledgebase.Queries;
using TicketCore.Data.Masters.Queries;
using TicketCore.Data.MenuCategorys.Queries;
using TicketCore.Data.MenuMasters.Queries;
using TicketCore.Data.Notices.Queries;
using TicketCore.Data.Notifications.Queries;
using TicketCore.Data.Profiles.Queries;
using TicketCore.Data.Reports.Queries;
using TicketCore.Data.Rolemasters.Queries;
using TicketCore.Data.SmtpEmailSetting.Queries;
using TicketCore.Data.Tickets.Queries;
using TicketCore.Data.Usermaster.Queries;
using TicketCore.Web.Messages;

namespace TicketCore.Web.Extensions
{
    public static partial class ServiceCollectionQueriesExtensions
    {
        public static IServiceCollection AddServicesQueries(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IRoleQueries, RoleQueries>();
            services.AddScoped<IUserMasterQueries, UserMasterQueries>();
       
            services.AddScoped<IMenuCategoryQueries, MenuCategoryQueries>();
            services.AddScoped<IMenuMasterQueries, MenuMasterQueries>();
            services.AddScoped<INoticeQueries, NoticeQueries>();
            services.AddScoped<IVerificationQueries, VerificationQueries>();
            services.AddScoped<IProfileQueries, ProfileQueries>();
            services.AddScoped<ISubMenuMasterQueries, SubMenuMasterQueries>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IDepartmentQueries, DepartmentQueries>();
            services.AddScoped<ISmtpSettingsQueries, SmtpSettingsQueries>();
            services.AddScoped<IGeneralSettingsQueries, GeneralSettingsQueries>();
            services.AddScoped<IBusinessHoursQueries, BusinessHoursQueries>();
            services.AddScoped<IDepartmentConfigrationQueries, DepartmentConfigrationQueries>();
            services.AddScoped<IHolidayQueries, HolidayQueries>();
            services.AddScoped<IPriorityQueries, PriorityQueries>();
            services.AddScoped<ISlaPoliciesQueries, SlaPoliciesQueries>();
            services.AddScoped<IAssignmentloadQueries, AssignmentloadQueries>();
            services.AddScoped<ITicketNumberGeneratorQueries, TicketNumberGeneratorQueries>();
            services.AddScoped<ITicketQueries, TicketQueries>();
            services.AddScoped<ICheckInStatusQueries, CheckInStatusQueries>();
            services.AddScoped<IStatusQueries, StatusQueries>();
            services.AddScoped<ITicketViewQueries, TicketViewQueries>();
            services.AddScoped<ITicketsReplyQueries, TicketsReplyQueries>();
            services.AddScoped<IDashboardQueries, DashboardQueries>();
            services.AddScoped<IKnowledgebaseQueries, KnowledgebaseQueries>();
            services.AddScoped<IKnowledgebaseTypeQueries, KnowledgebaseTypeQueries>();
            services.AddScoped<IReportQueries, ReportQueries>();
            services.AddScoped<ITicketsMastersQueries, TicketsMastersQueries>();
            services.AddScoped<ITicketNotificationQueries, TicketNotificationQueries>();
            services.AddScoped<IAuditQueries, AuditQueries>();
            services.AddScoped<IChartsQueries, ChartsQueries>();
            services.AddScoped<IConfigureJobsQueries, ConfigureJobsQueries>();
            


            return services;
        }
    }
}