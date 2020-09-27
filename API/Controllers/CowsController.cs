using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dtos;
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

        public CowsController(ICowDataProvider cowDataProvider, IAuditDataProvider auditDataProvider, IMapper mapper)
        {
            _cowDataProvider = cowDataProvider;
            _auditDataProvider = auditDataProvider;
            _mapper = mapper;
        }

        /// <summary>
        /// Get list of cows
        /// </summary>
        /// <returns><seealso cref="IReadOnlyList{CowDto}"/></returns>
        [HttpGet()]
        [ProducesResponseType(typeof(IReadOnlyList<CowDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IReadOnlyList<CowDto>>> GetCows()
        {
            var cowList = await _cowDataProvider.GetCows();
            return Ok(_mapper.Map<IReadOnlyList<CowDto>>(cowList));
        }

        /// <summary>
        /// Get Cow based on cow ID
        /// </summary>
        /// <param name="id">Cow Id</param>
        /// <returns><seealso cref="CowDto"/></returns>
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

        /// <summary>
        /// Update the cow's status based on CowId
        /// </summary>
        /// <param name="cowId">cowId to update cow</param>
        /// <param name="stateDto">Dto to update status</param>
        /// <returns><seealso cref="CowDto"/></returns>
        [HttpPut("{cowId}")]
        [ProducesResponseType(typeof(CowDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CowDto>> Update(int cowId, [FromBody]StateDto stateDto)
        {
            var outcome = await _cowDataProvider.Update(cowId, stateDto);

            if (outcome.Successful)
            {
                var cowDto = _mapper.Map<CowDto>(outcome.Result);
                return Ok(cowDto);
            }

            return BadRequest(outcome.ErrorMessage);
        }

        /// <summary>
        /// Get Cow count base on farm , state and Date
        /// </summary>
        /// <param name="farm">farm</param>
        /// <param name="state">state like Open, Inseminated, Pregnant, Dry</param>
        /// <param name="onDate">onDate</param>
        /// <returns>int</returns>
        [HttpGet("count")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> GetCowCountBasedOnDate(string farm, string state, DateTime onDate)
        {
            var countOutcome = await Task.FromResult(_auditDataProvider.GetStateCountPerDate(onDate, state, "Cows", farm));

            if (countOutcome.Successful)
                return Ok(countOutcome.Result);

            return BadRequest(countOutcome.ErrorMessage);
        }

    }
}