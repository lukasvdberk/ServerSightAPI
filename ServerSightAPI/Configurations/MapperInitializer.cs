using AutoMapper;
using ServerSightAPI.DTO.Server;
using ServerSightAPI.DTO.User;
using ServerSightAPI.Models;
using ServerSightAPI.Models.Server;

namespace ServerSightAPI.Configurations
{
    public class MapperInitializer : Profile
    {
        public MapperInitializer()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            
            // server mapping
            CreateMap<CreateServerDto, Server>().ReverseMap();
            CreateMap<Server, CreateServerDto>().ReverseMap();
            CreateMap<ServerDto, Server>().ReverseMap();
            CreateMap<Server, ServerDto>().ReverseMap();
        }
    }
}