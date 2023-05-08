using Microsoft.AspNetCore.Mvc;
using Redington.ProbabilityCalculator.Models;
using Redington.ProbabilityCalculator.Services.Services;
using System.Diagnostics;

namespace Redington.ProbabilityCalculator.Controllers
{
    public class CalculatorController : Controller
    {
        private readonly ICalculatorService _calculatorService;
        private readonly ILogger<CalculatorController> _logger;

        public CalculatorController(ILogger<CalculatorController> logger, ICalculatorService calculatorService)
        {
            _logger = logger;
            _calculatorService = calculatorService;
        }

        public IActionResult Index()
        {
            var vm = new CalculatorViewModel();
            return View(vm);
        }

        [HttpPost]
        public ActionResult Index(CalculatorViewModel model)
        {            
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var serviceResult = GetServiceResult(model);

            if (serviceResult != null)
            {
                foreach (var error in serviceResult.Validations)
                {
                    ModelState.AddModelError(error.MemberNames.First(), error.ToString());
                }
            }                

            model.Result = serviceResult?.ProbabilityResult;
            _logger.LogInformation($"Calculation performed on {DateTime.UtcNow}: {nameof(CalculationType)}: {model.CalculationType}, " +
                $"{nameof(model.Probability1)}: {model.Probability1}, {nameof(model.Probability2)}: {model.Probability2}, {nameof(model.Result)}: {model.Result}");
            return View(model);
        }

        private CalculatorServiceResult? GetServiceResult(CalculatorViewModel model)
        {
            switch (model.CalculationType)
            {
                case CalculationType.Intersection:
                    return _calculatorService.HandleIntersect(model.Probability1, model.Probability2);
                case CalculationType.Union:
                    return _calculatorService.HandleUnion(model.Probability1, model.Probability2);
                default:
                    ModelState.AddModelError(nameof(CalculationType), "Invalid calculation type");
                    throw new ArgumentException("Invalid calculation type");
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}