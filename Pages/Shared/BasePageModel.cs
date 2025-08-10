using Microsoft.AspNetCore.Mvc.RazorPages;
using sr_hrms_net8.Models;

namespace sr_hrms_net8.Pages.Shared
{
    public abstract class BasePageModel : PageModel
    {
        protected readonly DbAdapter _dbAdapter;
        
        protected BasePageModel(DbAdapter dbAdapter)
        {
            _dbAdapter = dbAdapter ?? throw new ArgumentNullException(nameof(dbAdapter));
        }
    }
}