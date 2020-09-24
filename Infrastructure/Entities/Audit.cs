using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities
{
    public class Audit
    {
        public int AuditId { get; set; }
        public string TableName { get; set; }
        public DateTime AuditDate { get; set; }
        public string KeyValues { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }

        [NotMapped]
        public Dictionary<string, object> NewValuesSerialized
        {
            get { return NewValues == null ? null : JsonConvert.DeserializeObject<Dictionary<string, object>>(NewValues); }
            set { NewValues = JsonConvert.SerializeObject(value); }
        }
    }
}
