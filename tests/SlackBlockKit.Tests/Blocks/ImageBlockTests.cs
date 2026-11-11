namespace SlackBlockKit.Tests.Blocks;

public class ImageBlockTests
{
    private const string Url = "https://example.com/image.png";

    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var block = new ImageBlock("alt", Url, null, new PlainTextObject("Title"), "image_1");

        Assert.Equal("alt", block.AltText);
        Assert.Equal(Url, block.ImageUrl);
        Assert.Null(block.SlackFile);
        Assert.Equal("Title", block.Title!.Text);
        Assert.Equal("image_1", block.BlockId);
        Assert.Equal("image", block.Type);
    }

    [Fact]
    public void Constructor_AcceptsSlackFile()
    {
        var file = SlackFileObject.FromId("F12345678");

        var block = new ImageBlock("alt", slackFile: file);

        Assert.Same(file, block.SlackFile);
        Assert.Null(block.ImageUrl);
    }

    [Fact]
    public void FromImageUrl_SetsUrlAltAndTitle()
    {
        var block = ImageBlock.FromImageUrl("alt", Url, "Title", "image_1");

        Assert.Equal(Url, block.ImageUrl);
        Assert.Equal("alt", block.AltText);
        Assert.Equal("Title", block.Title!.Text);
        Assert.Equal("image_1", block.BlockId);
        Assert.Null(block.SlackFile);
    }

    [Fact]
    public void FromImageUrl_LeavesTitleNullWhenOmitted()
    {
        Assert.Null(ImageBlock.FromImageUrl("alt", Url).Title);
    }

    [Fact]
    public void FromSlackFile_SetsSlackFileAltAndTitle()
    {
        var file = SlackFileObject.FromId("F12345678");

        var block = ImageBlock.FromSlackFile("alt", file, "Title", "image_1");

        Assert.Same(file, block.SlackFile);
        Assert.Null(block.ImageUrl);
        Assert.Equal("Title", block.Title!.Text);
        Assert.Equal("image_1", block.BlockId);
    }

    [Fact]
    public void FromSlackFile_LeavesTitleNullWhenOmitted()
    {
        Assert.Null(ImageBlock.FromSlackFile("alt", SlackFileObject.FromId("F12345678")).Title);
    }

    [Fact]
    public void Serialize_WithImageUrl_WritesAllFields()
    {
        var json = ToJson(new ImageBlock("alt", Url, null, new PlainTextObject("Title"), "image_1"));

        AssertProperties(json, "type", "alt_text", "image_url", "title", "block_id");
        Assert.Equal("image", json.GetProperty("type").GetString());
        Assert.Equal("plain_text", json.GetProperty("title").GetProperty("type").GetString());
    }

    [Fact]
    public void Serialize_WithSlackFile_WritesSlackFile()
    {
        var json = ToJson(new ImageBlock("alt", slackFile: SlackFileObject.FromUrl(Url)));

        AssertProperties(json, "type", "alt_text", "slack_file");
        Assert.Equal(Url, json.GetProperty("slack_file").GetProperty("url").GetString());
    }

    [Fact]
    public void Constructor_ThrowsWithoutImageSource()
    {
        Assert.Throws<ArgumentException>(() => new ImageBlock("alt", (string?)null));
    }

    [Fact]
    public void AltText_ThrowsWhenNullOrTooLong()
    {
        Assert.Throws<ArgumentNullException>(() => new ImageBlock(null!, Url));
        Assert.NotNull(new ImageBlock(Chars(2000), Url));
        Assert.Throws<ArgumentException>(() => new ImageBlock(Chars(2001), Url));
    }

    [Fact]
    public void ImageUrl_ThrowsWhenEmptyWhitespaceOrTooLong()
    {
        Assert.Throws<ArgumentException>(() => new ImageBlock("alt", "", null, null, null));
        Assert.Throws<ArgumentException>(() => new ImageBlock("alt", " ", null, null, null));
        Assert.Throws<ArgumentException>(() =>
            new ImageBlock("alt", "   ", SlackFileObject.FromId("F0123ABCD"), null, null));
        Assert.NotNull(new ImageBlock("alt", Chars(3000), null, null, null));
        Assert.Throws<ArgumentException>(() => new ImageBlock("alt", Chars(3001), null, null, null));
    }

    [Fact]
    public void Title_ThrowsWhenTooLong()
    {
        Assert.NotNull(new ImageBlock("alt", Url, null, new PlainTextObject(Chars(2000))));
        Assert.Throws<ArgumentException>(() => new ImageBlock("alt", Url, null, new PlainTextObject(Chars(2001))));
    }

    [Fact]
    public void Constructor_AcceptsBothImageSources()
    {
        var block = new ImageBlock("alt", Url, SlackFileObject.FromId("F0123ABCD"));

        Assert.Equal(Url, block.ImageUrl);
        Assert.NotNull(block.SlackFile);
    }

    [Fact]
    public void Initializer_ThrowsWhenNoImageSourceRemains()
    {
        Assert.Throws<ArgumentException>(() => new ImageBlock("alt", Url) { ImageUrl = null });
        Assert.Throws<ArgumentException>(() =>
            new ImageBlock("alt", slackFile: SlackFileObject.FromId("F0123ABCD")) { SlackFile = null });
    }

    [Fact]
    public void Setters_AllowClearingOneImageSourceWhenTheOtherRemains()
    {
        var block = ImageBlock.FromImageUrl("alt", Url);

        Assert.Throws<ArgumentException>(() => block.ImageUrl = null);

        block.SlackFile = SlackFileObject.FromId("F0123ABCD");
        block.ImageUrl = null;

        Assert.Null(block.ImageUrl);
        Assert.NotNull(block.SlackFile);
        Assert.Throws<ArgumentException>(() => block.SlackFile = null);
    }

    [Fact]
    public void Deserialize_RoundTripsEitherImageSource()
    {
        var withUrl = SlackSerializer.Serialize(ImageBlock.FromImageUrl("alt", Url));
        var withFile = SlackSerializer.Serialize(ImageBlock.FromSlackFile("alt", SlackFileObject.FromId("F0123ABCD")));

        Assert.Equal(Url, SlackSerializer.Deserialize<ImageBlock>(withUrl)!.ImageUrl);
        Assert.Equal("F0123ABCD", SlackSerializer.Deserialize<ImageBlock>(withFile)!.SlackFile!.Id);
    }
}