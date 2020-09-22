using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities
{
    public class Farm : BaseEntity
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int FarmId { get; set; }

        public string Name { get; set; }
    }
}
