using CleanArchitecture.Core.Interfaces;

namespace CleanArchitecture.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        readonly CleanArchitectureContext _context;

        public UnitOfWork(CleanArchitectureContext context)
        {
            _context = context;
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
    }
}
