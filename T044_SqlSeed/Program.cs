using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Globalization;
using System.Text.Json;
using T044_SqlSeed.Data;
using T044_SqlSeed.Models;

IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
DataContextDapper dapper = new(config);

string tableCreateSql = File.ReadAllText("Users.sql");
dapper.ExecuteSQL(tableCreateSql);

// TutorialAppSchema.Users seed
string userJson = File.ReadAllText("Users.json");
IEnumerable<UserModel>? users = JsonSerializer.Deserialize<IEnumerable<UserModel>>(userJson);

if (users != null)
{
	using IDbConnection dbConnection = new SqlConnection(config.GetConnectionString("DefaultConnection"));

	string sql = @"
		SET IDENTITY_INSERT TutorialAppSchema.Users ON;
		INSERT INTO TutorialAppSchema.Users (
			UserId,
			FirstName,
			LastName,
			Email,
			Gender,
			Active
		) VALUES";

	foreach (var user in users)
	{
		string sqlToAdd = @$"
			({user.UserId}, 
			'{user.FirstName.Replace("'", "''")}', 
			'{user.LastName.Replace("'", "''")}', 
			'{user.Email.Replace("'", "''")}', 
			'{user.Gender}', 
			'{user.Active}'
			),";

		if ((sql + sqlToAdd).Length > 4000)
		{
			DataContextDapper.ExecuteProcedureMulti(sql.Trim(','), dbConnection);
			sql = @"
				SET IDENTITY_INSERT TutorialAppSchema.Users ON;
				INSERT INTO TutorialAppSchema.Users (
					UserId,
					FirstName,
					LastName,
					Email,
					Gender,
					Active
				) VALUES";
		}

		sql += sqlToAdd;
	}

	DataContextDapper.ExecuteProcedureMulti(sql.Trim(','), dbConnection);
}

dapper.ExecuteSQL("SET IDENTITY_INSERT TutorialAppSchema.Users OFF");

// TutorialAppSchema.UserSalary seed
string userSalaryJson = File.ReadAllText("UserSalary.json");
IEnumerable<UserSalaryModel>? usersSalary =
	JsonSerializer.Deserialize<IEnumerable<UserSalaryModel>>(userSalaryJson);

dapper.ExecuteSQL("TRUNCATE TABLE TutorialAppSchema.UserSalary");

if (usersSalary != null)
{
	using IDbConnection dbConnection = new SqlConnection(config.GetConnectionString("DefaultConnection"));

	string sql = @"
		INSERT INTO TutorialAppSchema.UserSalary (
			UserId,
			Salary
		) VALUES";

	foreach (var userSalary in usersSalary)
	{
		string sqlToAdd = @$"
			({userSalary.UserId}, 
			'{userSalary.Salary.ToString("0.00", CultureInfo.InvariantCulture)}'
			),";

		if ((sql + sqlToAdd).Length > 4000)
		{
			DataContextDapper.ExecuteProcedureMulti(sql.Trim(','), dbConnection);
			sql = @"
				INSERT INTO TutorialAppSchema.UserSalary (
					UserId,
					Salary
				) VALUES";
		}

		sql += sqlToAdd;
	}

	DataContextDapper.ExecuteProcedureMulti(sql.Trim(','), dbConnection);
}

// TutorialAppSchema.UserJobInfo seed
string userJobInfoJson = File.ReadAllText("UserJobInfo.json");
IEnumerable<UserJobInfoModel>? usersJobInfo =
	JsonSerializer.Deserialize<IEnumerable<UserJobInfoModel>>(userJobInfoJson);

dapper.ExecuteSQL("TRUNCATE TABLE TutorialAppSchema.UserJobInfo");

if (usersJobInfo != null)
{
	using IDbConnection dbConnection = new SqlConnection(config.GetConnectionString("DefaultConnection"));

	string sql = @"
		INSERT INTO TutorialAppSchema.UserJobInfo (
			UserId,
			Department,
			JobTitle
		) VALUES";

	foreach (var userJobInfo in usersJobInfo)
	{
		string sqlToAdd = @$"
			({userJobInfo.UserId}, 
			'{userJobInfo.Department}',
			'{userJobInfo.JobTitle}'
			),";

		if ((sql + sqlToAdd).Length > 4000)
		{
			DataContextDapper.ExecuteProcedureMulti(sql.Trim(','), dbConnection);
			sql = @"
				INSERT INTO TutorialAppSchema.UserJobInfo (
					UserId,
					Department,
					JobTitle
				) VALUES";
		}

		sql += sqlToAdd;
	}

	DataContextDapper.ExecuteProcedureMulti(sql.Trim(','), dbConnection);
}

Console.WriteLine("SQL Seed Completed Successfully");
