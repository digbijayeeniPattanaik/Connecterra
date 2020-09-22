using Infrastructure.Data.Specifications;
using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Infrastructure.Specifications
{
    public class CowSpecification : BaseSpecification<Cow>
    {
        public CowSpecification(Expression<Func<Cow, bool>> criteria) : base(criteria)
        {
            AddInclude(a => a.Farm);
        }
    }
}
