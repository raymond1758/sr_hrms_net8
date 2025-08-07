

using System.Data;

namespace sr_hrms_net8.Models
{
    public class PunchRecord : BaseModel
    {
        public PunchRecord(DbAdapter dbAdapter) : base(dbAdapter)
        {
        }

        public bool IsExisted(int batchId, string empId, DateTime workDate, string clockType)
        {
            var sql = @"SELECT COUNT(*) 
                        FROM attendance.punch_records 
                        WHERE batch_id = @batchId 
                          AND emp_id = @empId 
                          AND work_date = @workDate 
                          AND clock_type = @clockType";
            var parameters = new[]
            {
                DbAdapter.CreateParameter("@batchId", batchId),
                DbAdapter.CreateParameter("@empId", empId),
                DbAdapter.CreateParameter("@workDate", workDate),
                DbAdapter.CreateParameter("@clockType", clockType)
            };
            var count = _dbAdapter.ExecuteScalar<int>(sql, parameters);
            return count > 0;
        }

        public void Insert(int batchId, string empId, string empNameZh, string deptNameZh,
            DateTime workDate, string clockType, DateTime? scheduledTime, DateTime? punchTime,
            string checkResult, string anomalyResolution, string dataSource, string remark,
            string excessiveAttendance, string createUser)
        {
            var sql = @"INSERT INTO attendance.punch_records 
                        (batch_id, emp_id, emp_name_zh, dept_name_zh, work_date, clock_type, 
                         scheduled_time, punch_time, check_result, anomaly_resolution, 
                         data_source, remark, excessive_attendance, create_user) 
                        VALUES (@batchId, @empId, @empNameZh, @deptNameZh, @workDate, @clockType, 
                                @scheduledTime, @punchTime, @checkResult, @anomalyResolution, 
                                @dataSource, @remark, @excessiveAttendance, @createUser)";

            var parameters = new[]
            {
                DbAdapter.CreateParameter("@batchId", batchId),
                DbAdapter.CreateParameter("@empId", empId),
                DbAdapter.CreateParameter("@empNameZh", empNameZh),
                DbAdapter.CreateParameter("@deptNameZh", deptNameZh),
                DbAdapter.CreateParameter("@workDate", workDate),
                DbAdapter.CreateParameter("@clockType", clockType),
                DbAdapter.CreateParameter("@scheduledTime", scheduledTime),
                DbAdapter.CreateParameter("@punchTime", punchTime),
                DbAdapter.CreateParameter("@checkResult", checkResult),
                DbAdapter.CreateParameter("@anomalyResolution", anomalyResolution),
                DbAdapter.CreateParameter("@dataSource", dataSource),
                DbAdapter.CreateParameter("@remark", remark),
                DbAdapter.CreateParameter("@excessiveAttendance", excessiveAttendance),
                DbAdapter.CreateParameter("@createUser", createUser)
            };

            _dbAdapter.ExecuteCommand(sql, parameters);
        }
    }
}
