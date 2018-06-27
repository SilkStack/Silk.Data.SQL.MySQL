using Microsoft.VisualStudio.TestTools.UnitTesting;
using Silk.Data.SQL.Providers;
using Silk.Data.SQL.ProviderTests;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Silk.Data.SQL.MySQL.Tests
{
	[TestClass]
	public class MySQLProviderTests : SqlProviderTests
	{
		public override IDataProvider CreateDataProvider(string connectionString)
		{
			return new MySQLDataProvider(connectionString);
		}

		[TestMethod]
		public override async Task Data_StoreFloat()
		{
			//  for some reason MySQL needs a slightly higher precision to store the maximum float value in .NET
			await Data_TestStoreDataType(SqlDataType.Float(25), float.MaxValue);
		}

		public override void Dispose()
		{
			DataProvider.Dispose();
		}
	}
}
