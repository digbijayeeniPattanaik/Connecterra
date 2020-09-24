using Infrastructure.Custom;
using Infrastructure.Entities;
using System;
using System.Collections.Generic;

namespace API.Providers.Interfaces
{
    public interface IAuditDataProvider
    {
        IEnumerable<Audit> GetAuditList(DateTime? onDate, string state, string searchType, string farm);
        IEnumerable<AveragePerMonthDto> GetAveragePerMonth(string state, int year, string searchType);
        IntReturn GetStateCountPerMonth(string state, string month, string searchType);
        int GetStateCountPerDate(DateTime? onDate, string state, string searchType, string farm);
    }
}
