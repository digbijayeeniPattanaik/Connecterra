using API.Dtos;
using Infrastructure;
using Infrastructure.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Providers.Interfaces
{
    public interface ISensorDataProvider
    {
        Task<IReadOnlyList<Sensor>> GetSensors();
        Task<Sensor> GetSensor(int id);
        Task<Sensor> Add(SensorAddDto sensorDto);
        Task<Outcome<Sensor>> Update(int sensorId, StateDto stateDto);
    }
}
