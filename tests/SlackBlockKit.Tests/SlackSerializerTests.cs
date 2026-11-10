using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace SlackBlockKit.Tests;

public class SlackSerializerTests
{
    [JsonPolymorphic]
    [JsonDerivedType(typeof(DefaultDiscriminatorDerived), "derived")]
    public abstract class DefaultDiscriminatorBase;

    public class DefaultDiscriminatorDerived : DefaultDiscriminatorBase;

    public class UnregisteredBlock : Block
    {
        public override string Type => "unregistered";
    }

    private static object Payload() => new
    {
        blocks = new List<Block>
        {
            new AlertBlock("Careful", AlertLevel.Warning),
            new CardBlock(title: new PlainTextObject("Title"),
                actions: [new ButtonElement("Go", "go", style: ButtonStyle.Primary)])
        }
    };

    [Fact]
    public void Options_ProduceSameJsonAsSerialize()
    {
        var payload = Payload();

        Assert.Equal(SlackSerializer.Serialize(payload), JsonSerializer.Serialize(payload, SlackSerializer.Options));
    }

    [Fact]
    public void Options_WriteSlackCompatibleJson()
    {
        using var doc = JsonDocument.Parse(JsonSerializer.Serialize(Payload(), SlackSerializer.Options));
        var blocks = doc.RootElement.GetProperty("blocks");
        var button = blocks[1].GetProperty("actions")[0];

        AssertNoProperty(blocks[0], "Type");
        Assert.Equal("warning", blocks[0].GetProperty("level").GetString());
        AssertNoProperty(button, "Type");
        Assert.Equal("button", button.GetProperty("type").GetString());
        Assert.Equal("primary", button.GetProperty("style").GetString());
    }

    [Fact]
    public async Task Options_WorkWithJsonContent()
    {
        var payload = Payload();

        using var content = JsonContent.Create(payload, payload.GetType(), options: SlackSerializer.Options);

        Assert.Equal(SlackSerializer.Serialize(payload), await content.ReadAsStringAsync());
    }

    [Fact]
    public void Options_AreReadOnly()
    {
        Assert.True(SlackSerializer.Options.IsReadOnly);
        Assert.Throws<InvalidOperationException>(() => SlackSerializer.Options.WriteIndented = true);
    }

    [Fact]
    public void Options_CanBeCopiedAndCustomized()
    {
        var options = new JsonSerializerOptions(SlackSerializer.Options) { WriteIndented = true };
        var button = new ButtonElement("Go");

        Assert.Equal(SlackSerializer.Serialize(button, indented: true), JsonSerializer.Serialize(button, options));
    }

    [Fact]
    public void Serialize_ConcreteTypeOfPolymorphicBase_UsesDefaultDiscriminatorName()
    {
        Assert.Equal("""{"$type":"derived"}""", SlackSerializer.Serialize(new DefaultDiscriminatorDerived()));
    }

    [Fact]
    public void Serialize_ConcreteTypeNotRegisteredOnBase_Throws()
    {
        Assert.Throws<NotSupportedException>(() => SlackSerializer.Serialize(new UnregisteredBlock()));
    }
}
