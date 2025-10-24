using Xunit;

namespace RepositoryTest
{
    public class EmployeeRepositoryTest : BaseTest
    {
        [Fact]
        public async Task GetEmployeeServicesAsync_ReturnsAllServices()
        {
            // Act
            var result = await _employeeRepository.GetEmployeeServicesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public async Task CallNextCustomerAsync_WithWaitingTickets_ReturnsNextTicket()
        {
            // Act
            var result = await _employeeRepository.CallNextCustomerAsync();

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.CalledAt);
        }

        [Fact]
        public async Task GetQueueByServiceAsync_ValidService_ReturnsTickets()
        {
            // Act
            var result = await _employeeRepository.GetQueueByServiceAsync("depositi");

            // Assert
            Assert.NotNull(result);
            Assert.All(result, ticket =>
            {
                Assert.Equal("depositi", ticket.ServiceType);
                Assert.False(ticket.IsServed);
            });
        }

        [Fact]
        public async Task GetTicketsServedTodayAsync_ReturnsCount()
        {
            // Act
            var count = await _employeeRepository.GetTicketsServedTodayAsync();

            // Assert
            Assert.True(count >= 0);
        }
    }
}