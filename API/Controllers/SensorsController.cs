using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dtos;
using API.Providers.Interfaces;
using AutoMapper;
using Infrastructure.Custom;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorsController : ControllerBase
    {
        private readonly ISensorDataProvider _sensorDataProvider;
        private readonly IAuditDataProvider _auditDataProvider;
        private readonly IMapper _mapper;

        public SensorsController(ISensorDataProvider sensorDataProvider, IAuditDataProvider auditDataProvider, IMapper mapper)
        {
            _sensorDataProvider = sensorDataProvider;
            _auditDataProvider = auditDataProvider;
            _mapper = mapper;
        }

        /// <summary>
        /// Get list of Sensors
        /// </summary>
        /// <returns><seealso cref="IReadOnlyList{SensorDto}"/></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<SensorDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IReadOnlyList<SensorDto>>> GetSensors()
        {
            var sensorList = await _sensorDataProvider.GetSensors();
            return Ok(_mapper.Map<IReadOnlyList<SensorDto>>(sensorList));
        }

        /// <summary>
        /// Get Sensor based on Sensor ID
        /// </summary>
        /// <param name="id">sensor ID</param>
        /// <returns><seealso cref="SensorDto"/></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SensorDto), StatusCodes.Status200OK)]
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

        /// <summary>
        /// Add a Sensor
        /// </summary>
        /// <param name="sensorDto"> Sensor object </param>
        /// <returns><seealso cref="SensorDto"/></returns>
        [HttpPost]
        [ProducesResponseType(typeof(SensorDto), StatusCodes.Status200OK)]
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

        /// <summary>
        /// Update a Sensor
        /// </summary>
        /// <param name="sensorId">Sensor ID</param>
        /// <param name="stateDto">State dto</param>
        /// <returns><seealso cref="SensorDto"/></returns>
        [HttpPut("{sensorId}")]
        [ProducesResponseType(typeof(SensorDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SensorDto>> Update(int sensorId, [FromBody]StateDto stateDto)
        {
            var outcome = await _sensorDataProvider.Update(sensorId, stateDto);

            if (outcome.Successful)
            {
                var sensorDto = _mapper.Map<SensorDto>(outcome.Result);
                return Ok(sensorDto);
            }

            return BadRequest(outcome.ErrorMessage);
        }

        /// <summary>
        /// Get Sensor Average per month
        /// </summary>
        /// <param name="state">Sensor state like Inventory, Deployed, FarmerTriage, Returned, Dead, Refurbished</param>
        /// <param name="year">Year</param>
        /// <returns>decimal</returns>
        [HttpGet("average")]
        [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IReadOnlyList<DecimalReturn>>> GetSensorsAveragePerMonth(string state, int year)
        {
            var outcome = await Task.FromResult(_auditDataProvider.GetAveragePerMonth(state, year, "Sensors"));
            if (outcome.Successful)
                return Ok(outcome.Result);
            else return BadRequest(outcome.ErrorMessage);
        }

        /// <summary>
        /// Get Sensor Count per month
        /// </summary>
        /// <param name="state">Sensor state like Inventory, Deployed, FarmerTriage, Returned, Dead, Refurbished</param>
        /// <param name="month">Month like January ,February ,March ,April ,May ,June ,July ,August ,September ,October ,November ,December</param>
        /// <returns>int</returns>
        [HttpGet("count")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> GetSensorCountPerMonth(string state, string month)
        {
            var outcome = await Task.FromResult(_auditDataProvider.GetStateCountPerMonth(state, month, "Sensors"));
            if (outcome.Successful)
                return Ok(outcome.Result);
            else return BadRequest(outcome.ErrorMessage);
        }
    }
}