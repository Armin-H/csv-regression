using CsvRegression.Domain;
using NUnit.Framework;

namespace CsvRegression.Domain.UnitTests;

public class NumericColumnDetectorTests
{
    [Test]
    [Category("Smoke")]
    public void AllNumericColumn_IsNumeric()
    {
        var table = new CsvTable(["value"], [["1"], ["2.5"], ["-3"]]);

        Assert.That(NumericColumnDetector.IsNumeric(table, 0), Is.True);
    }

    [Test]
    [Category("Regression")]
    public void BlankCells_AreSkipped_ColumnStillNumeric()
    {
        var table = new CsvTable(["value"], [["1"], [""], ["3"]]);

        Assert.That(NumericColumnDetector.IsNumeric(table, 0), Is.True);
    }

    [Test]
    [Category("Regression")]
    public void NonNumericCell_MakesColumnNotNumeric()
    {
        var table = new CsvTable(["value"], [["1"], ["abc"], ["3"]]);

        Assert.That(NumericColumnDetector.IsNumeric(table, 0), Is.False);
    }

    [Test]
    [Category("Regression")]
    public void AllBlankColumn_IsNotNumeric()
    {
        var table = new CsvTable(["value"], [[""], [""]]);

        Assert.That(NumericColumnDetector.IsNumeric(table, 0), Is.False);
    }

    [Test]
    [Category("Regression")]
    public void ScientificNotationAndNegatives_AreNumeric()
    {
        var table = new CsvTable(["value"], [["1e10"], ["-2.5"]]);

        Assert.That(NumericColumnDetector.IsNumeric(table, 0), Is.True);
    }
}
