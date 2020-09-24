using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Helpers;
using API.Providers.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CowsController : ControllerBase
    {
        private readonly ICowDataProvider _cowDataProvider;
        private readonly IAuditDataProvider _auditDataProvider;
        private readonly IMapper _mapper;

        public CowsController(ICowDataProvider cowDataProvider,IAuditDataProvider auditDataProvider, IMapper mapper)
        {
            _cowDataProvider = cowDataProvider;
            _auditDataProvider = auditDataProvider;
            _mapper = mapper;
        }

        [HttpGet()]
        [ProducesResponseType(typeof(IReadOnlyList<CowDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IReadOnlyList<CowDto>>> GetCows()
        {
            var cowList = await _cowDataProvider.GetCows();
            return Ok(_mapper.Map<IReadOnlyList<CowDto>>(cowList));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CowDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CowDto>> GetCow(int id)
        {
            var cow = await _cowDataProvider.GetCow(id);

            var mappedDto = _mapper.Map<CowDto>(cow);

            if (mappedDto != null)
                return Ok(mappedDto);
            else
                return NotFound("Cow not found");
        }

        [HttpPost("{cowId}")]
        [ProducesResponseType(typeof(CowDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CowDto>> Update(int cowId, [FromBody]StateDto stateDto)
        {
            var cow = await _cowDataProvider.Update(cowId, stateDto);

            if (cow != null)
            {
                var cowDto = _mapper.Map<CowDto>(cow);
                return Ok(cowDto);
            }

            return NotFound("Cow not found");
        }

        [HttpGet("count")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<int>> GetCowCountBasedOnDate(string farm, string state, DateTime onDate)
        {
            var count = await Task.FromResult(_auditDataProvider.GetStateCountPerDate(onDate, state, "Cows", farm));

            return Ok(count);
        }

    }
}