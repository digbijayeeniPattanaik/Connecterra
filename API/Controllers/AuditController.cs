using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using AutoMapper;
using Infrastructure.Common;
using Infrastructure.Entities;
using Infrastructure.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly FarmContext _farmContext;
        private readonly IMapper _mapper;

        public AuditController(IUnitOfWork uow, FarmContext farmContext, IMapper mapper)
        {
            _uow = uow;
            _farmContext = farmContext;
            _mapper = mapper;
        }

        [HttpGet()]
        public async Task<ActionResult<IReadOnlyList<Audit>>> GetAuditList(DateTime? onDate, string state, string searchType, string farm)
        {
            int? farmId = null;
            if (!string.IsNullOrWhiteSpace(farm))
            {
                var spec = new FarmNameSpecifications(farm);

                var farmEntity = await _uow.Repository<Farm>().GetEntityWithSpec(spec);

                if (farmEntity != null)
                    farmId = farmEntity.FarmId;
            }

            SqlParameter farmIdParameter = new SqlParameter("@FarmId", value: farmId);
            var auditList = _farmContext.Audits.FromSqlRaw("SELECT * FROM Audits WHERE JSON_VALUE(NewValues, '$.FarmId') =  @FarmId", farmIdParameter).ToList();

            //var auditList = _farmContext.Audits.Where(itema => (int?)JObject.Parse(itema.NewValues)["FarmId"] == farmId).ToList();

            ////var auditList = _farmContext.Audits.ToList();
            //var auditList = _farmContext.Audits.Where(itema => itema.NewValuesSerialized != null && Convert.ToInt32(itema.NewValuesSerialized["FarmId"]) == Convert.ToInt32(farmId)).ToList();
            foreach (var item in auditList)
            {
                var test = JObject.Parse(item.NewValues);
                var f = (int?)test["FarmId"];
                var boolsss = (int?)JObject.Parse(item.NewValues)["FarmId"] == farmId;
            }
            ////var auditSpecifications = new AuditSpecifications(onDate, state, searchType, farmId);
            ////var auditList = await _uow.Repository<Audit>().ListAsync(auditSpecifications);

            return Ok(auditList);
        }
    }
}