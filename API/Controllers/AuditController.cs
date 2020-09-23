using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Common;
using Infrastructure.Entities;
using Infrastructure.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public AuditController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        [HttpGet()]
        public async Task<ActionResult<IReadOnlyList<Audit>>> GetAuditList(DateTime? onDate, string state, string searchType, string farm)
        {
            string whereClause = string.Empty;
            if (onDate != null || !string.IsNullOrWhiteSpace(state) || !string.IsNullOrWhiteSpace(searchType) || !string.IsNullOrWhiteSpace(farm)) whereClause = " WHERE ";

            var sqlParameters = new List<SqlParameter>();

            int? farmId = null;
            if (!string.IsNullOrWhiteSpace(farm))
            {
                var spec = new FarmNameSpecifications(farm);

                var farmEntity = await _uow.Repository<Farm>().GetEntityWithSpec(spec);

                if (farmEntity != null)
                    farmId = farmEntity.FarmId;

                whereClause += " JSON_VALUE(NewValues, '$.FarmId') =  @FarmId AND ";
                SqlParameter farmIdParameter = new SqlParameter() { ParameterName = "@FarmId", Value = farmId };
                sqlParameters.Add(farmIdParameter);
            }

            if (onDate != null)
            {
                whereClause += " cast(AuditDate as date) = @OnDate AND ";
                SqlParameter onDateParameter = new SqlParameter()
                {
                    ParameterName = "@OnDate",
                    SqlDbType = System.Data.SqlDbType.DateTime,
                    IsNullable = true,
                    Value = onDate
                };
                sqlParameters.Add(onDateParameter);
            }

            if (!string.IsNullOrWhiteSpace(state))
            {
                whereClause += " JSON_VALUE(NewValues, '$.State') =  @State AND ";
                SqlParameter stateParameter = new SqlParameter() { ParameterName = "@State", Value = state };
                sqlParameters.Add(stateParameter);
            }

            if (!string.IsNullOrWhiteSpace(searchType))
            {
                whereClause += "  TableName = @SearchType ";
                SqlParameter searchTypeParameter = new SqlParameter() { ParameterName = "@SearchType", Value = searchType };
                sqlParameters.Add(searchTypeParameter);
            }

            if (whereClause.EndsWith(" AND ")) whereClause = whereClause.Substring(0, whereClause.LastIndexOf(" AND "));

           
            string sqlQuery = string.Format("SELECT * FROM Audits {0}", whereClause);
            var auditList = _uow.Repository<Audit>().QueryFromSqlRaw(sqlQuery, sqlParameters.ToArray());


            return Ok(auditList);
        }
    }
}