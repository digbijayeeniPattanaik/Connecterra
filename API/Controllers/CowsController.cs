using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using API.Data;
using API.Dtos;
using API.Helpers;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CowsController : ControllerBase
    {
        private readonly FarmContext _farmContext;
        private readonly IMapper _mapper;

        public CowsController(FarmContext farmContext, IMapper mapper)
        {
            _farmContext = farmContext;
            _mapper = mapper;
        }
        public async Task<ActionResult<IReadOnlyList<CowDto>>> GetCows()
        {
            var cowList = await _farmContext.Cows.Include(a => a.Farm).ToListAsync();

            return Ok(_mapper.Map<IReadOnlyList<CowDto>>(cowList));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CowDto>> GetCow(int id)
        {
            var cow = await _farmContext.Cows.Include(a => a.Farm).FirstOrDefaultAsync(a => a.CowId == id);

            var mappedDto = _mapper.Map<CowDto>(cow);

            if (mappedDto != null)
                return Ok(mappedDto);
            else
                return NotFound((int)HttpStatusCode.NotFound);
        }

        [HttpPost]
        public async Task<ActionResult<CowDto>> Update([FromBody]CowDto cowDto)
        {
            var cow = await _farmContext.Cows.Include(a => a.Farm).FirstOrDefaultAsync(a => a.CowId == cowDto.CowId);
            cow.State = (CowState)Enum.Parse(typeof(CowState), cowDto.State);
            cow.UpdateDt = DateTime.Now;
            _farmContext.Cows.Attach(cow);
            _farmContext.Entry(cow).State = EntityState.Modified;
            int output = await _farmContext.SaveChangesAsync();
            cowDto = _mapper.Map<CowDto>(cow);
            return Ok(cowDto);
        }

        [HttpPost("{cowId}")]
        public async Task<ActionResult<CowDto>> UpdateState(int cowId, [FromBody]StateDto stateDto)
        {
            var cow = await _farmContext.Cows.Include(a => a.Farm).FirstOrDefaultAsync(a => a.CowId == cowId);
            cow.State = (CowState)Enum.Parse(typeof(CowState), stateDto.State);
            cow.UpdateDt = DateTime.Now;
            _farmContext.Cows.Attach(cow);
            _farmContext.Entry(cow).State = EntityState.Modified;
            int output = await _farmContext.SaveChangesAsync();

            var cowDto = _mapper.Map<CowDto>(cow);

            return Ok(cowDto);
        }
    }
}