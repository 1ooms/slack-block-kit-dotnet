namespace SlackBlockKit.Tests.Blocks;

public class InputBlockTests
{
    private static PlainTextInputElement Input() => new("input_action");

    [Fact]
    public void TextObjectConstructor_SetsAllProperties()
    {
        var element = Input();

        var block = new InputBlock(new PlainTextObject("Label"), element, "input_1", true,
            new PlainTextObject("Hint"), true);

        Assert.Equal("Label", block.Label.Text);
        Assert.Same(element, block.Element);
        Assert.Equal("input_1", block.BlockId);
        Assert.True(block.DispatchAction);
        Assert.Equal("Hint", block.Hint!.Text);
        Assert.True(block.Optional);
        Assert.Equal("input", block.Type);
    }

    [Fact]
    public void StringConstructor_WrapsLabelAndHint()
    {
        var block = new InputBlock("Label", Input(), hint: "Hint");

        Assert.Equal("Label", block.Label.Text);
        Assert.Equal("Hint", block.Hint!.Text);
    }

    [Fact]
    public void StringConstructor_LeavesHintNullWhenOmitted()
    {
        Assert.Null(new InputBlock("Label", Input()).Hint);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new InputBlock("Label", Input(), "input_1", true, "Hint", false));

        AssertProperties(json, "type", "label", "element", "block_id", "dispatch_action", "hint", "optional");
        Assert.Equal("input", json.GetProperty("type").GetString());
        Assert.Equal("plain_text_input", json.GetProperty("element").GetProperty("type").GetString());
        Assert.False(json.GetProperty("optional").GetBoolean());
    }

    [Fact]
    public void Serialize_OmitsNullFields()
    {
        AssertProperties(ToJson(new InputBlock("Label", Input())), "type", "label", "element");
    }

    [Fact]
    public void Serialize_SupportsEveryInputElementType()
    {
        IInputElement[] elements =
        [
            new CheckboxesElement([Option()]), new DatePickerElement(), new DateTimePickerElement(),
            new EmailInputElement(), new FileInputElement(), new NumberInputElement(), Input(),
            new RadioButtonGroupElement([Option()]), new RichTextInputElement(), new TimePickerElement(),
            new UrlInputElement(), new StaticSelect(options: [Option()]), new MultiUsersSelect()
        ];

        var types = elements.Select(e => ToJson(new InputBlock("Label", e)).GetProperty("element")
            .GetProperty("type").GetString());

        Assert.Equal(["checkboxes", "datepicker", "datetimepicker", "email_text_input", "file_input",
            "number_input", "plain_text_input", "radio_buttons", "rich_text_input", "timepicker", "url_text_input",
            "static_select", "multi_users_select"], types);
    }

    [Fact]
    public void Label_ThrowsWhenNullOrTooLong()
    {
        Assert.Throws<ArgumentNullException>(() => new InputBlock((PlainTextObject)null!, Input()));
        Assert.NotNull(new InputBlock(Chars(2000), Input()));
        Assert.Throws<ArgumentException>(() => new InputBlock(Chars(2001), Input()));
    }

    [Fact]
    public void Element_ThrowsWhenNull()
    {
        Assert.Throws<ArgumentNullException>(() => new InputBlock("Label", null!));
    }

    [Fact]
    public void Hint_ThrowsWhenTooLong()
    {
        Assert.NotNull(new InputBlock("Label", Input(), hint: Chars(2000)));
        Assert.Throws<ArgumentException>(() => new InputBlock("Label", Input(), hint: Chars(2001)));
    }

    [Fact]
    public void RoundTrip_PreservesValues()
    {
        var block = RoundTrip(new InputBlock("Label", new PlainTextInputElement("input", "Placeholder"), "input_1",
            true, "Hint", true));

        Assert.Equal("Label", block.Label.Text);
        Assert.Equal("input", Assert.IsType<PlainTextInputElement>(block.Element).ActionId);
        Assert.Equal("input_1", block.BlockId);
        Assert.True(block.DispatchAction);
        Assert.Equal("Hint", block.Hint!.Text);
        Assert.True(block.Optional);
    }
}
