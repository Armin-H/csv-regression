using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvRegression.Domain;

namespace CsvRegression.Data;

public static class CsvTableLoader
{
    public static CsvTable Load(string filePath)
    {
        using var reader = new StreamReader(filePath);

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            // messy real-world CSVs shouldn't crash the app — missing/bad cells just become blank
            MissingFieldFound = null,
            BadDataFound = null,
        };
        using var csv = new CsvReader(reader, config);

        if (!csv.Read())
            return new CsvTable([], []);

        csv.ReadHeader();
        var headers = csv.HeaderRecord!.ToList();

        var rows = new List<IReadOnlyList<string>>();
        while (csv.Read())
        {
            var row = headers.Select((_, i) => csv.GetField(i) ?? "").ToList();
            rows.Add(row);
        }

        return new CsvTable(headers, rows);
    }
}
