namespace SlackBlockKit.Tests.Surfaces;

/// <summary>
/// Checks which blocks each surface accepts, following the <c>available_in_surfaces</c> list on each block's
/// Slack documentation page.
/// </summary>
public class SurfaceBlockTests
{
    public static TheoryData<Type, bool, bool, bool> Availability => new()
    {
        // block, message, modal, home
        { typeof(ActionsBlock), true, true, true },
        { typeof(AlertBlock), false, true, true },
        { typeof(CardBlock), true, true, true },
        { typeof(CarouselBlock), true, false, true },
        { typeof(ContainerBlock), true, false, true },
        { typeof(ContextActionsBlock), true, false, false },
        { typeof(ContextBlock), true, true, true },
        { typeof(DataTableBlock), true, false, true },
        { typeof(DataVisualizationBlock), true, false, true },
        { typeof(DividerBlock), true, true, true },
        { typeof(FileBlock), true, false, false },
        { typeof(HeaderBlock), true, true, true },
        { typeof(ImageBlock), true, true, true },
        { typeof(InputBlock), true, true, true },
        { typeof(MarkdownBlock), true, false, false },
        { typeof(PlanBlock), true, false, false },
        { typeof(RichTextBlock), true, true, true },
        { typeof(SectionBlock), true, true, true },
        { typeof(TableBlock), true, false, true },
        { typeof(TaskCardBlock), true, false, false },
        { typeof(VideoBlock), true, true, true }
    };

    [Theory]
    [MemberData(nameof(Availability))]
    public void Block_ImplementsInterfacesOfItsSurfaces(Type block, bool message, bool modal, bool home)
    {
        Assert.Equal(message, typeof(IMessageBlock).IsAssignableFrom(block));
        Assert.Equal(modal, typeof(IModalBlock).IsAssignableFrom(block));
        Assert.Equal(home, typeof(IHomeBlock).IsAssignableFrom(block));
    }

    [Fact]
    public void EveryBlock_IsCovered()
    {
        var covered = Availability.Select(row => (Type)row[0]).ToHashSet();
        var blocks = typeof(Block).Assembly.GetTypes()
            .Where(type => type is { IsClass: true, IsAbstract: false } && type.IsSubclassOf(typeof(Block)));

        Assert.All(blocks, block => Assert.Contains(block, covered));
    }

    [Fact]
    public void Serialize_WritesBlockDiscriminatorThroughInterface()
    {
        var json = ToJson(new ModalSurface("Title", [new AlertBlock("Careful"), new DividerBlock()]));

        Assert.Equal("alert", json.GetProperty("blocks")[0].GetProperty("type").GetString());
        Assert.Equal("divider", json.GetProperty("blocks")[1].GetProperty("type").GetString());
    }

    [Fact]
    public void Deserialize_ThrowsForBlockNotAvailableInSurface()
    {
        const string json = """{"text":"Hi","blocks":[{"type":"alert","text":{"type":"mrkdwn","text":"Careful"}}]}""";

        Assert.Throws<JsonException>(() => SlackSerializer.Deserialize<MessageSurface>(json));
    }

    [Fact]
    public void Deserialize_ReadsBlockAvailableInSurface()
    {
        const string json =
            """{"type":"modal","title":{"type":"plain_text","text":"T"},"blocks":[{"type":"alert","text":{"type":"mrkdwn","text":"Careful"}}]}""";

        var modal = SlackSerializer.Deserialize<ModalSurface>(json)!;

        Assert.IsType<AlertBlock>(Assert.Single(modal.Blocks));
    }
}
