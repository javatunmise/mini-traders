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
            var locations = await _provider.GetLocations();
            var subLocations = locations.Where(x => x.Parent?.Id > 0 && x.Parent?.Id == id);

            return Ok(subLocations);
        }
    }
}
