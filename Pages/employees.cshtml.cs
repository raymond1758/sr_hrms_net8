using Microsoft.AspNetCore.Mvc;
using sr_hrms_net8.Pages.Shared;
using sr_hrms_net8.Services;
using System.Data;

namespace sr_hrms_net8.Pages;

public class EmployeesModel : BasePageModel
{
    public DataTable EmployeeData { get; set; } = new DataTable();
    public string? SuccessMessage { get; set; }
    public string? ErrorMessage { get; set; }
    public Dictionary<string, string> ColumnNameMap { get; set; } = new Dictionary<string, string>();
    private readonly EmployeeService _employeeSvc;

    public EmployeesModel()
    {
        _employeeSvc = new EmployeeService(_dbAdapter);
        LoadEmployeeData();
        LoadColumnNameMap();
    }

    private void LoadEmployeeData()
    {
        EmployeeData = _employeeSvc.QueryAll();
    }

    private void LoadColumnNameMap()
    {
        ColumnNameMap = new Dictionary<string, string>
        {
            { "emp_id", "員工編號" },
            { "emp_name_zh", "中文姓名" },
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
}