namespace SlackBlockKit.Tests.Blocks;

public class ContextActionsBlockTests
{
    private static FeedbackButtonsElement Feedback() =>
        new(new FeedbackButtonObject("Good", "1"), new FeedbackButtonObject("Bad", "0"));

    [Fact]
    public void ListConstructor_SetsProperties()
    {
        var elements = new List<IContextActionsElement> { Feedback() };

        var block = new ContextActionsBlock(elements, "ctx_actions");

        Assert.Same(elements, block.Elements);
        Assert.Equal("ctx_actions", block.BlockId);
        Assert.Equal("context_actions", block.Type);
    }

    [Fact]
    public void ParamsConstructor_CollectsElements()
    {
        var block = new ContextActionsBlock(Feedback(), new IconButtonElement(IconButtonIcon.Trash, "Delete"));

        Assert.Equal(2, block.Elements.Count);
    }

    [Fact]
    public void Serialize_WritesElementsWithDiscriminators()
    {
        var json = ToJson(new ContextActionsBlock([Feedback(), new IconButtonElement(IconButtonIcon.Trash, "Del")],
            "ctx_actions"));

        AssertProperties(json, "type", "elements", "block_id");
        Assert.Equal("context_actions", json.GetProperty("type").GetString());
        Assert.Equal(["feedback_buttons", "icon_button"],
            json.GetProperty("elements").EnumerateArray().Select(e => e.GetProperty("type").GetString()));
    }

    [Fact]
    public void Elements_ThrowsWhenNullOrEmpty()
    {
        Assert.Throws<ArgumentNullException>(() => new ContextActionsBlock((List<IContextActionsElement>)null!));
        Assert.Throws<ArgumentException>(() => new ContextActionsBlock());
    }

    [Fact]
    public void Elements_AllowsUpTo5()
    {
        var elements = Enumerable.Range(0, 5).Select(_ => (IContextActionsElement)Feedback()).ToList();

        Assert.Equal(5, new ContextActionsBlock(elements).Elements.Count);

        elements.Add(Feedback());
        Assert.Throws<ArgumentException>(() => new ContextActionsBlock(elements));
    }

    [Fact]
    public void RoundTrip_PreservesValues()
    {
        var block = RoundTrip(new ContextActionsBlock([Feedback()], "ctx_actions"));

        var feedback = Assert.IsType<FeedbackButtonsElement>(Assert.Single(block.Elements));
        Assert.Equal("Good", feedback.PositiveButton.Text.Text);
        Assert.Equal("ctx_actions", block.BlockId);
    }
}
