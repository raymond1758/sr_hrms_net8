using Microsoft.AspNetCore.Mvc;
using sr_hrms_net8.Models;
using sr_hrms_net8.Pages.Shared;
using sr_hrms_net8.Services;
using System.Data;

namespace sr_hrms_net8.Pages
{
    public class PayrollBatchesModel : BasePageModel
    {
        [BindProperty(SupportsGet = true)]
        public int SelectedYear { get; set; } = DateTime.Now.Year;

        [BindProperty]
        public int NewPayrollYear { get; set; }
        [BindProperty]
        public int NewPayrollMonth { get; set; }
        [BindProperty]
        public string? NewDescription { get; set; }
        [BindProperty]
        public string? NewStatus { get; set; } = "Init";

        [BindProperty]
        public int EditBatchId { get; set; }
        [BindProperty]
        public int EditPayrollYear { get; set; }
        [BindProperty]
        public int EditPayrollMonth { get; set; }
        [BindProperty]
        public string? EditDescription { get; set; }
        [BindProperty]
        public string? EditStatus { get; set; }

        public DataTable PayrollBatchData { get; set; } = new DataTable();
        public string? SuccessMessage { get; set; }
        public string? ErrorMessage { get; set; }
        public Dictionary<string, string> ColumnNameMap { get; set; } = new Dictionary<string, string>();

        private readonly PayrollBatchService _payrollBatchSvc;

        public PayrollBatchesModel(DbAdapter dbAdapter) : base(dbAdapter)
        {
            _payrollBatchSvc = new PayrollBatchService(_dbAdapter);
            LoadColumnNameMap();
        }

        public void OnGet()
        {
            LoadPayrollBatchData();
        }

        public IActionResult OnPost()
        {
            LoadPayrollBatchData();
            return Page();
        }

        private void LoadPayrollBatchData()
        {
            try
            {
                PayrollBatchData = _payrollBatchSvc.QueryByYear(SelectedYear);
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error loading payroll batch data: " + ex.Message;
                PayrollBatchData = new DataTable();
            }
        }

        private void LoadColumnNameMap()
        {
            ColumnNameMap = new Dictionary<string, string>
            {
                { "batch_id", "Batch ID" },
                { "payroll_year", "Year" },
                { "payroll_month", "Month" },
                { "description", "Description" },
                { "status", "Status" },
                { "create_date", "Create Date" },
                { "create_user", "Create User" },
                { "upd_date", "Update Date" },
                { "upd_user", "Update User" }
            };
        }

        public IActionResult OnPostCreateBatch()
        {
            try
            {
                var payrollBatch = new PayrollBatch(_dbAdapter);
                // TODO: Get from current user context
                payrollBatch.Insert(NewPayrollYear, NewPayrollMonth, NewDescription ?? "", NewStatus ?? "Init", "system");
                SuccessMessage = "Payroll batch created successfully.";
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error creating payroll batch: " + ex.Message;
            }
            LoadPayrollBatchData();
            return Page();
        }

        public IActionResult OnPostUpdateBatch()
        {
            try
            {
                var payrollBatch = new PayrollBatch(_dbAdapter);
                // TODO: Get from current user context
                payrollBatch.Update(EditBatchId, EditDescription ?? "", EditStatus ?? "", "system");
                SuccessMessage = "Payroll batch updated successfully.";
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error updating payroll batch: " + ex.Message;
            }
            LoadPayrollBatchData();
            return Page();
        }

        public IActionResult OnPostDeleteBatch(int batchId)
        {
            try
            {
                var payrollBatch = new PayrollBatch(_dbAdapter);
                payrollBatch.Delete(batchId);
                SuccessMessage = "Payroll batch deleted successfully.";
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error deleting payroll batch: " + ex.Message;
            }
            LoadPayrollBatchData();
            return Page();
        }
    }
}
