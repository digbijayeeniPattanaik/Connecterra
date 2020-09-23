using System;
using System.Collections.Generic;
using API.Providers.Interfaces;
using AutoMapper;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditController : ControllerBase
    {
        private readonly IAuditDataProvider auditDataProvider;

        private readonly IMapper _mapper;

        public AuditController(IAuditDataProvider auditDataProvider, IMapper mapper)
        {
            this.auditDataProvider = auditDataProvider;
            _mapper = mapper;
        }

        [HttpGet()]
        public ActionResult<List<Audit>> GetAuditList(DateTime? onDate, string state, string searchType, string farm)
        {
            var auditList = auditDataProvider.GetAuditList(onDate, state, searchType, farm);
            return Ok(auditList);
        }
    }
}