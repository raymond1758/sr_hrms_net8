using sr_hrms_net8.Models;

namespace sr_hrms_net8.Services
{
    public abstract class BaseService
    {
        protected readonly DbAdapter _dbAdapter;

        protected BaseService(DbAdapter dbAdapter)
        {
            _dbAdapter = dbAdapter ?? throw new ArgumentNullException(nameof(dbAdapter));
        }
    }
}