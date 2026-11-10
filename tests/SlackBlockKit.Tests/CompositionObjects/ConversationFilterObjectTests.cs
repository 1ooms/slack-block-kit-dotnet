namespace SlackBlockKit.Tests.CompositionObjects;

public class ConversationFilterObjectTests
{
    [Fact]
    public void Constructor_SetsProperties()
    {
        var filter = new ConversationFilterObject([ConversationType.Im, ConversationType.Public], true, false);

        Assert.Equal([ConversationType.Im, ConversationType.Public], filter.Include);
        Assert.True(filter.ExcludeExternalSharedChannels);
        Assert.False(filter.ExcludeBotUsers);
    }

    [Fact]
    public void Serialize_WritesSnakeCaseFieldsAndValues()
    {
        var json = ToJson(new ConversationFilterObject(
            [ConversationType.Im, ConversationType.Mpim, ConversationType.Private, ConversationType.Public],
            true, true));

        AssertProperties(json, "include", "exclude_external_shared_channels", "exclude_bot_users");
        Assert.Equal(["im", "mpim", "private", "public"],
            json.GetProperty("include").EnumerateArray().Select(e => e.GetString()));
        Assert.True(json.GetProperty("exclude_external_shared_channels").GetBoolean());
        Assert.True(json.GetProperty("exclude_bot_users").GetBoolean());
    }

    [Fact]
    public void Serialize_OmitsNullFields()
    {
        AssertProperties(ToJson(new ConversationFilterObject(excludeBotUsers: true)), "exclude_bot_users");
    }

    [Fact]
    public void Constructor_ThrowsWhenNoFilterProvided()
    {
        Assert.Throws<ArgumentException>(() => new ConversationFilterObject());
    }

    [Fact]
    public void Constructor_AcceptsAnySingleFilter()
    {
        Assert.NotNull(new ConversationFilterObject([ConversationType.Im]));
        Assert.NotNull(new ConversationFilterObject(excludeExternalSharedChannels: false));
        Assert.NotNull(new ConversationFilterObject(excludeBotUsers: false));
    }

    [Fact]
    public void Include_ThrowsWhenEmpty()
    {
        Assert.Throws<ArgumentException>(() => new ConversationFilterObject([]));
    }
}
