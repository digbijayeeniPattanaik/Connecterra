using Infrastructure;
using Infrastructure.Custom;
using Infrastructure.Entities;
using System;
using System.Collections.Generic;

namespace API.Providers.Interfaces
{
    public interface IAuditDataProvider
    {
        IEnumerable<Audit> GetAuditList(DateTime? onDate, string state, string searchType, string farm);
        Outcome<decimal> GetAveragePerMonth(string state, int year, string searchType);
        Outcome<int> GetStateCountPerMonth(string state, string month, string searchType);
        Outcome<int> GetStateCountPerDate(DateTime? onDate, string state, string searchType, string farm);
    }
}
