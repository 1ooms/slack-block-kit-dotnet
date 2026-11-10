namespace SlackBlockKit.Tests.Elements;

public class ImageElementTests
{
    private const string Url = "https://example.com/image.png";

    [Fact]
    public void FromImageUrl_SetsImageUrl()
    {
        var image = ImageElement.FromImageUrl("alt", Url);

        Assert.Equal("alt", image.AltText);
        Assert.Equal(Url, image.ImageUrl);
        Assert.Null(image.SlackFile);
        Assert.Equal("image", image.Type);
    }

    [Fact]
    public void FromSlackFile_SetsSlackFile()
    {
        var file = SlackFileObject.FromId("F12345678");

        var image = ImageElement.FromSlackFile("alt", file);

        Assert.Same(file, image.SlackFile);
        Assert.Null(image.ImageUrl);
    }

    [Fact]
    public void Serialize_WithImageUrl()
    {
        var json = ToJson(ImageElement.FromImageUrl("alt", Url));

        AssertProperties(json, "type", "alt_text", "image_url");
        Assert.Equal("image", json.GetProperty("type").GetString());
        Assert.Equal("alt", json.GetProperty("alt_text").GetString());
        Assert.Equal(Url, json.GetProperty("image_url").GetString());
    }

    [Fact]
    public void Serialize_WithSlackFile()
    {
        var json = ToJson(ImageElement.FromSlackFile("alt", SlackFileObject.FromId("F12345678")));

        AssertProperties(json, "type", "alt_text", "slack_file");
        Assert.Equal("F12345678", json.GetProperty("slack_file").GetProperty("id").GetString());
    }

    [Fact]
    public void Constructor_ThrowsWithoutImageSource()
    {
        Assert.Throws<ArgumentException>(() => new ImageElement("alt"));
        Assert.Throws<ArgumentException>(() => new ImageElement("alt", " "));
    }

    [Fact]
    public void AltText_ThrowsWhenNull()
    {
        Assert.Throws<ArgumentNullException>(() => new ImageElement(null!, Url));
    }

    [Fact]
    public void ImageUrl_ThrowsWhenTooLong()
    {
        Assert.NotNull(new ImageElement("alt", Chars(3000)));
        Assert.Throws<ArgumentException>(() => new ImageElement("alt", Chars(3001)));
    }
}
