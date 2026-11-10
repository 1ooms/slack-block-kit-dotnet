namespace SlackBlockKit.Tests.Surfaces;

public class HomeSurfaceTests
{
    [Fact]
    public void Constructor_SetsProperties()
    {
        List<IHomeBlock> blocks = [new HeaderBlock("Header")];

        var home = new HomeSurface(blocks, "metadata", "callback", "external");

        Assert.Same(blocks, home.Blocks);
        Assert.Equal("metadata", home.PrivateMetadata);
        Assert.Equal("callback", home.CallbackId);
        Assert.Equal("external", home.ExternalId);
        Assert.Equal("home", home.Type);
    }

    [Fact]
    public void ParamsConstructor_SetsBlocks()
    {
        var home = new HomeSurface(new HeaderBlock("Header"), new DividerBlock());

        Assert.Equal(2, home.Blocks.Count);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new HomeSurface([new HeaderBlock("Header")], "metadata", "callback", "external"));

        AssertProperties(json, "type", "blocks", "private_metadata", "callback_id", "external_id");
        Assert.Equal("home", json.GetProperty("type").GetString());
        Assert.Equal("header", json.GetProperty("blocks")[0].GetProperty("type").GetString());
    }

    [Fact]
    public void Serialize_OmitsNullFields()
    {
        AssertProperties(ToJson(new HomeSurface()), "type", "blocks");
    }

    [Fact]
    public void Blocks_ThrowsWhenNullOrTooMany()
    {
        Assert.Throws<ArgumentNullException>(() => new HomeSurface((List<IHomeBlock>)null!));
        Assert.NotNull(new HomeSurface(Enumerable.Range(0, 100).Select(_ => (IHomeBlock)new DividerBlock()).ToList()));
        Assert.Throws<ArgumentException>(() =>
            new HomeSurface(Enumerable.Range(0, 101).Select(_ => (IHomeBlock)new DividerBlock()).ToList()));
    }

    [Fact]
    public void Identifiers_ThrowWhenTooLong()
    {
        Assert.NotNull(new HomeSurface([], Chars(3000), Chars(255)));
        Assert.Throws<ArgumentException>(() => new HomeSurface([], privateMetadata: Chars(3001)));
        Assert.Throws<ArgumentException>(() => new HomeSurface([], callbackId: Chars(256)));
    }

    [Fact]
    public void RoundTrip_PreservesValues()
    {
        var home = RoundTrip(new HomeSurface([new HeaderBlock("Header")], "metadata", "callback", "external"));

        Assert.Equal("Header", Assert.IsType<HeaderBlock>(Assert.Single(home.Blocks)).Text.Text);
        Assert.Equal("metadata", home.PrivateMetadata);
        Assert.Equal("callback", home.CallbackId);
        Assert.Equal("external", home.ExternalId);
    }

    [Fact]
    public void Deserialize_ReadsSlackPayloadWithTypeAfterOtherProperties()
    {
        const string json =
            """{"id":"V123","team_id":"T123","blocks":[{"type":"divider"}],"private_metadata":"metadata","type":"home"}""";

        var home = SlackSerializer.Deserialize<HomeSurface>(json)!;

        Assert.IsType<DividerBlock>(Assert.Single(home.Blocks));
        Assert.Equal("metadata", home.PrivateMetadata);
    }
}
