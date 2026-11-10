namespace SlackBlockKit.Tests.Elements;

public class WorkflowMentionElementTests
{
    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var style = new RichTextStyle(bold: true);

        var element = new WorkflowMentionElement("Wf1", "Ft1", "Run", "https://example.com", "C1", "1.2", style);

        Assert.Equal("Wf1", element.WorkflowId);
        Assert.Equal("Ft1", element.FunctionTriggerId);
        Assert.Equal("Run", element.Text);
        Assert.Equal("https://example.com", element.Url);
        Assert.Equal("C1", element.ChannelId);
        Assert.Equal("1.2", element.Ts);
        Assert.Same(style, element.Style);
        Assert.Equal("workflow_mention", element.Type);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new WorkflowMentionElement("Wf1", "Ft1", "Run", "https://example.com", "C1", "1.2",
            new RichTextStyle(bold: true)));

        AssertProperties(json, "type", "workflow_id", "function_trigger_id", "text", "url", "channel_id", "ts",
            "style");
        Assert.Equal("workflow_mention", json.GetProperty("type").GetString());
    }

    [Fact]
    public void Serialize_OmitsNullFields()
    {
        AssertProperties(ToJson(new WorkflowMentionElement("Wf1", "Ft1", "Run")), "type", "workflow_id",
            "function_trigger_id", "text");
    }

    [Fact]
    public void RequiredStrings_ThrowWhenNullOrEmpty()
    {
        Assert.Throws<ArgumentNullException>(() => new WorkflowMentionElement(null!, "Ft1", "Run"));
        Assert.Throws<ArgumentException>(() => new WorkflowMentionElement("", "Ft1", "Run"));
        Assert.Throws<ArgumentException>(() => new WorkflowMentionElement("Wf1", "", "Run"));
        Assert.Throws<ArgumentException>(() => new WorkflowMentionElement("Wf1", "Ft1", ""));
    }

    [Fact]
    public void OptionalStrings_ThrowWhenEmpty()
    {
        Assert.Throws<ArgumentException>(() => new WorkflowMentionElement("Wf1", "Ft1", "Run", ""));
        Assert.Throws<ArgumentException>(() => new WorkflowMentionElement("Wf1", "Ft1", "Run", channelId: ""));
        Assert.Throws<ArgumentException>(() => new WorkflowMentionElement("Wf1", "Ft1", "Run", ts: ""));
    }
}
