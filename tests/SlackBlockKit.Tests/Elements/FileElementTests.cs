namespace SlackBlockKit.Tests.Elements;

public class FileElementTests
{
    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var style = new RichTextStyle(bold: true);

        var element = new FileElement("F123", "Report", "https://example.com/file", true, style);

        Assert.Equal("F123", element.FileId);
        Assert.Equal("Report", element.Text);
        Assert.Equal("https://example.com/file", element.Url);
        Assert.True(element.IsSkillInvocation);
        Assert.Same(style, element.Style);
        Assert.Equal("file", element.Type);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new FileElement("F123", "Report", "https://example.com/file", true,
            new RichTextStyle(bold: true)));

        AssertProperties(json, "type", "file_id", "text", "url", "is_skill_invocation", "style");
        Assert.Equal("file", json.GetProperty("type").GetString());
    }

    [Fact]
    public void Serialize_OmitsNullFields()
    {
        AssertProperties(ToJson(new FileElement("F123")), "type", "file_id");
    }

    [Fact]
    public void FileId_ThrowsWhenNull()
    {
        Assert.Throws<ArgumentNullException>(() => new FileElement(null!));
    }

    [Fact]
    public void OptionalStrings_ThrowWhenEmpty()
    {
        Assert.Throws<ArgumentException>(() => new FileElement("F123", ""));
        Assert.Throws<ArgumentException>(() => new FileElement("F123", url: ""));
    }
}
