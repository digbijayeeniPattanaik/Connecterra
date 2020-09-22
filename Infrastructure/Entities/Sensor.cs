using API.Helpers;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities
{
    public class Sensor : BaseEntity
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int SensorId { get; set; }
        public SensorState State { get; set; }
        public Farm Farm { get; set; }
        public int FarmId { get; set; }
        public string RecordFlag { get; set; }
        public DateTime? UpdateDt { get; set; }
        public DateTime? CreateDt { get; set; }

    }
}
