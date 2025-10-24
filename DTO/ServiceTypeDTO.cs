namespace TicketService.DTO
{
    public class ServiceTypeCreateRequest
    {
        public string Tag { get; set; } = string.Empty;
        public int AverageServiceTimeMinutes { get; set; }
    }

    public class ServiceTypeResponse
    {
        public int Id { get; set; }
        public string Tag { get; set; } = string.Empty;
        public int AverageServiceTimeMinutes { get; set; }
    }
}