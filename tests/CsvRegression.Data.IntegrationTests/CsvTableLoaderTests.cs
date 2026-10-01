using CsvRegression.Data;
using NUnit.Framework;

namespace CsvRegression.Data.IntegrationTests;

public class CsvTableLoaderTests
{
    [Test]
    [Category("Smoke")]
    public void WellFormedCsv_LoadsHeadersAndRows()
    {
        var path = WriteTempCsv("x,y\n1,2\n3,4\n");
        try
        {
            var table = CsvTableLoader.Load(path);

            Assert.That(table.Headers, Is.EqualTo(new[] { "x", "y" }));
            Assert.That(table.Rows.Count, Is.EqualTo(2));
            Assert.That(table.Rows[0], Is.EqualTo(new[] { "1", "2" }));
            Assert.That(table.Rows[1], Is.EqualTo(new[] { "3", "4" }));
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Test]
    [Category("Regression")]
    public void HeaderOnlyCsv_LoadsWithNoRows()
    {
        var path = WriteTempCsv("x,y\n");
        try
        {
            var table = CsvTableLoader.Load(path);

            Assert.That(table.Headers, Is.EqualTo(new[] { "x", "y" }));
            Assert.That(table.Rows, Is.Empty);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Test]
    [Category("Regression")]
    public void EmptyFile_LoadsEmptyTable()
    {
        var path = WriteTempCsv("");
        try
        {
            var table = CsvTableLoader.Load(path);

            Assert.That(table.Headers, Is.Empty);
            Assert.That(table.Rows, Is.Empty);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Test]
    [Category("Regression")]
    public void RowMissingTrailingField_BecomesBlankCell()
    {
        var path = WriteTempCsv("x,y\n1\n");
        try
        {
            var table = CsvTableLoader.Load(path);

            Assert.That(table.Rows[0], Is.EqualTo(new[] { "1", "" }));
        }
        finally
        {
            File.Delete(path);
        }
    }

    private static string WriteTempCsv(string content)
    {
        var path = Path.GetTempFileName();
        File.WriteAllText(path, content);
        return path;
    }
}
