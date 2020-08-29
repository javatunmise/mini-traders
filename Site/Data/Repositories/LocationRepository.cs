using Microsoft.EntityFrameworkCore;
using Site.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace site.Data.Repositories
{
    public class LocationRepository
    {
        private readonly ApplicationDbContext context;

        public LocationRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<MarketLocation>> GetCampuses()
        {
            var campuses = await context.Campuses.ToListAsync();
            return campuses.Select(e => new MarketLocation { Id = e.Id, Name = e.Name });
        }

        public async Task<IEnumerable<MarketLocation>> GetHostels(int campusId)
        {
            var hostels = await context.Hostels.Where(e => e.CampusId == campusId).ToListAsync();
            return hostels.Select(e => new MarketLocation { Id = e.Id, Name = e.Name });
        }
    }
}
