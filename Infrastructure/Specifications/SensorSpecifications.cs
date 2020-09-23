using System;
using System.Linq.Expressions;
using Infrastructure.Data.Specifications;
using Infrastructure.Entities;

namespace Infrastructure.Specifications
{
    public class SensorSpecifications : BaseSpecification<Sensor>
    {
        public SensorSpecifications(Expression<Func<Sensor, bool>> criteria = null) : base(criteria)
        {
            AddInclude(a => a.Farm);
        }
    }
}
