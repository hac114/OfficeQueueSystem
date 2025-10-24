using Microsoft.EntityFrameworkCore;
using Database;
using TicketService.DTO;
using Repository.Interface;

namespace Repository.Service
{
    public class ServiceTypeRepository : IServiceTypeRepository
    {
        private readonly TicketDbContext _context;

        public ServiceTypeRepository(TicketDbContext context)
        {
            _context = context;
        }

        public async Task<List<ServiceTypes>> GetAllAsync()
        {
            return await _context.ServiceTypes.ToListAsync();
        }

        public async Task<ServiceTypes?> GetByIdAsync(int id)
        {
            return await _context.ServiceTypes.FindAsync(id);
        }

        public async Task<ServiceTypes> CreateAsync(ServiceTypeCreateRequest request)
        {
            var serviceType = new ServiceTypes
            {
                Tag = request.Tag,
                AverageServiceTimeMinutes = request.AverageServiceTimeMinutes
            };

            _context.ServiceTypes.Add(serviceType);
            await _context.SaveChangesAsync();
            return serviceType;
        }

        public async Task<ServiceTypes?> UpdateAsync(ServiceTypes serviceType)
        {
            _context.Entry(serviceType).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return serviceType;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var serviceType = await _context.ServiceTypes.FindAsync(id);
            if (serviceType == null)
                return false;

            _context.ServiceTypes.Remove(serviceType);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}