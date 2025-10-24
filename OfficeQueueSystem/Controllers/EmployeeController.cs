using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Database;
using TicketService.DTO;
using Microsoft.AspNetCore.Authorization;

namespace OfficeQueueSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class EmployeeController : SecureBaseController
    {
        private readonly TicketDbContext _context;

        public EmployeeController(TicketDbContext context)
        {
            _context = context;
        }

        // IMPIEGATO: Lista servizi che può gestire
        [HttpGet("services")]
        //[Authorize(Roles = "employee,admin")]
        public async Task<ActionResult<List<ServiceTypes>>> GetEmployeeServices()
        {
            return await _context.ServiceTypes.ToListAsync();
        }

        // IMPIEGATO: Chiamare prossimo cliente
        [HttpPost("call-next")]
        //[Authorize(Roles = "employee,admin")]
        public async Task<ActionResult<Tickets>> CallNextCustomer()
        {
            var nextTicket = await _context.Tickets
                .Where(t => !t.IsServed && t.CalledAt == null)
                .OrderBy(t => t.CreatedAt)
                .FirstOrDefaultAsync();

            if (nextTicket == null)
                return NotFound("Nessun ticket in attesa");

            nextTicket.CalledAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Ok(nextTicket);
        }

        // IMPIEGATO: Vedere coda per servizio specifico
        [HttpGet("queue/{serviceType}")]
        //[Authorize(Roles = "employee,admin")]
        public async Task<ActionResult<List<Tickets>>> GetQueueByService(string serviceType)
        {
            var tickets = await _context.Tickets
                .Where(t => t.ServiceType == serviceType && !t.IsServed)
                .OrderBy(t => t.CreatedAt)
                .ToListAsync();

            return tickets;
        }

        // IMPIEGATO: Statistiche personali
        [HttpGet("statistics")]
        //[Authorize(Roles = "employee,admin")]
        public async Task<ActionResult<EmployeeStatistics>> GetEmployeeStatistics()
        {
            var servedToday = await _context.Tickets
                .CountAsync(t => t.ServedAt != null &&
                                t.ServedAt.Value.Date == DateTime.Today);

            return new EmployeeStatistics
            {
                TicketsServedToday = servedToday,
                CurrentDate = DateTime.Today
            };
        }
    }

    public class EmployeeStatistics
    {
        public int TicketsServedToday { get; set; }
        public DateTime CurrentDate { get; set; }
    }
}