using Microsoft.AspNetCore.Mvc;
using sr_hrms_net8.Pages.Shared;
using sr_hrms_net8.Services;
using System.Data;

namespace sr_hrms_net8.Pages;

public class CalendarModel : BasePageModel
{
    [BindProperty]
    public IFormFile? UploadFile { get; set; }
    [BindProperty]
    public bool OverwriteExisting { get; set; } = false;
    [BindProperty]
    public int SelectedYear { get; set; }
    public string? SuccessMessage { get; set; }
    public string? ErrorMessage { get; set; }
    public List<string> AvailableYears { get; set; } = new List<string>();
    private CalendarService _calendarSvc;

    public CalendarModel()
    {
        _calendarSvc = new CalendarService(_dbAdapter);
        RefreshAvailableYears();
    }

    private void RefreshAvailableYears()
    {
        if (_calendarSvc is not null)
        {
            var _df_years = _calendarSvc.QueryAvailableYears();
            if (_df_years.Rows.Count > 0)
            {
                AvailableYears = _df_years.AsEnumerable()
                                .Select(row => row.Field<int>("Year").ToString())
                                .ToList();
                SelectedYear = int.Parse(AvailableYears.FirstOrDefault() ?? DateTime.Now.Year.ToString());
            }
            Console.WriteLine($"Available Years: {string.Join(", ", AvailableYears)}");
        }
    }
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
                int _records_effected = _calendarSvc.ImportCSV(stream, OverwriteExisting);

                if (_records_effected > 0)
                {
                    SuccessMessage = $"匯入成功，資料筆數：{_records_effected}";
                    RefreshAvailableYears();
                }
                else
                {
                    ErrorMessage = "匯入失敗，請確認檔案格式是否正確或是否重複匯入";
                }
            }
        }
        return Page();
    }

}