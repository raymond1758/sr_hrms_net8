using sr_hrms_net8.Models;
using System.Data;

namespace sr_hrms_net8.Services
{
    public class CalendarService : BaseService
    {
        public CalendarService(DbAdapter dbAdapter) : base(dbAdapter)
        {
        }

        public int ImportCSV(Stream csvStream, bool overwrite = false)
        {
            var renameColumns = new Dictionary<string, string>
            {
                { "西元日期", "calendar_date" },
                { "星期", "day_of_week" },
                { "是否放假", "state" },
                { "備註", "remark" }
            };
            var dt = sr_hrms_net8.Utilities.CsvHelper.ToDataTable(csvStream, renameColumns);
            var calendar = new sr_hrms_net8.Models.Calendar(_dbAdapter);
            // loop the data table and insert into database
            int _count = 0;
            foreach (DataRow row in dt.Rows)
            {
                // check if record exists
                var calendarDateString = row["calendar_date"]?.ToString();

                if (string.IsNullOrWhiteSpace(calendarDateString) || calendarDateString.Length != 8)
                {
                    Console.WriteLine($"Error getting calendar date: {calendarDateString}");
                    continue;
                }

                if (!DateTime.TryParseExact(calendarDateString, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime calendarDate))
                {
                    Console.WriteLine($"Error parsing calendar date: {calendarDateString}");
                    continue;
                }
                var state = int.Parse(row["state"]?.ToString() ?? "0");
                var remark = row["remark"]?.ToString() ?? "";
                var recordExists = calendar.IsExisted(calendarDate);
                if (!recordExists)
                {
                    calendar.Insert(calendarDate, state, remark);
                    _count++;
                }
                else
                {
                    if (overwrite)
                    {
                        calendar.Update(calendarDate, state, remark);
                        _count++;
                    }
                }
            }

            return _count;
        }

        public DataTable QueryAvailableYears()
        {
            var sql = @"SELECT distinct extract (year from calendar_date)::integer as Year
                        FROM core.calendar
                        ORDER BY Year desc";
            return _dbAdapter.ExecuteQuery(sql);
        }

        public DataTable QueryCalendarByYear(int year)
        {
            var sql = @"SELECT * 
                        FROM core.calendar 
                        WHERE extract(year from calendar_date) = @Year";
            var parameters = new[]
            {
                DbAdapter.CreateParameter("@Year", year)
            };
            return _dbAdapter.ExecuteQuery(sql, parameters);
        }
    }
}