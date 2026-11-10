namespace SlackBlockKit.Tests.Blocks;

public class SectionBlockTests
{
    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var text = new MarkdownTextObject("*Text*");
        var fields = new List<TextObject> { new PlainTextObject("A") };
        var accessory = new ButtonElement("Go");

        var block = new SectionBlock(text, fields, accessory, true, "section_1");

        Assert.Same(text, block.Text);
        Assert.Same(fields, block.Fields);
        Assert.Same(accessory, block.Accessory);
        Assert.True(block.Expand);
        Assert.Equal("section_1", block.BlockId);
        Assert.Equal("section", block.Type);
    }

    [Fact]
    public void MarkdownConstructor_WrapsTextInMarkdown()
    {
        var accessory = Image();

        var block = new SectionBlock("*Text*", accessory, "section_1");

        Assert.IsType<MarkdownTextObject>(block.Text);
        Assert.Same(accessory, block.Accessory);
        Assert.Equal("section_1", block.BlockId);
        Assert.Null(block.Fields);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new SectionBlock(new MarkdownTextObject("*Text*"),
            [new PlainTextObject("A"), new MarkdownTextObject("B")], new ButtonElement("Go"), false, "section_1"));

        AssertProperties(json, "type", "text", "fields", "accessory", "expand", "block_id");
        Assert.Equal("section", json.GetProperty("type").GetString());
        Assert.Equal(["plain_text", "mrkdwn"],
            json.GetProperty("fields").EnumerateArray().Select(f => f.GetProperty("type").GetString()));
        Assert.Equal("button", json.GetProperty("accessory").GetProperty("type").GetString());
    }

    [Fact]
    public void Serialize_OmitsNullFields()
    {
        AssertProperties(ToJson(new SectionBlock("Text")), "type", "text");
    }

    [Fact]
    public void Serialize_SupportsEverySectionAccessoryType()
    {
        ISectionElement[] accessories =
        [
            new ButtonElement("Go"), new CheckboxesElement([Option()]), new DatePickerElement(), Image(),
            new OverflowMenuElement([Option()]), new RadioButtonGroupElement([Option()]), new TimePickerElement(),
            new WorkflowButtonElement("Run", "a", "https://slack.com/x"), new UsersSelect(), new MultiChannelsSelect()
        ];

        var types = accessories.Select(a => ToJson(new SectionBlock("Text", a)).GetProperty("accessory")
            .GetProperty("type").GetString());

        Assert.Equal(["button", "checkboxes", "datepicker", "image", "overflow", "radio_buttons", "timepicker",
            "workflow_button", "users_select", "multi_channels_select"], types);
    }

    [Fact]
    public void Constructor_ThrowsWithoutTextOrFields()
    {
        Assert.Throws<ArgumentException>(() => new SectionBlock());
        Assert.Throws<ArgumentException>(() => new SectionBlock(fields: []));
    }

    [Fact]
    public void Constructor_AcceptsFieldsWithoutText()
    {
        Assert.Null(new SectionBlock(fields: [new PlainTextObject("A")]).Text);
    }

    [Fact]
    public void Text_AllowsUpTo3000Characters()
    {
        Assert.NotNull(new SectionBlock(Chars(3000)));
        Assert.Throws<ArgumentException>(() => new SectionBlock(Chars(3001)));
    }

    [Fact]
    public void Fields_AllowsUpTo10()
    {
        var fields = Enumerable.Range(0, 10).Select(i => (TextObject)new PlainTextObject($"{i}")).ToList();

        Assert.Equal(10, new SectionBlock(fields: fields).Fields!.Count);

        fields.Add(new PlainTextObject("extra"));
        Assert.Throws<ArgumentException>(() => new SectionBlock(fields: fields));
    }

    [Fact]
    public void Fields_EachAllowsUpTo2000Characters()
    {
        Assert.NotNull(new SectionBlock(fields: [new PlainTextObject(Chars(2000))]));
        Assert.Throws<ArgumentException>(() => new SectionBlock(fields: [new PlainTextObject(Chars(2001))]));
    }

    [Fact]
    public void Fields_ThrowsWhenEmptyAlongsideText()
    {
        Assert.Throws<ArgumentException>(() => new SectionBlock(new PlainTextObject("Text"), []));
    }

    [Fact]
    public void RoundTrip_PreservesValues()
    {
        var block = RoundTrip(new SectionBlock(new MarkdownTextObject("*Text*"), [new PlainTextObject("Field")],
            new ButtonElement("Go", "go"), true, "section_1"));

        Assert.Equal("*Text*", Assert.IsType<MarkdownTextObject>(block.Text).Text);
        Assert.Equal("Field", Assert.IsType<PlainTextObject>(Assert.Single(block.Fields!)).Text);
        Assert.Equal("go", Assert.IsType<ButtonElement>(block.Accessory).ActionId);
        Assert.True(block.Expand);
        Assert.Equal("section_1", block.BlockId);
    }
}
