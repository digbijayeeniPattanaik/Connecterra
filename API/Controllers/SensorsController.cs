using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dtos;
using API.Providers.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorsController : ControllerBase
    {
        private readonly ISensorDataProvider _sensorDataProvider;
        private readonly IMapper _mapper;

        public SensorsController(ISensorDataProvider sensorDataProvider, IMapper mapper)
        {
            _sensorDataProvider = sensorDataProvider;
            _mapper = mapper;
        }

        [HttpGet()]
        public async Task<ActionResult<IReadOnlyList<SensorDto>>> GetSensors()
        {
            var sensorList = await _sensorDataProvider.GetSensors();
            return Ok(_mapper.Map<IReadOnlyList<SensorDto>>(sensorList));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SensorDto>> GetSensor(int id)
        {
            var sensor = await _sensorDataProvider.GetSensor(id);
            var mappedDto = _mapper.Map<SensorDto>(sensor);

            if (mappedDto != null)
                return Ok(mappedDto);
            else
                return NotFound("Sensor not found");
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SensorDto>> Add([FromBody]SensorAddDto sensorDto)
        {
            var sensor = await _sensorDataProvider.Add(sensorDto);

            if (sensor != null)
            {
                return Ok(_mapper.Map<SensorDto>(sensor));
            }

            return NotFound("Farm not found");
        }

        [HttpPost("{sensorId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SensorDto>> Update(int sensorId, [FromBody]StateDto stateDto)
        {
            var sensor = await _sensorDataProvider.Update(sensorId, stateDto);

            if (sensor != null)
            {
                var sensorDto = _mapper.Map<SensorDto>(sensor);
                return Ok(sensorDto);
            }

            return NotFound("Sensor not found");
        }
    }
}