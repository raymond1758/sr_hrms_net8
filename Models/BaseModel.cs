namespace sr_hrms_net8.Models
{
    public abstract class BaseModel
    {
        protected readonly DbAdapter _dbAdapter;

        protected BaseModel(DbAdapter dbAdapter)
        {
            _dbAdapter = dbAdapter ?? throw new ArgumentNullException(nameof(dbAdapter));
        }

    }
}