using Silk.Data.SQL.Expressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silk.Data.SQL.MySQL.Expressions
{
	public class MySQLRawQueryExpression : QueryExpression
	{
		public string SqlText { get; }

		public override ExpressionNodeType NodeType => ExpressionNodeType.Query;

		public MySQLRawQueryExpression(string sqlText)
		{
			SqlText = sqlText;
		}
	}
}
