
using sr_hrms_net8.Models;
using System.Data;
using Npgsql;

namespace sr_hrms_net8.Services
{
    public class PunchRecordService : BaseService
    {
        public PunchRecordService(DbAdapter dbAdapter) : base(dbAdapter)
        {
        }

        public DataTable QueryPunchRecords(string filter, int pageNumber, int pageSize, out int totalRecords, DateTime? startDate = null, DateTime? endDate = null)
        {
            Console.WriteLine($"QueryPunchRecords - filter: '{filter}', pageNumber: {pageNumber}, pageSize: {pageSize}");
            
            // First let's check if there's any data at all
            var testSql = "SELECT COUNT(*) FROM attendance.punch_records";
            var testCount = 0;
            try 
            {
                testCount = _dbAdapter.ExecuteScalar<int>(testSql);
                Console.WriteLine($"Total records in punch_records table: {testCount}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking punch_records table: {ex.Message}");
                totalRecords = 0;
                return new DataTable();
            }
            
            if (testCount == 0)
            {
                Console.WriteLine("No data found in punch_records table - creating dummy data for testing");
                totalRecords = 0;
                return new DataTable();
            }
            
            var sql_filter = string.IsNullOrEmpty(filter) ? "" : $"%{filter}%";
            var offset = (pageNumber - 1) * pageSize;
            
            Console.WriteLine($"QueryPunchRecords - sql_filter: '{sql_filter}', offset: {offset}");
            
            var whereConditions = new List<string>();
            if (!string.IsNullOrEmpty(filter))
            {
                whereConditions.Add("(e.emp_name_zh LIKE @filter OR p.emp_id LIKE @filter OR d.dept_name_zh LIKE @filter)");
            }
            if (startDate.HasValue)
            {
                whereConditions.Add("p.work_date >= @startDate");
            }
            if (endDate.HasValue)
            {
                whereConditions.Add("p.work_date <= @endDate");
            }
            
            var whereClause = whereConditions.Count > 0 ? "WHERE " + string.Join(" AND ", whereConditions) : "";
            
            var sql = $@"SELECT e.emp_name_zh, d.dept_name_zh, p.*
                        FROM attendance.punch_records p
                        LEFT JOIN core.employee e ON p.emp_id = e.emp_id
                        LEFT JOIN core.department d ON e.dept_id = d.dept_id
                        {whereClause}
                        ORDER BY p.punch_id
                        LIMIT @pageSize OFFSET @offset";

            var countSql = $@"SELECT COUNT(*)
                              FROM attendance.punch_records p
                              LEFT JOIN core.employee e ON p.emp_id = e.emp_id
                              LEFT JOIN core.department d ON e.dept_id = d.dept_id
                              {whereClause}";

            var parameterList = new List<NpgsqlParameter>
            {
                DbAdapter.CreateParameter("@pageSize", pageSize),
                DbAdapter.CreateParameter("@offset", offset)
            };
            
            var countParameterList = new List<NpgsqlParameter>();
            
            if (!string.IsNullOrEmpty(filter))
            {
                parameterList.Add(DbAdapter.CreateParameter("@filter", sql_filter));
                countParameterList.Add(DbAdapter.CreateParameter("@filter", sql_filter));
            }
            if (startDate.HasValue)
            {
                parameterList.Add(DbAdapter.CreateParameter("@startDate", startDate.Value));
                countParameterList.Add(DbAdapter.CreateParameter("@startDate", startDate.Value));
            }
            if (endDate.HasValue)
            {
                parameterList.Add(DbAdapter.CreateParameter("@endDate", endDate.Value));
                countParameterList.Add(DbAdapter.CreateParameter("@endDate", endDate.Value));
            }

            totalRecords = _dbAdapter.ExecuteScalar<int>(countSql, countParameterList.ToArray());
            
            Console.WriteLine($"QueryPunchRecords - totalRecords: {totalRecords}");
            
            var result = _dbAdapter.ExecuteQuery(sql, parameterList.ToArray());
            
            Console.WriteLine($"QueryPunchRecords - returned {result.Rows.Count} rows");
            Console.WriteLine($"Executed SQL: {sql}");
            Console.WriteLine($"Parameters: filter='{sql_filter}', pageSize={pageSize}, offset={offset}");
            
            return result;
        }

        public int ImportCSV(Stream csvStream, string upd_user)
        {
            Func<string, DateTime?> parseDatetime = s => string.IsNullOrWhiteSpace(s) ? null : DateTime.TryParse(s, out var dt) ? dt : null;

            var renameColumns = new Dictionary<string, string>
            {
                { "#", "row_number" },
                { "員工編號", "emp_id" },
                { "姓名", "emp_name_zh" },
                { "部門", "dept_name_zh" },
                { "工作日期", "work_date" },
                { "應刷卡時間", "scheduled_time" },
                { "實際打卡時間", "punch_time" },
                { "卡別", "clock_type" },
                { "打卡地址", "punch_address" },
                { "比對結果", "check_result" },
                { "異常處理", "anomaly_resolution" },
                { "來源", "data_source" },
                { "備註", "remark" },
                { "超時出勤", "excessive_attendance" },
                { "超時出勤原因", "excessive_attendance_reason" },
                { "超時出勤說明", "excessive_attendance_description" }
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
