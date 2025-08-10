using Microsoft.AspNetCore.Mvc;
using sr_hrms_net8.Models;
using sr_hrms_net8.Pages.Shared;
using sr_hrms_net8.Services;
using System.Data;
using System.Runtime.InteropServices;

namespace sr_hrms_net8.Pages;

public class EmployeesModel : BasePageModel
{
    [BindProperty]
    public IFormFile? UploadFile { get; set; }
    [BindProperty]
    public string? SearchText { get; set; }
    [BindProperty(SupportsGet = true)]
    public string? SelectedDepartment { get; set; }
    public DataTable EmployeeData { get; set; } = new DataTable();
    public DataTable DepartmentData { get; set; } = new DataTable();
    public string? SuccessMessage { get; set; }
    public string? ErrorMessage { get; set; }
    
    public Dictionary<string, string> ColumnNameMap { get; set; } = new Dictionary<string, string>();
    private readonly EmployeeService _employeeSvc;
    private readonly DepartmentService _departmentSvc;

    [BindProperty(SupportsGet = true)]
    public string? EmploymentStatus { get; set; }

    public EmployeesModel(DbAdapter dbAdapter) : base(dbAdapter)
    {
        _employeeSvc = new EmployeeService(_dbAdapter);
        _departmentSvc = new DepartmentService(_dbAdapter);
        LoadColumnNameMap();
        LoadDepartmentData();
    }

    public void OnGet()
    {
        // Load employee data with filtering
        LoadEmployeeData();
    }

    public void OnPost()
    {
    }

    private void LoadEmployeeData()
    {
        EmployeeData = _employeeSvc.QueryEmployees(EmploymentStatus ?? "%", SearchText ?? "", SelectedDepartment ?? "");
    }

    private void LoadDepartmentData()
    {
        DepartmentData = _departmentSvc.GetAllDepartments();
    }

    private void LoadColumnNameMap()
    {
        ColumnNameMap = new Dictionary<string, string>
        {
            { "emp_id", "員工編號" },
            { "emp_name_zh", "中文姓名" },
            { "dept_name_zh", "部門" },
            { "gender", "性別" },
            { "nationality", "國籍" },
            { "domestic_or_foreign", "本國(D)/外國(F)" },
            { "identity_group", "身分別" },
            { "disability", "障礙別" },
            { "job_category", "職務類別" },
            { "employment_status", "在職狀態" },
            { "onboard_date", "到職日期" },
            { "suspension_or_resignation_date", "停職/離職日期" },
            { "reinstatement_date", "復職日期" }
        };
    }

    public IActionResult OnPostQueryEmployees()
    {
        Console.WriteLine("QueryEmployees invoked.");
        LoadEmployeeData();
        return Page();
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
                var user = User.Identity?.Name ?? "system";
                var count = _employeeSvc.ImportCSV(stream, "264");
                SuccessMessage = $"匯入完成，共匯入 {count} 筆資料";
            }
        }
        LoadEmployeeData();
        return Page();
    }
}