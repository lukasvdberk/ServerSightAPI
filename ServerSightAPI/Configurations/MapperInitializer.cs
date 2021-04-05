using AutoMapper;
using ServerSightAPI.DTO.ApiKey;
using ServerSightAPI.DTO.Server;
using ServerSightAPI.DTO.Server.HardDiskServer;
using ServerSightAPI.DTO.Server.NetworkAdapterServer;
using ServerSightAPI.DTO.Server.PortServer;
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

            CreateMap<CreateServerDto, Server>().ReverseMap();
            CreateMap<Server, CreateServerDto>().ReverseMap();
            CreateMap<ServerDto, Server>().ReverseMap();
            CreateMap<Server, ServerDto>().ReverseMap();

            CreateMap<UpdateServerDto, Server>().ReverseMap();
            CreateMap<Server, UpdateServerDto>().ReverseMap();

            CreateMap<ApiKey, ApiKeyDto>().ReverseMap();
            CreateMap<ApiKeyDto, ApiKey>().ReverseMap();

            CreateMap<NetworkAdapterServer, CreateNetworkAdapterServerDto>().ReverseMap();
            CreateMap<CreateNetworkAdapterServerDto, NetworkAdapterServer>().ReverseMap();

            CreateMap<NetworkAdapterServer, NetworkAdapterServerDto>().ReverseMap();
            CreateMap<NetworkAdapterServerDto, NetworkAdapterServer>().ReverseMap();

            CreateMap<CreateHardDiskServer, HardDiskServer>().ReverseMap();
            CreateMap<HardDiskServer, CreateHardDiskServer>().ReverseMap();

            CreateMap<PortServer, CreatePortServerDto>().ReverseMap();
            CreateMap<CreatePortServerDto, PortServer>().ReverseMap();
            
            CreateMap<CpuUsageDto, CpuUsageServer>().ReverseMap();
            CreateMap<CpuUsageServer, CpuUsageDto>().ReverseMap();
            
            CreateMap<RamUsage, CreateRamUsageDto>().ReverseMap();
            CreateMap<CreateRamUsageDto, RamUsage>().ReverseMap();
            
            CreateMap<RamUsage, RamUsageDto>().ReverseMap();
            CreateMap<RamUsageDto, RamUsage>().ReverseMap();
            
            CreateMap<CpuUsageServer, CpuUsageDto>().ReverseMap();
            CreateMap<CpuUsageDto, CpuUsageServer>().ReverseMap();
        }
    }
}