namespace SlackBlockKit.Tests.CompositionObjects;

public class TextOptionObjectTests
{
    [Fact]
    public void StringConstructor_UsesPlainText()
    {
        var option = new TextOptionObject("Label", "value", "Description");

        Assert.IsType<PlainTextObject>(option.Text);
        Assert.Equal("Label", option.Text.Text);
        Assert.Equal("value", option.Value);
        Assert.IsType<PlainTextObject>(option.Description);
        Assert.Equal("Description", option.Description!.Text);
    }

    [Fact]
    public void TextObjectConstructor_AcceptsMarkdownTextAndDescription()
    {
        var option = new TextOptionObject(new MarkdownTextObject("*Label*"), "value", new MarkdownTextObject("_Description_"));

        Assert.IsType<MarkdownTextObject>(option.Text);
        Assert.IsType<MarkdownTextObject>(option.Description);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new TextOptionObject(new MarkdownTextObject("*Label*"), "value",
            new MarkdownTextObject("_Description_")));

        AssertProperties(json, "text", "value", "description");
        Assert.Equal("mrkdwn", json.GetProperty("text").GetProperty("type").GetString());
        Assert.Equal("*Label*", TextOf(json, "text"));
        Assert.Equal("mrkdwn", json.GetProperty("description").GetProperty("type").GetString());
        Assert.Equal("_Description_", TextOf(json, "description"));
    }

    [Fact]
    public void Serialize_OmitsOptionalFields()
    {
        AssertProperties(ToJson(new TextOptionObject("Label", "value")), "text", "value");
    }

    [Fact]
    public void Text_ThrowsWhenNull()
    {
        Assert.Throws<ArgumentNullException>(() => new TextOptionObject((TextObject)null!, "value"));
    }

    [Fact]
    public void Text_ThrowsWhenTooLong()
    {
        Assert.NotNull(new TextOptionObject(Chars(75), "value"));
        Assert.Throws<ArgumentException>(() => new TextOptionObject(Chars(76), "value"));
    }

    [Fact]
    public void Value_ThrowsWhenNullEmptyOrTooLong()
    {
        Assert.Throws<ArgumentNullException>(() => new TextOptionObject("Label", null!));
        Assert.Throws<ArgumentException>(() => new TextOptionObject("Label", ""));
        Assert.NotNull(new TextOptionObject("Label", Chars(75)));
        Assert.Throws<ArgumentException>(() => new TextOptionObject("Label", Chars(76)));
    }

    [Fact]
    public void Description_ThrowsWhenTooLong()
    {
        Assert.NotNull(new TextOptionObject("Label", "value", Chars(75)));
        Assert.Throws<ArgumentException>(() => new TextOptionObject("Label", "value", Chars(76)));
        Assert.Throws<ArgumentException>(() =>
            new TextOptionObject(new PlainTextObject("Label"), "value", new MarkdownTextObject(Chars(76))));
    }

    [Fact]
    public void RoundTrip_PreservesMarkdown()
    {
        var option = RoundTrip(new TextOptionObject(new MarkdownTextObject("*Text*"), "value",
            new MarkdownTextObject("_Description_")));

        Assert.Equal("*Text*", Assert.IsType<MarkdownTextObject>(option.Text).Text);
        Assert.Equal("value", option.Value);
        Assert.Equal("_Description_", Assert.IsType<MarkdownTextObject>(option.Description).Text);
    }
}
