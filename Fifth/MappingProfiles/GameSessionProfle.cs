using AutoMapper;
using Fifth.Models;
using Fifth.ViewModels;

namespace Fifth.MappingProfiles
{
    public class GameSessionProfle : Profile
    {
        public GameSessionProfle()
        {
            CreateMap<GameSession, GameSessionVM>();
        }
    }
}