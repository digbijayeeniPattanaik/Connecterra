using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Providers.Interfaces;
using AutoMapper;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Http;
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

        /// <summary>
        /// Get Audit List
        /// </summary>
        /// <param name="onDate">onDate</param>
        /// <param name="state">state Like if SearchType is Cows then ->State: Open, Inseminated, Pregnant, Dry. If SearchType is Sensor ->State: Inventory, Deployed, FarmerTriage, Returned, Dead, Refurbished</param>
        /// <param name="searchType">searchType like Cows or Sensors</param>
        /// <param name="farm">Farm name</param>
        /// <returns><seealso cref="List{Audit}"/></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<Audit>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Audit>>> GetAuditList(DateTime? onDate, string state, string searchType, string farm)
        {
            var auditList = await Task.FromResult(auditDataProvider.GetAuditList(onDate, state, searchType, farm));
            return Ok(auditList);
        }
    }
}