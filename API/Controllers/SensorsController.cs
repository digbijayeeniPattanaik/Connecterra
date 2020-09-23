using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dtos;
using API.Helpers;
using AutoMapper;
using Infrastructure.Common;
using Infrastructure.Entities;
using Infrastructure.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorsController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public SensorsController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }
        public async Task<ActionResult<IReadOnlyList<SensorDto>>> GetSensors()
        {
            var sensorSpecification = new SensorSpecifications();
            var sensorList = await _uow.Repository<Sensor>().ListAsync(sensorSpecification);

            return Ok(_mapper.Map<IReadOnlyList<SensorDto>>(sensorList));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SensorDto>> GetSensor(int id)
        {
            var sensorSpecification = new SensorSpecifications(a => a.SensorId == id);
            var sensor = await _uow.Repository<Sensor>().ListAsync(sensorSpecification);
            var mappedDto = _mapper.Map<SensorDto>(sensor);

            if (mappedDto != null)
                return Ok(mappedDto);
            else
                return NotFound("Sensor not found");
        }

        [HttpPost]
        public async Task<ActionResult<SensorDto>> Add([FromBody]SensorAddDto sensorDto)
        {
            var farm = await _uow.Repository<Farm>().GetEntityWithSpec(new FarmNameSpecifications(a=> a.Name.ToLower() == sensorDto.Farm.ToLower()));

            if (farm != null)
            {
                var sensor = new Sensor();
                sensor.FarmId = farm.FarmId;
                sensor.State = (SensorState)Enum.Parse(typeof(SensorState), sensorDto.State);
                sensor.CreateDt = DateTime.Now;
                _uow.Repository<Sensor>().Add(sensor);
                int output = await _uow.Complete();
                return Ok(_mapper.Map<SensorDto>(sensor));
            }

            return NotFound("Farm not found");
        }

        [HttpPost("{sensorId}")]
        public async Task<ActionResult<SensorDto>> Update(int sensorId, [FromBody]StateDto stateDto)
        {
            var sensor = await _uow.Repository<Sensor>().GetEntityWithSpec(new SensorSpecifications(a => a.SensorId == sensorId));

            if (sensor != null)
            {
                sensor.State = (SensorState)Enum.Parse(typeof(SensorState), stateDto.State);
                sensor.UpdateDt = DateTime.Now;
                _uow.Repository<Sensor>().Update(sensor);
                int output = await _uow.Complete();

                var sensorDto = _mapper.Map<SensorDto>(sensor);

                return Ok(sensorDto);
            }

            return NotFound("Sensor not found");
        }
    }
}