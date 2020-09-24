using Infrastructure.Entities;

namespace Infrastructure.Custom
{
    public class StatusPerMonthDto : BaseEntity
    {
        public int Id { get; set; }
        public int Count { get; set; }
    }
}
