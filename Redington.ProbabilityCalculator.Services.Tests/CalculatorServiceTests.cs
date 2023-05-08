using Moq;
using Redington.ProbabilityCalculator.Services.Calculators;
using Redington.ProbabilityCalculator.Services.Services;
using Redington.ProbabilityCalculator.Services.Validators;
using Shouldly;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redington.ProbabilityCalculator.Services.Tests
{
    public class CalculatorServiceTests
    {
        private readonly Mock<IProbabilityCalculator> _probabilityCalculatorMock = new Mock<IProbabilityCalculator>();
        private readonly Mock<ICalculatorValidator> _calculatorValidatorMock = new Mock<ICalculatorValidator>();
        private readonly CalculatorService _calculatorService;

        public CalculatorServiceTests()
        {
            _calculatorService = new CalculatorService(_probabilityCalculatorMock.Object, _calculatorValidatorMock.Object);
        }

        [Fact]
        public void HandleIntersect_ValidProbabilities_ReturnsProbabilityResult()
        {
            // Arrange
            decimal probability1 = 0.2M;
            decimal probability2 = 0.4M;
            decimal expectedProbability = 0.08M;
            _calculatorValidatorMock.Setup(v => v.Validate(probability1, probability2)).Returns(new List<ValidationResult>());
            _probabilityCalculatorMock.Setup(c => c.CalculateIntersection(probability1, probability2)).Returns(expectedProbability);

            // Act
            var result = _calculatorService.HandleIntersect(probability1, probability2);

            // Assert
            result.Validations.ShouldBeEmpty();
            result.ProbabilityResult.ShouldBe(expectedProbability);
        }

        [Fact]
        public void HandleIntersect_InvalidProbabilities_ReturnsValidationResult()
        {
            // Arrange
            decimal probability1 = 1.2M;
            decimal probability2 = -0.5M;
            var expectedValidationResults = new List<ValidationResult>
        {
            new ValidationResult("Probability is out of expected range 0-1 inclusive", new List<string> { nameof(probability1) }),
            new ValidationResult("Probability is out of expected range 0-1 inclusive", new List<string> { nameof(probability2) })
        };
            _calculatorValidatorMock.Setup(v => v.Validate(probability1, probability2)).Returns(expectedValidationResults);

            // Act
            var result = _calculatorService.HandleIntersect(probability1, probability2);

            // Assert
            result.ProbabilityResult.ShouldBeNull();
            result.Validations.ShouldContain(expectedValidationResults[0]);
            result.Validations.ShouldContain(expectedValidationResults[1]);
        }

        [Fact]
        public void HandleUnion_ValidProbabilities_ReturnsProbabilityResult()
        {
            // Arrange
            decimal probability1 = 0.3M;
            decimal probability2 = 0.5M;
            decimal expectedProbability = 0.65M;
            _calculatorValidatorMock.Setup(v => v.Validate(probability1, probability2)).Returns(new List<ValidationResult>());
            _probabilityCalculatorMock.Setup(c => c.CalculateUnion(probability1, probability2)).Returns(expectedProbability);

            // Act
            var result = _calculatorService.HandleUnion(probability1, probability2);

            // Assert
            result.Validations.ShouldBeEmpty();
            result.ProbabilityResult.ShouldBe(expectedProbability);
        }

        [Fact]
        public void HandleUnion_InvalidProbabilities_ReturnsValidationResult()
        {
            // Arrange
            decimal probability1 = -0.5M;
            decimal probability2 = 1.5M;
            var expectedValidationResults = new List<ValidationResult>
            {
                new ValidationResult("Probability is out of expected range 0-1 inclusive", new List<string> { nameof(probability1) }),
                new ValidationResult("Probability is out of expected range 0-1 inclusive", new List<string> { nameof(probability2) })
            };
            _calculatorValidatorMock.Setup(v => v.Validate(probability1, probability2)).Returns(expectedValidationResults);

            // Act
            var result = _calculatorService.HandleUnion(probability1, probability2);

            // Assert
            result.ProbabilityResult.ShouldBeNull();
            result.Validations.ShouldContain(expectedValidationResults[0]);
            result.Validations.ShouldContain(expectedValidationResults[1]);
        }
    }
    
}
