namespace SlackBlockKit.Tests.Elements;

public class InterfaceElementConverterTests
{
    [Fact]
    public void Serialize_ActionsElementList_WritesDiscriminators()
    {
        var elements = new List<IActionsElement> { new ButtonElement("Click"), new DatePickerElement("date") };

        var json = ToJson(elements);

        Assert.Equal("button", json[0].GetProperty("type").GetString());
        Assert.Equal("datepicker", json[1].GetProperty("type").GetString());
    }

    [Fact]
    public void Serialize_InputElement_WritesDiscriminator()
    {
        var json = ToJson(new List<IInputElement> { new EmailInputElement("email") });

        Assert.Equal("email_text_input", json[0].GetProperty("type").GetString());
    }

    [Fact]
    public void Serialize_SectionElement_WritesDiscriminator()
    {
        var json = ToJson(new List<ISectionElement> { Image() });

        Assert.Equal("image", json[0].GetProperty("type").GetString());
    }

    [Fact]
    public void Serialize_ContextActionsElement_WritesDiscriminator()
    {
        var json = ToJson(new List<IContextActionsElement>
            { new IconButtonElement(IconButtonIcon.Trash, "Delete") });

        Assert.Equal("icon_button", json[0].GetProperty("type").GetString());
    }

    [Fact]
    public void Serialize_RichTextSubElement_WritesDiscriminator()
    {
        var json = ToJson(new List<IRichTextSubElement> { new TextElement("hi"), new UserElement("U123") });

        Assert.Equal("text", json[0].GetProperty("type").GetString());
        Assert.Equal("user", json[1].GetProperty("type").GetString());
    }

    [Fact]
    public void Deserialize_ActionsElement_ReturnsConcreteType()
    {
        var element = SlackSerializer.Deserialize<IActionsElement>(
            """{"type":"datepicker","action_id":"date"}""");

        var datePicker = Assert.IsType<DatePickerElement>(element);
        Assert.Equal("date", datePicker.ActionId);
    }

    [Fact]
    public void Deserialize_RichTextSubElement_ReturnsConcreteType()
    {
        var element = SlackSerializer.Deserialize<IRichTextSubElement>("""{"type":"user","user_id":"U123"}""");

        Assert.Equal("U123", Assert.IsType<UserElement>(element).UserId);
    }

    [Fact]
    public void Deserialize_ElementNotImplementingInterface_ReturnsNull()
    {
        var element = SlackSerializer.Deserialize<IInputElement>("""{"type":"user","user_id":"U123"}""");

        Assert.Null(element);
    }
}
