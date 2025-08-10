using Microsoft.AspNetCore.Mvc;
using sr_hrms_net8.Models;
using sr_hrms_net8.Pages.Shared;
using sr_hrms_net8.Services;
using System.Data;

namespace sr_hrms_net8.Pages;

public class DepartmentModel : BasePageModel
{
    private readonly DepartmentService _departmentSvc;
    public DataTable DepartmentData { get; set; } = new DataTable();
    public string? SuccessMessage { get; set; }
    public string? ErrorMessage { get; set; }
    public Dictionary<string, string> ColumnNameMap { get; set; } = new Dictionary<string, string>();
    public DepartmentModel(DbAdapter dbAdapter) : base(dbAdapter)
    {
        _departmentSvc = new DepartmentService(_dbAdapter);
    }
    public void OnGet()
    {
        LoadDepartmentData();
        LoadColumnNameMap();
    }
    private void LoadDepartmentData()
    {
        DepartmentData = _departmentSvc.GetAllDepartments();
    }

    private void LoadColumnNameMap()
    {
        ColumnNameMap = new Dictionary<string, string>
        {
            { "dept_id", "部門編號" },
            { "dept_name_zh", "部門名稱" },
            { "up_dept", "上層部門" },
            { "path", "部門階層" },
            { "level", "層級" }
        };
    }
}
