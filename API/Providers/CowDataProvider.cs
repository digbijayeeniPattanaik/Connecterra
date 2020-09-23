using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dtos;
using API.Helpers;
using API.Providers.Interfaces;
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

        public async Task<Cow> Update(int cowId, StateDto stateDto)
        {
            var cowSpecification = new CowSpecifications(a => a.CowId == cowId);
            var cow = await _uow.Repository<Cow>().GetEntityWithSpec(cowSpecification);
            if (cow != null)
            {
                cow.State = (CowState)Enum.Parse(typeof(CowState), stateDto.State);
                cow.UpdateDt = DateTime.Now;
                _uow.Repository<Cow>().Update(cow);
                int output = await _uow.Complete();
                if (output >= 1)
                {
                    return cow;
                }
            }

            return null;
        }
    }
}
