using Infrastructure.Data.Specifications;
using Infrastructure.Entities;
using Newtonsoft.Json.Linq;
using System;

namespace Infrastructure.Specifications
{
    public class AuditSpecifications : BaseSpecification<Audit>
    {
        public AuditSpecifications(DateTime? onDate, string state, string searchType, int? farmId)
           : base(a =>
           (string.IsNullOrWhiteSpace(searchType) || a.TableName.Equals(searchType, StringComparison.InvariantCultureIgnoreCase)) &&
           (!farmId.HasValue || (int?)JObject.Parse(a.NewValues)["FarmId"] == farmId) &&
           (string.IsNullOrWhiteSpace(state) || a.NewValues.ToLower().Contains(state.ToLower())) &&
           (!onDate.HasValue || a.DateTime == onDate))
        {
        }
    }
}
