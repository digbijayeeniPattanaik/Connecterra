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
        private readonly IMapper _mapper;

        public CowsController(ICowDataProvider cowDataProvider, IMapper mapper)
        {
            _cowDataProvider = cowDataProvider;
            _mapper = mapper;
        }

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IReadOnlyList<CowDto>>> GetCows()
        {
            var cowList = await _cowDataProvider.GetCows();
            return Ok(_mapper.Map<IReadOnlyList<CowDto>>(cowList));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
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
    }
}