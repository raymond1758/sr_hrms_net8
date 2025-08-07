
using sr_hrms_net8.Models;
using System.Data;

namespace sr_hrms_net8.Services
{
    public class PunchRecordService : BaseService
    {
        public PunchRecordService(DbAdapter dbAdapter) : base(dbAdapter)
        {
        }

        public DataTable QueryPunchRecords(string filter, int pageNumber, int pageSize, out int totalRecords)
        {
            var sql_filter = string.IsNullOrEmpty(filter) ? "" : $"%{filter}%";
            var sql = $@"SELECT p.*, e.emp_name_zh, d.dept_name_zh
                        FROM attendance.punch_records p
                        LEFT JOIN core.employee e ON p.emp_id = e.emp_id
                        LEFT JOIN core.department d ON e.dept_id = d.dept_id
                        WHERE (@filter = '' OR (e.emp_name_zh LIKE @filter OR p.emp_id LIKE @filter))
                        ORDER BY p.punch_id
                        LIMIT @pageSize OFFSET @offset";

            var countSql = $@"SELECT COUNT(*)
                              FROM attendance.punch_records p
                              LEFT JOIN core.employee e ON p.emp_id = e.emp_id
                              WHERE (@filter = '' OR (e.emp_name_zh LIKE @filter OR p.emp_id LIKE @filter))";

            var parameters = new[]
            {
                DbAdapter.CreateParameter("@filter", sql_filter),
                DbAdapter.CreateParameter("@pageSize", pageSize),
                DbAdapter.CreateParameter("@offset", (pageNumber - 1) * pageSize)
            };

            totalRecords = _dbAdapter.ExecuteScalar<int>(countSql, new[] { DbAdapter.CreateParameter("@filter", sql_filter) });
            return _dbAdapter.ExecuteQuery(sql, parameters);
        }

        public int ImportCSV(Stream csvStream, string upd_user)
        {
            Func<string, DateTime?> parseDatetime = s => string.IsNullOrWhiteSpace(s) ? null : DateTime.TryParse(s, out var dt) ? dt : null;

            var renameColumns = new Dictionary<string, string>
            {
                { "員工編號", "emp_id" },
                { "中文姓名", "emp_name_zh" },
                { "部門", "dept_name_zh" },
                { "工作日", "work_date" },
                { "打卡類別 (上班/下班)", "clock_type" },
                { "表定時間", "scheduled_time" },
                { "打卡時間", "punch_time" },
                { "比對結果", "check_result" },
                { "異常原因", "anomaly_resolution" },
                { "資料來源", "data_source" },
                { "備註", "remark" },
                { "超時出勤", "excessive_attendance" }
            };
            var dt = sr_hrms_net8.Utilities.CsvHelper.ToDataTable(csvStream, renameColumns);
            
            var punchRecord = new sr_hrms_net8.Models.PunchRecord(_dbAdapter);
            int _count = 0;

            foreach (DataRow row in dt.Rows)
            {
                var batchId = 0; // Assuming a default batch_id for now
                var emp_Id = row["emp_id"]?.ToString() ?? "";
                var emp_name_zh = row["emp_name_zh"]?.ToString() ?? "";
                var dept_name_zh = row["dept_name_zh"]?.ToString() ?? "";
                var work_date = parseDatetime(row["work_date"]?.ToString() ?? "");
                var clock_type = row["clock_type"]?.ToString() ?? "";
                var scheduled_time = parseDatetime(row["scheduled_time"]?.ToString() ?? "");
                var punch_time = parseDatetime(row["punch_time"]?.ToString() ?? "");
                var check_result = row["check_result"]?.ToString() ?? "";
                var anomaly_resolution = row["anomaly_resolution"]?.ToString() ?? "";
                var data_source = row["data_source"]?.ToString() ?? "";
                var remark = row["remark"]?.ToString() ?? "";
                var excessive_attendance = row["excessive_attendance"]?.ToString() ?? "";

                if (work_date.HasValue)
                {
                    var recordExists = punchRecord.IsExisted(batchId, emp_Id, work_date.Value, clock_type);
                    if (!recordExists)
                    {
                        punchRecord.Insert(batchId, emp_Id, emp_name_zh, dept_name_zh, work_date.Value, clock_type, 
                                         scheduled_time, punch_time, check_result, anomaly_resolution, 
                                         data_source, remark, excessive_attendance, upd_user);
                        _count++;
                    }
                }
            }

            return _count;
        }
    }
}
