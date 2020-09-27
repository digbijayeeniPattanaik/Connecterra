using System;
using System.Collections.Generic;
using System.Linq;
using API.Helpers;
using API.Providers.Interfaces;
using AutoMapper;
using Infrastructure;
using Infrastructure.Common;
using Infrastructure.Custom;
using Infrastructure.Entities;
using Infrastructure.Specifications;
using Microsoft.Data.SqlClient;

namespace API.Providers
{
    public class AuditDataProvider : IAuditDataProvider
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public AuditDataProvider(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public IEnumerable<Audit> GetAuditList(DateTime? onDate, string state, string searchType, string farm)
        {
            string whereClause = string.Empty;
            if (onDate != null || !string.IsNullOrWhiteSpace(state) || !string.IsNullOrWhiteSpace(searchType) || !string.IsNullOrWhiteSpace(farm)) whereClause = " WHERE ";

            var sqlParameters = new List<SqlParameter>();

            int? farmId = null;
            if (!string.IsNullOrWhiteSpace(farm))
            {
                Farm farmEntity = GetFarmDetails(farm);

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
            var auditList = _uow.Repository<Audit>().QueryFromSqlRawReturnList(sqlQuery, sqlParameters.ToArray());

            return auditList;
        }

        public Outcome<decimal> GetAveragePerMonth(string state, int year, string searchType)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            if (year != 0)
            {
                SqlParameter yearParameter = new SqlParameter() { ParameterName = "@Year", Value = year };
                sqlParameters.Add(yearParameter);
            }

            if (!string.IsNullOrWhiteSpace(state))
            {
                SqlParameter stateParameter = new SqlParameter() { ParameterName = "@State", Value = state };
                sqlParameters.Add(stateParameter);
            }

            if (!string.IsNullOrWhiteSpace(searchType))
            {
                SqlParameter searchTypeParameter = new SqlParameter() { ParameterName = "@SearchType", Value = searchType };
                sqlParameters.Add(searchTypeParameter);
            }

            string sqlQuery = string.Format(@"SELECT cast(CONVERT(decimal(10,2), Count(*))/CONVERT(decimal(10,2), 12) AS decimal(10,2)) AS [Value] FROM Audits WHERE TableName = @SearchType  AND YEAR(AuditDate) = @Year 
              AND  JSON_VALUE(NewValues, '$.State') =  @State");

            var outcome = _uow.Repository<DecimalReturn>().QueryFromSqlRaw(sqlQuery, sqlParameters.ToArray());

            if (outcome != null)
                return new Outcome<decimal> { Result = outcome.Value };
            else return new Outcome<decimal>("No record found");
        }

        public Outcome<int> GetStateCountPerMonth(string state, string month, string searchType)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            if (!string.IsNullOrWhiteSpace(month))
            {
                SqlParameter yearParameter = new SqlParameter() { ParameterName = "@Month", Value = month };
                sqlParameters.Add(yearParameter);
            }

            if (!string.IsNullOrWhiteSpace(state))
            {
                SqlParameter stateParameter = new SqlParameter() { ParameterName = "@State", Value = state };
                sqlParameters.Add(stateParameter);
            }

            if (!string.IsNullOrWhiteSpace(searchType))
            {
                SqlParameter searchTypeParameter = new SqlParameter() { ParameterName = "@SearchType", Value = searchType };
                sqlParameters.Add(searchTypeParameter);
            }

            string sqlQuery = string.Format(@"SELECT COUNT(*) AS [Value] FROM Audits WHERE TableName = @SearchType  AND DATENAME(MONTH, AuditDate) =  @Month
               AND  JSON_VALUE(NewValues, '$.State') =  @State  ");

            var outcome = _uow.Repository<IntReturn>().QueryFromSqlRaw(sqlQuery, sqlParameters.ToArray());

            if (outcome != null)
                return new Outcome<int> { Result = outcome.Value };
            else return new Outcome<int>("No record found");
        }

        public Outcome<int> GetStateCountPerDate(DateTime? onDate, string state, string searchType, string farm)
        {
            var result = new Outcome<int>();
            List<string> errorMessages = new List<string>();
            if (!Enum.GetNames(typeof(CowState)).Any(x => x.Equals(state, StringComparison.InvariantCultureIgnoreCase))) errorMessages.Add("State is not valid");

            var farmEntity = GetFarmDetails(farm);
            int? farmId = null;
            if (farmEntity != null)
                farmId = farmEntity.FarmId;
            else
                errorMessages.Add("Farm is not valid");

            if (errorMessages != null && errorMessages.Any())
            {
                result.ErrorMessage = string.Join(",", errorMessages.ToList());
                return result;
            }
            var sqlParameters = new List<SqlParameter>();

            if (!string.IsNullOrWhiteSpace(farm))
            {
                SqlParameter farmIdParameter = new SqlParameter() { ParameterName = "@FarmId", Value = farmId };
                sqlParameters.Add(farmIdParameter);
            }

            if (onDate != null)
            {
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
                SqlParameter stateParameter = new SqlParameter() { ParameterName = "@State", Value = state };
                sqlParameters.Add(stateParameter);
            }

            if (!string.IsNullOrWhiteSpace(searchType))
            {
                SqlParameter searchTypeParameter = new SqlParameter() { ParameterName = "@SearchType", Value = searchType };
                sqlParameters.Add(searchTypeParameter);
            }

            string sqlQuery = string.Format(@"
                    select Cast( JSON_VALUE(KeyValues, '$.CowId') as int) as Id , Count(*) as [Count] from Audits where tablename = @SearchType  and cast(AuditDate as date) = @OnDate
                    and  JSON_VALUE(NewValues, '$.State') =  @State
                    and  JSON_VALUE(NewValues, '$.FarmId') =  @FarmId   
                    group by JSON_VALUE(KeyValues, '$.CowId') ");

            var auditList = _uow.Repository<StatusPerMonthDto>().QueryFromSqlRawReturnList(sqlQuery, sqlParameters.ToArray());
            int count = 0;
            if (auditList == null || !auditList.Any()) result.Result = count;
            else result.Result = auditList.Count;

            return result;
        }


        private Farm GetFarmDetails(string farm)
        {
            var spec = new FarmNameSpecifications(a => a.Name.ToLower() == farm.ToLower());

            var farmEntity = _uow.Repository<Farm>().GetEntityWithSpec(spec).Result;
            return farmEntity;
        }

    }
}
