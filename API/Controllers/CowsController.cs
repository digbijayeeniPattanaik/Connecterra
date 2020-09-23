using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using API.Dtos;
using API.Helpers;
using AutoMapper;
using Infrastructure.Common;
using Infrastructure.Entities;
using Infrastructure.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CowsController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CowsController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }
        public async Task<ActionResult<IReadOnlyList<CowDto>>> GetCows()
        {
            var cowSpecification = new CowSpecifications();

            var cowList = await _uow.Repository<Cow>().ListAsync(cowSpecification);
            return Ok(_mapper.Map<IReadOnlyList<CowDto>>(cowList));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CowDto>> GetCow(int id)
        {
            var cowSpecification = new CowSpecifications(a => a.CowId == id);
            var cow = await _uow.Repository<Cow>().GetEntityWithSpec(cowSpecification);

            var mappedDto = _mapper.Map<CowDto>(cow);

            if (mappedDto != null)
                return Ok(mappedDto);
            else
                return NotFound("Cow not found");
        }

        [HttpPost("{cowId}")]
        public async Task<ActionResult<CowDto>> Update(int cowId, [FromBody]StateDto stateDto)
        {
            var cowSpecification = new CowSpecifications(a => a.CowId == cowId);
            var cow = await _uow.Repository<Cow>().GetEntityWithSpec(cowSpecification);
            if (cow != null)
            {
                cow.State = (CowState)Enum.Parse(typeof(CowState), stateDto.State);
                cow.UpdateDt = DateTime.Now;
                _uow.Repository<Cow>().Update(cow);
                int output = await _uow.Complete();

                var cowDto = _mapper.Map<CowDto>(cow);

                return Ok(cowDto);
            }

            return NotFound("Cow not found");
        }
    }
}