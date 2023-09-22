using BL.Core.Domain;
using Dapper;
using System.Data;

namespace BL.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly IDbConnection db;

        public ProductRepository(IDbConnection connection)
        {
            db = connection;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            var products = await db.QueryAsync<Product>("SELECT * FROM Products");
            return products.ToList();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            var product = await db.QuerySingleOrDefaultAsync<Product>("SELECT * FROM Products WHERE Id = @id", new { id });
            return product;
        }

        public async Task<Product> InsertAsync(Product product)
        {
            var sql =
                "INSERT INTO Products (Name, Description, Price) VALUES(@Name, @Description, @Price); " +
                "SELECT CAST(SCOPE_IDENTITY() AS int)";

            var id = await db.QuerySingleAsync<int>(sql, product);
            product.Id = id;
            return product;
        }

        public async Task<Product> UpdateAsync(Product product)
        {
            var sql =
                "UPDATE Products " +
                "SET Name = @Name, " +
                "    Description = @Description, " +
                "    Price = @Price " +
                "WHERE Id = @Id";

            await db.ExecuteAsync(sql, product);
            return product;
        }

        public async Task DeleteByIdAsync(int id)
        {
            await db.ExecuteAsync("DELETE FROM Products WHERE Id = @Id", new { id });
        }
    }
}
