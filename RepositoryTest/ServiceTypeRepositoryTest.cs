using TicketService.DTO;
using Xunit;

namespace RepositoryTest
{
    public class ServiceTypeRepositoryTest : BaseTest
    {
        [Fact]
        public async Task GetAllAsync_ReturnsAllServiceTypes()
        {
            // Act
            var result = await _serviceTypeRepository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count); // I 3 service types di test
            Assert.Contains(result, st => st.Tag == "depositi");
            Assert.Contains(result, st => st.Tag == "spedizioni");
            Assert.Contains(result, st => st.Tag == "conti");
        }

        [Fact]
        public async Task GetByIdAsync_ExistingId_ReturnsServiceType()
        {
            // Act
            var result = await _serviceTypeRepository.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("depositi", result.Tag);
            Assert.Equal(5, result.AverageServiceTimeMinutes);
        }

        [Fact]
        public async Task GetByIdAsync_NonExistingId_ReturnsNull()
        {
            // Act
            var result = await _serviceTypeRepository.GetByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAsync_ValidServiceType_ReturnsCreated()
        {
            // Arrange
            var newServiceTypeRequest = new ServiceTypeCreateRequest
            {
                Tag = "prestiti",
                AverageServiceTimeMinutes = 20
            };

            // Act
            var result = await _serviceTypeRepository.CreateAsync(newServiceTypeRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("prestiti", result.Tag);
            Assert.Equal(20, result.AverageServiceTimeMinutes);
            Assert.True(result.Id > 0); // ID assegnato dal database
        }

        [Fact]
        public async Task UpdateAsync_ExistingServiceType_ReturnsUpdated()
        {
            // Arrange
            var serviceType = await _serviceTypeRepository.GetByIdAsync(1);
            serviceType!.AverageServiceTimeMinutes = 8; // Modifica tempo

            // Act
            var result = await _serviceTypeRepository.UpdateAsync(serviceType);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(8, result.AverageServiceTimeMinutes); // Tempo modificato
            Assert.Equal("depositi", result.Tag); // Tag invariato
        }

        [Fact]
        public async Task DeleteAsync_ExistingId_ReturnsTrue()
        {
            // Act
            var result = await _serviceTypeRepository.DeleteAsync(1);

            // Assert
            Assert.True(result);

            // Verifica che sia stato eliminato
            var deleted = await _serviceTypeRepository.GetByIdAsync(1);
            Assert.Null(deleted);
        }

        [Fact]
        public async Task DeleteAsync_NonExistingId_ReturnsFalse()
        {
            // Act
            var result = await _serviceTypeRepository.DeleteAsync(999);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task CreateAsync_DuplicateTag_AllowsDuplicateInMemory()
        {
            // Arrange - InMemory DB non applica vincoli UNIQUE
            var duplicateServiceTypeRequest = new ServiceTypeCreateRequest
            {
                Tag = "depositi", // Tag già esistente
                AverageServiceTimeMinutes = 25
            };

            // Act
            var result = await _serviceTypeRepository.CreateAsync(duplicateServiceTypeRequest);

            // Assert - InMemory permette duplicati, quindi non viene lanciata eccezione
            Assert.NotNull(result);
            Assert.Equal("depositi", result.Tag);
            Assert.Equal(25, result.AverageServiceTimeMinutes);
        }
    }
}