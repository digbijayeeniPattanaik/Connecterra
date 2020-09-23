using Infrastructure.Data.Specifications;
using Infrastructure.Entities;
using System;
using System.Linq.Expressions;

namespace Infrastructure.Specifications
{
    public class FarmNameSpecifications : BaseSpecification<Farm>
    {
        //public FarmNameSpecifications(string name)
        //   : base(a => a.Name.ToLower().Equals(name.ToLower()))
        //{
        //}
        public FarmNameSpecifications(Expression<Func<Farm, bool>> criteria) : base(criteria)
        {
        }
    }
}
