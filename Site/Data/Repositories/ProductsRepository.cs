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
using Shared;
using System.Data;
using System.Data.Common;
using Microsoft.Extensions.Logging;

namespace site.Repositories
{
    public class ProductsRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly ICurrentDate _serverDate;
        private readonly ILogger<ProductsRepository> _logger;

        public ProductsRepository(ApplicationDbContext context, 
                                  IConfiguration configuration, 
                                  ICurrentDate currentDate,
                                  ILogger<ProductsRepository> logger)
        {
            _dbContext = context;
            _configuration = configuration;
            _serverDate = currentDate;
            _logger = logger;
        }

        internal async Task<List<Product>> GetServices(int storeId)
        {
            return await _dbContext.Products.Where(e => e.StoreId == storeId && e.RenderedAsService).ToListAsync();
        }

        internal async Task<IEnumerable<Product>> GetProducts(int id)
        {
            return await _dbContext.Products.Where(e => e.StoreId == id && !e.RenderedAsService).ToListAsync();
        }

        internal async Task Create(Product product)
        {
            product.CreatedOn = _serverDate.Now();
            product.LastModifiedOn = _serverDate.Now();
            
            _dbContext.Products.Add(product);
            
            await _dbContext.SaveChangesAsync();

            try
            {
                await InsertProductKeywords(product.Id, GetValidKeywords(product));
            }
            catch(DbException ex)
            {
                _logger.LogDebug(ex.Message);
            }
        }

        private async Task InsertProductKeywords(int productId, List<string> tags)
        {
            //_dbContext.Tags.Add(new Tag { Name = tags[0], ProductTags = tags.Select( });           

            var parameters = new DynamicParameters();
            parameters.Add("@ProductId", productId);
            parameters.Add("@TagXml", ToXML(tags), DbType.Xml);

            using var conn = new SqlConnection(Config.GetConnectionString(_configuration));
            conn.Open();
            await conn.ExecuteAsync("Insert_ProductTags", parameters, commandType: CommandType.StoredProcedure);
        }

        internal async Task<int> CountByStore(int id)
        {
            return await _dbContext.Products.AsNoTracking().CountAsync(e => e.StoreId == id);
        }

        private static string ToXML(List<string> tags)
        {
            return string.Join("", tags.Select(e => $"<Tag>{e}</Tag>"));
        }

        internal async Task<IEnumerable<Product>> GetRelatedProducts(string name, string productDetails)
        {
            var tagsXml = ToXML(GetValidKeywords(new Product { Name = name, ProductDetails = productDetails }));
            var parameters = new DynamicParameters();
            parameters.Add("@tagsXml", tagsXml, DbType.Xml);

            using var conn = new SqlConnection(Config.GetConnectionString(_configuration));
            conn.Open();
            return await conn.QueryAsync<Product>("Product_GetRelated", parameters, commandType: CommandType.StoredProcedure);
        }

        private List<string> GetValidKeywords(Product product)
        {
            var noiseWords = new[] {
"about","after","all","also","an","and","another","any","are","as","at","be","because","been","before",
"being","between","both","but","by","came","can","come","could","did","do","each","for","from","get",
"got","has","had","he","have","her","here","him","himself","his","how","if","in","into","is","it","like",
"make","many","me","might","more","most","much","must","my","never","now","of","on","only","or","other",
"our","out","over","said","same","see","should","since","some","still","such","take","than","that",
"the","their","them","then","there","these","they","this","those","through","to","too","under","up",
"very","was","way","we","well","were","what","where","which","while","who","with","would","you","your","a",
"b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z","$",
                "1","2","3","4","5","6","7","8","9","0" };

            var productKeywords = product.Name.ToLower().Split()
                                  .Concat(product.ProductDetails.ToLower().Split())
                                  .Distinct()
                                  .Select(e => e.ToLower().Trim(',',';','.','?','(',')','[',']', ' '))
                                  .Where(e => !e.All(char.IsDigit));

            return (from kw in productKeywords
                    where !noiseWords.Contains(kw)
                    select kw).ToList();
        }

        internal async Task Update(Product updated)
        {
            var _product = await _dbContext.Products.FirstOrDefaultAsync(e => e.Id == updated.Id);
            _product.Name = updated.Name;
            _product.ProductDetails = updated.ProductDetails;
            _product.CategoryId = updated.CategoryId;
            _product.OldPrice = _product.Price;
            _product.Price = updated.Price;
            _product.LastModifiedOn = updated.LastModifiedOn;
            _product.Specifications = updated.Specifications;
            _product.RenderedAsService = updated.RenderedAsService;
            _product.LastModifiedOn = _serverDate.Now();
            _product.Status = updated.Status;

            if (!string.IsNullOrWhiteSpace(updated.ImageUrl))
                _product.ImageUrl = updated.ImageUrl;
            if (!string.IsNullOrWhiteSpace(updated.SmallImageUrl))
                _product.SmallImageUrl = updated.SmallImageUrl;

            if(!string.IsNullOrWhiteSpace(updated.ImageUrl))
                _product.OtherImageUrlsJson = updated.OtherImageUrlsJson;

            await _dbContext.SaveChangesAsync();

            await InsertProductKeywords(_product.Id, GetValidKeywords(_product));
        }

        internal async Task AddReview(ProductReview review)
        {
            var query = @"Product_InsertReview";
            using var conn = new SqlConnection(Config.GetConnectionString(_configuration));
            conn.Open();
            
            await conn.ExecuteAsync(query, new {
                review.ProductId,
                review.StoreId,
                review.Message,
                review.ReviewerId,
                review.ReviewerName,
                review.Rating,
                review.HideUserIdentity,
                CreatedOn = _serverDate.Now()
            }, commandType: System.Data.CommandType.StoredProcedure);
        }

        internal async Task<ProductView> GetProductView(int productId)
        {
            var query = @"GetProductView";
            using var conn = new SqlConnection(Config.GetConnectionString(_configuration));
            conn.Open();
            return await conn.QueryFirstOrDefaultAsync<ProductView>(query, new { productId },
                                                        commandType: System.Data.CommandType.StoredProcedure);
        }

        internal async Task<IEnumerable<ProductReview>> GetProductReviews(int productId)
        {
            var query = @"GetProductReviews";
            using var conn = new SqlConnection(Config.GetConnectionString(_configuration));
            conn.Open();
            return await conn.QueryAsync<ProductReview>(query, new { productId },
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
