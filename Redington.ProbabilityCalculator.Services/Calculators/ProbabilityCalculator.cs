namespace Redington.ProbabilityCalculator.Services.Calculators
{
    public class ProbabilityCalculator : IProbabilityCalculator
    {
        public decimal CalculateIntersection(decimal probability1, decimal probability2)
        {
            var intersectionProbability = CalculateProduct(probability1, probability2);
            return intersectionProbability;
        }

        public decimal CalculateUnion(decimal probability1, decimal probability2)
        {
            var unionProbability = probability1 + probability2 - CalculateProduct(probability1, probability2);
            return unionProbability;
        }

        private decimal CalculateProduct(decimal probability1, decimal probability2) => probability1*probability2;        
    }
}
