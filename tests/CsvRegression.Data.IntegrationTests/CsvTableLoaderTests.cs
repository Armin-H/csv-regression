using CsvRegression.Data;
using Xunit;

namespace CsvRegression.Data.IntegrationTests;

public class CsvTableLoaderTests
{
    [Fact]
    [Trait("Category", "Smoke")]
    public void WellFormedCsv_LoadsHeadersAndRows()
    {
        var path = WriteTempCsv("x,y\n1,2\n3,4\n");
        try
        {
            var table = CsvTableLoader.Load(path);

            Assert.Equal(["x", "y"], table.Headers);
            Assert.Equal(2, table.Rows.Count);
            Assert.Equal(["1", "2"], table.Rows[0]);
            Assert.Equal(["3", "4"], table.Rows[1]);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    [Trait("Category", "Regression")]
    public void HeaderOnlyCsv_LoadsWithNoRows()
    {
        var path = WriteTempCsv("x,y\n");
        try
        {
            var table = CsvTableLoader.Load(path);

            Assert.Equal(["x", "y"], table.Headers);
            Assert.Empty(table.Rows);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    [Trait("Category", "Regression")]
    public void EmptyFile_LoadsEmptyTable()
    {
        var path = WriteTempCsv("");
        try
        {
            var table = CsvTableLoader.Load(path);

            Assert.Empty(table.Headers);
            Assert.Empty(table.Rows);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    [Trait("Category", "Regression")]
    public void RowMissingTrailingField_BecomesBlankCell()
    {
        var path = WriteTempCsv("x,y\n1\n");
        try
        {
            var table = CsvTableLoader.Load(path);

            Assert.Equal(["1", ""], table.Rows[0]);
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
