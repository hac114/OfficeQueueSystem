using Database;  // Riferimento al progetto Database
using TicketService.DTO;  // Riferimento al progetto DTO

namespace Repository.Interface
{
    public interface ITicketRepository
    {
        Task<Tickets> CreateAsync(TicketRequest request);
        Task<Tickets?> GetByIdAsync(int id);
        Task<Tickets?> GetByCodeAsync(string ticketCode);
        Task<List<Tickets>> GetQueueAsync();
        Task<List<Tickets>> GetQueueByServiceAsync(string serviceType);
        Task<Tickets?> CallNextAsync();
        Task<Tickets?> MarkAsServedAsync(int id);
        Task<int> GetQueuePositionAsync(string serviceType, int ticketId);
        Task<int> GetEstimatedWaitTimeAsync(string serviceType);
        Task<int> GetTicketsServedTodayAsync();
    }
}