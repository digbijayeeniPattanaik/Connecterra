using System.Collections.Generic;
using System.Threading.Tasks;
using API.Data;
using Infrastructure.Entities;
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
        public async Task<ActionResult<IReadOnlyList<Farm>>> GetCows()
        {
            var farmList = await _farmContext.Farms.ToListAsync();

            return Ok(farmList);
        }
    }
}