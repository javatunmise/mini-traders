using Microsoft.EntityFrameworkCore;
using Sellers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace site.Repositories
{
    public interface ISellerRepository
    {
        Task Save(Store store);
    }


    public class SellerRepository : ISellerRepository
    {
        private readonly DbContext dbContext;

        public SellerRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task Save(Store store)
        {
            throw new NotImplementedException();
        }
    }
}
