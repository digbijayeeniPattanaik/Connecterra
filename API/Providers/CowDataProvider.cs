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
        private readonly IAuditDataProvider _auditDataProvider;

        public CowDataProvider(IUnitOfWork uow, IAuditDataProvider auditDataProvider)
        {
            _uow = uow;
            _auditDataProvider = auditDataProvider;
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
                var auditData = _auditDataProvider.GetAuditList(DateTime.Now.Date, string.Empty, "Cows", cow.Farm.Name, cow.CowId);
                if (auditData != null && auditData.Any())
                {
                    cow.RecordFlag = "Error";
                }
                var state = Enum.GetNames(typeof(CowState)).FirstOrDefault(x => x.Equals(stateDto.State, StringComparison.InvariantCultureIgnoreCase));

                cow.State = (CowState)Enum.Parse(typeof(CowState), state);
                cow.UpdateDt = DateTime.Now;
                _uow.Repository<Cow>().Update(cow);
                int output = await _uow.Complete();

                result.Result = cow;
            }
            else
                result.ErrorMessage = "Cow not found";

            return result;
        }
    }
}
