using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Windows;
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

    [ObservableProperty]
    private ObservableCollection<string> _numericColumns = new();

    [ObservableProperty]
    private string? _selectedXColumn;

    [ObservableProperty]
    private string? _selectedYColumn;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(RemoveSelectedRowCommand))]
    private DataRowView? _selectedRow;

    [RelayCommand]
    private void OpenCsv()
    {
        var dialog = new OpenFileDialog { Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*" };
        if (dialog.ShowDialog() != true) return;

        var table = CsvTableLoader.Load(dialog.FileName);
        if (table.Headers.Count == 0)
        {
            MessageBox.Show("The selected file appears to be empty or not a valid CSV.", "Invalid file",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        Rows = ToDataView(table);
        NumericColumns = new ObservableCollection<string>(FindNumericColumns(table));
        SelectedXColumn = null;
        SelectedYColumn = null;
        StatusText = $"Loaded {table.Rows.Count} rows, {table.Headers.Count} columns from {Path.GetFileName(dialog.FileName)}";
    }

    [RelayCommand(CanExecute = nameof(CanRemoveSelectedRow))]
    private void RemoveSelectedRow()
    {
        var table = SelectedRow!.Row.Table;
        SelectedRow.Row.Delete();
        table.AcceptChanges();
        StatusText = $"Removed row. {Rows!.Count} rows remaining.";
    }

    private bool CanRemoveSelectedRow() => SelectedRow != null;

    partial void OnSelectedXColumnChanged(string? value) => WarnIfSameColumn();

    partial void OnSelectedYColumnChanged(string? value) => WarnIfSameColumn();

    private void WarnIfSameColumn()
    {
        if (SelectedXColumn != null && SelectedXColumn == SelectedYColumn)
        {
            MessageBox.Show("X and Y must be different columns.", "Invalid selection",
                MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private static List<string> FindNumericColumns(CsvTable table)
    {
        var result = new List<string>();
        for (var i = 0; i < table.Headers.Count; i++)
        {
            if (NumericColumnDetector.IsNumeric(table, i))
                result.Add(table.Headers[i]);
        }
        return result;
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
