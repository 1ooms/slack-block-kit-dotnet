namespace SlackBlockKit.Tests.CompositionObjects;

public class ConfirmationDialogObjectTests
{
    [Fact]
    public void StringConstructor_WrapsValuesInPlainText()
    {
        var dialog = new ConfirmationDialogObject("Title", "Text", "Yes", "No", ConfirmationDialogStyle.Primary);

        Assert.Equal("Title", dialog.Title.Text);
        Assert.IsType<PlainTextObject>(dialog.Text);
        Assert.Equal("Text", dialog.Text.Text);
        Assert.Equal("Yes", dialog.Confirm.Text);
        Assert.Equal("No", dialog.Deny.Text);
        Assert.Equal(ConfirmationDialogStyle.Primary, dialog.Style);
    }

    [Fact]
    public void TextObjectConstructor_AcceptsMarkdownText()
    {
        var dialog = new ConfirmationDialogObject("Title", new MarkdownTextObject("*Sure?*"), "Yes", "No");

        Assert.IsType<MarkdownTextObject>(dialog.Text);
        Assert.Equal("mrkdwn", ToJson(dialog).GetProperty("text").GetProperty("type").GetString());
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new ConfirmationDialogObject("Title", "Text", "Yes", "No", ConfirmationDialogStyle.Danger));

        AssertProperties(json, "title", "text", "confirm", "deny", "style");
        Assert.Equal("plain_text", json.GetProperty("title").GetProperty("type").GetString());
        Assert.Equal("Title", TextOf(json, "title"));
        Assert.Equal("Text", TextOf(json, "text"));
        Assert.Equal("Yes", TextOf(json, "confirm"));
        Assert.Equal("No", TextOf(json, "deny"));
        Assert.Equal("danger", json.GetProperty("style").GetString());
    }

    [Fact]
    public void Serialize_OmitsStyleWhenNull()
    {
        AssertNoProperty(ToJson(Confirm()), "style");
    }

    [Theory]
    [InlineData(ConfirmationDialogStyle.Primary, "primary")]
    [InlineData(ConfirmationDialogStyle.Danger, "danger")]
    public void Style_SerializesAsSnakeCase(ConfirmationDialogStyle style, string expected)
    {
        var json = ToJson(new ConfirmationDialogObject("T", "T", "Y", "N", style));

        Assert.Equal(expected, json.GetProperty("style").GetString());
    }

    [Fact]
    public void Constructor_AllowsMaximumLengths()
    {
        var dialog = new ConfirmationDialogObject(Chars(100), Chars(300), Chars(30), Chars(30));

        Assert.Equal(100, dialog.Title.Text.Length);
        Assert.Equal(300, dialog.Text.Text.Length);
    }

    [Fact]
    public void Title_ThrowsWhenTooLong()
    {
        Assert.Throws<ArgumentException>(() => new ConfirmationDialogObject(Chars(101), "Text", "Yes", "No"));
    }

    [Fact]
    public void Text_ThrowsWhenTooLong()
    {
        Assert.Throws<ArgumentException>(() => new ConfirmationDialogObject("Title", Chars(301), "Yes", "No"));
    }

    [Fact]
    public void Confirm_ThrowsWhenTooLong()
    {
        Assert.Throws<ArgumentException>(() => new ConfirmationDialogObject("Title", "Text", Chars(31), "No"));
    }

    [Fact]
    public void Deny_ThrowsWhenTooLong()
    {
        Assert.Throws<ArgumentException>(() => new ConfirmationDialogObject("Title", "Text", "Yes", Chars(31)));
    }

    [Fact]
    public void RequiredProperties_ThrowWhenNull()
    {
        var text = new PlainTextObject("x");

        Assert.Throws<ArgumentNullException>(() => new ConfirmationDialogObject(null!, text, text, text));
        Assert.Throws<ArgumentNullException>(() => new ConfirmationDialogObject(text, null!, text, text));
        Assert.Throws<ArgumentNullException>(() => new ConfirmationDialogObject(text, text, null!, text));
        Assert.Throws<ArgumentNullException>(() => new ConfirmationDialogObject(text, text, text, null!));
    }

    [Fact]
    public void RoundTrip_PreservesValues()
    {
        var dialog = RoundTrip(new ConfirmationDialogObject(new PlainTextObject("Title"),
            new MarkdownTextObject("*Text*"), new PlainTextObject("Yes"), new PlainTextObject("No"),
            ConfirmationDialogStyle.Danger));

        Assert.Equal("Title", dialog.Title.Text);
        Assert.Equal("*Text*", Assert.IsType<MarkdownTextObject>(dialog.Text).Text);
        Assert.Equal("Yes", dialog.Confirm.Text);
        Assert.Equal("No", dialog.Deny.Text);
        Assert.Equal(ConfirmationDialogStyle.Danger, dialog.Style);
    }
}
