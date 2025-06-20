using Microsoft.AspNetCore.Mvc;
using sr_hrms_net8.Pages.Shared;
using Microsoft.Data.Analysis;

namespace sr_hrms_net8.Pages;

public class CalendarModel : BasePageModel
{
    public CalendarModel()
    {
    }
    [BindProperty]
    public IFormFile? UploadFile { get; set; }


    public IActionResult OnPostSubmitCSV()
    {
        if (UploadFile == null || UploadFile.Length == 0)
            return BadRequest("CSV file is required.");

        string fname = UploadFile?.FileName ?? string.Empty;
        if (UploadFile != null && UploadFile.Length > 0)
        {
            // load the uploaded csv file into a dataframe without saving it to storage, i.e use memorystream to load the file
            using (var stream = UploadFile.OpenReadStream())
            {
                var df = DataFrame.LoadCsv(stream);
                df.Columns.RenameColumn("西元日期", "calendar_date");
                df.Columns.RenameColumn("星期", "weekday");
                df.Columns.RenameColumn("是否放假", "is_holiday");
                df.Columns.RenameColumn("備註", "remark");

                base._connection.Open();
                // _db.Calendars.AddRange(calendars);
                // _db.SaveChanges();
                // Render df as table to the client
                Console.WriteLine(df);
            }
        }
        return Page();
    }

}