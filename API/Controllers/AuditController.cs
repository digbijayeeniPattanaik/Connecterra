using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Common;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public AuditController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ActionResult<IReadOnlyList<Audit>>> GetAuditList(string farm, string searchType, DateTime onDate)
        {
            var auditList = await _uow.Repository<Audit>().GetListAllAsync();

            return Ok(auditList);
        }
    }
}