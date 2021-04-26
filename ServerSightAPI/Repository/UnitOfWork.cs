using System;
using ServerSightAPI.Configurations;
using ServerSightAPI.Models;
using ServerSightAPI.Models.Server;

namespace ServerSightAPI.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _context;
        private IGenericRepository<ApiKey> _apiKeys;
        private IGenericRepository<HardDiskServer> _hardDiskServer;
        private IGenericRepository<NetworkAdapterServer> _networkAdapterServer;
        private IGenericRepository<NetworkUsage> _networkUsage;
        private IGenericRepository<PortServer> _portServers;
        private IGenericRepository<CpuUsageServer> _cpuUsages;
        private IGenericRepository<RamUsage> _ramUsages;
        private IGenericRepository<Server> _server;
        private IGenericRepository<ServerEvent> _serverEvents;
        private IGenericRepository<FirebaseDevice> _firebaseDevices;


        public UnitOfWork(DatabaseContext context)
        {
            _context = context;
        }

        // ??= is a shortcut for set if it does not exist
        public IGenericRepository<Server> Servers => _server ??= new GenericRepository<Server>(_context);
        public IGenericRepository<ApiKey> ApiKeys => _apiKeys ??= new GenericRepository<ApiKey>(_context);

        public IGenericRepository<NetworkAdapterServer> NetworkAdaptersServer =>
            _networkAdapterServer ??= new GenericRepository<NetworkAdapterServer>(_context);

        public IGenericRepository<NetworkUsage> NetworkUsages =>
            _networkUsage ??= new GenericRepository<NetworkUsage>(_context);
        


        public IGenericRepository<HardDiskServer> HardDisksServers =>
            _hardDiskServer ??= new GenericRepository<HardDiskServer>(_context);

        public IGenericRepository<PortServer> PortsServer =>
            _portServers ??= new GenericRepository<PortServer>(_context);

        public IGenericRepository<CpuUsageServer> CpuUsagesServers =>
            _cpuUsages ??= new GenericRepository<CpuUsageServer>(_context);

        public IGenericRepository<RamUsage> RAMUsages =>
            _ramUsages ??= new GenericRepository<RamUsage>(_context);

        public IGenericRepository<ServerEvent> ServerEvents =>
            _serverEvents ??= new GenericRepository<ServerEvent>(_context);

        public IGenericRepository<FirebaseDevice> FirebaseDevices =>
            _firebaseDevices ??= new GenericRepository<FirebaseDevice>(_context);


        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(true);
        }
    }
}