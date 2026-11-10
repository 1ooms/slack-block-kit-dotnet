namespace SlackBlockKit.Tests.Elements.MultiSelectMenu;

public class MultiSelectMenuBaseTests
{
    private class TestMultiSelect(PlainTextObject? placeholder = null, int? maxSelectedItems = null)
        : MultiSelectMenuBase("action", placeholder, null, maxSelectedItems)
    {
        public override string Type => "test_multi_select";
    }

    [Fact]
    public void Constructor_SetsSharedProperties()
    {
        var select = new TestMultiSelect("Pick", 3);

        Assert.Equal("action", select.ActionId);
        Assert.Equal("Pick", select.Placeholder!.Text);
        Assert.Equal(3, select.MaxSelectedItems);
        Assert.Null(select.Confirm);
    }

    [Fact]
    public void Placeholder_AllowsUpTo150Characters()
    {
        Assert.NotNull(new TestMultiSelect(Chars(150)));
        Assert.Throws<ArgumentException>(() => new TestMultiSelect(Chars(151)));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    public void MaxSelectedItems_AcceptsPositiveValues(int value)
    {
        Assert.Equal(value, new TestMultiSelect(maxSelectedItems: value).MaxSelectedItems);
    }

    [Theory]
    [InlineData(0)]
    public void MaxSelectedItems_ThrowsWhenLessThan1(int value)
    {
        Assert.Throws<ArgumentException>(() => new TestMultiSelect(maxSelectedItems: value));
    }
}
