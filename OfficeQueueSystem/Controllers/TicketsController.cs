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
    public class TicketsController : SecureBaseController
    {
        private readonly TicketDbContext _context;

        public TicketsController(TicketDbContext context)
        {
            _context = context;
        }

        // CLIENTE: Creare ticket - accesso pubblico
        [HttpPost]
        public async Task<ActionResult<Tickets>> CreateTicket([FromBody] TicketRequest request)
        {
            var serviceType = await _context.ServiceTypes
                .FirstOrDefaultAsync(st => st.Tag == request.ServiceType);

            if (serviceType == null)
                return BadRequest("Tipo di servizio non valido");

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

            return Ok(ticket);
        }

        // CLIENTE: Vedere ticket - autenticazione richiesta
        [HttpGet("{id}")]
        //[Authorize] // ✅ Solo utenti autenticati (customer, employee, admin)
        public async Task<ActionResult<Tickets>> GetTicket(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
                return NotFound();

            return ticket;
        }

        // CLIENTE: Tempo di attesa - accesso pubblico
        [HttpGet("wait-time/{ticketCode}")]
        public async Task<ActionResult<WaitTimeResponse>> GetEstimatedWaitTime(string ticketCode)
        {
            var ticket = await _context.Tickets
                .FirstOrDefaultAsync(t => t.TicketCode == ticketCode);

            if (ticket == null)
                return NotFound();

            var waitTime = await CalculateEstimatedWaitTime(ticket.ServiceType);

            return new WaitTimeResponse
            {
                TicketCode = ticketCode,
                EstimatedWaitMinutes = waitTime
            };
        }

        // CLIENTE: Stato ticket - accesso pubblico
        [HttpGet("{ticketCode}/status")]
        public async Task<ActionResult<TicketStatusResponse>> GetTicketStatus(string ticketCode)
        {
            var ticket = await _context.Tickets
                .FirstOrDefaultAsync(t => t.TicketCode == ticketCode);

            if (ticket == null)
                return NotFound("Ticket non trovato");

            var positionInQueue = await CalculateQueuePosition(ticket.ServiceType, ticket.Id);
            var estimatedWaitTime = await CalculateEstimatedWaitTime(ticket.ServiceType);

            return new TicketStatusResponse
            {
                TicketCode = ticket.TicketCode,
                ServiceType = ticket.ServiceType,
                PositionInQueue = positionInQueue,
                EstimatedWaitMinutes = estimatedWaitTime,
                Status = ticket.IsServed ? "Servito" :
                         ticket.CalledAt != null ? "Chiamato" : "In attesa",
                CreatedAt = ticket.CreatedAt,
                CalledAt = ticket.CalledAt,
                ServedAt = ticket.ServedAt
            };
        }

        // IMPIEGATO: Segnare ticket come servito
        [HttpPut("{id}/serve")]
        //[Authorize(Roles = "employee,admin")] // ✅ Solo impiegati e admin
        public async Task<IActionResult> MarkAsServed(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
                return NotFound();

            ticket.IsServed = true;
            ticket.ServedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Ok();
        }

        // IMPIEGATO/ADMIN: Vedere tutte le code
        [HttpGet("queue")]
        //[Authorize(Roles = "employee,admin")] // ✅ Solo impiegati e admin
        public async Task<ActionResult<List<Tickets>>> GetQueue()
        {
            var tickets = await _context.Tickets
                .Where(t => !t.IsServed)
                .OrderBy(t => t.CreatedAt)
                .ToListAsync();

            return tickets;
        }

        private string GenerateTicketCode(string serviceType)
        {
            var date = DateTime.Now.ToString("yyyyMMdd");
            var random = new Random();
            return $"{serviceType}-{date}-{random.Next(1000, 9999)}";
        }

        private async Task<int> CalculateEstimatedWaitTime(string serviceType)
        {
            var ticketsInQueue = await _context.Tickets
                .CountAsync(t => t.ServiceType == serviceType && !t.IsServed);

            var service = await _context.ServiceTypes
                .FirstAsync(st => st.Tag == serviceType);

            return ticketsInQueue * service.AverageServiceTimeMinutes;
        }

        private async Task<int> CalculateQueuePosition(string serviceType, int ticketId)
        {
            var ticketsBeforeMe = await _context.Tickets
                .Where(t => t.ServiceType == serviceType &&
                           !t.IsServed &&
                           t.CalledAt == null &&
                           t.Id < ticketId)
                .CountAsync();

            return ticketsBeforeMe + 1;
        }
    }
}