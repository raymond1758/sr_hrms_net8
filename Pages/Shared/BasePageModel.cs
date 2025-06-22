using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;
using sr_hrms_net8.Models;


namespace sr_hrms_net8.Pages.Shared
{
    public abstract class BasePageModel : PageModel
    {
        protected DbAdapter _dbAdapter;
        public BasePageModel()
        {
            var connectionString = "Host=127.0.0.1;Port=5632;Database=hrms;Username=postgres;Password=ddgpemhf;Timeout=3";
            _dbAdapter = new DbAdapter(connectionString);
        }
    }
}