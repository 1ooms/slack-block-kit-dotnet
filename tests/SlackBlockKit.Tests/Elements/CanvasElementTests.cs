namespace SlackBlockKit.Tests.Elements;

public class CanvasElementTests
{
    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var style = new RichTextStyle(bold: true);

        var element = new CanvasElement("F123", "Label", true, "S1", "Text", "https://example.com", false, style);

        Assert.Equal("F123", element.FileId);
        Assert.Equal("Label", element.Label);
        Assert.True(element.HideTitle);
        Assert.Equal("S1", element.SectionId);
        Assert.Equal("Text", element.Text);
        Assert.Equal("https://example.com", element.Url);
        Assert.False(element.IsSkillInvocation);
        Assert.Same(style, element.Style);
        Assert.Equal("canvas", element.Type);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new CanvasElement("F123", "Label", true, "S1", "Text", "https://example.com", false,
            new RichTextStyle(bold: true)));

        AssertProperties(json, "type", "file_id", "label", "hide_title", "section_id", "text", "url",
            "is_skill_invocation", "style");
        Assert.Equal("canvas", json.GetProperty("type").GetString());
        Assert.Equal("F123", json.GetProperty("file_id").GetString());
    }

    [Fact]
    public void Serialize_OmitsNullFields()
    {
        AssertProperties(ToJson(new CanvasElement("F123")), "type", "file_id");
    }

    [Fact]
    public void FileId_ThrowsWhenNullOrEmpty()
    {
        Assert.Throws<ArgumentNullException>(() => new CanvasElement(null!));
        Assert.Throws<ArgumentException>(() => new CanvasElement(""));
    }

    [Fact]
    public void OptionalStrings_ThrowWhenEmpty()
    {
        Assert.Throws<ArgumentException>(() => new CanvasElement("F123", sectionId: ""));
        Assert.Throws<ArgumentException>(() => new CanvasElement("F123", text: ""));
        Assert.Throws<ArgumentException>(() => new CanvasElement("F123", url: ""));
    }
}
