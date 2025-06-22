using System.Data;

namespace sr_hrms_net8.Models
{
    public class Account : BaseModel
    {
        public Account(DbAdapter dbAdapter) : base(dbAdapter)
        {
        }

        /// <summary>
        /// Gets all account records
        /// </summary>
        /// <returns>DataTable containing all account data</returns>
        public DataTable QueryAll()
        {
            var sql = @"SELECT account_id, emp_id, account_name, auth_hash, email, disabled, 
                               create_date, create_user, upd_date, upd_user
                        FROM core.account 
                        ORDER BY account_id";

            return _dbAdapter.ExecuteQuery(sql);
        }

        /// <summary>
        /// Checks if an account record exists for a specific employee ID
        /// </summary>
        /// <param name="empId">Employee ID to check</param>
        /// <returns>True if record exists, false otherwise</returns>
        public bool IsExisted(string empId)
        {
            var sql = @"SELECT COUNT(*) 
                        FROM core.account 
                        WHERE emp_id = @empId";
            var parameters = new[]
            {
                DbAdapter.CreateParameter("@empId", empId)
            };
            var count = _dbAdapter.ExecuteScalar<int>(sql, parameters);
            return count > 0;
        }

        /// <summary>
        /// Inserts a new account record
        /// </summary>
        /// <param name="empId">Employee ID</param>
        /// <param name="accountName">Account name</param>
        /// <param name="authHash">Password hash</param>
        /// <param name="email">Email address</param>
        /// <param name="disabled">Whether account is disabled</param>
        /// <param name="createUser">User who created the record</param>
        /// <returns>Number of affected rows</returns>
        public int Insert(string empId, string accountName, string authHash, string email, bool disabled, string createUser)
        {
            var sql = @"
                INSERT INTO core.account 
                (emp_id, account_name, auth_hash, email, disabled, create_user, upd_user) 
                VALUES 
                (@empId, @accountName, @authHash, @email, @disabled, @createUser, @createUser)";

            var parameters = new[]
            {
                DbAdapter.CreateParameter("@empId", empId),
                DbAdapter.CreateParameter("@accountName", accountName),
                DbAdapter.CreateParameter("@authHash", authHash),
                DbAdapter.CreateParameter("@email", email),
                DbAdapter.CreateParameter("@disabled", disabled),
                DbAdapter.CreateParameter("@createUser", createUser)
            };

            return _dbAdapter.ExecuteCommand(sql, parameters);
        }

        /// <summary>
        /// Updates an account record
        /// </summary>
        /// <param name="accountId">Account ID</param>
        /// <param name="accountName">Account name</param>
        /// <param name="authHash">Password hash</param>
        /// <param name="email">Email address</param>
        /// <param name="disabled">Whether account is disabled</param>
        /// <param name="updUser">User who updated the record</param>
        /// <returns>Number of affected rows</returns>
        public int Update(int accountId, string accountName, string authHash, string email, bool disabled, string updUser)
        {
            var sql = @"UPDATE core.account 
                        SET account_name = @accountName, 
                            auth_hash = @authHash, 
                            email = @email, 
                            disabled = @disabled, 
                            upd_date = CURRENT_TIMESTAMP, 
                            upd_user = @updUser 
                        WHERE account_id = @accountId";

            var parameters = new[]
            {
                DbAdapter.CreateParameter("@accountId", accountId),
                DbAdapter.CreateParameter("@accountName", accountName),
                DbAdapter.CreateParameter("@authHash", authHash),
                DbAdapter.CreateParameter("@email", email),
                DbAdapter.CreateParameter("@disabled", disabled),
                DbAdapter.CreateParameter("@updUser", updUser)
            };

            return _dbAdapter.ExecuteCommand(sql, parameters);
        }
    }
}