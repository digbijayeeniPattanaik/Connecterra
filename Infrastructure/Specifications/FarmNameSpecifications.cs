using Infrastructure.Data.Specifications;
using Infrastructure.Entities;
using System;

namespace Infrastructure.Specifications
{
    public class FarmNameSpecifications : BaseSpecification<Farm>
    {
        public FarmNameSpecifications(string name)
           : base(a => a.Name.ToLower().Equals(name.ToLower()))
        {
        }
    }
}
