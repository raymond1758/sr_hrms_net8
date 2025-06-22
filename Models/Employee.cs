using System.Data;

namespace sr_hrms_net8.Models
{
    public class Employee : BaseModel
    {
        public Employee(DbAdapter dbAdapter) : base(dbAdapter)
        {
        }

        /// <summary>
        /// Gets all employee records
        /// </summary>
        /// <returns>DataTable containing all employee data</returns>
        public DataTable QueryAll()
        {
            var sql = @"SELECT * 
                        FROM core.employee 
                        ORDER BY emp_id";

            return _dbAdapter.ExecuteQuery(sql);
        }

        /// <summary>
        /// Checks if an employee record exists for a specific employee ID
        /// </summary>
        /// <param name="empId">Employee ID to check</param>
        /// <returns>True if record exists, false otherwise</returns>
        public bool IsExisted(string empId)
        {
            var sql = @"SELECT COUNT(*) 
                        FROM core.employee 
                        WHERE emp_id = @empId";
            var parameters = new[]
            {
                DbAdapter.CreateParameter("@empId", empId)
            };
            var count = _dbAdapter.ExecuteScalar<int>(sql, parameters);
            return count > 0;
        }

        /// <summary>
        /// Inserts a new employee record
        /// </summary>
        /// <param name="deptId">部門代碼</param>
        /// <param name="empId">員工編號</param>
        /// <param name="empNameZh">中文姓名</param>
        /// <param name="empNameEn">英文姓名</param>
        /// <param name="birthday">生日</param>
        /// <param name="gender">性別 (M/F)</param>
        /// <param name="nationality">國籍</param>
        /// <param name="domesticOrForeign">本國或移工 (D/F)</param>
        /// <param name="identityGroup">身份種類</param>
        /// <param name="disability">身障類別</param>
        /// <param name="onboardDate">到職日期</param>
        /// <param name="jobCategory">職務類別</param>
        /// <param name="jobTitle">職稱</param>
        /// <param name="empType">員工類型 Direct/Indirect</param>
        /// <param name="employmentStatus">員工任職狀態</param>
        /// <param name="createUser">建立者</param>
        /// <param name="suspensionOrResignationDate">留停或離職日</param>
        /// <param name="reinstatementDate">復職日期</param>
        /// <returns>Number of affected rows</returns>
        public int Insert(string deptId, string empId, string empNameZh, string empNameEn, 
            DateTime birthday, char gender, string nationality, char domesticOrForeign, 
            string identityGroup, string disability, DateTime onboardDate, string jobCategory, 
            string jobTitle, char empType, string employmentStatus, string createUser,
            DateTime? suspensionOrResignationDate = null, DateTime? reinstatementDate = null)
        {
            var sql = @"
                INSERT INTO core.employee 
                (dept_id, emp_id, emp_name_zh, emp_name_en, birthday, gender, nationality, 
                 domestic_or_foreign, identity_group, disability, onboard_date, job_category, 
                 job_title, emp_type, employment_status, suspension_or_resignation_date, 
                 reinstatement_date, create_user, upd_user) 
                VALUES 
                (@deptId, @empId, @empNameZh, @empNameEn, @birthday, @gender, @nationality, 
                 @domesticOrForeign, @identityGroup, @disability, @onboardDate, @jobCategory, 
                 @jobTitle, @empType, @employmentStatus, @suspensionOrResignationDate, 
                 @reinstatementDate, @createUser, @createUser)";

            var parameters = new[]
            {
                DbAdapter.CreateParameter("@deptId", deptId),
                DbAdapter.CreateParameter("@empId", empId),
                DbAdapter.CreateParameter("@empNameZh", empNameZh),
                DbAdapter.CreateParameter("@empNameEn", empNameEn),
                DbAdapter.CreateParameter("@birthday", birthday),
                DbAdapter.CreateParameter("@gender", gender),
                DbAdapter.CreateParameter("@nationality", nationality),
                DbAdapter.CreateParameter("@domesticOrForeign", domesticOrForeign),
                DbAdapter.CreateParameter("@identityGroup", identityGroup),
                DbAdapter.CreateParameter("@disability", disability),
                DbAdapter.CreateParameter("@onboardDate", onboardDate),
                DbAdapter.CreateParameter("@jobCategory", jobCategory),
                DbAdapter.CreateParameter("@jobTitle", jobTitle),
                DbAdapter.CreateParameter("@empType", empType),
                DbAdapter.CreateParameter("@employmentStatus", employmentStatus),
                DbAdapter.CreateParameter("@suspensionOrResignationDate", suspensionOrResignationDate),
                DbAdapter.CreateParameter("@reinstatementDate", reinstatementDate),
                DbAdapter.CreateParameter("@createUser", createUser)
            };

            return _dbAdapter.ExecuteCommand(sql, parameters);
        }

