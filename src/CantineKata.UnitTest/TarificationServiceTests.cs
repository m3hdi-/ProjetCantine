using CantineKata.Application.Interfaces;
using CantineKata.Domain.Entities;
using CantineKata.Domain.Enums;
using Moq;

namespace CantineKata.UnitTest
{
    public class TarificationServiceTests
    {
        private readonly Mock<ITarificationService> _tarificationServiceMock;

        public TarificationServiceTests()
        {
            _tarificationServiceMock = new Mock<ITarificationService>();
        }

        [Theory]
        [InlineData(TypeClient.Interne, 20, 7.5)]
        [InlineData(TypeClient.Prestataire, 20, 6)]
        [InlineData(TypeClient.Vip, 20, 20)]
        [InlineData(TypeClient.Stagiaire, 20, 10)]
        [InlineData(TypeClient.Visiteur, 20, 0)]
        public void CalculerPriseEncharge_ShouldReturn_CorrectAmount(TypeClient typeClient, decimal montantTotal, decimal expected)
        {
            // Arrange
            var client = new Client { TypeClient = typeClient, Solde = montantTotal };
            _tarificationServiceMock.Setup(t => t.CalculerPriseEnchange(client, montantTotal)).Returns(expected);

            // Act
            var result = _tarificationServiceMock.Object.CalculerPriseEnchange(client, montantTotal);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(TypeClient.Interne, 5, true)]
        [InlineData(TypeClient.Vip, 5, true)]
        [InlineData(TypeClient.Prestataire, 10, true)]
        [InlineData(TypeClient.Prestataire, 15, false)]
        [InlineData(TypeClient.Stagiaire, 8, false)]
        public void VerifierSoldeSuffisant_ShouldReturn_CorrectResult(TypeClient typeClient, decimal solde, bool expected)
        {
            // Arrange
            var client = new Client { TypeClient = typeClient, Solde = solde };
            decimal montantPayer = 10;
            _tarificationServiceMock.Setup(t => t.VerifierSoldeSuffisant(client, montantPayer)).Returns(expected);

            // Act
            var result = _tarificationServiceMock.Object.VerifierSoldeSuffisant(client, montantPayer);

            // Assert
            Assert.Equal(expected, result);
        }


        [Fact]
        public void CalculerTotalAvecSupplements_AvecAucunSupplement_Retourne10Euros()
        {
            // Arrange
            var supplements = new List<string> { "Boisson", "Fromage", "Petit Salade Bar" };
            var expectedTotal = 10m + 1m + 1m + 4m;
            var expectedProduits = new List<string> { "Entrée", "Plat", "Dessert", "Pain", "Boisson", "Fromage", "Petit Salade Bar" };
            _tarificationServiceMock.Setup(t => t.CalculTotalSupplement(supplements, out expectedProduits)).Returns(expectedTotal);

            // Act
            var result = _tarificationServiceMock.Object.CalculTotalSupplement(supplements, out List<string> produits);

            // Assert
            Assert.Equal(expectedTotal, result);
            Assert.Equal(expectedProduits, produits);
        }
    }
}