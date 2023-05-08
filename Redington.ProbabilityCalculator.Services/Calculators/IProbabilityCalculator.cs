namespace Redington.ProbabilityCalculator.Services.Calculators
{
    public interface IProbabilityCalculator
    {
        public decimal CalculateIntersection(decimal probability1, decimal probability2);

        public decimal CalculateUnion(decimal probability1, decimal probability2);
    }
}
