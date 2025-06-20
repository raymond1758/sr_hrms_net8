using Npgsql;
using System.Data;

namespace sr_hrms_net8
{
    public class DataModule : IDisposable
    {
        private readonly NpgsqlConnection _connection;
        private NpgsqlTransaction? _transaction;
        private bool _disposed = false;

        public DataModule(string connectionString)
        {
            _connection = new NpgsqlConnection(connectionString);
        }

        /// <summary>
        /// Opens the database connection if not already open
        /// </summary>
        public void OpenConnection()
        {
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }
        }

        /// <summary>
        /// Closes the database connection
        /// </summary>
        public void CloseConnection()
        {
            if (_connection.State != ConnectionState.Closed)
            {
                _connection.Close();
            }
        }

        /// <summary>
        /// Begins a database transaction
        /// </summary>
        /// <returns>The transaction object</returns>
        public NpgsqlTransaction BeginTransaction()
        {
            OpenConnection();
            _transaction = _connection.BeginTransaction();
            return _transaction;
        }

        /// <summary>
        /// Commits the current transaction
        /// </summary>
        public void CommitTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Commit();
                _transaction.Dispose();
                _transaction = null;
            }
        }

        /// <summary>
        /// Rolls back the current transaction
        /// </summary>
        public void RollbackTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Rollback();
                _transaction.Dispose();
                _transaction = null;
            }
        }

        /// <summary>
        /// Executes a SQL query and returns a DataTable with results
        /// </summary>
        /// <param name="sql">SQL query string</param>
        /// <param name="parameters">Optional parameters</param>
        /// <returns>DataTable containing query results</returns>
        public DataTable ExecuteQuery(string sql, params NpgsqlParameter[] parameters)
        {
            OpenConnection();
            
            using var command = new NpgsqlCommand(sql, _connection, _transaction);
            
            if (parameters != null && parameters.Length > 0)
            {
                command.Parameters.AddRange(parameters);
            }

            using var adapter = new NpgsqlDataAdapter(command);
            var dataTable = new DataTable();
            adapter.Fill(dataTable);
            
            return dataTable;
        }

        /// <summary>
        /// Executes a SQL query and returns a NpgsqlDataReader
        /// </summary>
        /// <param name="sql">SQL query string</param>
        /// <param name="parameters">Optional parameters</param>
        /// <returns>NpgsqlDataReader for reading results</returns>
        public NpgsqlDataReader ExecuteReader(string sql, params NpgsqlParameter[] parameters)
        {
            OpenConnection();
            
            var command = new NpgsqlCommand(sql, _connection, _transaction);
            
            if (parameters != null && parameters.Length > 0)
            {
                command.Parameters.AddRange(parameters);
            }

            return command.ExecuteReader();
        }

        /// <summary>
        /// Executes a SQL command (INSERT, UPDATE, DELETE) and returns affected rows count
        /// </summary>
        /// <param name="sql">SQL command string</param>
        /// <param name="parameters">Optional parameters</param>
        /// <returns>Number of affected rows</returns>
        public int ExecuteCommand(string sql, params NpgsqlParameter[] parameters)
        {
            OpenConnection();
            
            using var command = new NpgsqlCommand(sql, _connection, _transaction);
            
            if (parameters != null && parameters.Length > 0)
            {
                command.Parameters.AddRange(parameters);
            }

            return command.ExecuteNonQuery();
        }

        /// <summary>
        /// Executes a SQL command and returns a single scalar value
        /// </summary>
        /// <param name="sql">SQL command string</param>
        /// <param name="parameters">Optional parameters</param>
        /// <returns>Scalar value result</returns>
        public object? ExecuteScalar(string sql, params NpgsqlParameter[] parameters)
        {
            OpenConnection();
            
            using var command = new NpgsqlCommand(sql, _connection, _transaction);
            
            if (parameters != null && parameters.Length > 0)
            {
                command.Parameters.AddRange(parameters);
            }

            return command.ExecuteScalar();
        }

        /// <summary>
        /// Executes a SQL command and returns a single scalar value of type T
        /// </summary>
        /// <typeparam name="T">Type to cast the result to</typeparam>
        /// <param name="sql">SQL command string</param>
        /// <param name="parameters">Optional parameters</param>
        /// <returns>Scalar value result cast to type T</returns>
        public T? ExecuteScalar<T>(string sql, params NpgsqlParameter[] parameters)
        {
            var result = ExecuteScalar(sql, parameters);
            
            if (result == null || result == DBNull.Value)
                return default(T);
                
            return (T)Convert.ChangeType(result, typeof(T));
        }

        /// <summary>
        /// Creates a new NpgsqlParameter
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter value</param>
        /// <returns>NpgsqlParameter object</returns>
        public static NpgsqlParameter CreateParameter(string name, object? value)
        {
            return new NpgsqlParameter(name, value ?? DBNull.Value);
        }

        /// <summary>
        /// Creates a new NpgsqlParameter with specific type
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="dbType">PostgreSQL data type</param>
        /// <param name="value">Parameter value</param>
        /// <returns>NpgsqlParameter object</returns>
        public static NpgsqlParameter CreateParameter(string name, NpgsqlTypes.NpgsqlDbType dbType, object? value)
        {
            return new NpgsqlParameter(name, dbType) { Value = value ?? DBNull.Value };
        }

        /// <summary>
        /// Gets the current connection state
        /// </summary>
        public ConnectionState ConnectionState => _connection.State;

        /// <summary>
        /// Gets whether a transaction is currently active
        /// </summary>
        public bool HasActiveTransaction => _transaction != null;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Rollback any active transaction
                    if (_transaction != null)
                    {
                        try
                        {
                            _transaction.Rollback();
                        }
                        catch
                        {
                            // Ignore rollback errors during disposal
                        }
                        finally
                        {
                            _transaction.Dispose();
                            _transaction = null;
                        }
                    }

                    // Close and dispose connection
                    if (_connection != null)
                    {
                        try
                        {
                            _connection.Close();
                        }
                        catch
                        {
                            // Ignore close errors during disposal
                        }
                        finally
                        {
                            _connection.Dispose();
                        }
                    }
                }

                _disposed = true;
            }
        }

        ~DataModule()
        {
            Dispose(false);
        }
    }
}