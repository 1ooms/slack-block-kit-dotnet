using System.Reflection;
using System.Text.Json.Serialization;

namespace SlackBlockKit.Tests.Elements;

public class ElementInterfaceSerializationTests
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
    public void Deserialize_ElementNotImplementingInterface_Throws()
    {
        Assert.Throws<JsonException>(() =>
            SlackSerializer.Deserialize<IInputElement>("""{"type":"user","user_id":"U123"}"""));
    }

    [Fact]
    public void Deserialize_ActionsBlockWithDisallowedElement_Throws()
    {
        const string json =
            """{"type":"actions","elements":[{"type":"image","alt_text":"a","image_url":"https://example.com/i.png"}]}""";

        Assert.Throws<JsonException>(() => SlackSerializer.Deserialize<Block>(json));
    }

    [Fact]
    public void Deserialize_RichTextSectionWithDisallowedElement_Throws()
    {
        const string json = """{"type":"rich_text_section","elements":[{"type":"button","text":{"type":"plain_text","text":"Go"}}]}""";

        Assert.Throws<JsonException>(() => SlackSerializer.Deserialize<RichTextElement>(json));
    }

    public static TheoryData<Type> PolymorphicInterfaces
    {
        get
        {
            var data = new TheoryData<Type>();
            foreach (var type in typeof(SlackSerializer).Assembly.GetExportedTypes())
                if (type.IsInterface && type.IsDefined(typeof(JsonPolymorphicAttribute), false))
                    data.Add(type);
            return data;
        }
    }

    /// <summary>
    /// Serializing an element through an interface whose JsonDerivedType list doesn't include it throws
    /// at runtime, so every concrete implementer must be listed.
    /// </summary>
    [Theory]
    [MemberData(nameof(PolymorphicInterfaces))]
    public void PolymorphicInterface_ListsEveryConcreteImplementer(Type interfaceType)
    {
        var listed = DerivedTypes(interfaceType).Select(attribute => attribute.DerivedType).ToHashSet();

        var missing = typeof(SlackSerializer).Assembly.GetExportedTypes()
            .Where(type => type is { IsClass: true, IsAbstract: false } && interfaceType.IsAssignableFrom(type))
            .Where(type => !listed.Contains(type))
            .Select(type => type.Name)
            .ToList();

        Assert.True(missing.Count == 0,
            $"{interfaceType.Name} is missing [JsonDerivedType] for: {string.Join(", ", missing)}");
    }

    /// <summary>
    /// An element must be written with the same "type" discriminator whether it is serialized
    /// through <see cref="Element"/> or through one of its marker interfaces.
    /// </summary>
    [Theory]
    [MemberData(nameof(PolymorphicInterfaces))]
    public void PolymorphicInterface_UsesSameDiscriminatorsAsElement(Type interfaceType)
    {
        var elementIds = DerivedTypes(typeof(Element))
            .ToDictionary(attribute => attribute.DerivedType, attribute => attribute.TypeDiscriminator);

        var mismatched = DerivedTypes(interfaceType)
            .Where(attribute => elementIds.TryGetValue(attribute.DerivedType, out var id)
                                && !Equals(id, attribute.TypeDiscriminator))
            .Select(attribute => $"{attribute.DerivedType.Name} ('{attribute.TypeDiscriminator}')")
            .ToList();

        Assert.True(mismatched.Count == 0,
            $"{interfaceType.Name} uses a different discriminator than Element for: {string.Join(", ", mismatched)}");
    }

    private static IEnumerable<JsonDerivedTypeAttribute> DerivedTypes(Type type)
    {
        return type.GetCustomAttributes<JsonDerivedTypeAttribute>(false);
    }
}
