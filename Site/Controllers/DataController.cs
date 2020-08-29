using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using site.Data;

namespace site.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly ISiteContentProvider _provider;

        public DataController(ISiteContentProvider provider)
        {
            _provider = provider;
        }

        [HttpGet("sublocations/{id}")]
        public async Task<IActionResult> GetSubLocations(int id)
        {
            var subLocations = await _provider.GetSubLocations(id);

            return Ok(subLocations);
        }
    }
}
