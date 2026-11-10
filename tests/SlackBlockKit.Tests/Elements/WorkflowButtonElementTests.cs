namespace SlackBlockKit.Tests.Elements;

public class WorkflowButtonElementTests
{
    private const string TriggerUrl = "https://slack.com/shortcuts/Ft0123/abc";

    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var workflow = new WorkflowObject(TriggerUrl);

        var element = new WorkflowButtonElement(new PlainTextObject("Run"), workflow, "action",
            ButtonStyle.Primary, "Run workflow");

        Assert.Equal("Run", element.Text.Text);
        Assert.Same(workflow, element.Workflow);
        Assert.Equal("action", element.ActionId);
        Assert.Equal(ButtonStyle.Primary, element.Style);
        Assert.Equal("Run workflow", element.AccessibilityLabel);
        Assert.Equal("workflow_button", element.Type);
    }

    [Fact]
    public void StringConstructor_CreatesWorkflowFromTriggerUrl()
    {
        var element = new WorkflowButtonElement("Run", "action", TriggerUrl, ButtonStyle.Danger);

        Assert.Equal("Run", element.Text.Text);
        Assert.Equal(TriggerUrl, element.Workflow.Trigger.Url);
        Assert.Equal(ButtonStyle.Danger, element.Style);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new WorkflowButtonElement(new PlainTextObject("Run"), new WorkflowObject(TriggerUrl),
            "action", ButtonStyle.Primary, "Run workflow"));

        AssertProperties(json, "type", "text", "workflow", "action_id", "style", "accessibility_label");
        Assert.Equal("workflow_button", json.GetProperty("type").GetString());
        Assert.Equal(TriggerUrl, json.GetProperty("workflow").GetProperty("trigger").GetProperty("url").GetString());
        Assert.Equal("primary", json.GetProperty("style").GetString());
    }

    [Fact]
    public void Serialize_OmitsNullFields()
    {
        AssertProperties(ToJson(new WorkflowButtonElement("Run", "action", TriggerUrl)), "type", "text",
            "workflow", "action_id");
    }

    [Fact]
    public void Text_ThrowsWhenNullOrTooLong()
    {
        var workflow = new WorkflowObject(TriggerUrl);

        Assert.Throws<ArgumentNullException>(() => new WorkflowButtonElement(null!, workflow, "action"));
        Assert.NotNull(new WorkflowButtonElement(Chars(75), "action", TriggerUrl));
        Assert.Throws<ArgumentException>(() => new WorkflowButtonElement(Chars(76), "action", TriggerUrl));
    }

    [Fact]
    public void Workflow_ThrowsWhenNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new WorkflowButtonElement(new PlainTextObject("Run"), null!, "action"));
    }

    [Fact]
    public void AccessibilityLabel_ThrowsWhenTooLong()
    {
        var workflow = new WorkflowObject(TriggerUrl);
        var text = new PlainTextObject("Run");

        Assert.NotNull(new WorkflowButtonElement(text, workflow, "a", accessibilityLabel: Chars(75)));
        Assert.Throws<ArgumentException>(() =>
            new WorkflowButtonElement(text, workflow, "a", accessibilityLabel: Chars(76)));
    }

    [Fact]
    public void RoundTrip_PreservesValues()
    {
        var button = RoundTrip(new WorkflowButtonElement(new PlainTextObject("Run"),
            new WorkflowObject("https://example.com/trigger"), "run", ButtonStyle.Primary, "Label"));

        Assert.Equal("Run", button.Text.Text);
        Assert.Equal("https://example.com/trigger", button.Workflow.Trigger.Url);
        Assert.Equal("run", button.ActionId);
        Assert.Equal(ButtonStyle.Primary, button.Style);
        Assert.Equal("Label", button.AccessibilityLabel);
    }
}
