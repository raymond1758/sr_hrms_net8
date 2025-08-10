using Microsoft.AspNetCore.Mvc;
using sr_hrms_net8.Models;
using sr_hrms_net8.Pages.Shared;
using sr_hrms_net8.Services;
using System.Data;
using System.Runtime.InteropServices;

namespace sr_hrms_net8.Pages;

public class PunchRecordsModel : BasePageModel
{
    public DataTable PunchRecordData { get; set; } = new DataTable();
    public Dictionary<string, string> ColumnNameMap { get; set; } = new Dictionary<string, string>();
    
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 25;
    public int TotalRecords { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalRecords / PageSize);
    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;
    
    public string? Filter { get; set; }
    
    private readonly PunchRecordService _punchRecordSvc;
    public PunchRecordsModel(DbAdapter dbAdapter) : base(dbAdapter)
    {
        _punchRecordSvc = new PunchRecordService(_dbAdapter);
        LoadColumnNameMap();
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

    private void LoadPunchRecordData()
    {
        PunchRecordData = _punchRecordSvc.QueryPunchRecords(Filter ?? "", CurrentPage, PageSize, out var totalRecords);
        TotalRecords = totalRecords;
    }

    public void OnGet(int page = 1, int pageSize = 25)
    {
        CurrentPage = page < 1 ? 1 : page;
        PageSize = pageSize < 1 ? 25 : pageSize;
        
        Console.WriteLine($"PunchRecords OnGet - Page: {CurrentPage}, PageSize: {PageSize}");
        
        LoadPunchRecordData();
        
        Console.WriteLine($"PunchRecords - Loaded {PunchRecordData.Rows.Count} records, Total: {TotalRecords}");
    }

    public void OnPost(int page = 1, int pageSize = 25, string? filter = null)
    {
        CurrentPage = page < 1 ? 1 : page;
        PageSize = pageSize < 1 ? 25 : pageSize;
        Filter = filter;
        
        Console.WriteLine($"PunchRecords OnPost - Page: {CurrentPage}, PageSize: {PageSize}");
        Console.WriteLine($"Filter: '{Filter}'");
        
        LoadPunchRecordData();
        
        Console.WriteLine($"PunchRecords - Loaded {PunchRecordData.Rows.Count} records, Total: {TotalRecords}");
    }
}
