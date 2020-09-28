using API.Controllers;
using API.Dtos;
using API.Providers.Interfaces;
using AutoMapper;
using Infrastructure;
using Infrastructure.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnitTests.Controllers
{
    [TestClass]
    public class SensorsControllerTest
    {
        private Mock<ISensorDataProvider> _mocksensorDataProvider;
        private Mock<IAuditDataProvider> _mockauditDataProvider;
        private Mock<IMapper> _mockmapper;
        private SensorsController sensorsController;

        [TestInitialize]
        public void Init()
        {
            _mocksensorDataProvider = new Mock<ISensorDataProvider>();
            _mockauditDataProvider = new Mock<IAuditDataProvider>();
            _mockmapper = new Mock<IMapper>();

            sensorsController = new SensorsController(_mocksensorDataProvider.Object, _mockauditDataProvider.Object, _mockmapper.Object);
        }

        [TestMethod]
        public async Task Test_GetSensors_WhenResults_Successful()
        {
            _mocksensorDataProvider.Setup(a => a.GetSensors()).ReturnsAsync(new List<Sensor>()
            {
              new Sensor(){ SensorId = 1, CreateDt = DateTime.Now, FarmId = 1 , State = API.Helpers.SensorState.Dead}
            });

            _mockmapper.Setup(a => a.Map<IReadOnlyList<SensorDto>>(It.IsAny<IReadOnlyList<Sensor>>())).Returns(new List<SensorDto>()
            {
              new SensorDto(){ SensorId = 1, Farm = "Rich", State =  API.Helpers.SensorState.Dead.ToString()}
            });

            var result = await sensorsController.GetSensors();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Microsoft.AspNetCore.Mvc.ActionResult<IReadOnlyList<SensorDto>>));
        }

        [TestMethod]
        public async Task Test_GetSensor_WhenResults_Successful()
        {
            _mocksensorDataProvider.Setup(a => a.GetSensor(It.IsAny<int>())).ReturnsAsync(new Sensor() { SensorId = 1, CreateDt = DateTime.Now, FarmId = 1, State = API.Helpers.SensorState.Dead }
            );

            _mockmapper.Setup(a => a.Map<SensorDto>(It.IsAny<Sensor>())).Returns(
              new SensorDto() { SensorId = 1, Farm = "Rich", State = API.Helpers.SensorState.Dead.ToString() }
            );

            var result = await sensorsController.GetSensor(1);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Microsoft.AspNetCore.Mvc.ActionResult<SensorDto>));
        }

        [TestMethod]
        public async Task Test_Update_Successful()
        {
            _mocksensorDataProvider.Setup(a => a.Update(It.IsAny<int>(), It.IsAny<StateDto>())).ReturnsAsync(
              new Outcome<Sensor>()
              {
                  Result = new Sensor() { SensorId = 1, CreateDt = DateTime.Now, FarmId = 1, State = API.Helpers.SensorState.Dead }
              }
            );

            _mockmapper.Setup(a => a.Map<SensorDto>(It.IsAny<Sensor>())).Returns(
              new SensorDto() { SensorId = 1, Farm = "Rich", State = API.Helpers.SensorState.Dead.ToString() }
            );

            var result = await sensorsController.Update(It.IsAny<int>(), It.IsAny<StateDto>());

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Microsoft.AspNetCore.Mvc.ActionResult<SensorDto>));
        }

        [TestMethod]
        public async Task Test_GetSensorCountPerMonth_Successful()
        {
            _mockauditDataProvider.Setup(a => a.GetStateCountPerMonth(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(
              new Outcome<int> { Result = 20 }
            );

            var result = await sensorsController.GetSensorCountPerMonth(It.IsAny<string>(), It.IsAny<string>());

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Microsoft.AspNetCore.Mvc.ActionResult<int>));
        }

        [TestMethod]
        public async Task Test_GetSensorsAveragePerMonth_Successful()
        {
            _mockauditDataProvider.Setup(a => a.GetAveragePerMonth(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>())).Returns(
              new Outcome<decimal> { Result = 20 }
            );

            var result = await sensorsController.GetSensorsAveragePerMonth(It.IsAny<string>(), It.IsAny<int>());

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Microsoft.AspNetCore.Mvc.ActionResult<decimal>));
        }
    }
}