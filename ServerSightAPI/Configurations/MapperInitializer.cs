using AutoMapper;
using ServerSightAPI.DTO.User;
using ServerSightAPI.Models;

namespace ServerSightAPI.Configurations
{
    public class MapperInitializer : Profile
    {
        public MapperInitializer()
        {
            // TODO setup mapping.
            CreateMap<User, UserDTO>().ReverseMap();
        }
    }
}