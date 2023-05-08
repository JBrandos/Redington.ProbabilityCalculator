using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redington.ProbabilityCalculator.Services.Validators
{
    public interface ICalculatorValidator
    {
        public List<ValidationResult> Validate(decimal probability1, decimal probability2);
    }
}
