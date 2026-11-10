namespace SlackBlockKit.Tests.Elements;

public class WorkObjectMentionElementTests
{
    private const string Url = "https://example.com/work/1";

    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var style = new RichTextStyle(bold: true);

        var element = new WorkObjectMentionElement("E1", "A1", "Ticket", Url, "https://example.com/icon.png", true,
            "Tracker", style);

        Assert.Equal("E1", element.EntityId);
        Assert.Equal("A1", element.AppId);
        Assert.Equal("Ticket", element.Text);
        Assert.Equal(Url, element.Url);
        Assert.Equal("https://example.com/icon.png", element.IconUrl);
        Assert.True(element.FullSizePreviewEnabled);
        Assert.Equal("Tracker", element.ProductName);
        Assert.Same(style, element.Style);
        Assert.Equal("work_object_mention", element.Type);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new WorkObjectMentionElement("E1", "A1", "Ticket", Url, "https://example.com/icon.png",
            true, "Tracker", new RichTextStyle(bold: true)));

        AssertProperties(json, "type", "entity_id", "app_id", "text", "url", "icon_url",
            "full_size_preview_enabled", "product_name", "style");
        Assert.Equal("work_object_mention", json.GetProperty("type").GetString());
    }

    [Fact]
    public void Serialize_OmitsNullFields()
    {
        AssertProperties(ToJson(new WorkObjectMentionElement("E1", "A1", "Ticket", Url)), "type", "entity_id",
            "app_id", "text", "url");
    }

    [Fact]
    public void RequiredStrings_ThrowWhenNullOrEmpty()
    {
        Assert.Throws<ArgumentNullException>(() => new WorkObjectMentionElement(null!, "A1", "Ticket", Url));
        Assert.Throws<ArgumentException>(() => new WorkObjectMentionElement("", "A1", "Ticket", Url));
        Assert.Throws<ArgumentException>(() => new WorkObjectMentionElement("E1", "", "Ticket", Url));
        Assert.Throws<ArgumentException>(() => new WorkObjectMentionElement("E1", "A1", "", Url));
    }

    [Fact]
    public void Url_ThrowsWhenNullOrInvalid()
    {
        Assert.Throws<ArgumentNullException>(() => new WorkObjectMentionElement("E1", "A1", "Ticket", null!));
        Assert.Throws<ArgumentException>(() => new WorkObjectMentionElement("E1", "A1", "Ticket", "not a url"));
        Assert.Throws<ArgumentException>(() => new WorkObjectMentionElement("E1", "A1", "Ticket", "ftp://x.com"));
    }

    [Fact]
    public void OptionalStrings_ThrowWhenEmpty()
    {
        Assert.Throws<ArgumentException>(() => new WorkObjectMentionElement("E1", "A1", "Ticket", Url, ""));
        Assert.Throws<ArgumentException>(() =>
            new WorkObjectMentionElement("E1", "A1", "Ticket", Url, productName: ""));
    }
}
