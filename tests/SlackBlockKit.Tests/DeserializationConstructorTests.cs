using System.Reflection;
using System.Text.Json.Serialization;

namespace SlackBlockKit.Tests;

public class DeserializationConstructorTests
{
    private const BindingFlags AnyInstance = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

    /// <summary>
    /// System.Text.Json refuses to deserialize a type with several public constructors unless one of them
    /// is annotated, so a new convenience constructor without <see cref="JsonConstructorAttribute"/> would
    /// break <see cref="SlackSerializer.Deserialize{T}"/> for that type at runtime.
    /// </summary>
    [Fact]
    public void TypesWithSeveralPublicConstructors_DesignateOneForDeserialization()
    {
        var offenders = typeof(SlackSerializer).Assembly.GetExportedTypes()
            .Where(type => type is { IsClass: true, IsAbstract: false, Namespace: not "SlackBlockKit.Builders" })
            .Where(type => type.GetConstructors().Length > 1)
            .Where(type => type.GetConstructors(AnyInstance).Count(HasJsonConstructor) != 1)
            .Select(type => type.FullName)
            .ToList();

        Assert.True(offenders.Count == 0, $"Missing [JsonConstructor]: {string.Join(", ", offenders)}");
    }

    private static bool HasJsonConstructor(ConstructorInfo constructor)
    {
        return constructor.IsDefined(typeof(JsonConstructorAttribute), false);
    }

    public static TheoryData<Block> BlocksWithSeveralConstructors => new()
    {
        new ActionsBlock([new ButtonElement("Go", "go")], "block_id"),
        new AlertBlock("*Careful*", AlertLevel.Warning, "block_id"),
        new CarouselBlock([new CardBlock(title: new PlainTextObject("Card"))], "block_id"),
        new ContextActionsBlock(
            [new FeedbackButtonsElement(new FeedbackButtonObject("Good", "1"), new FeedbackButtonObject("Bad", "0"))],
            "block_id"),
        new ContextBlock([new PlainTextObject("Text"), Image()], "block_id"),
        new HeaderBlock("Title", "block_id", 2),
        new InputBlock("Label", new PlainTextInputElement("input", "Placeholder"), "block_id", true, "Hint", true),
        new PlanBlock("Plan", [new TaskCardBlock("t1", "Task", TaskCardStatus.Complete)], "block_id"),
        new SectionBlock(new MarkdownTextObject("*Text*"), [new PlainTextObject("Field")],
            new ButtonElement("Go", "go"), true, "block_id"),
        new VideoBlock("Alt", "Title", "https://example.com/thumb.png", "https://example.com/video",
            "https://example.com/title", "Provider", "block_id")
    };

    [Theory]
    [MemberData(nameof(BlocksWithSeveralConstructors))]
    public void Deserialize_AsBlockList_RestoresTypeAndJson(Block block)
    {
        var json = SlackSerializer.Serialize(new List<Block> { block });

        var result = Assert.Single(SlackSerializer.Deserialize<List<Block>>(json)!);

        Assert.IsType(block.GetType(), result);
        Assert.Equal(json, SlackSerializer.Serialize(new List<Block> { result }));
    }

    [Fact]
    public void Deserialize_ReadsAlertBlockFromHandWrittenJson()
    {
        const string json = """[{"type":"alert","text":{"type":"mrkdwn","text":"*Careful*"},"level":"warning"}]""";

        var alert = Assert.IsType<AlertBlock>(Assert.Single(SlackSerializer.Deserialize<List<Block>>(json)!));

        Assert.Equal("*Careful*", alert.Text.Text);
        Assert.Equal(AlertLevel.Warning, alert.Level);
    }
}
