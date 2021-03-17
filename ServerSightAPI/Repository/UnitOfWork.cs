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
        private IGenericRepository<PortServer> _portServers;
        private IGenericRepository<Server> _server;

        public UnitOfWork(DatabaseContext context)
        {
            _context = context;
        }

        // ??= is a shortcut for set if it does not exist
        public IGenericRepository<Server> Servers => _server ??= new GenericRepository<Server>(_context);
        public IGenericRepository<ApiKey> ApiKeys => _apiKeys ??= new GenericRepository<ApiKey>(_context);

        public IGenericRepository<NetworkAdapterServer> NetworkAdaptersServer =>
            _networkAdapterServer ??= new GenericRepository<NetworkAdapterServer>(_context);

        public IGenericRepository<HardDiskServer> HardDisksServers =>
            _hardDiskServer ??= new GenericRepository<HardDiskServer>(_context);

        public IGenericRepository<PortServer> PortsServer =>
            _portServers ??= new GenericRepository<PortServer>(_context);


        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(true);
        }
    }
}