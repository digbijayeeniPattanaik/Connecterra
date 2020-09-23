using Infrastructure.Data.Specifications;
using Infrastructure.Entities;
using System;
using System.Linq.Expressions;

namespace Infrastructure.Specifications
{
    public class CowSpecifications : BaseSpecification<Cow>
    {
        public CowSpecifications(Expression<Func<Cow, bool>> criteria = null) : base(criteria)
        {
            AddInclude(a => a.Farm);
        }
    }
}
