using System.Data;

namespace Cross.SharedKernel.Interfaces;

/// <summary>
/// The connection factory interface to interact with the database using Dapper.
/// </summary>
public interface IConnectionFactory
{
    /// <summary>
    /// Use it if you want to start a transaction to perform multiple operations.
    /// </summary>
    void BeginTransaction();
    
    /// <summary>
    /// When we want to save the changes made in the transaction, we can use this method.
    /// </summary>
    void Commit();
    
    /// <summary>
    /// If we want to discard the changes made in the transaction, we can use this method.
    /// </summary>
    void Rollback();
    
    /// <summary>
    /// The instance of the current connection. Used to interact with the database.
    /// </summary>
    IDbConnection Connection { get; }
    
    /// <summary>
    /// If we want to perform multiple operations in a single transaction, we can use this property to interact with the database.
    /// </summary>
    IDbTransaction Transaction { get; }
    
    /// <summary>
    /// Disposes the connection and transaction.
    /// </summary>
    void Dispose();
}