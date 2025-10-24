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
    public class ServiceTypesController : SecureBaseController
    {
        private readonly TicketDbContext _context;

        public ServiceTypesController(TicketDbContext context)
        {
            _context = context;
        }

        // PUBBLICO: Lista tutti i servizi disponibili
        [HttpGet]
        public async Task<ActionResult<List<ServiceTypes>>> GetServiceTypes()
        {
            return await _context.ServiceTypes.ToListAsync();
        }

        // PUBBLICO: Dettaglio singolo servizio
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceTypes>> GetServiceType(int id)
        {
            var serviceType = await _context.ServiceTypes.FindAsync(id);
            if (serviceType == null)
                return NotFound();

            return serviceType;
        }

        // ADMIN: Creare nuovo servizio
        [HttpPost]
        //[Authorize(Roles = "admin")]
        public async Task<ActionResult<ServiceTypes>> CreateServiceType([FromBody] ServiceTypeCreateRequest request)
        {
            var serviceType = new ServiceTypes
            {
                Tag = request.Tag,
                AverageServiceTimeMinutes = request.AverageServiceTimeMinutes
            };

            _context.ServiceTypes.Add(serviceType);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetServiceType), new { id = serviceType.Id }, serviceType);
        }

        // ADMIN: Aggiornare servizio
        [HttpPut("{id}")]
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateServiceType(int id, [FromBody] ServiceTypes serviceType)
        {
            if (id != serviceType.Id)
                return BadRequest();

            _context.Entry(serviceType).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ADMIN: Eliminare servizio
        [HttpDelete("{id}")]
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteServiceType(int id)
        {
            var serviceType = await _context.ServiceTypes.FindAsync(id);
            if (serviceType == null)
                return NotFound();

            _context.ServiceTypes.Remove(serviceType);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}