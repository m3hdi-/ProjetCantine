using CantineKata.Application.Commands;
using CantineKata.Application.Handlers;
using CantineKata.Domain.Entities;
using CantineKata.Infrastructure.Interfaces;
using Moq;

namespace CantineKata.UnitTest
{
    public class CrediterCompteHandlerTests
    {
        private readonly Mock<IClientRepository> _clientRepositoryMock;
        private readonly CrediterCompteHandler _handler;

        public CrediterCompteHandlerTests()
        {
            _clientRepositoryMock = new Mock<IClientRepository>();
            _handler = new CrediterCompteHandler(_clientRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_ClientExists_ShouldReturnTrueAndCreditAmount()
        {
            // Arrange
            var clientId = 1;
            var montant = 100;
            var client = new Client { Id = clientId, Solde = 20 };


            _clientRepositoryMock
                .Setup(repo => repo.GetByIdAsync(clientId))
                .ReturnsAsync(client);

            _clientRepositoryMock
                .Setup(repo => repo.UpdateAsync(It.IsAny<Client>()))
                .Returns(Task.CompletedTask);

            var command = new CrediterCompteCommand { ClientId = clientId, Montant = montant };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result); 
            Assert.Equal(120, client.Solde); 
        }

        [Fact]
        public async Task Handle_ClientNotFound_ShouldReturnFalse()
        {
            // Arrange
            var clientId = 1;
            var montant = 100;

            _clientRepositoryMock
                .Setup(repo => repo.GetByIdAsync(clientId))
                .Returns(Task.FromResult<Client>(null));

            var command = new CrediterCompteCommand { ClientId = clientId, Montant = montant };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
        }
    }
}
