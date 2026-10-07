using CsvRegression;

namespace CsvRegression.App.UnitTests;

public class MainViewModelTests
{
    [Test]
    [Category("Smoke")]
    public void HasNumericColumns_NumericColumnsEmpty_IsFalse()
    {
        var viewModel = new MainViewModel();

        Assert.That(viewModel.HasNumericColumns, Is.False);
    }

    [Test]
    [Category("Smoke")]
    public void HasNumericColumns_NumericColumnsNotEmpty_IsTrue()
    {
        var viewModel = new MainViewModel { NumericColumns = new(["x", "y"]) };

        Assert.That(viewModel.HasNumericColumns, Is.True);
    }

    [Test]
    [Category("Smoke")]
    public void YSelectableColumns_XColumnSelected_ExcludesThatColumn()
    {
        var viewModel = new MainViewModel
        {
            NumericColumns = new(["x", "y", "z"]),
            SelectedXColumn = "x"
        };

        Assert.That(viewModel.YSelectableColumns, Is.EqualTo(new[] { "y", "z" }));
    }

    [Test]
    [Category("Regression")]
    public void YSelectableColumns_NoXColumnSelected_IncludesAllNumericColumns()
    {
        var viewModel = new MainViewModel { NumericColumns = new(["x", "y", "z"]) };

        Assert.That(viewModel.YSelectableColumns, Is.EqualTo(new[] { "x", "y", "z" }));
    }

    [Test]
    [Category("Smoke")]
    public void XSelectableColumns_YColumnSelected_ExcludesThatColumn()
    {
        var viewModel = new MainViewModel
        {
            NumericColumns = new(["x", "y", "z"]),
            SelectedYColumn = "y"
        };

        Assert.That(viewModel.XSelectableColumns, Is.EqualTo(new[] { "x", "z" }));
    }

    [Test]
    [Category("Regression")]
    public void XSelectableColumns_NoYColumnSelected_IncludesAllNumericColumns()
    {
        var viewModel = new MainViewModel { NumericColumns = new(["x", "y", "z"]) };

        Assert.That(viewModel.XSelectableColumns, Is.EqualTo(new[] { "x", "y", "z" }));
    }

    [Test]
    [Category("Smoke")]
    public void ClearSelectionCommand_BothColumnsSelected_SetsBothToNull()
    {
        var viewModel = new MainViewModel
        {
            NumericColumns = new(["x", "y"]),
            SelectedXColumn = "x",
            SelectedYColumn = "y"
        };

        viewModel.ClearSelectionCommand.Execute(null);

        Assert.That(viewModel.SelectedXColumn, Is.Null);
        Assert.That(viewModel.SelectedYColumn, Is.Null);
    }
}
