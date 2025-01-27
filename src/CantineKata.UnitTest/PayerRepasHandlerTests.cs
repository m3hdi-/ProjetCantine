using CantineKata.Application.Commands;
using CantineKata.Application.Handlers;
using CantineKata.Application.Interfaces;
using CantineKata.Domain.Entities;
using CantineKata.Domain.Enums;
using CantineKata.Infrastructure.Interfaces;
using Moq;

namespace CantineKata.UnitTest
{
    public class PayerRepasHandlerTests
    {
        private readonly Mock<IClientRepository> _clientRepositoryMock;
        private readonly Mock<ITicketService> _ticketServiceMock;
        private readonly Mock<ITarificationService> _tarificationServiceMock;
        private readonly PayerRepasHandler _handler;

        public PayerRepasHandlerTests()
        {
            _clientRepositoryMock = new Mock<IClientRepository>();
            _ticketServiceMock = new Mock<ITicketService>();
            _tarificationServiceMock = new Mock<ITarificationService>();

            _handler = new PayerRepasHandler(
                _clientRepositoryMock.Object,
                _tarificationServiceMock.Object,
                _ticketServiceMock.Object
            );
        }

        [Fact]
        public async Task Handle_ClientInexistant_RetourneMessageErreur()
        {
            // Arrange
            _clientRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                                 .Returns(Task.FromResult<Client>(null));

            var command = new PayerRepasCommand(new Int32(), new List<string>());

            // Act & Assert
            var exception = await Assert.ThrowsAnyAsync<Exception>(async () => await _handler.Handle(command, CancellationToken.None));


            Assert.Equal("Client introuvable.", exception.Message);
        }

        [Fact]
        public async Task Handle_ClientAvecSoldeInsuffisant_RetourneMessageErreur()
        {
            // Arrange
            var client = new Client { Solde = 2m, TypeClient = TypeClient.Visiteur };

            _clientRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                                 .ReturnsAsync(client);

            _tarificationServiceMock.Setup(service => service.CalculTotalSupplement(It.IsAny<List<string>>(), out It.Ref<List<string>>.IsAny))
                                    .Returns(15m);

            var command = new PayerRepasCommand(new Int32(), new List<string>());

            // Act & Assert
            var exception = await Assert.ThrowsAnyAsync<Exception>(async () => await _handler.Handle(command, CancellationToken.None));


            Assert.Equal("Solde insuffisant pour effectuer le paiement.", exception.Message);
        }

        [Fact]
        public async Task Handle_ClientInterne_AutorisationDecouvert_RetourneTicket()
        {
            // Arrange
            var client = new Client { Solde = 0m, TypeClient = TypeClient.Interne };

            _clientRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                                 .ReturnsAsync(client);

            _tarificationServiceMock.Setup(service => service.CalculTotalSupplement(It.IsAny<List<string>>(), out It.Ref<List<string>>.IsAny))
                                    .Returns(15m);

            _ticketServiceMock.Setup(service => service.GenererTicketAsync(It.IsAny<Client>(), It.IsAny<List<string>>(), It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<DateTime>()))
                              .Returns(Task.FromResult("Ticket généré avec succès."));

            var command = new PayerRepasCommand(new Int32(), new List<string>());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<string>(result);
            Assert.False(string.IsNullOrWhiteSpace(result));
        }

        [Fact]
        public async Task Handle_ClientVip_PaieZero_RetourneTicket()
        {
            // Arrange
            var client = new Client { Solde = 5m, TypeClient = TypeClient.Vip };

            _clientRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                                 .ReturnsAsync(client);

            _tarificationServiceMock.Setup(service => service.CalculTotalSupplement(It.IsAny<List<string>>(), out It.Ref<List<string>>.IsAny))
            .Returns(20m);

            _ticketServiceMock.Setup(service => service.GenererTicketAsync(It.IsAny<Client>(), It.IsAny<List<string>>(), It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<DateTime>()))
                              .Returns(Task.FromResult("Ticket généré avec succès."));

            var command = new PayerRepasCommand(new Int32(), new List<string>());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<string>(result);
            Assert.False(string.IsNullOrWhiteSpace(result));
        }

        [Fact]
        public async Task Handle_ClientNormal_SoldeSuffisant_RetourneTicket()
        {
            // Arrange
            var client = new Client { Solde = 20m, TypeClient = TypeClient.Prestataire };

            _clientRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                                 .ReturnsAsync(client);

            _tarificationServiceMock.Setup(service => service.CalculTotalSupplement(It.IsAny<List<string>>(), out It.Ref<List<string>>.IsAny))
                                    .Returns(15m);

            _ticketServiceMock.Setup(service => service.GenererTicketAsync(It.IsAny<Client>(), It.IsAny<List<string>>(), It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<DateTime>()))
                              .ReturnsAsync("Ticket généré avec succès.");

            var command = new PayerRepasCommand(new Int32(), new List<string>());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<string>(result);
            Assert.False(string.IsNullOrWhiteSpace(result));
        }
    }
}
