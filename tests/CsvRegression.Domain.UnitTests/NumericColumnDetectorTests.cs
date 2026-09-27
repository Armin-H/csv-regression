using CsvRegression.Domain;
using Xunit;

namespace CsvRegression.Domain.UnitTests;

public class NumericColumnDetectorTests
{
    [Fact]
    [Trait("Category", "Smoke")]
    public void AllNumericColumn_IsNumeric()
    {
        var table = new CsvTable(["value"], [["1"], ["2.5"], ["-3"]]);

        Assert.True(NumericColumnDetector.IsNumeric(table, 0));
    }

    [Fact]
    [Trait("Category", "Regression")]
    public void BlankCells_AreSkipped_ColumnStillNumeric()
    {
        var table = new CsvTable(["value"], [["1"], [""], ["3"]]);

        Assert.True(NumericColumnDetector.IsNumeric(table, 0));
    }

    [Fact]
    [Trait("Category", "Regression")]
    public void NonNumericCell_MakesColumnNotNumeric()
    {
        var table = new CsvTable(["value"], [["1"], ["abc"], ["3"]]);

        Assert.False(NumericColumnDetector.IsNumeric(table, 0));
    }

    [Fact]
    [Trait("Category", "Regression")]
    public void AllBlankColumn_IsNotNumeric()
    {
        var table = new CsvTable(["value"], [[""], [""]]);

        Assert.False(NumericColumnDetector.IsNumeric(table, 0));
    }

    [Fact]
    [Trait("Category", "Regression")]
    public void ScientificNotationAndNegatives_AreNumeric()
    {
        var table = new CsvTable(["value"], [["1e10"], ["-2.5"]]);

        Assert.True(NumericColumnDetector.IsNumeric(table, 0));
    }
}
