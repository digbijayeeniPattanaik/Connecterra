using API.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Providers.Interfaces
{
    public interface ISensorDataProvider
    {
        Task<IReadOnlyList<SensorDto>> GetSensors();
        Task<SensorDto> GetSensor(int id);
        Task<SensorDto> Add(SensorAddDto sensorDto);
        Task<SensorDto> Update(int sensorId, StateDto stateDto);
    }
}
