using AutoMapper;
using Fifth.Models;
using Fifth.ViewModels;

namespace Fifth.MappingProfiles
{
    public class SessionDataProfile : Profile
    {
        public SessionDataProfile()
        {
            CreateMap<SessionData, SessionVM>().ForMember(vm => vm.UserName, opt => opt.MapFrom(m=>m.Creator.Login));
        }
    }
}