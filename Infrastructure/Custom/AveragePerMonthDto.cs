using Infrastructure.Entities;

namespace Infrastructure.Custom
{
    public class AveragePerMonthDto : BaseEntity
    {
        public string Month { get; set; }
        public int Count { get; set; }
    }
}
