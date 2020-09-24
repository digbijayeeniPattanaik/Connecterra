using API.Dtos;
using Infrastructure;
using Infrastructure.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Providers.Interfaces
{
    public interface ICowDataProvider
    {
        Task<IReadOnlyList<Cow>> GetCows();

        Task<Cow> GetCow(int id);

        Task<Outcome<Cow>> Update(int cowId, StateDto stateDto);
    }
}
