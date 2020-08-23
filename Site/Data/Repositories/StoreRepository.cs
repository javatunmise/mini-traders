using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Entities;
using Site.Data;
using SQLitePCL;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace site.Repositories
{
    public class StoreRepository
    {
        private readonly ApplicationDbContext _context;

        public StoreRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task Create(Shared.Store store)
        {
            var campus = _context.Campuses.Find(store.CampusId);

            EnsureNonNull(campus, "Campus");

            Hostel hostel = null;
            if (store.HostelId > 0)
            {
                hostel = _context.Hostels.Find(store.HostelId);
                EnsureNonNull(hostel, "Hostel");
            }

            var _store = new Shared.Entities.Store()
            {
                Campus = campus,
                Hostel = hostel,
                StoreName = store.Name,
                StoreDescription = store.StoreDescription,
                ReferrerCode = store.ReferrerCode,
                LogoPath = store.LogoPath,
                UploadedDocPath = store.UploadedDocLocation,
                SiteUserId = store.Owner.Id
            };

            _context.Stores.Add(_store);

            return _context.SaveChangesAsync();
        }

        public Task Update(Shared.Store updatedStore)
        {
            var _store = _context.Stores.FirstOrDefault(_ => _.SiteUserId == updatedStore.Owner.Id);

            _store.StoreName = updatedStore.Name;
            _store.StoreDescription = updatedStore.StoreDescription;
            _store.PhoneNumber = updatedStore.PhoneNumber;

            return _context.SaveChangesAsync();
        }

        private string UpdateIfChanged(string oldValue, string newValue)
        {
            return oldValue != newValue ? newValue : oldValue;
        }

        private void EnsureNonNull(object o, string paramName)
        {
            if (o == null)
                throw new ArgumentNullException(paramName);
        }
    }
}
