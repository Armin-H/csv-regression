using System.Data;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CsvRegression.Data;
using CsvRegression.Domain;
using Microsoft.Win32;

namespace CsvRegression;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private DataView? _rows;

    [ObservableProperty]
    private string _statusText = "Ready";

    [RelayCommand]
    private void OpenCsv()
    {
        var dialog = new OpenFileDialog { Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*" };
        if (dialog.ShowDialog() != true) return;

        var table = CsvTableLoader.Load(dialog.FileName);
        Rows = ToDataView(table);
        StatusText = $"Loaded {table.Rows.Count} rows, {table.Headers.Count} columns from {Path.GetFileName(dialog.FileName)}";
    }

    private static DataView ToDataView(CsvTable table)
    {
        var dataTable = new DataTable();
        foreach (var header in table.Headers)
            dataTable.Columns.Add(header);

        foreach (var row in table.Rows)
            dataTable.Rows.Add(row.ToArray());

        return dataTable.DefaultView;
    }
}
