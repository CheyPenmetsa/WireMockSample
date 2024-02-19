namespace RealEstate.Business.Utilities
{
    public class ResidentUtilityDto
    {
        public int ResidentId { get; set; }

        public double ElectricityBalance { get; set; }

        public double WaterBalance { get; set; }

        public double TrashBalance { get; set; }

        public DateTime PeriodStart { get; set; }

        public DateTime PeriodEnd { get; set; }

        public double TotalPastDueBalance { get; set; }
    }
}
