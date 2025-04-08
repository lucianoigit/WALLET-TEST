using APPLICATION.Exceptions;
using APPLICATION.Abstractions.Clock;
using APPLICATION.Abstractions.Data;
using DOMAIN.Movements;
using DOMAIN.Wallets;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace INFRAESTRUCTURE
{
    public sealed class ApplicationDbContext : DbContext, IUnitOfWork
    {
        private static readonly JsonSerializerSettings JsonSerializerSettings = new()
        {
            TypeNameHandling = TypeNameHandling.All,
        };

        private readonly IDateTimeProvider _dateTimeProvider;


        public ApplicationDbContext(
            DbContextOptions options,
            IDateTimeProvider dateTimeProvider)
            : base(options)
        {
            _dateTimeProvider = dateTimeProvider;
        }


        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Movement> Movements { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            base.OnModelCreating(modelBuilder);

        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {

                int result = await base.SaveChangesAsync(cancellationToken);

                return result;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new ConcurrencyException("Concurrency exception occurred.", ex);
            }
        }
    }

}
