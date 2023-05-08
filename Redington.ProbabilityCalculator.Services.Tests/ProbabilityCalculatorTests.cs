using Redington.ProbabilityCalculator.Services;
using Redington.ProbabilityCalculator.Services.Calculators;
using Shouldly;

namespace Redington.ProbabilityCalculator.Services.Tests
{
    public class ProbabilityCalculatorTests
    {
        [Theory]
        [InlineData(0.5, 0.5, 0.25)]
        [InlineData(0.25, 0.75, 0.1875)]
        public void CalculateIntersection_ShouldReturnExpectedResult(decimal probability1, decimal probability2, decimal expected)
        {
            // Arrange
            var calculator = new Calculators.ProbabilityCalculator();

            // Act
            var result = calculator.CalculateIntersection(probability1, probability2);

            // Assert
            result.ShouldBe(expected);
        }

        [Theory]
        [InlineData(0.5, 0.5, 0.75)]
        [InlineData(0.25, 0.75, 0.8125)]
        [InlineData(0.1, 0.1, 0.19)]
        public void CalculateUnion_ShouldReturnExpectedResult(decimal probability1, decimal probability2, decimal expected)
        {
            // Arrange
            var calculator = new Calculators.ProbabilityCalculator();

            // Act
            var result = calculator.CalculateUnion(probability1, probability2);

            // Assert
            result.ShouldBe(expected);
        }
    }
}