using System.Data;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace sr_hrms_net8.Utilities
{
    public static class CsvHelper
    {
        public static DataTable ToDataTable(Stream csvStream, Dictionary<string, string>? renameColumns = null)
        {
            var dataTable = new DataTable();
            
            using var reader = new StreamReader(csvStream);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                TrimOptions = TrimOptions.Trim
            });

            // Read the header record to create columns
            csv.Read();
            csv.ReadHeader();
            
            if (csv.HeaderRecord != null)
            {
                foreach (var header in csv.HeaderRecord)
                {
                    // Check if column should be renamed
                    var columnName = header;
                    if (renameColumns != null && renameColumns.ContainsKey(header))
                    {
                        columnName = renameColumns[header];
                    }
                    
                    dataTable.Columns.Add(columnName, typeof(string));
                }
            }

            // Read all records and add to DataTable
            while (csv.Read())
            {
                var row = dataTable.NewRow();
                for (int i = 0; i < csv.HeaderRecord?.Length; i++)
                {
                    row[i] = csv.GetField(i) ?? string.Empty;
                }
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }
    }
}