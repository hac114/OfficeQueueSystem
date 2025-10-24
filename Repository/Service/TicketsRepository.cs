using Microsoft.EntityFrameworkCore;
using Database;
using TicketService.DTO;
using Repository.Interface;

namespace Repository.Service
{
    public class TicketRepository : ITicketRepository
    {
        private readonly TicketDbContext _context;

        public TicketRepository(TicketDbContext context)
        {
            _context = context;
        }

        public async Task<Tickets> CreateAsync(TicketRequest request)
        {
            var ticketCode = GenerateTicketCode(request.ServiceType);

            var ticket = new Tickets
            {
                TicketCode = ticketCode,
                ServiceType = request.ServiceType,
                CreatedAt = DateTime.UtcNow,
                IsServed = false
            };

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();
            return ticket;
        }

        public async Task<Tickets?> GetByIdAsync(int id)
        {
            return await _context.Tickets.FindAsync(id);
        }

        public async Task<Tickets?> GetByCodeAsync(string ticketCode)
        {
            return await _context.Tickets
                .FirstOrDefaultAsync(t => t.TicketCode == ticketCode);
        }

        public async Task<List<Tickets>> GetQueueAsync()
        {
            return await _context.Tickets
                .Where(t => !t.IsServed)
                .OrderBy(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Tickets>> GetQueueByServiceAsync(string serviceType)
        {
            return await _context.Tickets
                .Where(t => t.ServiceType == serviceType && !t.IsServed)
                .OrderBy(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<Tickets?> CallNextAsync()
        {
            var nextTicket = await _context.Tickets
                .Where(t => !t.IsServed && t.CalledAt == null)
                .OrderBy(t => t.CreatedAt)
                .FirstOrDefaultAsync();

            if (nextTicket != null)
            {
                nextTicket.CalledAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }

            return nextTicket;
        }

        public async Task<Tickets?> MarkAsServedAsync(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                ticket.IsServed = true;
                ticket.ServedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
            return ticket;
        }

        public async Task<int> GetQueuePositionAsync(string serviceType, int ticketId)
        {
            var ticketsBeforeMe = await _context.Tickets
                .Where(t => t.ServiceType == serviceType &&
                           !t.IsServed &&
                           t.CalledAt == null &&
                           t.Id < ticketId)
                .CountAsync();

            return ticketsBeforeMe + 1;
        }

        public async Task<int> GetEstimatedWaitTimeAsync(string serviceType)
        {
            var ticketsInQueue = await _context.Tickets
                .CountAsync(t => t.ServiceType == serviceType && !t.IsServed);

            var service = await _context.ServiceTypes
                .FirstAsync(st => st.Tag == serviceType);

            return ticketsInQueue * service.AverageServiceTimeMinutes;
        }

        public async Task<int> GetTicketsServedTodayAsync()
        {
            var today = DateTime.Today;
            return await _context.Tickets
                .CountAsync(t => t.ServedAt != null &&
                                t.ServedAt.Value.Date == today);
        }

        private string GenerateTicketCode(string serviceType)
        {
            var date = DateTime.Now.ToString("yyyyMMdd");
            var random = new Random();
            return $"{serviceType}-{date}-{random.Next(1000, 9999)}";
        }
    }
}