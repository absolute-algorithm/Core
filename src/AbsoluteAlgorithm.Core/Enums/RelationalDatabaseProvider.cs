namespace AbsoluteAlgorithm.Core.Enums;

/// <summary>
/// Specifies the relational database provider.
/// </summary>
public enum RelationalDatabaseProvider : byte
{
    /// <summary>
    /// PostgreSQL.
    /// </summary>
    PostgreSQL = 1,

    /// <summary>
    /// Microsoft SQL Server.
    /// </summary>
    MSSQL,

    /// <summary>
    /// MySQL.
    /// </summary>
    MySQL
}