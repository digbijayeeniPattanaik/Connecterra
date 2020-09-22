using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using API.Data;
using API.Dtos;
using API.Helpers;
using AutoMapper;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorsController : ControllerBase
    {
        private readonly FarmContext _farmContext;
        private readonly IMapper _mapper;

        public SensorsController(FarmContext farmContext, IMapper mapper)
        {
            _farmContext = farmContext;
            _mapper = mapper;
        }
        public async Task<ActionResult<IReadOnlyList<SensorDto>>> GetSensors()
        {
            var sensorList = await _farmContext.Sensors.Include(a => a.Farm).ToListAsync();

            return Ok(_mapper.Map<IReadOnlyList<SensorDto>>(sensorList));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SensorDto>> GetSensor(int id)
        {
            var sensor = await _farmContext.Sensors.Include(a => a.Farm).FirstOrDefaultAsync(a => a.SensorId == id);

            var mappedDto = _mapper.Map<SensorDto>(sensor);

            if (mappedDto != null)
                return Ok(mappedDto);
            else
                return NotFound((int)HttpStatusCode.NotFound);
        }

        [HttpPost]
        public async Task<ActionResult<SensorDto>> Add([FromBody]SensorAddDto sensorDto)
        {
            var farm = await _farmContext.Farms.Where(a => a.Name == sensorDto.Farm).FirstOrDefaultAsync();

            if (farm != null)
            {
                var sensor = new Sensor();
                sensor.FarmId = farm.FarmId;
                sensor.State = (SensorState)Enum.Parse(typeof(SensorState), sensorDto.State);
                sensor.CreateDt = DateTime.Now;
                _farmContext.Sensors.Add(sensor);
                _farmContext.Entry(sensor).State = EntityState.Added;
                int output = await _farmContext.SaveChangesAsync();
                return Ok(_mapper.Map<SensorDto>(sensor));
            }

            return NotFound("Farm not found");
        }

        [HttpPost("{sensorId}")]
        public async Task<ActionResult<SensorDto>> Update(int sensorId, [FromBody]StateDto stateDto)
        {
            var sensor = await _farmContext.Sensors.Include(a => a.Farm).FirstOrDefaultAsync(a => a.SensorId == sensorId);
            sensor.State = (SensorState)Enum.Parse(typeof(SensorState), stateDto.State);
            sensor.UpdateDt = DateTime.Now;
            _farmContext.Sensors.Attach(sensor);
            _farmContext.Entry(sensor).State = EntityState.Modified;
            int output = await _farmContext.SaveChangesAsync();

            var sensorDto = _mapper.Map<SensorDto>(sensor);

            return Ok(sensorDto);
        }
    }
}