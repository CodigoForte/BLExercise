using BL.Core.Domain;
using Dapper;
using System.Data;

namespace BL.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection db;

        public UserRepository(IDbConnection connection)
        {
            db = connection;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            var user = await db.QuerySingleOrDefaultAsync<User>("SELECT * FROM Users WHERE Email = @Email", new { email });
            return user;
        }
        
        public async Task<User> InsertAsync(User user)
        {
            var sql =
               "INSERT INTO Users (Name, Email, Password) VALUES(@Name, @Email, @Password); " +
               "SELECT CAST(SCOPE_IDENTITY() AS int)";

            var id = await db.QuerySingleAsync<int>(sql, user);
            user.Id = id;
            return user;
        }
    }
}
