using System.Collections.Generic;
using System.Threading.Tasks;
using API.Data;
using AutoMapper;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditController : ControllerBase
    {
        private readonly FarmContext _farmContext;
        private readonly IMapper _mapper;

        public AuditController(FarmContext farmContext, IMapper mapper)
        {
            _farmContext = farmContext;
            _mapper = mapper;
        }

        public async Task<ActionResult<IReadOnlyList<Audit>>> GetAuditList()
        {
            var auditList = await _farmContext.Audits.ToListAsync();

            return Ok(auditList);
        }
    }
}