        /// <summary>
        /// Updates an employee record
        /// </summary>
        /// <param name="empId">員工編號</param>
        /// <param name="deptId">部門代碼</param>
        /// <param name="empNameZh">中文姓名</param>
        /// <param name="empNameEn">英文姓名</param>
        /// <param name="birthday">生日</param>
        /// <param name="gender">性別 (M/F)</param>
        /// <param name="nationality">國籍</param>
        /// <param name="domesticOrForeign">本國或移工 (D/F)</param>
        /// <param name="identityGroup">身份種類</param>
        /// <param name="disability">身障類別</param>
        /// <param name="onboardDate">到職日期</param>
        /// <param name="jobCategory">職務類別</param>
        /// <param name="jobTitle">職稱</param>
        /// <param name="empType">員工類型 Direct/Indirect</param>
        /// <param name="employmentStatus">員工任職狀態</param>
        /// <param name="updUser">更新者</param>
        /// <param name="suspensionOrResignationDate">留停或離職日</param>
        /// <param name="reinstatementDate">復職日期</param>
        /// <returns>Number of affected rows</returns>
        public int Update(string empId, string deptId, string empNameZh, string empNameEn, 
            DateTime birthday, char gender, string nationality, char domesticOrForeign, 
            string identityGroup, string disability, DateTime onboardDate, string jobCategory, 
            string jobTitle, char empType, string employmentStatus, string updUser,
            DateTime? suspensionOrResignationDate = null, DateTime? reinstatementDate = null)
        {
            var sql = @"UPDATE core.employee 
                        SET dept_id = @deptId, 
                            emp_name_zh = @empNameZh, 
                            emp_name_en = @empNameEn, 
                            birthday = @birthday, 
                            gender = @gender, 
                            nationality = @nationality, 
                            domestic_or_foreign = @domesticOrForeign, 
                            identity_group = @identityGroup, 
                            disability = @disability, 
                            onboard_date = @onboardDate, 
                            job_category = @jobCategory, 
                            job_title = @jobTitle, 
                            emp_type = @empType, 
                            employment_status = @employmentStatus, 
                            suspension_or_resignation_date = @suspensionOrResignationDate, 
                            reinstatement_date = @reinstatementDate, 
                            upd_date = CURRENT_TIMESTAMP, 
                            upd_user = @updUser 
                        WHERE emp_id = @empId";

            var parameters = new[]
            {
                DbAdapter.CreateParameter("@empId", empId),
                DbAdapter.CreateParameter("@deptId", deptId),
                DbAdapter.CreateParameter("@empNameZh", empNameZh),
                DbAdapter.CreateParameter("@empNameEn", empNameEn),
                DbAdapter.CreateParameter("@birthday", birthday),
                DbAdapter.CreateParameter("@gender", gender),
                DbAdapter.CreateParameter("@nationality", nationality),
                DbAdapter.CreateParameter("@domesticOrForeign", domesticOrForeign),
                DbAdapter.CreateParameter("@identityGroup", identityGroup),
                DbAdapter.CreateParameter("@disability", disability),
                DbAdapter.CreateParameter("@onboardDate", onboardDate),
                DbAdapter.CreateParameter("@jobCategory", jobCategory),
                DbAdapter.CreateParameter("@jobTitle", jobTitle),
                DbAdapter.CreateParameter("@empType", empType),
                DbAdapter.CreateParameter("@employmentStatus", employmentStatus),
                DbAdapter.CreateParameter("@suspensionOrResignationDate", suspensionOrResignationDate),
                DbAdapter.CreateParameter("@reinstatementDate", reinstatementDate),
                DbAdapter.CreateParameter("@updUser", updUser)
            };

            return _dbAdapter.ExecuteCommand(sql, parameters);
        }
    }
}