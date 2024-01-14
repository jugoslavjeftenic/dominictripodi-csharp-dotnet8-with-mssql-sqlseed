using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace T044_SqlSeed.Data
{
	internal class DataContextDapper(IConfiguration config)
	{
		private readonly IConfiguration _config = config;

		public IEnumerable<T> LoadData<T>(string sql)
		{
			using IDbConnection dbConnection =
				new SqlConnection(_config.GetConnectionString("DefaultConnection"));

			dbConnection.Open();

			using IDbTransaction tran = dbConnection.BeginTransaction(IsolationLevel.ReadCommitted);
			var holdVal = dbConnection.Query<T>(sql, null, transaction: tran, commandTimeout: 999999999);

			dbConnection.Close();

			return holdVal;
		}

		public int ExecuteSQL(string sql)
		{
			using IDbConnection dbConnection =
				new SqlConnection(_config.GetConnectionString("DefaultConnection"));

			return dbConnection.Execute(sql);
		}

		public static void ExecuteProcedureMulti(string sql, IDbConnection dbConnection)
		{
			dbConnection.Execute(sql);
		}
	}
}
