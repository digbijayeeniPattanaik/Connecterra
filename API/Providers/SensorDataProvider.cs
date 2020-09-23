using API.Dtos;
using API.Providers.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Providers
{
    public class SensorDataProvider : ISensorDataProvider
    {
        public Task<SensorDto> Add(SensorAddDto sensorDto)
        {
            throw new System.NotImplementedException();
        }

        public Task<SensorDto> GetSensor(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<IReadOnlyList<SensorDto>> GetSensors()
        {
            throw new System.NotImplementedException();
        }

        public Task<SensorDto> Update(int sensorId, StateDto stateDto)
        {
            throw new System.NotImplementedException();
        }
    }
}
