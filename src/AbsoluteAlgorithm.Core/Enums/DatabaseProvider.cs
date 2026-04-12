namespace AbsoluteAlgorithm.Core.Enums;

/// <summary>
/// Specifies the database provider.
/// </summary>
public enum DatabaseProvider : byte
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
    /// MongoDB.
    /// </summary>
    MongoDb
}