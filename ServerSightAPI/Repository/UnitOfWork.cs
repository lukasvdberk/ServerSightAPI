using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ServerSightAPI.Configurations;
using ServerSightAPI.Models.Server;

namespace ServerSightAPI.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _context;
        private IGenericRepository<Server> _server;
        
        public UnitOfWork(DatabaseContext context)
        {
            _context = context;
        }
        // ??= is a shortcut for set if it does not exist
        public IGenericRepository<Server> Servers => _server ??= new GenericRepository<Server>(_context);
        
        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(true);
        }
    }
}