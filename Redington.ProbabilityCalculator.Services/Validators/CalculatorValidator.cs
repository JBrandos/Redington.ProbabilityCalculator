using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redington.ProbabilityCalculator.Services.Validators
{
    public class CalculatorValidator : ICalculatorValidator
    {
        private const string OutOfRangeErrorMessage = "Probability is out of expected range 0-1 inclusive";

        public List<ValidationResult> Validate(decimal probability1, decimal probability2)
        {
            var listResults = new List<ValidationResult>();

            if (!IsExpectedRange(probability1)) 
            {
                listResults.Add(new ValidationResult(OutOfRangeErrorMessage, new List<string>() { nameof(probability1) }));
            }
            if (!IsExpectedRange(probability2))
            {
                listResults.Add(new ValidationResult(OutOfRangeErrorMessage, new List<string>() { nameof(probability2) }));
            }

            return listResults;
        }

        public bool IsExpectedRange(decimal probability)
        {
            return probability >= 0.0M && probability <= 1.0M;
        }
    }
}
