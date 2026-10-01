using System.Globalization;

namespace CsvRegression.Domain;

public static class PointExtractor
{
    public static List<(double X, double Y)> Extract(CsvTable table, string xColumn, string yColumn)
    {
        var xIndex = table.Headers.ToList().IndexOf(xColumn);
        var yIndex = table.Headers.ToList().IndexOf(yColumn);

        var points = new List<(double X, double Y)>();
        foreach (var row in table.Rows)
        {
            if (TryParse(row[xIndex], out var x) && TryParse(row[yIndex], out var y))
                points.Add((x, y));
        }

        return points;
    }

    private static bool TryParse(string cell, out double value) =>
        double.TryParse(cell.Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out value);
}
