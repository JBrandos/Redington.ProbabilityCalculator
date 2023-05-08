using Redington.ProbabilityCalculator.Services.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redington.ProbabilityCalculator.Services.Services
{
    public class CalculatorServiceResult
    {
        public decimal? ProbabilityResult { get; set; }
        public List<ValidationResult> Validations { get; set; }

        public CalculatorServiceResult(decimal probabilityResult)
        {
            ProbabilityResult = probabilityResult;
            Validations = new List<ValidationResult>();
        }

        public CalculatorServiceResult(List<ValidationResult> validations)
        {
            ProbabilityResult = null;
            Validations = validations;
        }
    }
}
