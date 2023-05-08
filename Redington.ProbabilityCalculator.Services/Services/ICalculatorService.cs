using Redington.ProbabilityCalculator.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redington.ProbabilityCalculator.Services.Services
{
    public interface ICalculatorService
    {
        public CalculatorServiceResult HandleIntersect(decimal probability1, decimal probability2);
        public CalculatorServiceResult HandleUnion(decimal probability1, decimal probability2);
    }
}
