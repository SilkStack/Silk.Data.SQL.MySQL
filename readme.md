# Overview

MySQL/MariaDB provider for `Silk.Data.SQL.Base`.

# Installing

`Silk.Data.SQL.MySQL` is available as a NuGet package: https://www.nuget.org/packages/Silk.Data.SQL.MySQL

You can install it from the NuGet package manager in Visual Studio or from command line with dotnet core:

~~~~
dotnet add package Silk.Data.SQL.MySQL
~~~~


# Usage

To execute SQL statements just create an instance of `MySQLDataProvider`, passing in the name of the file you wish to store your database in.

    var provider = new MySQLDataProvider("hostname", "database", "username", "password");

## Executing Queries

Non-reader queries:

    provider.ExecuteNonReaderAsync(
        QueryExpression.Insert(
            "Accounts",
            new[] { "DisplayName" },
            new object[] { "John" },
            new object[] { "Jane" }
        )
    );

Queries with results need to be disposed:

    using (var queryResult = provider.ExecuteReader(
        QueryExpression.Select(
            new[] { Expression.Value("Hello World!") }
    )))
    {
        Assert.IsTrue(queryResult.HasRows);
        Assert.IsTrue(queryResult.Read());
        Assert.AreEqual("Hello World!", queryResult.GetString(0));
    }

## Raw SQL

A raw SQL expression is provided on the `MSSql` helper class.

    var rawSQL = MySQL.Raw("SELECT random()");

Raw SQL expressions are safe to be used within `TransactionExpression`:

    var transaction = QueryExpression.Transaction(
        MySQL.Raw("SELECT date()"),
        MySQL.Raw("SELECT time()")
    );

# License

`Silk.Data.SQL.MySQL` is made available under the MIT license.