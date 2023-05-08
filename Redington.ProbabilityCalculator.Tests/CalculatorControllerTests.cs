using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Redington.ProbabilityCalculator.Models;
using Redington.ProbabilityCalculator.Services.Services;
using Shouldly;
using System.Net.NetworkInformation;

namespace Redington.ProbabilityCalculator.Controllers.Tests
{
    public class CalculatorControllerTests
    {
        private Mock<ILogger<CalculatorController>> _mockLogger;
        private Mock<ICalculatorService> _mockCalculatorService;

        private CalculatorController _controller;

        public CalculatorControllerTests()
        {
            _mockLogger = new Mock<ILogger<CalculatorController>>();
            _mockCalculatorService = new Mock<ICalculatorService>();
            _controller = new CalculatorController(_mockLogger.Object, _mockCalculatorService.Object);
        }



        [Fact]
        public void Index_ReturnsView()
        {
            // Arrange
            var controller = new CalculatorController(_mockLogger.Object, _mockCalculatorService.Object);

            // Act
            var result = controller.Index();

            // Assert
            result.ShouldBeOfType<ViewResult>();
        }

        [Fact]
        public void Index_InvalidModel_ReturnsViewWithModel()
        {
            // Arrange
            var controller = new CalculatorController(_mockLogger.Object, _mockCalculatorService.Object);
            var model = new CalculatorViewModel();
            controller.ModelState.AddModelError("Probability1", "Probability1 is required");

            // Act
            var result = controller.Index(model);

            // Assert
            result.ShouldBeOfType<ViewResult>();
            var resultAsViewResult = controller.Index(model) as ViewResult;
            resultAsViewResult.ShouldNotBeNull();
            resultAsViewResult.ViewName.ShouldBeNullOrEmpty();
            resultAsViewResult.Model.ShouldNotBeNull();
            resultAsViewResult.Model.ShouldBeSameAs(model);
            _mockCalculatorService.Verify(x => x.HandleIntersect(It.IsAny<decimal>(), It.IsAny<decimal>()), Times.Never);
            _mockCalculatorService.Verify(x => x.HandleUnion(It.IsAny<decimal>(), It.IsAny<decimal>()), Times.Never);
        }

        [Fact]
        public void Index_ValidModelIntersection_ReturnsViewWithModelAndResult()
        {
            // Arrange
            _mockCalculatorService
                .Setup(x => x.HandleIntersect(0.5M, 0.6M))
                .Returns(new CalculatorServiceResult(0.3M));
            var controller = new CalculatorController(_mockLogger.Object, _mockCalculatorService.Object);
            var model = new CalculatorViewModel
            {
                CalculationType = CalculationType.Intersection,
                Probability1 = 0.5M,
                Probability2 = 0.6M
            };

            // Act
            var result = controller.Index(model) as ViewResult;

            // Assert
            result.ShouldBeOfType<ViewResult>();
            result.ShouldNotBeNull();
            result.ViewName.ShouldBeNullOrEmpty();
            result.Model.ShouldNotBeNull();
            result.Model.ShouldBeSameAs(model);
            model.Result.ShouldBe(0.3M);
            _mockCalculatorService.Verify(x => x.HandleIntersect(model.Probability1, model.Probability2), Times.Once);
            _mockLogger.Invocations.Where(x => (LogLevel)x.Arguments[0] == LogLevel.Information).Count().ShouldBe(1); 
        }
                
        [Fact]
        public void Index_ValidModelUnion_ReturnsViewWithModelAndResult()
        {
            // Arrange
            _mockCalculatorService
                .Setup(x => x.HandleUnion(0.5M, 0.6M))
                .Returns(new CalculatorServiceResult(0.3M));
            var controller = new CalculatorController(_mockLogger.Object, _mockCalculatorService.Object);
            var model = new CalculatorViewModel
            {
                CalculationType = CalculationType.Union,
                Probability1 = 0.5M,
                Probability2 = 0.6M
            };

            // Act
            var result = controller.Index(model) as ViewResult;

            // Assert
            result.ShouldBeOfType<ViewResult>();
            result.ShouldNotBeNull();
            result.ViewName.ShouldBeNullOrEmpty();
            result.Model.ShouldNotBeNull();
            result.Model.ShouldBeSameAs(model);
            model.Result.ShouldBe(0.3M);
            _mockCalculatorService.Verify(x => x.HandleUnion(model.Probability1, model.Probability2), Times.Once);
            _mockLogger.Invocations.Where(x => (LogLevel)x.Arguments[0] == LogLevel.Information).Count().ShouldBe(1);
        }

        [Fact]
        public void Index_InvalidCalculationType_ThrowsArgumentException()
        {
            // Arrange
            var controller = new CalculatorController(_mockLogger.Object, _mockCalculatorService.Object);
            var model = new CalculatorViewModel
            {
                CalculationType = (CalculationType)100,
                Probability1 = 0.5M,
                Probability2 = 0.6M
            };

            // Act & Assert
            Should.Throw<ArgumentException>(() => controller.Index(model))
                .Message.ShouldBe("Invalid calculation type");
        }

        [Fact]
        public void Error_ReturnsView()
        {
            // Arrange
            var controller = new CalculatorController(_mockLogger.Object, _mockCalculatorService.Object);
            controller.ControllerContext.HttpContext = new DefaultHttpContext { TraceIdentifier = "123" };

            // Act
            var result = controller.Error();

            // Assert
            result.ShouldBeOfType<ViewResult>();
        }
    }
}
