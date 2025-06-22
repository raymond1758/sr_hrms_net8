using System.Data;

namespace sr_hrms_net8.Models
{
    public class Department : BaseModel
    {
        public Department(DbAdapter dbAdapter) : base(dbAdapter)
        {
        }

        /// <summary>
        /// Gets all department records
        /// </summary>
        /// <returns>DataTable containing all department data</returns>
        public DataTable QueryAll()
        {
            var sql = @"SELECT *
                        FROM core.department 
                        ORDER BY dept_id";

            return _dbAdapter.ExecuteQuery(sql);
        }

        /// <summary>
        /// Checks if a department record exists for a specific department ID
        /// </summary>
        /// <param name="deptId">Department ID to check</param>
        /// <returns>True if record exists, false otherwise</returns>
        public bool IsExisted(string deptId)
        {
            var sql = @"SELECT COUNT(*) 
                        FROM core.department 
                        WHERE dept_id = @deptId";
            var parameters = new[]
            {
                DbAdapter.CreateParameter("@deptId", deptId)
            };
            var count = _dbAdapter.ExecuteScalar<int>(sql, parameters);
            return count > 0;
        }
    }
}