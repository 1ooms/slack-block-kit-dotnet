namespace SlackBlockKit.Tests.Elements;

public class RichTextStyleTests
{
    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var style = new RichTextStyle(true, false, true, false, true, false, true, false);

        Assert.True(style.Bold);
        Assert.False(style.Italic);
        Assert.True(style.Strike);
        Assert.False(style.Highlight);
        Assert.True(style.ClientHighlight);
        Assert.False(style.Underline);
        Assert.True(style.Unlink);
        Assert.False(style.Code);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new RichTextStyle(true, true, true, true, true, true, true, true));

        AssertProperties(json, "bold", "italic", "strike", "highlight", "client_highlight", "underline", "unlink",
            "code");
    }

    [Fact]
    public void Serialize_OmitsNullFields()
    {
        var json = ToJson(new RichTextStyle(bold: true));

        AssertProperties(json, "bold");
        Assert.True(json.GetProperty("bold").GetBoolean());
    }

    [Fact]
    public void RoundTrip_PreservesValues()
    {
        var style = RoundTrip(new RichTextStyle(italic: true, code: false));

        Assert.True(style.Italic);
        Assert.False(style.Code);
        Assert.Null(style.Bold);
    }
}
