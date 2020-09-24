using Infrastructure.Entities;

namespace Infrastructure.Custom
{
    public class CowStatusPerMonthDto : BaseEntity
    {
        public int CowID { get; set; }
        public int Count { get; set; }
    }
}
