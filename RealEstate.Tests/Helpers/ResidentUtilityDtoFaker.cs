using Bogus;
using RealEstate.Business.Utilities;

namespace RealEstate.Tests.Helpers
{
    public class ResidentUtilityDtoFaker : Faker<ResidentUtilityDto>
    {
        public ResidentUtilityDtoFaker(int customerId)
        {
            RuleFor(u => u.ResidentId, f => customerId);
            RuleFor(u => u.ElectricityBalance, f => f.Random.Double());
            RuleFor(u => u.WaterBalance, f => f.Random.Double());
            RuleFor(u => u.TrashBalance, f => f.Random.Double());
            RuleFor(u => u.TotalPastDueBalance, f => f.Random.Double());
            RuleFor(u => u.PeriodStart, f => f.Date.Past());
            RuleFor(u => u.PeriodEnd, f => f.Date.Past());
        }
    }
}
