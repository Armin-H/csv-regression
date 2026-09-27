using System.Globalization;

namespace CsvRegression.Domain;

public static class NumericColumnDetector
{
    public static bool IsNumeric(CsvTable table, int columnIndex)
    {
        var sawValue = false;

        foreach (var row in table.Rows)
        {
            var cell = row[columnIndex].Trim();
            if (cell.Length == 0) continue;

            // invariant culture: don't let the machine's locale decide what a decimal point looks like
            if (!double.TryParse(cell, NumberStyles.Float, CultureInfo.InvariantCulture, out _))
                return false;

            sawValue = true;
        }

        return sawValue;
    }
}
