using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities
{
    public class Farm
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int FarmId { get; set; }

        public string Name { get; set; }
    }
}
