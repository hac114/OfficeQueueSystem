using Database;  // Per ServiceTypes
using TicketService.DTO;  // Per ServiceTypeCreateRequest

namespace Repository.Interface
{
    public interface IServiceTypeRepository
    {
        Task<List<ServiceTypes>> GetAllAsync();
        Task<ServiceTypes?> GetByIdAsync(int id);
        Task<ServiceTypes> CreateAsync(ServiceTypeCreateRequest request);
        Task<ServiceTypes?> UpdateAsync(ServiceTypes serviceType);
        Task<bool> DeleteAsync(int id);
    }
}