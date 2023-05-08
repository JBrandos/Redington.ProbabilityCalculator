using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redington.ProbabilityCalculator.Services.Tests
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Redington.ProbabilityCalculator.Services.Validators;
    using Shouldly;
    using Xunit;

    namespace MyNamespace
    {
        public class CalculatorValidatorTests
        {
            private readonly CalculatorValidator _validator;

            public CalculatorValidatorTests()
            {
                _validator = new CalculatorValidator();
            }

            [Theory]
            [InlineData(0)]
            [InlineData(0.5)]
            [InlineData(1)]
            public void IsExpectedRange_ValidProbabilities_ReturnsTrue(decimal probability)
            {
                // Arrange

                // Act
                bool result = _validator.IsExpectedRange(probability);

                // Assert
                result.ShouldBeTrue();
            }

            [Theory]
            [InlineData(-1)]
            [InlineData(2)]
            public void IsExpectedRange_InvalidProbabilities_ReturnsFalse(decimal probability)
            {
                // Arrange

                // Act
                bool result = _validator.IsExpectedRange(probability);

                // Assert
                result.ShouldBeFalse();
            }

            [Theory]
            [InlineData(0, 0, 0)]
            [InlineData(0, 1, 0)]
            [InlineData(1, 0, 0)]
            [InlineData(1, 1, 0)]
            [InlineData(0.5, 0.5, 0)]
            [InlineData(-1, 0, 1)]
            [InlineData(0, -1, 1)]
            [InlineData(2, 0, 1)]
            [InlineData(0, 2, 1)]
            [InlineData(-1, -1, 2)]
            [InlineData(2, 2, 2)]
            [InlineData(-1, 2, 2)]
            [InlineData(2, -1, 2)]
            public void Validate_InvalidProbabilities_ReturnsValidationResult(decimal probability1, decimal probability2, int expectedCount)
            {
                // Arrange

                // Act
                List<ValidationResult> results = _validator.Validate(probability1, probability2);

                // Assert
                results.Count.ShouldBe(expectedCount);
                
                if (expectedCount == 1)
                {
                    ValidationResult result = results[0];
                    result.ErrorMessage.ShouldBe("Probability is out of expected range 0-1 inclusive");

                    if (probability1 < 0 || probability1 > 1)
                    {
                        result.MemberNames.ShouldContain(nameof(probability1));
                    }

                    if (probability2 < 0 || probability2 > 1)
                    {
                        result.MemberNames.ShouldContain(nameof(probability2));
                    }
                }
                else if (expectedCount == 2)
                {
                    if (expectedCount == 1)
                    {
                        ValidationResult result1 = results[0];
                        ValidationResult result2 = results[1];
                        result1.ErrorMessage.ShouldBe("Probability is out of expected range 0-1 inclusive");
                        result2.ErrorMessage.ShouldBe("Probability is out of expected range 0-1 inclusive");
                        result1.MemberNames.ShouldContain(nameof(probability1));
                        result2.MemberNames.ShouldContain(nameof(probability2));                        
                    }
                }
            }
        }
    }
}
