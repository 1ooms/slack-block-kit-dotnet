namespace SlackBlockKit.Tests.Elements;

public class FileCitationDetailsTests
{
    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var details = new FileCitationDetails("F1", "Page 3");

        Assert.Equal("F1", details.FileId);
        Assert.Equal("Page 3", details.Descriptor);
        Assert.Equal("file", details.CitationType);
    }

    [Fact]
    public void Serialize_AsCitationDetails_WritesDiscriminatorOnce()
    {
        var json = ToJson(new List<CitationDetails> { new FileCitationDetails("F1", "Page 3") })[0];

        AssertProperties(json, "citation_type", "file_id", "descriptor");
        Assert.Equal("file", json.GetProperty("citation_type").GetString());
    }

    [Fact]
    public void Serialize_OmitsDescriptorWhenNull()
    {
        AssertProperties(ToJson(new FileCitationDetails("F1")), "citation_type", "file_id");
    }

    [Fact]
    public void FileId_ThrowsWhenNull()
    {
        Assert.Throws<ArgumentNullException>(() => new FileCitationDetails(null!));
    }

    [Fact]
    public void Deserialize_AsCitationDetails_ReturnsFileCitation()
    {
        var details = SlackSerializer.Deserialize<CitationDetails>(
            """{"citation_type":"file","file_id":"F1","descriptor":"d"}""");

        var file = Assert.IsType<FileCitationDetails>(details);
        Assert.Equal("F1", file.FileId);
        Assert.Equal("d", file.Descriptor);
    }
}
