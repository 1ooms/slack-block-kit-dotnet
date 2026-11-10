namespace SlackBlockKit.Tests.CompositionObjects.TextObjects;

public class PlainTextObjectTests
{
    [Fact]
    public void Constructor_SetsProperties()
    {
        var text = new PlainTextObject("Hello", true);

        Assert.Equal("Hello", text.Text);
        Assert.True(text.Emoji);
        Assert.Equal("plain_text", text.Type);
    }

    [Fact]
    public void Serialize_WritesTypeTextAndEmoji()
    {
        var json = ToJson(new PlainTextObject("Hello", true));

        AssertProperties(json, "type", "text", "emoji");
        Assert.Equal("plain_text", json.GetProperty("type").GetString());
        Assert.Equal("Hello", json.GetProperty("text").GetString());
        Assert.True(json.GetProperty("emoji").GetBoolean());
    }

    [Fact]
    public void Serialize_OmitsEmojiWhenNull()
    {
        AssertProperties(ToJson(new PlainTextObject("Hello")), "type", "text");
    }

    [Fact]
    public void Serialize_AsTextObject_WritesSingleDiscriminator()
    {
        var json = ToJson(new List<TextObject> { new PlainTextObject("Hello") })[0];

        AssertProperties(json, "type", "text");
        Assert.Equal("plain_text", json.GetProperty("type").GetString());
    }

    [Fact]
    public void ImplicitConversion_FromString_CreatesPlainText()
    {
        PlainTextObject text = "Hello";

        Assert.Equal("Hello", text.Text);
        Assert.Null(text.Emoji);
    }

    [Fact]
    public void ToString_ReturnsText()
    {
        Assert.Equal("Hello", new PlainTextObject("Hello").ToString());
    }

    [Fact]
    public void Text_AllowsMaximumLength()
    {
        Assert.Equal(3000, new PlainTextObject(Chars(3000)).Text.Length);
    }

    [Fact]
    public void Text_ThrowsWhenTooLong()
    {
        Assert.Throws<ArgumentException>(() => new PlainTextObject(Chars(3001)));
    }

    [Fact]
    public void Text_ThrowsWhenEmpty()
    {
        Assert.Throws<ArgumentException>(() => new PlainTextObject(""));
    }

    [Fact]
    public void Text_ThrowsWhenNull()
    {
        Assert.Throws<ArgumentNullException>(() => new PlainTextObject(null!));
    }

    [Fact]
    public void Deserialize_AsTextObject_ReturnsPlainText()
    {
        var text = SlackSerializer.Deserialize<TextObject>("""{"type":"plain_text","text":"Hi","emoji":true}""");

        var plain = Assert.IsType<PlainTextObject>(text);
        Assert.Equal("Hi", plain.Text);
        Assert.True(plain.Emoji);
    }
}
