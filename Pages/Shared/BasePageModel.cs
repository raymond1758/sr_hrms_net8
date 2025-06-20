using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;

namespace sr_hrms_net8.Pages.Shared
{
    public abstract class BasePageModel : PageModel
    {
        protected NpgsqlConnection _connection;
        public BasePageModel()
        {
            var connectionString = "Host=127.0.0.1;Port=5632;Database=sr_hrms;Username=postgres;Password=ddgpemhf";
            _connection = new NpgsqlConnection(connectionString);
            _connection.Open();
        }
    }
}