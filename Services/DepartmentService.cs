using sr_hrms_net8.Models;
using System.Data;

namespace sr_hrms_net8.Services
{
    public class DepartmentService : BaseService
    {
        public DepartmentService(DbAdapter dbAdapter) : base(dbAdapter)
        {
        }

        public DataTable GetAllDepartments()
        {
            var sql = @"WITH RECURSIVE dept_hierarchy AS (
                        -- Base case: top-level departments
                        SELECT 
                            dept_id,
                            dept_name_zh,
                            up_dept,
                            dept_name_zh::text AS path,
                            1 AS level
                        FROM core.department 
                        WHERE up_dept = ''

                        UNION ALL

                        -- Recursive case: child departments
                        SELECT 
                            d.dept_id,
                            d.dept_name_zh,
                            d.up_dept,
                            dh.path || ' → ' || d.dept_name_zh,
                            dh.level + 1
                        FROM core.department d
                        JOIN dept_hierarchy dh ON d.up_dept = dh.dept_id
                        )

                        SELECT * 
                        FROM dept_hierarchy
                        ORDER BY dept_id;";
            
            return _dbAdapter.ExecuteQuery(sql);
        }
    }
}
