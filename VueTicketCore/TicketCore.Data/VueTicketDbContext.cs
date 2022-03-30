using Microsoft.EntityFrameworkCore;
using TicketCore.Models.AgentCategoryAssigned;
using TicketCore.Models.ApplicationLog;
using TicketCore.Models.AssignmentLoad;
using TicketCore.Models.Audit;
using TicketCore.Models.Business;
using TicketCore.Models.CategoryConfigrations;

using TicketCore.Models.Department;
using TicketCore.Models.Knowledgebase;
using TicketCore.Models.Masters;
using TicketCore.Models.MenuCategorys;
using TicketCore.Models.Menus;
using TicketCore.Models.Notices;
using TicketCore.Models.Profiles;
using TicketCore.Models.Rolemasters;
using TicketCore.Models.SmtpEmailSettings;
using TicketCore.Models.Tickets;
using TicketCore.Models.Usermaster;
using TicketCore.Models.Verification;
using TicketCore.ViewModels.Tickets;

namespace TicketCore.Data
{
    public class VueTicketDbContext : DbContext
    {
        public VueTicketDbContext(DbContextOptions<VueTicketDbContext> options) : base(options)
        {

        }

        public DbSet<UserMaster> UserMasters { get; set; }
        public DbSet<AssignedRoles> AssignedRoles { get; set; }
        public DbSet<RoleMaster> RoleMasters { get; set; }
        public DbSet<MenuCategory> MenuCategorys { get; set; }
        public DbSet<MenuMaster> MenuMasters { get; set; }
        public DbSet<SubMenuMaster> SubMenuMasters { get; set; }
        public DbSet<ResetPasswordVerification> ResetPasswordVerification { get; set; }
        public DbSet<Notice> Notice { get; set; }
        public DbSet<NoticeDetails> NoticeDetails { get; set; }
        public DbSet<DepartmentConfigration> DepartmentConfigration { get; set; }
        public DbSet<AgentDepartmentAssigned> AgentDepartmentAssigned { get; set; }
        public DbSet<ProfileImage> ProfileImage { get; set; }
        public DbSet<ProfileImageStatus> ProfileImageStatus { get; set; }
        public DbSet<Signatures> Signatures { get; set; }
        public DbSet<Departments> Department { get; set; }
        public DbSet<SmtpEmailSettings> SmtpEmailSettings { get; set; }
        public DbSet<GeneralSettings> GeneralSettings { get; set; }
        public DbSet<EmailLogsModel> EmailLogsModel { get; set; }
        public DbSet<BusinessHoursType> BusinessHoursType { get; set; }
        public DbSet<BusinessHoursModel> BusinessHours { get; set; }
        public DbSet<BusinessHoursDetails> BusinessHoursDetails { get; set; }
        public DbSet<HolidayListModel> Holidays { get; set; }
        public DbSet<SlaPolicies> SlaPolicies { get; set; }
        public DbSet<Priority> Priority { get; set; }
        public DbSet<DefaultTicketSettings> DefaultTicketSettings { get; set; }
        public DbSet<SlaPoliciesReminder> SlaPoliciesReminder { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<TicketStatus> TicketStatus { get; set; }
        public DbSet<TicketSummary> TicketSummary { get; set; }
        public DbSet<TicketDetails> TicketDetails { get; set; }
        public DbSet<TicketHistoryModel> TicketHistory { get; set; }
        public DbSet<AttachmentsModel> AttachmentsModel { get; set; }
        public DbSet<AttachmentDetailsModel> AttachmentDetailsModel { get; set; }
        public DbSet<AgentCheckInStatusSummary> AgentCheckInStatusSummary { get; set; }

        public DbSet<TicketReplyModel> TicketReply { get; set; }
        public DbSet<TicketReplyDetailsModel> TicketReplyDetails { get; set; }

        public DbSet<ReplyAttachmentModel> ReplyAttachmentModel { get; set; }
        public DbSet<ReplyAttachmentDetailsModel> ReplyAttachmentDetailsModel { get; set; }

        public DbSet<KnowledgebaseModel> Knowledgebase { get; set; }
        public DbSet<KnowledgebaseAttachments> KnowledgebaseAttachments { get; set; }

        public DbSet<KnowledgebaseAttachmentsDetails> KnowledgebaseAttachmentsDetails { get; set; }
        public DbSet<KnowledgebaseDetails> KnowledgebaseDetails { get; set; }
        public DbSet<KnowledgebaseType> KnowledgebaseType { get; set; }

        public DbSet<OverdueTypes> OverdueTypes { get; set; }
        public DbSet<EmailVerificationModel> EmailVerification { get; set; }

        public DbSet<AuditModel> AuditModel { get; set; }
        public DbSet<LatestTicketReplyStatusModel> LatestTicketReplyStatusModel { get; set; }
        public DbSet<ConfigureJobModel> ConfigureJobModel { get; set; }
        
    }
}