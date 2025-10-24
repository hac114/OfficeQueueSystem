namespace TicketService.DTO
{
    public class TicketRequest
    {
        public string ServiceType { get; set; } = string.Empty;
    }

    public class WaitTimeResponse
    {
        public string TicketCode { get; set; } = string.Empty;
        public int EstimatedWaitMinutes { get; set; }
    }

    public class TicketStatusResponse
    {
        public string TicketCode { get; set; } = string.Empty;
        public string ServiceType { get; set; } = string.Empty;
        public int PositionInQueue { get; set; }
        public int EstimatedWaitMinutes { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? CalledAt { get; set; }
        public DateTime? ServedAt { get; set; }
    }
}