using AutoMapper;
using Fifth.Models;
using Fifth.ViewModels;

namespace Fifth.MappingProfiles
{
    public class GameSessionProfle : Profile
    {
        public GameSessionProfle()
        {
            CreateMap<GameData, GameSessionVM>().ForMember(vm => vm.UserName, opt => opt.MapFrom(m=>m.Creator.Login));
        }
    }
}