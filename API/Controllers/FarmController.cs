using System.Collections.Generic;
using System.Threading.Tasks;
using API.Data;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FarmController : ControllerBase
    {
        private readonly FarmContext _farmContext;

        public FarmController(FarmContext farmContext)
        {
            _farmContext = farmContext;
        }

        /// <summary>
        /// Get list of Farms
        /// </summary>
        /// <returns><seealso cref="IReadOnlyList{Farm}"/></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<Farm>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IReadOnlyList<Farm>>> GetFarms()
        {
            var farmList = await _farmContext.Farms.ToListAsync();

            return Ok(farmList);
        }
    }
}