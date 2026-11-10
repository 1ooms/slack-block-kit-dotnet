namespace SlackBlockKit.Tests.Elements;

public class AttachmentMentionElementTests
{
    private const string Url = "https://example.com/item";

    private static AttachmentMentionElement CreateFull() =>
        new(Url, "Item", "A123", "E123", "https://example.com/icon.png", "C123", "123.456", true, "file",
            "document", "Product", new RichTextStyle(bold: true));

    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var element = CreateFull();

        Assert.Equal(Url, element.Url);
        Assert.Equal("Item", element.Text);
        Assert.Equal("A123", element.AppId);
        Assert.Equal("E123", element.EntityId);
        Assert.Equal("https://example.com/icon.png", element.IconUrl);
        Assert.Equal("C123", element.ChannelId);
        Assert.Equal("123.456", element.Ts);
        Assert.True(element.FullSizePreviewEnabled);
        Assert.Equal("file", element.IconName);
        Assert.Equal("document", element.ReferenceObjectType);
        Assert.Equal("Product", element.ProductName);
        Assert.True(element.Style!.Bold);
        Assert.Equal("attachment_mention", element.Type);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(CreateFull());

        AssertProperties(json, "type", "url", "text", "app_id", "entity_id", "icon_url", "channel_id", "ts",
            "full_size_preview_enabled", "icon_name", "reference_object_type", "product_name", "style");
        Assert.Equal("attachment_mention", json.GetProperty("type").GetString());
        Assert.True(json.GetProperty("style").GetProperty("bold").GetBoolean());
    }

    [Fact]
    public void Serialize_OmitsNullFields()
    {
        AssertProperties(ToJson(new AttachmentMentionElement(Url)), "type", "url");
    }

    [Fact]
    public void Url_ThrowsWhenNullOrEmpty()
    {
        Assert.Throws<ArgumentNullException>(() => new AttachmentMentionElement(null!));
        Assert.Throws<ArgumentException>(() => new AttachmentMentionElement(""));
    }

    [Fact]
    public void OptionalStrings_ThrowWhenEmpty()
    {
        var element = new AttachmentMentionElement(Url);

        Assert.Throws<ArgumentException>(() => element.Text = "");
        Assert.Throws<ArgumentException>(() => element.AppId = "");
        Assert.Throws<ArgumentException>(() => element.EntityId = "");
        Assert.Throws<ArgumentException>(() => element.IconUrl = "");
        Assert.Throws<ArgumentException>(() => element.ChannelId = "");
        Assert.Throws<ArgumentException>(() => element.Ts = "");
        Assert.Throws<ArgumentException>(() => element.IconName = "");
        Assert.Throws<ArgumentException>(() => element.ReferenceObjectType = "");
        Assert.Throws<ArgumentException>(() => element.ProductName = "");
    }
}
