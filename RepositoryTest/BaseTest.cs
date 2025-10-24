using Microsoft.EntityFrameworkCore;
using Database;
using Repository.Interface;
using Repository.Service;

namespace RepositoryTest
{
    public abstract class BaseTest : IDisposable
    {
        protected readonly ITicketRepository _ticketRepository;
        protected readonly IServiceTypeRepository _serviceTypeRepository;
        protected readonly IEmployeeRepository _employeeRepository;
        protected readonly TicketDbContext _context;

        public BaseTest()
        {
            // ✅ CREA OPZIONI PER INMEMORY (SENZA configuration esterna)
            var options = new DbContextOptionsBuilder<TicketDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
                .Options;

            // ✅ CREA IL CONTEXT ISOLATO
            _context = new TicketDbContext(options);

            // ✅ CREA I REPOSITORY DIRETTAMENTE
            _ticketRepository = new TicketRepository(_context);
            _serviceTypeRepository = new ServiceTypeRepository(_context);
            _employeeRepository = new EmployeeRepository(_context);

            // ✅ INIZIALIZZA IL DATABASE
            InitializeTestDatabase();
        }

        private void InitializeTestDatabase()
        {
            try
            {
                // ✅ USA SOLO EnsureCreated
                _context.Database.EnsureCreated();

                // ✅ INIZIALIZZA SERVICE TYPES SOLO SE NECESSARIO
                if (!_context.ServiceTypes.Any())
                {
                    _context.ServiceTypes.AddRange(
                        new ServiceTypes { Id = 1, Tag = "depositi", AverageServiceTimeMinutes = 5 },
                        new ServiceTypes { Id = 2, Tag = "spedizioni", AverageServiceTimeMinutes = 10 },
                        new ServiceTypes { Id = 3, Tag = "conti", AverageServiceTimeMinutes = 15 }
                    );
                    _context.SaveChanges();
                }

                // ✅ INIZIALIZZA TICKETS DI TEST
                if (!_context.Tickets.Any())
                {
                    _context.Tickets.AddRange(
                        new Tickets
                        {
                            Id = 1,
                            TicketCode = "depositi-20241022-1234",
                            ServiceType = "depositi",
                            CreatedAt = DateTime.UtcNow.AddMinutes(-30),
                            IsServed = false
                        },
                        new Tickets
                        {
                            Id = 2,
                            TicketCode = "spedizioni-20241022-5678",
                            ServiceType = "spedizioni",
                            CreatedAt = DateTime.UtcNow.AddMinutes(-15),
                            IsServed = false
                        }
                    );
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Errore inizializzazione database test: {ex.Message}", ex);
            }
        }

        // ✅ METODO PER PULIRE TABELLE SPECIFICHE
        protected async Task CleanTableAsync<T>() where T : class
        {
            var entities = _context.Set<T>().ToList();
            if (entities.Any())
            {
                _context.Set<T>().RemoveRange(entities);
                await _context.SaveChangesAsync();
            }
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}