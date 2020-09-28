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
    public class CowsControllerTest
    {
        private Mock<ICowDataProvider> _mockcowDataProvider;
        private Mock<IAuditDataProvider> _mockauditDataProvider;
        private Mock<IMapper> _mockmapper;
        private CowsController cowsController;

        [TestInitialize]
        public void Init()
        {
            _mockcowDataProvider = new Mock<ICowDataProvider>();
            _mockauditDataProvider = new Mock<IAuditDataProvider>();
            _mockmapper = new Mock<IMapper>();

            cowsController = new CowsController(_mockcowDataProvider.Object, _mockauditDataProvider.Object, _mockmapper.Object);
        }

        [TestMethod]
        public async Task Test_GetCows_WhenResults_Successful()
        {
            _mockcowDataProvider.Setup(a => a.GetCows()).ReturnsAsync(new List<Cow>()
            {
              new Cow(){ CowId = 1, CreateDt = DateTime.Now, FarmId = 1 , State = API.Helpers.CowState.Dry}
            });

            _mockmapper.Setup(a => a.Map<IReadOnlyList<CowDto>>(It.IsAny<IReadOnlyList<Cow>>())).Returns(new List<CowDto>()
            {
              new CowDto(){ CowId = 1, Farm = "Rich", State =  API.Helpers.CowState.Dry.ToString()}
            });

            var result = await cowsController.GetCows();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Microsoft.AspNetCore.Mvc.ActionResult<IReadOnlyList<CowDto>>));
        }

        [TestMethod]
        public async Task Test_GetCow_WhenResults_Successful()
        {
            _mockcowDataProvider.Setup(a => a.GetCow(It.IsAny<int>())).ReturnsAsync(
              new Cow() { CowId = 1, CreateDt = DateTime.Now, FarmId = 1, State = API.Helpers.CowState.Dry }
            );

            _mockmapper.Setup(a => a.Map<CowDto>(It.IsAny<Cow>())).Returns(
              new CowDto() { CowId = 1, Farm = "Rich", State = API.Helpers.CowState.Dry.ToString() }
            );

            var result = await cowsController.GetCow(1);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Microsoft.AspNetCore.Mvc.ActionResult<CowDto>));
        }

        [TestMethod]
        public async Task Test_Update_Successful()
        {
            _mockcowDataProvider.Setup(a => a.Update(It.IsAny<int>(), It.IsAny<StateDto>())).ReturnsAsync(
              new Outcome<Cow>()
              {
                  Result = new Cow() { CowId = 1, CreateDt = DateTime.Now, FarmId = 1, State = API.Helpers.CowState.Dry }
              }
            );

            _mockmapper.Setup(a => a.Map<CowDto>(It.IsAny<Cow>())).Returns(
              new CowDto() { CowId = 1, Farm = "Rich", State = API.Helpers.CowState.Dry.ToString() }
            );

            var result = await cowsController.Update(It.IsAny<int>(), It.IsAny<StateDto>());

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Microsoft.AspNetCore.Mvc.ActionResult<CowDto>));
        }

        [TestMethod]
        public async Task Test_GetCowCountBasedOnDate_Successful()
        {
            _mockauditDataProvider.Setup(a => a.GetStateCountPerDate(It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(
              new Outcome<int> { Result = 20 }
            );
            
            var result = await cowsController.GetCowCountBasedOnDate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>());

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Microsoft.AspNetCore.Mvc.ActionResult<int>));
        }
    }
}
