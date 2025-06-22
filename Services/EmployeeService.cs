using sr_hrms_net8.Models;
using System.Data;

namespace sr_hrms_net8.Services
{
    public class EmployeeService : BaseService
    {
        public EmployeeService(DbAdapter dbAdapter) : base(dbAdapter)
        {
        }

        public DataTable QueryAll()
        {
            var sql = @"SELECT emp_id, emp_name_zh, gender, nationality, identity_Group, job_category, 
                            employment_status, onboard_date::date, suspension_or_resignation_date::date, reinstatement_date::date, disability 
                        FROM core.employee
                        ORDER BY emp_id";
            return _dbAdapter.ExecuteQuery(sql);
        }
    }
}