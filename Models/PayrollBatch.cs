using System.Data;

namespace sr_hrms_net8.Models
{
    public class PayrollBatch : BaseModel
    {
        public PayrollBatch(DbAdapter dbAdapter) : base(dbAdapter)
        {
        }

        /// <summary>
        /// Checks if a payroll batch record exists for a specific batch ID
        /// </summary>
        /// <param name="batchId">Batch ID to check</param>
        /// <returns>True if record exists, false otherwise</returns>
        public bool IsExisted(int batchId)
        {
            var sql = @"SELECT COUNT(*) 
                        FROM payroll.payroll_batch 
                        WHERE batch_id = @batchId";
            var parameters = new[]
            {
                DbAdapter.CreateParameter("@batchId", batchId)
            };
            var count = _dbAdapter.ExecuteScalar<int>(sql, parameters);
            return count > 0;
        }

        /// <summary>
        /// Inserts a new payroll batch record
        /// </summary>
        /// <param name="payrollYear">年度</param>
        /// <param name="payrollMonth">月份</param>
        /// <param name="description">說明</param>
        /// <param name="status">狀態 ( Init / Submitted / Closed / Canceled)</param>
        /// <param name="createUser">建立者</param>
        /// <returns>Number of affected rows</returns>
        public void Insert(int payrollYear, int payrollMonth, string description, string status, string createUser)
        {
            var sql = @"INSERT INTO payroll.payroll_batch 
                        (payroll_year, payroll_month, description, status, create_date, create_user, upd_date, upd_user) 
                        VALUES (@payrollYear, @payrollMonth, @description, @status, CURRENT_TIMESTAMP, @createUser, CURRENT_TIMESTAMP, @createUser)";

            var parameters = new[]
            {
                DbAdapter.CreateParameter("@payrollYear", payrollYear),
                DbAdapter.CreateParameter("@payrollMonth", payrollMonth),
                DbAdapter.CreateParameter("@description", description),
                DbAdapter.CreateParameter("@status", status),
                DbAdapter.CreateParameter("@createUser", createUser)
            };

            _dbAdapter.ExecuteCommand(sql, parameters);
        }

        /// <summary>
        /// Updates a payroll batch record
        /// </summary>
        /// <param name="batchId">Batch ID</param>
        /// <param name="description">說明</param>
        /// <param name="status">狀態 ( Init / Submitted / Closed / Canceled)</param>
        /// <param name="updUser">更新者</param>
        public void Update(int batchId, string description, string status, string updUser)
        {
            var sql = @"UPDATE payroll.payroll_batch 
                        SET description = @description, 
                            status = @status, 
                            upd_date = CURRENT_TIMESTAMP, 
                            upd_user = @updUser 
                        WHERE batch_id = @batchId";

            var parameters = new[]
            {
                DbAdapter.CreateParameter("@batchId", batchId),
                DbAdapter.CreateParameter("@description", description),
                DbAdapter.CreateParameter("@status", status),
                DbAdapter.CreateParameter("@updUser", updUser)
            };

            _dbAdapter.ExecuteCommand(sql, parameters);
        }

        /// <summary>
        /// Deletes a payroll batch record
        /// </summary>
        /// <param name="batchId">Batch ID to delete</param>
        public void Delete(int batchId)
        {
            var sql = @"DELETE FROM payroll.payroll_batch WHERE batch_id = @batchId";
            var parameters = new[]
            {
                DbAdapter.CreateParameter("@batchId", batchId)
            };
            _dbAdapter.ExecuteCommand(sql, parameters);
        }
    }
}