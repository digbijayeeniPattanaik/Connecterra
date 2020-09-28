using API.Controllers;
using API.Providers.Interfaces;
using AutoMapper;
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
    public class AuditControllerTest
    {
        private AuditController auditController;
        private Mock<IAuditDataProvider> _mockAuditDataProvider;
        private Mock<IMapper> _mockMapper;

        [TestInitialize]
        public void Init()
        {
            _mockAuditDataProvider = new Mock<IAuditDataProvider>();
            _mockMapper = new Mock<IMapper>();

            auditController = new AuditController(_mockAuditDataProvider.Object, _mockMapper.Object);
        }

        [TestMethod]
        public async Task Test_GetAudit_WhenResults_Successful()
        {
            _mockAuditDataProvider.Setup(a => a.GetAuditList(It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new List<Audit>()
            {
              new Audit(){ AuditId = 1, AuditDate = DateTime.Now,TableName = "Test"}
            });

            var result = await auditController.GetAuditList(It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Microsoft.AspNetCore.Mvc.ActionResult<List<Audit>>));
        }
    }
}
