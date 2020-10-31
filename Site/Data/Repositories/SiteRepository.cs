using Microsoft.EntityFrameworkCore;
using Shared.Entities;
using site.Helpers;
using Site.Data;
using System.Threading.Tasks;

namespace site.Repositories
{
    public class SiteRepository
    {
        private readonly ApplicationDbContext _context;

        public SiteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        internal async Task UpdateSiteUser(SiteUser updated)
        {
            var _siteUser = await _context.SiteUsers.FirstOrDefaultAsync(e => e.Id == updated.Id);
            if(_siteUser != null)
            {
                _siteUser.FirstName = updated.FirstName;
                _siteUser.LastName = updated.LastName;
                if (!updated.ProfilePicturePath.IsEmpty())
                    _siteUser.ProfilePicturePath = updated.ProfilePicturePath;

                await _context.SaveChangesAsync();
            }
        }

    }
}
