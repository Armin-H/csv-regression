using CsvRegression.Domain;
using NUnit.Framework;

namespace CsvRegression.Domain.UnitTests;

public class PointExtractorTests
{
    [Test]
    [Category("Smoke")]
    public void AllRowsValid_ExtractsAllPoints()
    {
        var table = new CsvTable(["x", "y"], [["1", "2"], ["3", "4"]]);

        var points = PointExtractor.Extract(table, "x", "y");

        Assert.That(points, Is.EqualTo(new[] { (1.0, 2.0), (3.0, 4.0) }));
    }

    [Test]
    [Category("Regression")]
    public void RowMissingXOrY_IsExcluded()
    {
        var table = new CsvTable(["x", "y"], [["1", "2"], ["", "4"], ["5", ""]]);

        var points = PointExtractor.Extract(table, "x", "y");

        Assert.That(points, Is.EqualTo(new[] { (1.0, 2.0) }));
    }

    [Test]
    [Category("Regression")]
    public void AllRowsMissingValues_ReturnsEmpty()
    {
        var table = new CsvTable(["x", "y"], [["", "2"], ["1", ""]]);

        var points = PointExtractor.Extract(table, "x", "y");

        Assert.That(points, Is.Empty);
    }
}
