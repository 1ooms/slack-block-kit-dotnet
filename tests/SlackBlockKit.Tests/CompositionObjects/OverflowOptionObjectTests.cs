namespace SlackBlockKit.Tests.CompositionObjects;

public class OverflowOptionObjectTests
{
    [Fact]
    public void StringConstructor_WrapsTextAndDescription()
    {
        var option = new OverflowOptionObject("Label", "value", "Description", "https://example.com");

        Assert.Equal("Label", option.Text.Text);
        Assert.Equal("value", option.Value);
        Assert.Equal("Description", option.Description!.Text);
        Assert.Equal("https://example.com", option.Url);
    }

    [Fact]
    public void StringConstructor_LeavesDescriptionNullWhenOmitted()
    {
        Assert.Null(new OverflowOptionObject("Label", "value").Description);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new OverflowOptionObject("Label", "value", "Description", "https://example.com"));

        AssertProperties(json, "text", "value", "description", "url");
        Assert.Equal("plain_text", json.GetProperty("text").GetProperty("type").GetString());
        Assert.Equal("Label", TextOf(json, "text"));
        Assert.Equal("value", json.GetProperty("value").GetString());
        Assert.Equal("plain_text", json.GetProperty("description").GetProperty("type").GetString());
        Assert.Equal("Description", TextOf(json, "description"));
        Assert.Equal("https://example.com", json.GetProperty("url").GetString());
    }

    [Fact]
    public void Serialize_OmitsOptionalFields()
    {
        AssertProperties(ToJson(new OverflowOptionObject("Label", "value")), "text", "value");
    }

    [Fact]
    public void Text_ThrowsWhenNull()
    {
        Assert.Throws<ArgumentNullException>(() => new OverflowOptionObject((PlainTextObject)null!, "value"));
    }

    [Fact]
    public void Text_ThrowsWhenTooLong()
    {
        Assert.NotNull(new OverflowOptionObject(Chars(75), "value"));
        Assert.Throws<ArgumentException>(() => new OverflowOptionObject(Chars(76), "value"));
    }

    [Fact]
    public void Value_ThrowsWhenNullEmptyOrTooLong()
    {
        Assert.Throws<ArgumentNullException>(() => new OverflowOptionObject("Label", null!));
        Assert.Throws<ArgumentException>(() => new OverflowOptionObject("Label", ""));
        Assert.NotNull(new OverflowOptionObject("Label", Chars(75)));
        Assert.Throws<ArgumentException>(() => new OverflowOptionObject("Label", Chars(76)));
    }

    [Fact]
    public void Description_ThrowsWhenTooLong()
    {
        Assert.NotNull(new OverflowOptionObject("Label", "value", Chars(75)));
        Assert.Throws<ArgumentException>(() => new OverflowOptionObject("Label", "value", Chars(76)));
    }

    [Fact]
    public void Url_AllowsUpTo3000Characters()
    {
        var url = "https://example.com/" + Chars(3000 - 20);

        Assert.Equal(3000, new OverflowOptionObject("Label", "value", url: url).Url!.Length);
        Assert.Throws<ArgumentException>(() => new OverflowOptionObject("Label", "value", url: url + "a"));
    }

    [Fact]
    public void Url_ThrowsWhenEmpty()
    {
        Assert.Throws<ArgumentException>(() => new OverflowOptionObject("Label", "value", url: ""));
    }

    [Fact]
    public void RoundTrip_PreservesValues()
    {
        var option = RoundTrip(new OverflowOptionObject("Text", "value", "Description", "https://example.com"));

        Assert.Equal("Text", option.Text.Text);
        Assert.Equal("value", option.Value);
        Assert.Equal("Description", option.Description!.Text);
        Assert.Equal("https://example.com", option.Url);
    }
}
