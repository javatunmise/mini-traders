using Microsoft.EntityFrameworkCore;
using Site.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Shared.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace site.Data.Repositories
{
    public class CategoriesRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public CategoriesRepository(ApplicationDbContext context, IConfiguration configuration)
        {
            _dbContext = context;
            _configuration = configuration;
        }

        public async Task<List<Shared.Entities.Category>> GetCategories()
        {
            return await _dbContext.Categories.ToListAsync();
        }

        internal async Task<IEnumerable<CategoryView>> GetCategorySummary(int? parentCategoryId)
        {
            var query = @"
SELECT c.Id, c.Name, c.ParentId, c.IconImagePath, cg._count AS ChildrenCount FROM categories c
LEFT JOIN (select ParentId, count(1) _count from dbo.Categories group by ParentId) cg
ON cg.ParentId = c.Id
where c.ParentId = @ParentId OR (@ParentId IS NULL AND c.ParentID is NULL)";

            using var conn = new SqlConnection(Config.GetConnectionString(_configuration));
            conn.Open();
            return await conn.QueryAsync<CategoryView>(query, new { ParentId = parentCategoryId });
        }

        internal async Task<IEnumerable<Shared.AggregateDto>> GetDependentObjects(int categoryId)
        {
            var query = @"
SELECT * FROM(
select 'Categories' as Entity, count(1) as TotalCount from categories where parentid = @categoryId
UNION
select 'Products', count(1) as TotalCount from products where CategoryId = @categoryId
) c
WHERE c.TotalCount > 0";

            using var conn = new SqlConnection(Config.GetConnectionString(_configuration));
            conn.Open();
            return await conn.QueryAsync<Shared.AggregateDto>(query, new { categoryId });
        }

        internal async Task<IEnumerable<Shared.Entities.Category>> Get2LevelDeepCategories()
        {
            var query = @"
Select * FROM(
    select

        c.Id,
        c.Name,
        c.ParentId,
        c.IconImagePath
    from Categories c where parentid is null
    UNION
    Select

        c.Id,
        c.Name,
        c.ParentId,
        c.IconImagePath
    from categories cp
    join Categories c on c.ParentId = cp.id
    where cp.parentid is null
) c order by c.ParentId";

            using var conn = new SqlConnection(Config.GetConnectionString(_configuration));
            conn.Open();
            return await conn.QueryAsync<Shared.Entities.Category>(query);

        }

        internal async Task Delete(int id)
        {
            using var conn = new SqlConnection(Config.GetConnectionString(_configuration));
            conn.Open();
            await conn.ExecuteAsync("DELETE FROM Categories WHERE Id = @id", new { id });
        }
    }
}
