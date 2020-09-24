using System;
using System.Collections.Generic;
using API.Providers.Interfaces;
using AutoMapper;
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

        public List<Audit> GetAuditList(DateTime? onDate, string state, string searchType, string farm)
        {
            string whereClause = string.Empty;
            if (onDate != null || !string.IsNullOrWhiteSpace(state) || !string.IsNullOrWhiteSpace(searchType) || !string.IsNullOrWhiteSpace(farm)) whereClause = " WHERE ";

            var sqlParameters = new List<SqlParameter>();

            int? farmId = null;
            if (!string.IsNullOrWhiteSpace(farm))
            {
                var spec = new FarmNameSpecifications(a => a.Name.ToLower() == farm.ToLower());

                var farmEntity = _uow.Repository<Farm>().GetEntityWithSpec(spec).Result;

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

        public List<AveragePerMonthDto> GetAveragePerMonth(string state, int year, string searchType)
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

            string sqlQuery = string.Format(@"SELECT DATENAME(MONTH, AuditDate) AS [Month], COUNT(*) AS [Count] FROM Audits WHERE TableName = @SearchType  AND YEAR(AuditDate) = @Year 
              AND  JSON_VALUE(NewValues, '$.State') =  @State
              GROUP BY DATENAME(MONTH, AuditDate)");

            var auditList = _uow.Repository<AveragePerMonthDto>().QueryFromSqlRawReturnList(sqlQuery, sqlParameters.ToArray());

            return auditList;
        }

        public IntReturn GetCountPerMonth(string state, string month, string searchType)
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

            var count = _uow.Repository<IntReturn>().QueryFromSqlRaw(sqlQuery, sqlParameters.ToArray());

            return count;
        }

        public IntReturn GetStatePerMonth(string state, int month, string searchType)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            if (month != 0)
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

            string sqlQuery = string.Format(@"SELECT MONTH(AuditDate) AS [Month], COUNT(*) AS [Count] FROM Audits WHERE TableName = @SearchType  AND YEAR(AuditDate) = @Year 
              AND  JSON_VALUE(NewValues, '$.State') =  @State
              GROUP BY MONTH(AuditDate)");

            var count = _uow.Repository<IntReturn>().QueryFromSqlRaw(sqlQuery, sqlParameters.ToArray());

            return count;
        }
    }
}
