using System;

namespace Infrastructure.Entities
{
    public class Audit : BaseEntity
    {
        public int AuditId { get; set; }
        public string TableName { get; set; }
        public DateTime DateTime { get; set; }
        public string KeyValues { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
    }
}
