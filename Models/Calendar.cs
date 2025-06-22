using System.Data;

namespace sr_hrms_net8.Models
{
    public class Calendar : BaseModel
    {
        public Calendar(DbAdapter dbAdapter) : base(dbAdapter)
        {
        }

        /// <summary>
        /// Gets all calendar records
        /// </summary>
        /// <returns>DataTable containing all calendar data</returns>
        public DataTable QueryAll()
        {
            var sql = @"SELECT * 
                        FROM core.calendar 
                        ORDER BY calendar_date";

            return _dbAdapter.ExecuteQuery(sql);
        }

        /// <summary>
        /// Gets calendar data for a specific date range
        /// </summary>
        /// <param name="startDate">Start date</param>
        /// <param name="endDate">End date</param>
        /// <returns>DataTable containing calendar data for the specified range</returns>
        public DataTable QueryByDateRange(DateTime startDate, DateTime endDate)
        {
            var sql = @"SELECT * 
                        FROM core.calendar 
                        WHERE calendar_date BETWEEN @startDate AND @endDate 
                        ORDER BY calendar_date";
            var parameters = new[]
            {
                DbAdapter.CreateParameter("@startDate", startDate),
                DbAdapter.CreateParameter("@endDate", endDate)
            };

            return _dbAdapter.ExecuteQuery(sql, parameters);
        }

        /// <summary>
        /// Checks if a calendar record exists for a specific date
        /// </summary>
        /// <param name="calendarDate">Calendar date to check</param>
        /// <returns>True if record exists, false otherwise</returns>
        public bool IsExisted(DateTime calendarDate)
        {
            var sql = @"SELECT COUNT(*) 
                        FROM core.calendar 
                        WHERE calendar_date = @calendarDate";
            var parameters = new[]
            {
                DbAdapter.CreateParameter("@calendarDate", calendarDate)
            };
            var count = _dbAdapter.ExecuteScalar<int>(sql, parameters);
            return count > 0;
        }

        /// <summary>
        /// Inserts a new calendar record
        /// </summary>
        /// <param name="calendarDate">Calendar date</param>
        /// <param name="state">Holiday state</param>
        /// <param name="remark">Remark</param>
        /// <returns>Number of affected rows</returns>
        public int Insert(DateTime calendarDate, int state, string? remark = null)
        {
            var sql = @"
                INSERT INTO core.calendar 
                (calendar_date, state, remark) 
                VALUES 
                (@calendarDate, @state, @remark)";

            var parameters = new[]
            {
                DbAdapter.CreateParameter("@calendarDate", calendarDate),
                DbAdapter.CreateParameter("@state", state),
                DbAdapter.CreateParameter("@remark", remark)
            };

            return _dbAdapter.ExecuteCommand(sql, parameters);
        }
        
        /// <summary>
        /// Updates a calendar record
        /// </summary>
        /// <param name="calendarDate">Calendar date</param>
        /// <param name="state">Holiday state</param>
        /// <param name="remark">Remark</param>
        /// <returns>Number of affected rows</returns>
        public int Update(DateTime calendarDate, int state, string? remark = null)
        {
            var sql = @"UPDATE core.calendar 
                        SET state = @state, 
                            remark = @remark 
                        WHERE calendar_date = @calendarDate";

            var parameters = new[]
            {
                DbAdapter.CreateParameter("@calendarDate", calendarDate),
                DbAdapter.CreateParameter("@state", state),
                DbAdapter.CreateParameter("@remark", remark)
            };

            return _dbAdapter.ExecuteCommand(sql, parameters);
        }
    }
}