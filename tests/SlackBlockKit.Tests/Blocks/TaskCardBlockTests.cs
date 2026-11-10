namespace SlackBlockKit.Tests.Blocks;

public class TaskCardBlockTests
{
    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var details = RichTextBlockOf("details");
        var output = RichTextBlockOf("output");
        var sources = new List<UrlSourceElement> { new("https://example.com", "Example") };

        var block = new TaskCardBlock("t1", "Task", TaskCardStatus.Complete, details, output, sources,
            SlackIcon.Check, true, "task_1");

        Assert.Equal("t1", block.TaskId);
        Assert.Equal("Task", block.Title);
        Assert.Equal(TaskCardStatus.Complete, block.Status);
        Assert.Same(details, block.Details);
        Assert.Same(output, block.Output);
        Assert.Same(sources, block.Sources);
        Assert.Equal(SlackIcon.Check, block.Icon);
        Assert.True(block.HideTitle);
        Assert.Equal("task_1", block.BlockId);
        Assert.Equal("task_card", block.Type);
    }

    [Fact]
    public void Serialize_WritesAllFieldsWithNestedDiscriminators()
    {
        var json = ToJson(new TaskCardBlock("t1", "Task", TaskCardStatus.InProgress, RichTextBlockOf(),
            RichTextBlockOf(), [new UrlSourceElement("https://example.com", "Example")], SlackIcon.EyeOpen, false,
            "task_1"));

        AssertProperties(json, "type", "task_id", "title", "status", "details", "output", "sources", "icon",
            "hide_title", "block_id");
        Assert.Equal("task_card", json.GetProperty("type").GetString());
        Assert.Equal("in_progress", json.GetProperty("status").GetString());
        Assert.Equal("eye-open", json.GetProperty("icon").GetString());
        Assert.Equal("rich_text", json.GetProperty("details").GetProperty("type").GetString());
        Assert.Equal("url", json.GetProperty("sources")[0].GetProperty("type").GetString());
    }

    [Fact]
    public void Serialize_OmitsNullFields()
    {
        AssertProperties(ToJson(new TaskCardBlock("t1", "Task", TaskCardStatus.Error)), "type", "task_id",
            "title", "status");
    }

    [Theory]
    [InlineData(TaskCardStatus.InProgress, "in_progress")]
    [InlineData(TaskCardStatus.Complete, "complete")]
    [InlineData(TaskCardStatus.Error, "error")]
    public void Status_SerializesAsSnakeCase(TaskCardStatus status, string expected)
    {
        Assert.Equal(expected, ToJson(new TaskCardBlock("t1", "Task", status)).GetProperty("status").GetString());
    }

    [Fact]
    public void RequiredStrings_ThrowWhenNull()
    {
        Assert.Throws<ArgumentNullException>(() => new TaskCardBlock(null!, "Task", TaskCardStatus.Complete));
        Assert.Throws<ArgumentNullException>(() => new TaskCardBlock("t1", null!, TaskCardStatus.Complete));
    }
}
