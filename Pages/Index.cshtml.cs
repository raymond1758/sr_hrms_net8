using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace sr_hrms_net8.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    // public void OnGet()
    // {        
    // }

    public IActionResult OnGet()
    {
        return RedirectToPage("/calendar");
    }
}
