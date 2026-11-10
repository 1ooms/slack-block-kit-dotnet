namespace SlackBlockKit.Tests.Elements;

public class FileInputElementTests
{
    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var element = new FileInputElement("action", ["pdf", "png"], 5);

        Assert.Equal("action", element.ActionId);
        Assert.Equal(["pdf", "png"], element.Filetypes);
        Assert.Equal(5, element.MaxFiles);
        Assert.Equal("file_input", element.Type);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new FileInputElement("action", ["pdf"], 3));

        AssertProperties(json, "type", "action_id", "filetypes", "max_files");
        Assert.Equal("file_input", json.GetProperty("type").GetString());
        Assert.Equal("pdf", json.GetProperty("filetypes")[0].GetString());
        Assert.Equal(3, json.GetProperty("max_files").GetInt32());
    }

    [Fact]
    public void Serialize_OmitsNullFields()
    {
        AssertProperties(ToJson(new FileInputElement()), "type");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    public void MaxFiles_AcceptsValuesBetween1And10(int maxFiles)
    {
        Assert.Equal(maxFiles, new FileInputElement(maxFiles: maxFiles).MaxFiles);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(11)]
    public void MaxFiles_ThrowsWhenOutOfRange(int maxFiles)
    {
        Assert.Throws<ArgumentException>(() => new FileInputElement(maxFiles: maxFiles));
    }
}
