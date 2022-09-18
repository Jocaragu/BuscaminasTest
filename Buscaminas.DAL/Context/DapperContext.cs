using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Buscaminas.DAL.Context
{
    public class DapperContext
	{
        private readonly IConfiguration _configuration;
        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IDbConnection Connect() => new SqlConnection(_configuration.GetConnectionString("default"));
    }
}
