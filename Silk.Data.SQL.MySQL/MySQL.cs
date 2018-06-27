using Silk.Data.SQL.MySQL.Expressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silk.Data.SQL.MySQL
{
	public static class MySQL
	{
		public static MySQLRawQueryExpression Raw(string sql)
		{
			return new MySQLRawQueryExpression(sql);
		}
	}
}
