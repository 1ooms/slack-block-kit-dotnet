namespace SlackBlockKit.Tests.Elements;

public class MessageMentionElementTests
{
    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var style = new RichTextStyle(bold: true);

        var element = new MessageMentionElement("C123", "1.2", "U1", "1.1", "Message", "https://example.com", style);

        Assert.Equal("C123", element.ChannelId);
        Assert.Equal("1.2", element.MessageTs);
        Assert.Equal("U1", element.AuthorId);
        Assert.Equal("1.1", element.ThreadTs);
        Assert.Equal("Message", element.Text);
        Assert.Equal("https://example.com", element.Url);
        Assert.Same(style, element.Style);
        Assert.Equal("message_mention", element.Type);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new MessageMentionElement("C123", "1.2", "U1", "1.1", "Message", "https://example.com",
            new RichTextStyle(bold: true)));

        AssertProperties(json, "type", "channel_id", "message_ts", "author_id", "thread_ts", "text", "url",
            "style");
        Assert.Equal("message_mention", json.GetProperty("type").GetString());
    }

    [Fact]
    public void Serialize_OmitsNullFields()
    {
        AssertProperties(ToJson(new MessageMentionElement("C123", "1.2")), "type", "channel_id", "message_ts");
    }

    [Fact]
    public void RequiredStrings_ThrowWhenNull()
    {
        Assert.Throws<ArgumentNullException>(() => new MessageMentionElement(null!, "1.2"));
        Assert.Throws<ArgumentNullException>(() => new MessageMentionElement("C123", null!));
    }

    [Fact]
    public void OptionalStrings_ThrowWhenEmpty()
    {
        Assert.Throws<ArgumentException>(() => new MessageMentionElement("C123", "1.2", authorId: ""));
        Assert.Throws<ArgumentException>(() => new MessageMentionElement("C123", "1.2", text: ""));
        Assert.Throws<ArgumentException>(() => new MessageMentionElement("C123", "1.2", url: ""));
    }
}
