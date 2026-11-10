namespace SlackBlockKit.Tests.CompositionObjects.TextObjects;

public class MarkdownTextObjectTests
{
    [Fact]
    public void Constructor_SetsProperties()
    {
        var text = new MarkdownTextObject("*bold*", true);

        Assert.Equal("*bold*", text.Text);
        Assert.True(text.Verbatim);
        Assert.Equal("mrkdwn", text.Type);
    }

    [Fact]
    public void Serialize_WritesTypeTextAndVerbatim()
    {
        var json = ToJson(new MarkdownTextObject("*bold*", false));

        AssertProperties(json, "type", "text", "verbatim");
        Assert.Equal("mrkdwn", json.GetProperty("type").GetString());
        Assert.Equal("*bold*", json.GetProperty("text").GetString());
        Assert.False(json.GetProperty("verbatim").GetBoolean());
    }

    [Fact]
    public void Serialize_OmitsVerbatimWhenNull()
    {
        AssertProperties(ToJson(new MarkdownTextObject("*bold*")), "type", "text");
    }

    [Fact]
    public void ImplicitConversion_FromString_CreatesMarkdown()
    {
        MarkdownTextObject text = "_italic_";

        Assert.Equal("_italic_", text.Text);
        Assert.Null(text.Verbatim);
    }

    [Fact]
    public void Deserialize_AsTextObject_ReturnsMarkdown()
    {
        var text = SlackSerializer.Deserialize<TextObject>("""{"type":"mrkdwn","text":"*x*","verbatim":true}""");

        var markdown = Assert.IsType<MarkdownTextObject>(text);
        Assert.Equal("*x*", markdown.Text);
        Assert.True(markdown.Verbatim);
    }
}
