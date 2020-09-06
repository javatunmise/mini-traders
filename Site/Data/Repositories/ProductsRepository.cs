using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shared.Entities;
using Site.Data;
using Site.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Dapper;
using Shared.ViewModels;
using site.Data.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace site.Repositories
{
    public class ProductsRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public ProductsRepository(ApplicationDbContext context, IConfiguration configuration)
        {
            _dbContext = context;
            _configuration = configuration;
        }

        internal async Task<List<Product>> GetServices(int id)
        {
            return await _dbContext.Products.Where(e => e.StoreId == id && e.RenderedAsService).ToListAsync();
        }

        internal async Task<IEnumerable<Product>> GetProducts(int id)
        {
            return await _dbContext.Products.Where(e => e.StoreId == id && !e.RenderedAsService).ToListAsync();
        }

        internal async Task Create(Product product)
        {
            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();
        }

        internal async Task Update(Product updated)
        {
            var _product = await _dbContext.Products.FirstOrDefaultAsync(e => e.Id == updated.Id);
            _product.Name = updated.Name;
            _product.ProductDetails = updated.ProductDetails;
            _product.CategoryId = updated.CategoryId;
            _product.Price = updated.Price;
            _product.LastModifiedOn = updated.LastModifiedOn;
            _product.Specifications = updated.Specifications;

            if (!string.IsNullOrWhiteSpace(updated.ImageUrl))
                _product.ImageUrl = updated.ImageUrl;

            await _dbContext.SaveChangesAsync();
        }

        internal async Task<ProductView> GetProductView(int productId)
        {
            var query = @"GetProductView";
            using var conn = new SqlConnection(Config.GetConnectionString(_configuration));
            conn.Open();
            return await conn.QueryFirstOrDefaultAsync<ProductView>(query, new { productId },
                                                        commandType: System.Data.CommandType.StoredProcedure);
        }

        internal async Task<ProductSearchView> SearchProduct(ProductSearchFilter filter)
        {
            var query = @"SearchProduct";
            using var conn = new SqlConnection(Config.GetConnectionString(_configuration));
            conn.Open();
            var records = await conn.QueryMultipleAsync(query, filter, commandType: System.Data.CommandType.StoredProcedure);

            var result = records.Read<ProductSearchView>().First();
            result.Records = records.Read<ProductSearchView.Data>().ToList();

            return result;
        }

        internal Task<Product> GetProduct(int productId)
        {
            return _dbContext.Products.FirstOrDefaultAsync(e => e.Id == productId);
        }

        internal Task<List<Product>> Search(Expression<Func<Product, bool>> filter)
        {
            return _dbContext.Products.Where(filter).ToListAsync();
        }

    }
}
