namespace SlackBlockKit.Tests.Elements;

public class MessageCitationDetailsTests
{
    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var details = new MessageCitationDetails("C123", "123.456");

        Assert.Equal("C123", details.Channel);
        Assert.Equal("123.456", details.MessageTs);
        Assert.Equal("message", details.CitationType);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new MessageCitationDetails("C123", "123.456"));

        AssertProperties(json, "citation_type", "channel", "message_ts");
        Assert.Equal("message", json.GetProperty("citation_type").GetString());
        Assert.Equal("C123", json.GetProperty("channel").GetString());
        Assert.Equal("123.456", json.GetProperty("message_ts").GetString());
    }

    [Fact]
    public void RequiredProperties_ThrowWhenNull()
    {
        Assert.Throws<ArgumentNullException>(() => new MessageCitationDetails(null!, "123.456"));
        Assert.Throws<ArgumentNullException>(() => new MessageCitationDetails("C123", null!));
    }
}
