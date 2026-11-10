namespace SlackBlockKit.Tests.Blocks;

public class ActionsBlockTests
{
    [Fact]
    public void ListConstructor_SetsProperties()
    {
        var elements = new List<IActionsElement> { new ButtonElement("Click") };

        var block = new ActionsBlock(elements, "actions_1");

        Assert.Same(elements, block.Elements);
        Assert.Equal("actions_1", block.BlockId);
        Assert.Equal("actions", block.Type);
    }

    [Fact]
    public void ParamsConstructor_CollectsElements()
    {
        var block = new ActionsBlock(new ButtonElement("A"), new DatePickerElement());

        Assert.Equal(2, block.Elements.Count);
        Assert.Null(block.BlockId);
    }

    [Fact]
    public void Serialize_WritesElementsWithDiscriminators()
    {
        var json = ToJson(new ActionsBlock([
            new ButtonElement("A"), new StaticSelect(options: [Option()]), new CheckboxesElement([TextOption()]),
            new OverflowMenuElement([OverflowOption()]), new WorkflowButtonElement("Run", "a", "https://slack.com/x")
        ], "actions_1"));

        AssertProperties(json, "type", "block_id", "elements");
        Assert.Equal("actions", json.GetProperty("type").GetString());
        Assert.Equal(["button", "static_select", "checkboxes", "overflow", "workflow_button"],
            json.GetProperty("elements").EnumerateArray().Select(e => e.GetProperty("type").GetString()));
    }

    [Fact]
    public void Serialize_OmitsBlockIdWhenNull()
    {
        AssertProperties(ToJson(new ActionsBlock(new ButtonElement("A"))), "type", "elements");
    }

    [Fact]
    public void Elements_ThrowsWhenNullOrEmpty()
    {
        Assert.Throws<ArgumentNullException>(() => new ActionsBlock((List<IActionsElement>)null!));
        Assert.Throws<ArgumentException>(() => new ActionsBlock());
    }

    [Fact]
    public void Elements_AllowsUpTo25()
    {
        var elements = Enumerable.Range(0, 25).Select(_ => (IActionsElement)new ButtonElement("A")).ToList();

        Assert.Equal(25, new ActionsBlock(elements).Elements.Count);

        elements.Add(new ButtonElement("A"));
        Assert.Throws<ArgumentException>(() => new ActionsBlock(elements));
    }

    [Fact]
    public void RoundTrip_PreservesValues()
    {
        var block = RoundTrip(new ActionsBlock([new ButtonElement("Go", "go")], "actions_1"));

        var button = Assert.IsType<ButtonElement>(Assert.Single(block.Elements));
        Assert.Equal("go", button.ActionId);
        Assert.Equal("actions_1", block.BlockId);
    }
}
