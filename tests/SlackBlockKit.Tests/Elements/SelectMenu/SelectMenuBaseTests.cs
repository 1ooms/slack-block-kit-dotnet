namespace SlackBlockKit.Tests.Elements.SelectMenu;

public class SelectMenuBaseTests
{
    private class TestSelect(PlainTextObject? placeholder = null) : SelectMenuBase("action", placeholder, null, true)
    {
        public override string Type => "test_select";
    }

    [Fact]
    public void Constructor_SetsSharedProperties()
    {
        var select = new TestSelect("Pick");

        Assert.Equal("action", select.ActionId);
        Assert.Equal("Pick", select.Placeholder!.Text);
        Assert.True(select.FocusOnLoad);
        Assert.Null(select.Confirm);
    }

    [Fact]
    public void Placeholder_AllowsUpTo150Characters()
    {
        Assert.Equal(150, new TestSelect(Chars(150)).Placeholder!.Text.Length);
        Assert.Throws<ArgumentException>(() => new TestSelect(Chars(151)));
    }
}
