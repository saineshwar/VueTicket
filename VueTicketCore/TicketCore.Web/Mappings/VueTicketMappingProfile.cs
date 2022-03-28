using AutoMapper;
using TicketCore.Models.MenuCategorys;
using TicketCore.Models.Menus;
using TicketCore.Models.Notices;
using TicketCore.Models.SmtpEmailSettings;
using TicketCore.Models.Tickets;
using TicketCore.Models.Usermaster;
using TicketCore.ViewModels.MenuCategorys;
using TicketCore.ViewModels.Menus;
using TicketCore.ViewModels.Notices;
using TicketCore.ViewModels.SmtpEmailSettings;
using TicketCore.ViewModels.Tickets;
using TicketCore.ViewModels.Usermaster;

namespace TicketCore.Web.Mappings
{
    public class TicketCoreMappingProfile : Profile
    {
        public TicketCoreMappingProfile()
        {
            CreateMap<CreateUserViewModel, UserMaster>();
          
            CreateMap<UserMaster, EditUserViewModel>();
            CreateMap<CreateMenuCategoryViewModel, MenuCategory>();
            CreateMap<CreateMenuMasterViewModel, MenuMaster>();
            CreateMap<CreateSubMenuMasterViewModel, SubMenuMaster>();
            CreateMap<CreateNoticeViewModel, Notice>();
            CreateMap<EditNoticeViewModel, Notice>();
            CreateMap<SmtpEmailSettingsViewModel, SmtpEmailSettings>()
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.IsDefault, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedOn, opt => opt.Ignore());

            CreateMap<GeneralSettingsViewModel, GeneralSettings>();
            CreateMap<UserMaster, CreateAgentViewModel>();

            CreateMap<TicketsUserViewModel, TicketSummary>()
                .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.DepartmentId))
              
                .ForMember(dest => dest.PriorityId, opt => opt.MapFrom(src => src.PriorityId))
                .ForMember(dest => dest.TrackingId, opt => opt.MapFrom(src => src.TrackingId))
                .ForMember(dest => dest.CreatedOn, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore());

            CreateMap<TicketCommonViewModel, TicketSummary>()
                .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.DepartmentId))

                .ForMember(dest => dest.PriorityId, opt => opt.MapFrom(src => src.PriorityId))
        
                .ForMember(dest => dest.CreatedOn, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore());

            
        }
    }
}