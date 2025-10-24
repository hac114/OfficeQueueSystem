using TicketService.DTO;
using Database;

namespace Repository.Interface
{
    public interface IEmployeeRepository
    {
        Task<List<ServiceTypes>> GetEmployeeServicesAsync();
        Task<Tickets?> CallNextCustomerAsync();
        Task<List<Tickets>> GetQueueByServiceAsync(string serviceType);
        Task<int> GetTicketsServedTodayAsync();
    }
}