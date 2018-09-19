using CleanArchitecture.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.SharedKernel;

namespace CleanArchitecture.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        private readonly IDomainEventDispatcher _dispatcher;

        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options, IDomainEventDispatcher dispatcher)
            : base(options)
        {
            _dispatcher = dispatcher;
        }

        public DbSet<ToDoItem> ToDoItems { get; set; }
        public DbSet<MessagingQueue> MessagingQueue { get; set; }
        public DbSet<EmailQueue> EmailQueue { get; set; }
        public DbSet<NotificationQueue> NotificationQueue { get; set; }
        public DbSet<CommAPIServiceMaster> CommAPIServiceMaster { get; set; }
        public DbSet<CommServiceMaster> CommServiceMaster { get; set; }
        public DbSet<CommServiceproviderMaster> CommServiceproviderMaster { get; set; }
        public DbSet<CommServiceTypeMaster> CommServiceTypeMaster { get; set; }
        public DbSet<RequestFormatMaster> RequestFormatMaster { get; set; }
        public DbSet<ServiceTypeMaster> ServiceTypeMaster { get; set; }
        public DbSet<TemplateMaster> TemplateMaster { get; set; }

        public override int SaveChanges()
        {
            int result = base.SaveChanges();

            // dispatch events only if save was successful
            var entitiesWithEvents = ChangeTracker.Entries<BaseEntity>()
                .Select(e => e.Entity)
                .Where(e => e.Events.Any())
                .ToArray();

            foreach (var entity in entitiesWithEvents)
            {
                var events = entity.Events.ToArray();
                entity.Events.Clear();
                foreach (var domainEvent in events)
                {
                    _dispatcher.Dispatch(domainEvent);
                }
            }

            return result;
        }
    }
}