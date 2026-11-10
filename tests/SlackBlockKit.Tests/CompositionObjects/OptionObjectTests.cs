namespace SlackBlockKit.Tests.CompositionObjects;

public class OptionObjectTests
{
    [Fact]
    public void StringConstructor_WrapsTextAndDescription()
    {
        var option = new OptionObject("Label", "value", "Description", "https://example.com");

        Assert.IsType<PlainTextObject>(option.Text);
        Assert.Equal("Label", option.Text.Text);
        Assert.Equal("value", option.Value);
        Assert.Equal("Description", option.Description!.Text);
        Assert.Equal("https://example.com", option.Url);
    }

    [Fact]
    public void StringConstructor_LeavesDescriptionNullWhenOmitted()
    {
        Assert.Null(new OptionObject("Label", "value").Description);
    }

    [Fact]
    public void TextObjectConstructor_AcceptsMarkdown()
    {
        var option = new OptionObject(new MarkdownTextObject("*Label*"), "value");

        Assert.IsType<MarkdownTextObject>(option.Text);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new OptionObject("Label", "value", "Description", "https://example.com"));

        AssertProperties(json, "text", "value", "description", "url");
        Assert.Equal("Label", TextOf(json, "text"));
        Assert.Equal("value", json.GetProperty("value").GetString());
        Assert.Equal("Description", TextOf(json, "description"));
        Assert.Equal("https://example.com", json.GetProperty("url").GetString());
    }

    [Fact]
    public void Serialize_OmitsOptionalFields()
    {
        AssertProperties(ToJson(new OptionObject("Label", "value")), "text", "value");
    }

    [Fact]
    public void Text_ThrowsWhenNull()
    {
        Assert.Throws<ArgumentNullException>(() => new OptionObject((TextObject)null!, "value"));
    }

    [Fact]
    public void Text_ThrowsWhenTooLong()
    {
        Assert.NotNull(new OptionObject(Chars(75), "value"));
        Assert.Throws<ArgumentException>(() => new OptionObject(Chars(76), "value"));
    }

    [Fact]
    public void Value_ThrowsWhenNullEmptyOrTooLong()
    {
        Assert.Throws<ArgumentNullException>(() => new OptionObject("Label", null!));
        Assert.Throws<ArgumentException>(() => new OptionObject("Label", ""));
        Assert.NotNull(new OptionObject("Label", Chars(75)));
        Assert.Throws<ArgumentException>(() => new OptionObject("Label", Chars(76)));
    }

    [Fact]
    public void Description_ThrowsWhenTooLong()
    {
        Assert.NotNull(new OptionObject("Label", "value", Chars(75)));
        Assert.Throws<ArgumentException>(() => new OptionObject("Label", "value", Chars(76)));
    }

    [Fact]
    public void Url_AllowsUpTo3000Characters()
    {
        var url = "https://example.com/" + Chars(3000 - 20);

        Assert.Equal(3000, new OptionObject("Label", "value", url: url).Url!.Length);
        Assert.Throws<ArgumentException>(() => new OptionObject("Label", "value", url: url + "a"));
    }

    [Fact]
    public void Url_ThrowsWhenEmpty()
    {
        Assert.Throws<ArgumentException>(() => new OptionObject("Label", "value", url: ""));
    }

    [Fact]
    public void RoundTrip_PreservesValues()
    {
        var option = RoundTrip(new OptionObject(new MarkdownTextObject("*Text*"), "value",
            new PlainTextObject("Description"), "https://example.com"));

        Assert.Equal("*Text*", Assert.IsType<MarkdownTextObject>(option.Text).Text);
        Assert.Equal("value", option.Value);
        Assert.Equal("Description", option.Description!.Text);
        Assert.Equal("https://example.com", option.Url);
    }
}
