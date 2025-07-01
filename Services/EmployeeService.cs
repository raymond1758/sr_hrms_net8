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
            var sql = @"SELECT emp_id, emp_name_zh, d.dept_name_zh, gender, nationality, job_category, 
                            employment_status, onboard_date::date, suspension_or_resignation_date, reinstatement_date, disability
                        FROM core.employee e
                        LEFT OUTER JOIN core.department d on e.dept_id = d.dept_id
                        ORDER BY emp_id";
            return _dbAdapter.ExecuteQuery(sql);
        }

        public DataTable QueryEmployees(string employmentStatus, string filter)
        {
            /* if filter is empty, return all records. else filter by emp_name_zh or emp_id */
            var sql_filter = string.IsNullOrEmpty(filter) ? "" : $"%{filter}%";
            var sql = $@"SELECT emp_id, emp_name_zh, d.dept_name_zh, gender, nationality, job_category, 
                            employment_status, onboard_date::date, suspension_or_resignation_date, reinstatement_date, disability
                        FROM core.employee e
                        LEFT OUTER JOIN core.department d on e.dept_id = d.dept_id
                        WHERE e.employment_status like @employmentStatus
                        and (@filter = '' or (e.emp_name_zh like @filter or e.emp_id like @filter))
                        ORDER BY emp_id";

            var parameters = new[]
            {
                DbAdapter.CreateParameter("@employmentStatus", employmentStatus),
                DbAdapter.CreateParameter("@filter", sql_filter)
            };

            return _dbAdapter.ExecuteQuery(sql, parameters);
        }
        
        public int ImportCSV(Stream csvStream, string upd_user)
        {
            Func<string, DateTime?> parseDatetime = s => string.IsNullOrWhiteSpace(s) ? null : DateTime.TryParse(s, out var dt) ? dt : null;

            var renameColumns = new Dictionary<string, string>
            {
                { "員工編號", "emp_id" },
                { "姓名", "emp_name_zh" },
                { "英文姓名", "emp_name_en" },
                { "身分證字號(外籍員工顯示護照/居留證號)", "id_no" },
                { "生日", "birthday" },
                { "性別", "gender" },
                { "國籍", "nationality" },
                { "職務類別", "job_category" },
                { "職位", "job_title" },
                { "在職狀態", "employment_status" },
                { "到職日期", "onboard_date" },
                { "留停/離職日期", "suspension_or_resignation_date" },
                { "復職日期", "reinstatement_date" },
                { "身心障礙類別", "disability" },
                { "部門", "dept_name_zh" },
                { "身份族群", "identity_group" },
                { "責任區分","emp_type"}
            };
            var dt = sr_hrms_net8.Utilities.CsvHelper.ToDataTable(csvStream, renameColumns);
#if DEBUG
            // Print all columns of DataTable to Console
            Console.WriteLine("DataTable Columns:");
            foreach (DataColumn column in dt.Columns)
            {
                Console.WriteLine($"Column: {column.ColumnName}, Type: {column.DataType}");
            }
            Console.WriteLine($"Total Columns: {dt.Columns.Count}");
            Console.WriteLine($"Total Rows: {dt.Rows.Count}");
#endif
            
            var employee = new sr_hrms_net8.Models.Employee(_dbAdapter);
            var department = new sr_hrms_net8.Models.Department(_dbAdapter);
            // loop the data table and insert into database
            int _count = 0;

            foreach (DataRow row in dt.Rows)
            {
                // check if record exists
                var emp_Id = row["emp_id"]?.ToString() ?? "";
                var emp_name_zh = row["emp_name_zh"]?.ToString() ?? "";
                var emp_name_en = row["emp_name_en"]?.ToString() ?? "";
                var id_no = row["id_no"]?.ToString() ?? "";
                var birthday = row["birthday"]?.ToString() ?? "";
                var gender = row["gender"]?.ToString() ?? "";
                var nationality = row["nationality"]?.ToString() ?? "";
                var job_category = row["job_category"]?.ToString() ?? "";
                var job_title = row["job_title"]?.ToString() ?? "";
                var emp_type = row["emp_type"]?.ToString() ?? "";
                var employment_status = row["employment_status"]?.ToString() ?? "";
                var onboard_date = row["onboard_date"]?.ToString() ?? "";
                var suspension_or_resignation_date = parseDatetime(row["suspension_or_resignation_date"]?.ToString() ?? "");
                var reinstatement_date = parseDatetime(row["reinstatement_date"]?.ToString() ?? "");
                var disability = row["disability"]?.ToString() ?? "";
                var dept_name_zh = row["dept_name_zh"]?.ToString() ?? "";
                var dept_id = department.GetDeptId(dept_name_zh);
                var domestic_or_foreign = (nationality == "台灣，中華民國" ? 'D' : 'F');
                var identity_group = row["identity_group"]?.ToString() ?? "";

                Console.WriteLine($"{dept_name_zh} = {dept_id}");

                var recordExists = employee.IsExisted(emp_Id);
                if (recordExists)
                {
                    employee.Update(emp_Id, dept_id, emp_name_en, identity_group, job_category, job_title, emp_type, employment_status, upd_user,
                                    suspension_or_resignation_date, reinstatement_date);

                    _count++;
                }
                else
                {
                    // employee.Insert(emp_Id, emp_name_zh, id_no, birthday, gender, nationality, job_category, employment_status, onboard_date, suspension_or_resignation_date, reinstatement_date, disability);
                    _count++;
                }

            }

            return _count;
        }
    }
}
 