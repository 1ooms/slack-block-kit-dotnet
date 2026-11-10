namespace SlackBlockKit.Tests.Elements;

public class ButtonElementTests
{
    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var confirm = Confirm();
        var button = new ButtonElement(new PlainTextObject("Click", true), "action", "value",
            "https://example.com", ButtonStyle.Primary, confirm, "Accessible");

        Assert.Equal("Click", button.Text.Text);
        Assert.Equal("action", button.ActionId);
        Assert.Equal("value", button.Value);
        Assert.Equal("https://example.com", button.Url);
        Assert.Equal(ButtonStyle.Primary, button.Style);
        Assert.Same(confirm, button.Confirm);
        Assert.Equal("Accessible", button.AccessibilityLabel);
        Assert.Equal("button", button.Type);
    }

    [Fact]
    public void StringConstructor_WrapsTextAndSetsOptionalValues()
    {
        var button = new ButtonElement("Click", "action", "value", ButtonStyle.Danger);

        Assert.Equal("Click", button.Text.Text);
        Assert.Equal("action", button.ActionId);
        Assert.Equal("value", button.Value);
        Assert.Equal(ButtonStyle.Danger, button.Style);
        Assert.Null(button.Url);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new ButtonElement(new PlainTextObject("Click"), "action", "value",
            "https://example.com", ButtonStyle.Danger, Confirm(), "Accessible"));

        AssertProperties(json, "type", "text", "action_id", "value", "url", "style", "confirm",
            "accessibility_label");
        Assert.Equal("button", json.GetProperty("type").GetString());
        Assert.Equal("plain_text", json.GetProperty("text").GetProperty("type").GetString());
        Assert.Equal("Click", TextOf(json, "text"));
        Assert.Equal("danger", json.GetProperty("style").GetString());
        Assert.Equal("Title", TextOf(json.GetProperty("confirm"), "title"));
        Assert.Equal("Accessible", json.GetProperty("accessibility_label").GetString());
    }

    [Fact]
    public void Serialize_OmitsNullFields()
    {
        AssertProperties(ToJson(new ButtonElement("Click")), "type", "text");
    }

    [Theory]
    [InlineData(ButtonStyle.Primary, "primary")]
    [InlineData(ButtonStyle.Danger, "danger")]
    public void Style_SerializesAsSnakeCase(ButtonStyle style, string expected)
    {
        Assert.Equal(expected, ToJson(new ButtonElement("Click", style: style)).GetProperty("style").GetString());
    }

    [Fact]
    public void Text_ThrowsWhenNull()
    {
        Assert.Throws<ArgumentNullException>(() => new ButtonElement((PlainTextObject)null!));
    }

    [Fact]
    public void Text_ThrowsWhenLongerThan75Characters()
    {
        Assert.NotNull(new ButtonElement(Chars(75)));
        Assert.Throws<ArgumentException>(() => new ButtonElement(Chars(76)));
    }

    [Fact]
    public void Value_ThrowsWhenEmptyOrTooLong()
    {
        Assert.Throws<ArgumentException>(() => new ButtonElement("Click", value: ""));
        Assert.NotNull(new ButtonElement("Click", value: Chars(2000)));
        Assert.Throws<ArgumentException>(() => new ButtonElement("Click", value: Chars(2001)));
    }

    [Fact]
    public void Url_ThrowsWhenEmptyOrTooLong()
    {
        var text = new PlainTextObject("Click");

        Assert.Throws<ArgumentException>(() => new ButtonElement(text, url: ""));
        Assert.NotNull(new ButtonElement(text, url: Chars(3000)));
        Assert.Throws<ArgumentException>(() => new ButtonElement(text, url: Chars(3001)));
    }

    [Fact]
    public void AccessibilityLabel_ThrowsWhenTooLong()
    {
        var text = new PlainTextObject("Click");

        Assert.NotNull(new ButtonElement(text, accessibilityLabel: Chars(75)));
        Assert.Throws<ArgumentException>(() => new ButtonElement(text, accessibilityLabel: Chars(76)));
    }

    [Fact]
    public void Serialize_AsElement_WritesSingleTypeProperty()
    {
        var json = ToJson(new List<Element> { new ButtonElement("Click") })[0];

        Assert.Single(json.EnumerateObject(), p => p.Name.Equals("type", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void RoundTrip_PreservesValues()
    {
        var button = RoundTrip(new ButtonElement(new PlainTextObject("Go"), "go", "value", "https://example.com",
            ButtonStyle.Danger, Confirm(), "Label"));

        Assert.Equal("Go", button.Text.Text);
        Assert.Equal("go", button.ActionId);
        Assert.Equal("value", button.Value);
        Assert.Equal("https://example.com", button.Url);
        Assert.Equal(ButtonStyle.Danger, button.Style);
        Assert.Equal("Title", button.Confirm!.Title.Text);
        Assert.Equal("Label", button.AccessibilityLabel);
    }
}
