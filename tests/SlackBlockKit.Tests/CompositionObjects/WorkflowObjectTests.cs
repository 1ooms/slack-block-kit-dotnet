namespace SlackBlockKit.Tests.CompositionObjects;

public class WorkflowObjectTests
{
    private const string Url = "https://slack.com/shortcuts/Ft0123/abc";

    [Fact]
    public void TriggerConstructor_SetsTrigger()
    {
        var trigger = new TriggerObject(Url);

        Assert.Same(trigger, new WorkflowObject(trigger).Trigger);
    }

    [Fact]
    public void UrlConstructor_CreatesTrigger()
    {
        var workflow = new WorkflowObject(Url, [new CustomizableInputParameter("a", "1")]);

        Assert.Equal(Url, workflow.Trigger.Url);
        Assert.Single(workflow.Trigger.CustomizableInputParameters!);
    }

    [Fact]
    public void UrlConstructor_WithoutParameters_LeavesParametersNull()
    {
        Assert.Null(new WorkflowObject(Url).Trigger.CustomizableInputParameters);
    }

    [Fact]
    public void DictionaryConstructor_CreatesTriggerWithParameters()
    {
        var workflow = new WorkflowObject(Url, new Dictionary<string, string> { ["a"] = "1" });

        Assert.Equal("a", workflow.Trigger.CustomizableInputParameters![0].Name);
    }

    [Fact]
    public void Serialize_WritesNestedTrigger()
    {
        var json = ToJson(new WorkflowObject(Url));

        AssertProperties(json, "trigger");
        Assert.Equal(Url, json.GetProperty("trigger").GetProperty("url").GetString());
    }

    [Fact]
    public void Trigger_ThrowsWhenNull()
    {
        Assert.Throws<ArgumentNullException>(() => new WorkflowObject((TriggerObject)null!));
        Assert.Throws<ArgumentNullException>(() => new WorkflowObject(Url).Trigger = null!);
    }

    [Fact]
    public void RoundTrip_PreservesValues()
    {
        var workflow = RoundTrip(new WorkflowObject("https://example.com/trigger",
            new Dictionary<string, string> { ["name"] = "value" }));

        Assert.Equal("https://example.com/trigger", workflow.Trigger.Url);
        Assert.Equal("name", Assert.Single(workflow.Trigger.CustomizableInputParameters!).Name);
    }
}
