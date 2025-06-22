using System.Data;


namespace sr_hrms_net8.Models
{
    public class Company: BaseModel
    {

        public Company(DbAdapter dbAdapter): base(dbAdapter)
        {
        }

        /// <summary>
        /// Query all companies from core.company table
        /// </summary>
        /// <returns>DataTable containing all company records</returns>
        public DataTable QueryAll()
        {
            string sql = @"SELECT *
                            FROM core.company 
                            ORDER BY company_code";
            return _dbAdapter.ExecuteQuery(sql);
        }

        /// <summary>
        /// Check if company exists by company_reg_id (primary key)
        /// </summary>
        /// <param name="companyRegId">營利事業登記 / 統一編號</param>
        /// <returns>True if company exists, false otherwise</returns>
        public bool IsExisted(string companyRegId)
        {
            var sql = @"SELECT COUNT(*)
                        FROM core.company 
                        WHERE company_reg_id = @company_reg_id";
            
            var parameters = new[]
            {
                DbAdapter.CreateParameter("@company_reg_id", companyRegId)
            };
             var count = _dbAdapter.ExecuteScalar<int>(sql, parameters);
            return count > 0;
        }

        /// <summary>
        /// Check if company exists by company_code (unique constraint)
        /// </summary>
        /// <param name="companyCode">公司簡碼</param>
        /// <returns>True if company code exists, false otherwise</returns>
        public bool IsExistedByCode(string companyCode)
        {
            string sql = "SELECT COUNT(*) FROM core.company WHERE company_code = @company_code";
            
            var parameters = new[]
            {
                DbAdapter.CreateParameter("@company_code", companyCode)
            };

            var count = _dbAdapter.ExecuteScalar<int>(sql, parameters);
            return count > 0;
        }

        /// <summary>
        /// Insert new company record
        /// </summary>
        /// <param name="companyRegId">營利事業登記 / 統一編號</param>
        /// <param name="companyNameZh">公司名稱 (中文)</param>
        /// <param name="companyNameEn">公司名稱 (英文)</param>
        /// <param name="address">地址</param>
        /// <param name="companyCode">公司簡碼</param>
        /// <param name="createUser">建立使用者</param>
        /// <returns>Number of affected rows</returns>
        public int Insert(string companyRegId, string companyNameZh, string companyNameEn, 
                         string address, string companyCode, string createUser)
        {
            string sql = @"
                INSERT INTO core.company 
                (company_reg_id, company_name_zh, company_name_en, address, company_code, 
                 create_date, create_user, upd_date, upd_user)
                VALUES 
                (@company_reg_id, @company_name_zh, @company_name_en, @address, @company_code,
                 CURRENT_TIMESTAMP, @create_user, CURRENT_TIMESTAMP, @upd_user)";

            var parameters = new[]
            {
                DbAdapter.CreateParameter("@company_reg_id", companyRegId),
                DbAdapter.CreateParameter("@company_name_zh", companyNameZh),
                DbAdapter.CreateParameter("@company_name_en", companyNameEn),
                DbAdapter.CreateParameter("@address", address),
                DbAdapter.CreateParameter("@company_code", companyCode),
                DbAdapter.CreateParameter("@create_user", createUser),
                DbAdapter.CreateParameter("@upd_user", createUser)
            };
            return _dbAdapter.ExecuteCommand(sql, parameters);
        }

        /// <summary>
        /// Update existing company record
        /// </summary>
        /// <param name="companyRegId">營利事業登記 / 統一編號 (Primary Key)</param>
        /// <param name="companyNameZh">公司名稱 (中文)</param>
        /// <param name="companyNameEn">公司名稱 (英文)</param>
        /// <param name="address">地址</param>
        /// <param name="companyCode">公司簡碼</param>
        /// <param name="updUser">更新使用者</param>
        /// <returns>Number of affected rows</returns>
        public int Update(string companyRegId, string companyNameZh, string companyNameEn,
                         string address, string companyCode, string updUser)
        {
            string sql = @"
                UPDATE core.company 
                SET 
                    company_name_zh = @company_name_zh,
                    company_name_en = @company_name_en,
                    address = @address,
                    company_code = @company_code,
                    upd_date = CURRENT_TIMESTAMP,
                    upd_user = @upd_user
                WHERE company_reg_id = @company_reg_id";

            var parameters = new[]
            {
                DbAdapter.CreateParameter("@company_reg_id", companyRegId),
                DbAdapter.CreateParameter("@company_name_zh", companyNameZh),
                DbAdapter.CreateParameter("@company_name_en", companyNameEn),
                DbAdapter.CreateParameter("@address", address),
                DbAdapter.CreateParameter("@company_code", companyCode),
                DbAdapter.CreateParameter("@upd_user", updUser)
            };

            return _dbAdapter.ExecuteCommand(sql, parameters);
        }
    }
}