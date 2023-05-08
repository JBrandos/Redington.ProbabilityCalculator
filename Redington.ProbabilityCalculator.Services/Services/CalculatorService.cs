using Redington.ProbabilityCalculator.Services.Calculators;
using Redington.ProbabilityCalculator.Services.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redington.ProbabilityCalculator.Services.Services
{
    public class CalculatorService : ICalculatorService
    {
        private readonly IProbabilityCalculator _probabilityCalculator;
        private readonly ICalculatorValidator _calculatorValidator;

        public CalculatorService(IProbabilityCalculator probabilityCalculator, ICalculatorValidator calculatorValidator)
        {
            _probabilityCalculator = probabilityCalculator;
            _calculatorValidator = calculatorValidator;
        }

        public CalculatorServiceResult HandleIntersect(decimal probability1, decimal probability2)
        {
            var validationResult = _calculatorValidator.Validate(probability1, probability2);
            return !validationResult.Any()
                ? new CalculatorServiceResult(_probabilityCalculator.CalculateIntersection(probability1, probability2))
                : new CalculatorServiceResult(validationResult);

        }
        public CalculatorServiceResult HandleUnion(decimal probability1, decimal probability2)
        {
            var validationResult = _calculatorValidator.Validate(probability1, probability2);
            return !validationResult.Any()
                ? new CalculatorServiceResult(_probabilityCalculator.CalculateUnion(probability1, probability2))
                : new CalculatorServiceResult(validationResult);

        }
    }
}
