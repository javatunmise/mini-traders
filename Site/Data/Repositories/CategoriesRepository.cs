using Microsoft.EntityFrameworkCore;
using Site.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace site.Data.Repositories
{
    public class CategoriesRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoriesRepository(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public async Task<List<Shared.Entities.Category>> GetCategories()
        {
            return await _dbContext.Categories.ToListAsync();
        }
    }
}
