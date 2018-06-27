using Silk.Data.SQL.Expressions;
using Silk.Data.SQL.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silk.Data.SQL.MySQL
{
	public class MySQLQueryConverter : QueryConverterCommonBase
	{
		protected override string ProviderName => MySQLDataProvider.PROVIDER_NAME;

		protected override string AutoIncrementSql => "AUTO_INCREMENT";

		public MySQLQueryConverter()
		{
			ExpressionWriter = new MySQLQueryWriter(Sql, this);
		}

		protected override string GetDbDatatype(SqlDataType sqlDataType)
		{
			switch (sqlDataType.BaseType)
			{
				case SqlBaseType.Guid: return "VARCHAR(36)";
				case SqlBaseType.TinyInt: return "TINYINT";
				case SqlBaseType.SmallInt: return "SMALLINT";
				case SqlBaseType.Int: return "INT";
				case SqlBaseType.BigInt: return "BIGINT";
				case SqlBaseType.Float: return $"FLOAT({sqlDataType.Parameters[0]})";
				case SqlBaseType.Bit: return "BIT(1)";
				case SqlBaseType.Decimal: return $"DECIMAL({sqlDataType.Parameters[0]}, {sqlDataType.Parameters[1]})";
				case SqlBaseType.Date: return "DATE";
				case SqlBaseType.Time: return "TIME";
				case SqlBaseType.DateTime: return "DATETIME";
				case SqlBaseType.Text: return sqlDataType.Parameters.Length > 0 ? $"VARCHAR({sqlDataType.Parameters[0]})" : "TEXT";
				case SqlBaseType.Binary: return "binary";
			}
			throw new System.NotSupportedException($"SQL data type not supported: {sqlDataType.BaseType}.");
		}

		protected override string QuoteIdentifier(string schemaComponent)
		{
			if (schemaComponent == "*")
				return "*";
			return $"`{schemaComponent}`";
		}

		protected override void WriteFunctionToSql(QueryExpression queryExpression)
		{
			switch (queryExpression)
			{
				case RandomFunctionExpression randomFunctionExpression:
					Sql.Append($"CAST(FLOOR(RAND() * {int.MaxValue}) AS INT)");
					return;
				case LastInsertIdFunctionExpression lastInsertIdExpression:
					Sql.Append("LAST_INSERT_ID()");
					return;
				case TableExistsVirtualFunctionExpression tableExistsExpression:
					Sql.Append($@"SELECT 1
FROM information_schema.tables
WHERE table_schema = DATABASE()
AND table_name = '{tableExistsExpression.Table.TableName}'");
					return;
			}
			base.WriteFunctionToSql(queryExpression);
		}

		private class MySQLQueryWriter : QueryWriter
		{
			public new MySQLQueryConverter Converter { get; }

			public MySQLQueryWriter(StringBuilder sql, MySQLQueryConverter converter) : base(sql, converter)
			{
				Converter = converter;
			}

			protected override void VisitQuery(QueryExpression queryExpression)
			{
				switch (queryExpression)
				{
					case TransactionExpression transaction:
						Sql.AppendLine("START TRANSACTION;");
						foreach (var query in transaction.Queries)
						{
							Visit(query);
						}
						Sql.AppendLine("COMMIT;");
						break;
					default:
						base.VisitQuery(queryExpression);
						break;
				}
			}
		}
	}
}
