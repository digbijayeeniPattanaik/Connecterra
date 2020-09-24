using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Helpers;
using API.Providers.Interfaces;
using Infrastructure;
using Infrastructure.Common;
using Infrastructure.Entities;
using Infrastructure.Specifications;

namespace API.Providers
{
    public class CowDataProvider : ICowDataProvider
    {
        private readonly IUnitOfWork _uow;

        public CowDataProvider(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Cow> GetCow(int id)
        {
            var cowSpecification = new CowSpecifications(a => a.CowId == id);
            var cow = await _uow.Repository<Cow>().GetEntityWithSpec(cowSpecification);
            return cow;
        }

        public async Task<IReadOnlyList<Cow>> GetCows()
        {
            var cowSpecification = new CowSpecifications();
            var cowList = await _uow.Repository<Cow>().ListAsync(cowSpecification);
            return cowList;
        }

        public async Task<Outcome<Cow>> Update(int cowId, StateDto stateDto)
        {
            var result = new Outcome<Cow>();

            if (!Enum.GetNames(typeof(CowState)).Any(x => x.Equals(stateDto.State, StringComparison.InvariantCultureIgnoreCase)))
            {
                result.ErrorMessage = "State is not valid";
                return result;
            }

            var cowSpecification = new CowSpecifications(a => a.CowId == cowId);
            var cow = await _uow.Repository<Cow>().GetEntityWithSpec(cowSpecification);
            if (cow != null)
            {
                var state = Enum.GetNames(typeof(CowState)).FirstOrDefault(x => x.Equals(stateDto.State, StringComparison.InvariantCultureIgnoreCase));
                //// if the state is same
                if (cow.State != (CowState)Enum.Parse(typeof(CowState), state))
                {
                    cow.State = (CowState)Enum.Parse(typeof(CowState), state);
                    cow.UpdateDt = DateTime.Now;
                    _uow.Repository<Cow>().Update(cow);
                    int output = await _uow.Complete();
                }
                result.Result = cow;
            }
            else
                result.ErrorMessage = "Cow not found";

            return result;
        }
    }
}
