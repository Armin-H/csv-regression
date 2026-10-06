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
}
