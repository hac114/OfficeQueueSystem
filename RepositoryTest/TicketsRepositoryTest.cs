using Database;
using TicketService.DTO;
using Xunit;

namespace RepositoryTest
{
    public class TicketsRepositoryTest : BaseTest
    {
        [Fact]
        public async Task CreateAsync_ValidRequest_ReturnsTicket()
        {
            // Arrange
            var request = new TicketRequest { ServiceType = "depositi" };

            // Act
            var result = await _ticketRepository.CreateAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("depositi", result.ServiceType);
            Assert.False(result.IsServed);
            Assert.NotNull(result.TicketCode);
            Assert.Contains("depositi", result.TicketCode);
        }

        [Fact]
        public async Task GetByIdAsync_ExistingId_ReturnsTicket()
        {
            // Arrange
            var request = new TicketRequest { ServiceType = "depositi" };
            var createdTicket = await _ticketRepository.CreateAsync(request);

            // Act
            var result = await _ticketRepository.GetByIdAsync(createdTicket.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(createdTicket.Id, result.Id);
            Assert.Equal(createdTicket.TicketCode, result.TicketCode);
        }

        [Fact]
        public async Task GetByIdAsync_NonExistingId_ReturnsNull()
        {
            // Act
            var result = await _ticketRepository.GetByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetByCodeAsync_ExistingCode_ReturnsTicket()
        {
            // Arrange
            var request = new TicketRequest { ServiceType = "depositi" };
            var createdTicket = await _ticketRepository.CreateAsync(request);

            // Act
            var result = await _ticketRepository.GetByCodeAsync(createdTicket.TicketCode);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(createdTicket.TicketCode, result.TicketCode);
        }

        [Fact]
        public async Task GetByCodeAsync_NonExistingCode_ReturnsNull()
        {
            // Act
            var result = await _ticketRepository.GetByCodeAsync("NON-EXISTENT-CODE");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetQueueAsync_ReturnsWaitingTickets()
        {
            // Arrange - I ticket di test sono già in coda (non serviti)

            // Act
            var result = await _ticketRepository.GetQueueAsync();

            // Assert
            Assert.NotNull(result);
            Assert.All(result, ticket => Assert.False(ticket.IsServed));
        }

        [Fact]
        public async Task GetQueueByServiceAsync_ValidService_ReturnsTickets()
        {
            // Act
            var result = await _ticketRepository.GetQueueByServiceAsync("depositi");

            // Assert
            Assert.NotNull(result);
            Assert.All(result, ticket =>
            {
                Assert.Equal("depositi", ticket.ServiceType);
                Assert.False(ticket.IsServed);
            });
        }

        [Fact]
        public async Task CallNextAsync_WithWaitingTickets_ReturnsNextTicket()
        {
            // Act
            var result = await _ticketRepository.CallNextAsync();

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.CalledAt);
            Assert.False(result.IsServed);
        }

        [Fact]
        public async Task MarkAsServedAsync_ExistingTicket_MarksAsServed()
        {
            // Arrange
            var request = new TicketRequest { ServiceType = "depositi" };
            var createdTicket = await _ticketRepository.CreateAsync(request);

            // Act
            var result = await _ticketRepository.MarkAsServedAsync(createdTicket.Id);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsServed);
            Assert.NotNull(result.ServedAt);
        }

        [Fact]
        public async Task GetQueuePositionAsync_ValidTicket_ReturnsPosition()
        {
            // Arrange
            await CleanTableAsync<Tickets>(); // Pulisci per test pulito

            var ticket1 = await _ticketRepository.CreateAsync(new TicketRequest { ServiceType = "depositi" });
            var ticket2 = await _ticketRepository.CreateAsync(new TicketRequest { ServiceType = "depositi" });

            // Act
            var position = await _ticketRepository.GetQueuePositionAsync("depositi", ticket2.Id);

            // Assert
            Assert.Equal(2, position); // ticket2 è il secondo in coda
        }

        [Fact]
        public async Task GetEstimatedWaitTimeAsync_ValidService_ReturnsWaitTime()
        {
            // Act
            var waitTime = await _ticketRepository.GetEstimatedWaitTimeAsync("depositi");

            // Assert
            Assert.True(waitTime >= 0); // Tempo di attesa non negativo
        }

        [Fact]
        public async Task GetTicketsServedTodayAsync_ReturnsCount()
        {
            // Act
            var count = await _ticketRepository.GetTicketsServedTodayAsync();

            // Assert
            Assert.True(count >= 0); // Count non negativo
        }
    }
}