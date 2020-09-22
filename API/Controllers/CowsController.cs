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
    public class CowsController : ControllerBase
    {
        private readonly FarmContext _farmContext;

        public CowsController(FarmContext farmContext)
        {
            _farmContext = farmContext;
        }
        public async Task<ActionResult<IReadOnlyCollection<Cow>>> GetCows()
        {
            return Ok(await _farmContext.Cows.ToListAsync());
        }

        public async Task<ActionResult<Cow>> Update(Cow cow)
        {
            _farmContext.Cows.Update(cow);
            var outcome = await _farmContext.SaveChangesAsync();

            return Ok(cow);
        }
    }
}