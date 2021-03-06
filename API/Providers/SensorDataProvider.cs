﻿using API.Dtos;
using API.Helpers;
using API.Providers.Interfaces;
using Infrastructure;
using Infrastructure.Common;
using Infrastructure.Entities;
using Infrastructure.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Providers
{
    public class SensorDataProvider : ISensorDataProvider
    {
        private readonly IUnitOfWork _uow;
        private readonly IAuditDataProvider _auditDataProvider;

        public SensorDataProvider(IUnitOfWork uow, IAuditDataProvider auditDataProvider)
        {
            _uow = uow;
            _auditDataProvider = auditDataProvider;
        }

        public async Task<Outcome<Sensor>> Add(SensorAddDto sensorDto)
        {
            var result = new Outcome<Sensor>();

            if (!Enum.GetNames(typeof(SensorState)).Any(x => x.Equals(sensorDto.State, StringComparison.InvariantCultureIgnoreCase)))
            {
                result.ErrorMessage = "State is not valid";
                return result;
            }

            var farm = await _uow.Repository<Farm>().GetEntityWithSpec(new FarmNameSpecifications(a => a.Name.ToLower() == sensorDto.Farm.ToLower()));

            if (farm != null)
            {
                var state = Enum.GetNames(typeof(SensorState)).FirstOrDefault(x => x.Equals(sensorDto.State, StringComparison.InvariantCultureIgnoreCase));
                var sensor = new Sensor();
                sensor.FarmId = farm.FarmId;
                sensor.State = (SensorState)Enum.Parse(typeof(SensorState), state);

                sensor.CreateDt = DateTime.Now;
                _uow.Repository<Sensor>().Add(sensor);
                int output = await _uow.Complete();

                if (output >= 1)
                {
                    result.Result = sensor;
                    return result;
                }
            }
            else result.ErrorMessage = "Farm not found";

            return result;
        }

        public async Task<Sensor> GetSensor(int id)
        {
            var sensorSpecification = new SensorSpecifications(a => a.SensorId == id);
            var sensor = await _uow.Repository<Sensor>().GetEntityWithSpec(sensorSpecification);
            return sensor;
        }

        public async Task<IReadOnlyList<Sensor>> GetSensors()
        {
            var sensorSpecification = new SensorSpecifications();
            var sensorList = await _uow.Repository<Sensor>().ListAsync(sensorSpecification);
            return sensorList;
        }

        public async Task<Outcome<Sensor>> Update(int sensorId, StateDto stateDto)
        {
            var result = new Outcome<Sensor>();

            if (!Enum.GetNames(typeof(SensorState)).Any(x => x.Equals(stateDto.State, StringComparison.InvariantCultureIgnoreCase)))
            {
                result.ErrorMessage = "State is not valid";
                return result;
            }

            var sensor = _uow.Repository<Sensor>().GetEntityWithSpec(new SensorSpecifications(a => a.SensorId == sensorId)).Result;

            if (sensor != null)
            {
                var auditData = _auditDataProvider.GetAuditList(DateTime.Now.Date, string.Empty, "Sensors", sensor.Farm.Name, sensor.SensorId);
                if (auditData != null && auditData.Any())
                {
                    sensor.RecordFlag = "Error";
                }
                var state = Enum.GetNames(typeof(SensorState)).FirstOrDefault(x => x.Equals(stateDto.State, StringComparison.InvariantCultureIgnoreCase));
                sensor.State = (SensorState)Enum.Parse(typeof(SensorState), state);
                sensor.UpdateDt = DateTime.Now;
                _uow.Repository<Sensor>().Update(sensor);
                int output = await _uow.Complete();
                result.Result = sensor;
            }
            else result.ErrorMessage = "Sensor not found";

            return result;

        }
    }
}
