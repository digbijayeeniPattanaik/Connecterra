using API.Controllers;
using AutoMapper;
using Infrastructure.Common;
using Infrastructure.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Controllers
{
    [TestClass]
    public class FarmControllerTest
    {
        private FarmController farmController;
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IMapper> _mockMapper;

        [TestInitialize]
        public void Init()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();

            farmController = new FarmController(_mockUnitOfWork.Object);
        }

        [TestMethod]
        public async Task Test_GetFarms_WhenResults_Successful()
        {
            _mockUnitOfWork.Setup(a => a.Repository<Farm>().GetListAllAsync()).ReturnsAsync(new List<Farm>()
            {
              new Farm(){ FarmId = 1, Name = "Test"}
            });

            var result = await farmController.GetFarms();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Microsoft.AspNetCore.Mvc.ActionResult<IReadOnlyList<Farm>>));
        }
    }
}
