using System.ComponentModel.DataAnnotations;

namespace Redington.ProbabilityCalculator.Models
{
    public class CalculatorViewModel
    {
        [Required]
        public decimal Probability1 { get; set; }

        [Required]
        public decimal Probability2 { get; set; }

        [Required]
        public CalculationType CalculationType { get; set; }

        public decimal? Result { get; set; }
    }

}
