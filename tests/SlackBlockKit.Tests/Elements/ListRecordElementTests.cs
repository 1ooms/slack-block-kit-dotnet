namespace SlackBlockKit.Tests.Elements;

public class ListRecordElementTests
{
    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var style = new RichTextStyle(bold: true);

        var element = new ListRecordElement("F123", "R1", "V1", "Record", "https://example.com", style);

        Assert.Equal("F123", element.FileId);
        Assert.Equal("R1", element.RecordId);
        Assert.Equal("V1", element.ViewId);
        Assert.Equal("Record", element.Text);
        Assert.Equal("https://example.com", element.Url);
        Assert.Same(style, element.Style);
        Assert.Equal("list_record", element.Type);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new ListRecordElement("F123", "R1", "V1", "Record", "https://example.com",
            new RichTextStyle(bold: true)));

        AssertProperties(json, "type", "file_id", "record_id", "view_id", "text", "url", "style");
        Assert.Equal("list_record", json.GetProperty("type").GetString());
    }

    [Fact]
    public void Serialize_OmitsNullFields()
    {
        AssertProperties(ToJson(new ListRecordElement("F123")), "type", "file_id");
    }

    [Fact]
    public void FileId_ThrowsWhenNullOrEmpty()
    {
        Assert.Throws<ArgumentNullException>(() => new ListRecordElement(null!));
        Assert.Throws<ArgumentException>(() => new ListRecordElement(""));
    }

    [Fact]
    public void OptionalStrings_ThrowWhenEmpty()
    {
        Assert.Throws<ArgumentException>(() => new ListRecordElement("F123", recordId: ""));
        Assert.Throws<ArgumentException>(() => new ListRecordElement("F123", viewId: ""));
        Assert.Throws<ArgumentException>(() => new ListRecordElement("F123", text: ""));
        Assert.Throws<ArgumentException>(() => new ListRecordElement("F123", url: ""));
    }
}
