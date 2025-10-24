using Microsoft.EntityFrameworkCore;
using Database;
using TicketService.DTO;
using Repository.Interface;

namespace Repository.Service
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly TicketDbContext _context;

        public EmployeeRepository(TicketDbContext context)
        {
            _context = context;
        }

        public async Task<List<ServiceTypes>> GetEmployeeServicesAsync()
        {
            return await _context.ServiceTypes.ToListAsync();
        }

        public async Task<Tickets?> CallNextCustomerAsync()
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

        public async Task<List<Tickets>> GetQueueByServiceAsync(string serviceType)
        {
            return await _context.Tickets
                .Where(t => t.ServiceType == serviceType && !t.IsServed)
                .OrderBy(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<int> GetTicketsServedTodayAsync()
        {
            var today = DateTime.Today;
            return await _context.Tickets
                .CountAsync(t => t.ServedAt != null &&
                                t.ServedAt.Value.Date == today);
        }
    }
}