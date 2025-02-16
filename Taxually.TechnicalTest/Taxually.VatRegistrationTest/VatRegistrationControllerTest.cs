using Moq;
using NUnit.Framework;
using Taxually.TechnicalTest.Controllers;
using Taxually.TechnicalTest.Models;
using Taxually.TechnicalTest.Handlers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Taxually.TechnicalTest.Tests
{
    [TestFixture]
    public class VatRegistrationControllerTests
    {
        private Mock<IVatRegistrationHandler> mockGBHandler;
        private Mock<IVatRegistrationHandler> mockFRHandler;
        private Mock<IVatRegistrationHandler> mockDEHandler;
        private VatRegistrationController controller;

        [SetUp]
        public void Init()
        {
            mockGBHandler = new Mock<IVatRegistrationHandler>();
            mockGBHandler.SetupGet(handler => handler.countryCode).Returns("GB");

            mockFRHandler = new Mock<IVatRegistrationHandler>();
            mockFRHandler.SetupGet(handler => handler.countryCode).Returns("FR");

            mockDEHandler = new Mock<IVatRegistrationHandler>();
            mockDEHandler.SetupGet(handler => handler.countryCode).Returns("DE");

            mockGBHandler.Setup(handler => handler.RegisterAsync(It.IsAny<VatRegistrationRequest>())).Returns(Task.CompletedTask);
            mockFRHandler.Setup(handler => handler.RegisterAsync(It.IsAny<VatRegistrationRequest>())).Returns(Task.CompletedTask);
            mockDEHandler.Setup(handler => handler.RegisterAsync(It.IsAny<VatRegistrationRequest>())).Returns(Task.CompletedTask);

            var vatRegistrationHandlers = new List<IVatRegistrationHandler>
            {
                mockGBHandler.Object,
                mockFRHandler.Object,
                mockDEHandler.Object
            };
            controller = new VatRegistrationController(vatRegistrationHandlers);
        }

        [Test]
        public async Task Test_GB_ValidCountry()
        {
            var request = new VatRegistrationRequest
            {
                CompanyName = "Test GB Company",
                CompanyId = "123",
                Country = "GB"
            };

            var result = await controller.Post(request);

            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual($"VAT registration request processed successfully for country {request.Country}", okResult.Value);
            Assert.AreEqual("GB", request.Country);
        }

        [Test]
        public async Task Test_InvalidCountry()
        {
            var request = new VatRegistrationRequest
            {
                CompanyName = "Test Invalid Company",
                CompanyId = "456",
                Country = "ER"
            };

            var result = await controller.Post(request);

            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.AreEqual("Given country code currently not supported for registration", badRequestResult.Value);
        }

        [Test]
        public async Task Test_FR_CorrectHandlerIsCalled()
        {
            var request = new VatRegistrationRequest
            {
                CompanyName = "Test FR Company",
                CompanyId = "789",
                Country = "FR"
            };

            await controller.Post(request);

            mockFRHandler.Verify(handler => handler.RegisterAsync(It.Is<VatRegistrationRequest>(h => h.Country == "FR")), Times.Once);
            mockGBHandler.Verify(handler => handler.RegisterAsync(It.IsAny<VatRegistrationRequest>()), Times.Never);
            mockDEHandler.Verify(handler => handler.RegisterAsync(It.IsAny<VatRegistrationRequest>()), Times.Never);
        }
    }
}
