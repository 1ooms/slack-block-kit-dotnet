namespace SlackBlockKit.Tests.Blocks;

public class VideoBlockTests
{
    private const string Thumbnail = "https://example.com/thumb.png";
    private const string Video = "https://example.com/embed/video";
    private const string TitleUrl = "https://example.com/video";

    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var block = new VideoBlock("alt", new PlainTextObject("Title"), Thumbnail, Video, TitleUrl, "Provider",
            "Author", new PlainTextObject("Description"), "https://example.com/icon.png", "video_1");

        Assert.Equal("alt", block.AltText);
        Assert.Equal("Title", block.Title.Text);
        Assert.Equal(Thumbnail, block.ThumbnailUrl);
        Assert.Equal(Video, block.VideoUrl);
        Assert.Equal(TitleUrl, block.TitleUrl);
        Assert.Equal("Provider", block.ProviderName);
        Assert.Equal("Author", block.AuthorName);
        Assert.Equal("Description", block.Description!.Text);
        Assert.Equal("https://example.com/icon.png", block.ProviderIconUrl);
        Assert.Equal("video_1", block.BlockId);
        Assert.Equal("video", block.Type);
    }

    [Fact]
    public void StringConstructor_WrapsTitle()
    {
        var block = new VideoBlock("alt", "Title", Thumbnail, Video, TitleUrl, "Provider", "video_1");

        Assert.Equal("Title", block.Title.Text);
        Assert.Equal("Provider", block.ProviderName);
        Assert.Equal("video_1", block.BlockId);
        Assert.Null(block.AuthorName);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new VideoBlock("alt", new PlainTextObject("Title"), Thumbnail, Video, TitleUrl,
            "Provider", "Author", new PlainTextObject("Description"), "https://example.com/icon.png", "video_1"));

        AssertProperties(json, "type", "alt_text", "title", "thumbnail_url", "video_url", "title_url",
            "provider_name", "author_name", "description", "provider_icon_url", "block_id");
        Assert.Equal("video", json.GetProperty("type").GetString());
        Assert.Equal("plain_text", json.GetProperty("title").GetProperty("type").GetString());
    }

    [Fact]
    public void Serialize_OmitsNullFields()
    {
        AssertProperties(ToJson(new VideoBlock("alt", "Title", Thumbnail, Video)), "type", "alt_text", "title",
            "thumbnail_url", "video_url");
    }

    [Fact]
    public void RequiredProperties_ThrowWhenNullOrEmpty()
    {
        Assert.Throws<ArgumentNullException>(() => new VideoBlock(null!, "Title", Thumbnail, Video));
        Assert.Throws<ArgumentNullException>(() => new VideoBlock("alt", (PlainTextObject)null!, Thumbnail, Video));
        Assert.Throws<ArgumentException>(() => new VideoBlock("alt", "Title", "", Video));
        Assert.Throws<ArgumentException>(() => new VideoBlock("alt", "Title", Thumbnail, ""));
    }

    [Fact]
    public void Title_AllowsUpTo200Characters()
    {
        Assert.NotNull(new VideoBlock("alt", Chars(200), Thumbnail, Video));
        Assert.Throws<ArgumentException>(() => new VideoBlock("alt", Chars(201), Thumbnail, Video));
    }

    [Fact]
    public void TitleUrl_MustBeHttps()
    {
        Assert.Throws<ArgumentException>(() => new VideoBlock("alt", "Title", Thumbnail, Video, "http://example.com"));
        Assert.Throws<ArgumentException>(() => new VideoBlock("alt", "Title", Thumbnail, Video, "not a url"));
    }

    [Fact]
    public void TitleUrl_AllowsUpTo200Characters()
    {
        const string prefix = "https://example.com/";

        Assert.NotNull(new VideoBlock("alt", "Title", Thumbnail, Video, prefix + Chars(200 - prefix.Length)));
        Assert.Throws<ArgumentException>(() =>
            new VideoBlock("alt", "Title", Thumbnail, Video, prefix + Chars(201 - prefix.Length)));
    }

    [Fact]
    public void AuthorName_AllowsUpTo50Characters()
    {
        var title = new PlainTextObject("Title");

        Assert.NotNull(new VideoBlock("alt", title, Thumbnail, Video, authorName: Chars(50)));
        Assert.Throws<ArgumentException>(() => new VideoBlock("alt", title, Thumbnail, Video, authorName: Chars(51)));
    }

    [Fact]
    public void Description_AllowsUpTo200Characters()
    {
        var title = new PlainTextObject("Title");

        Assert.NotNull(new VideoBlock("alt", title, Thumbnail, Video, description: new PlainTextObject(Chars(200))));
        Assert.Throws<ArgumentException>(() =>
            new VideoBlock("alt", title, Thumbnail, Video, description: new PlainTextObject(Chars(201))));
    }

    [Fact]
    public void RoundTrip_PreservesValues()
    {
        var block = RoundTrip(new VideoBlock("Alt", new PlainTextObject("Title"), "https://example.com/thumb.png",
            "https://example.com/video", "https://example.com/title", "Provider", "Author",
            new PlainTextObject("Description"), "https://example.com/icon.png", "video_1"));

        Assert.Equal("Alt", block.AltText);
        Assert.Equal("Title", block.Title.Text);
        Assert.Equal("https://example.com/thumb.png", block.ThumbnailUrl);
        Assert.Equal("https://example.com/video", block.VideoUrl);
        Assert.Equal("https://example.com/title", block.TitleUrl);
        Assert.Equal("Provider", block.ProviderName);
        Assert.Equal("Author", block.AuthorName);
        Assert.Equal("Description", block.Description!.Text);
        Assert.Equal("https://example.com/icon.png", block.ProviderIconUrl);
        Assert.Equal("video_1", block.BlockId);
    }
}
