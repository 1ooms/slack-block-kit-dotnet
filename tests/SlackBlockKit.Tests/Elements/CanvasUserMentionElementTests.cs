namespace SlackBlockKit.Tests.Elements;

public class CanvasUserMentionElementTests
{
    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var style = new RichTextStyle(bold: true);

        var element = new CanvasUserMentionElement("U123", "T1", style);

        Assert.Equal("U123", element.UserId);
        Assert.Equal("T1", element.ThreadId);
        Assert.Same(style, element.Style);
        Assert.Equal("canvas_user_mention", element.Type);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new CanvasUserMentionElement("U123", "T1", new RichTextStyle(bold: true)));

        AssertProperties(json, "type", "user_id", "thread_id", "style");
        Assert.Equal("canvas_user_mention", json.GetProperty("type").GetString());
        Assert.Equal("U123", json.GetProperty("user_id").GetString());
        Assert.Equal("T1", json.GetProperty("thread_id").GetString());
    }

    [Fact]
    public void Serialize_OmitsNullFields()
    {
        AssertProperties(ToJson(new CanvasUserMentionElement("U123")), "type", "user_id");
    }

    [Fact]
    public void UserId_ThrowsWhenNullOrEmpty()
    {
        Assert.Throws<ArgumentNullException>(() => new CanvasUserMentionElement(null!));
        Assert.Throws<ArgumentException>(() => new CanvasUserMentionElement(""));
    }

    [Fact]
    public void ThreadId_ThrowsWhenEmpty()
    {
        Assert.Throws<ArgumentException>(() => new CanvasUserMentionElement("U123", ""));
    }
}
