using Infrastructure.Custom;
using Infrastructure.Entities;
using System;
using System.Collections.Generic;

namespace API.Providers.Interfaces
{
    public interface IAuditDataProvider
    {
        List<Audit> GetAuditList(DateTime? onDate, string state, string searchType, string farm);
        List<AveragePerMonthDto> GetAveragePerMonth(string state, int year, string searchType);
        IntReturn GetCountPerMonth(string state, string month, string searchType);
    }
}
