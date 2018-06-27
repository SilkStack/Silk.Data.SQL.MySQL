using MySql.Data.MySqlClient;
using Silk.Data.SQL.Providers;
using Silk.Data.SQL.Queries;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silk.Data.SQL.MySQL
{
	public class MySQLDataProvider : DataProviderCommonBase
	{
		public const string PROVIDER_NAME = "mysql";
		private readonly string _connectionString;

		public override string ProviderName => PROVIDER_NAME;

		public MySQLDataProvider(string connectionString)
		{
			_connectionString = connectionString;
		}

		public MySQLDataProvider(MySqlConnectionStringBuilder connectionStringBuilder) :
			this(connectionStringBuilder.ConnectionString) { }

		public MySQLDataProvider(string hostname, string database, string username, string password) :
			this(new MySqlConnectionStringBuilder
			{
				UserID = username,
				Password = password,
				Database = database,
				Server = hostname
			}) { }

		public override void Dispose()
		{
		}

		protected override DbConnection Connect()
		{
			var connection = new MySqlConnection(_connectionString);
			connection.Open();
			return connection;
		}

		protected override async Task<DbConnection> ConnectAsync()
		{
			var connection = new MySqlConnection(_connectionString);
			await connection.OpenAsync();
			return connection;
		}

		protected override IQueryConverter CreateQueryConverter()
		{
			return new MySQLQueryConverter();
		}

#if DEBUG
		public override DbCommand CreateCommand(DbConnection connection, SqlQuery sqlQuery)
		{
			//  overridden in DEBUG builds to provide a useful breakpoint placement to look at raw SQL
			return base.CreateCommand(connection, sqlQuery);
		}
#endif
	}
}
