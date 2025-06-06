using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;


namespace sr_hrms_net8.Pages;

public class CalendarModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public CalendarModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {        
    }

    public DataTable GetAvailableYM()
    {
        return null;
    }
}