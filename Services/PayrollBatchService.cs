using sr_hrms_net8.Models;
using System.Data;

namespace sr_hrms_net8.Services
{
    public class PayrollBatchService : BaseService
    {
        public PayrollBatchService(DbAdapter dbAdapter) : base(dbAdapter)
        {
        }

        public DataTable QueryByYear(int year)
        {
            var sql = @"SELECT batch_id, payroll_year, payroll_month, description, status, create_date, create_user, upd_date, upd_user 
                        FROM payroll.payroll_batch";

            if (year != 0)
            {
                sql += " WHERE payroll_year = @year";
            }

            sql += " ORDER BY payroll_year DESC, payroll_month";
            
            if (year != 0)
            {
                var parameters = new[]
                {
                    DbAdapter.CreateParameter("@year", year)
                };
                return _dbAdapter.ExecuteQuery(sql, parameters);
            }
            else
            {
                return _dbAdapter.ExecuteQuery(sql);
            }
        }
    }
}