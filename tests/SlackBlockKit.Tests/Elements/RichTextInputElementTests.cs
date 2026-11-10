namespace SlackBlockKit.Tests.Elements;

public class RichTextInputElementTests
{
    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var initial = RichTextBlockOf();
        var dispatch = new DispatchActionConfigurationObject([TriggerActions.OnEnterPressed]);

        var element = new RichTextInputElement("action", initial, dispatch, true, "Write", 2, 10);

        Assert.Equal("action", element.ActionId);
        Assert.Same(initial, element.InitialValue);
        Assert.Same(dispatch, element.DispatchActionConfig);
        Assert.True(element.FocusOnLoad);
        Assert.Equal("Write", element.Placeholder!.Text);
        Assert.Equal(2, element.MinLines);
        Assert.Equal(10, element.MaxLines);
        Assert.Equal("rich_text_input", element.Type);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new RichTextInputElement("action", RichTextBlockOf("hello"),
            new DispatchActionConfigurationObject([TriggerActions.OnEnterPressed]), true, "Write", 1, 100));

        AssertProperties(json, "type", "action_id", "initial_value", "dispatch_action_config", "focus_on_load",
            "placeholder", "min_lines", "max_lines");
        Assert.Equal("rich_text_input", json.GetProperty("type").GetString());
        var initial = json.GetProperty("initial_value");
        Assert.Equal("rich_text", initial.GetProperty("type").GetString());
        Assert.Equal("hello", initial.GetProperty("elements")[0].GetProperty("elements")[0]
            .GetProperty("text").GetString());
    }

    [Fact]
    public void Serialize_OmitsNullFields()
    {
        AssertProperties(ToJson(new RichTextInputElement()), "type");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    public void Lines_AcceptValuesBetween1And100(int value)
    {
        var element = new RichTextInputElement(minLines: value, maxLines: value);

        Assert.Equal(value, element.MinLines);
        Assert.Equal(value, element.MaxLines);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(101)]
    public void Lines_ThrowWhenOutOfRange(int value)
    {
        Assert.Throws<ArgumentException>(() => new RichTextInputElement(minLines: value));
        Assert.Throws<ArgumentException>(() => new RichTextInputElement(maxLines: value));
    }

    [Fact]
    public void Placeholder_ThrowsWhenTooLong()
    {
        Assert.NotNull(new RichTextInputElement(placeholder: Chars(150)));
        Assert.Throws<ArgumentException>(() => new RichTextInputElement(placeholder: Chars(151)));
    }
}
