using BL.Setup.Code.Domain;
using Microsoft.Data.SqlClient;

namespace BL.Setup.Code.Repositories
{
    public class DatabaseRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public DatabaseRepository(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        public DatabaseParameters GetDataBaseParameters()
        {
            string db = _configuration.GetConnectionString("DataContext");
            SqlConnectionStringBuilder databaseBuilder = new SqlConnectionStringBuilder(db);

            return new DatabaseParameters()
            {
                Db = databaseBuilder.InitialCatalog,
                User = databaseBuilder.UserID,
                Pass = databaseBuilder.Password
            };
        }

        internal void DeleteDB()
        {
            DatabaseParameters parameters = this.GetDataBaseParameters();

            var sqlPath = Path.Combine(_env.ContentRootPath, "SqlScripts", "DeleteDB.sql");
            var sql = File.ReadAllText(sqlPath);

            sql = sql.Replace("<DB>", parameters.Db);

            this.Execute(sql);
        }

        internal void CreateDB()
        {
            DatabaseParameters parameters = this.GetDataBaseParameters();

            string sql;

            sql = $"CREATE DATABASE [{parameters.Db}]";
            this.Execute(sql);
        }

        internal void GenerateSchema()
        {
            DatabaseParameters parameters = this.GetDataBaseParameters();
            
            var sqlPath = Path.Combine(_env.ContentRootPath, "SqlScripts", "CreateFullSchema.sql");
            List<string> sqlLines = File.ReadAllLines(sqlPath).Skip(1).ToList();
            sqlLines.Insert(0, $"USE [{parameters.Db}] ");

            var sql = string.Join(Environment.NewLine, sqlLines.Select(a => string.Join(", ", a)));

            using (var connection = new SqlConnection(_configuration.GetConnectionString("Master")))
            {
                connection.Open();
                using (var command = new SqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        private void Execute(string sql)
        {
            var connString = _configuration.GetConnectionString("Master");

            using var connection = new SqlConnection(connString);

            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = sql;
            command.ExecuteNonQuery();
        }
    }
}
