namespace SlackBlockKit.Tests.CompositionObjects;

public class CustomizableInputParameterTests
{
    [Fact]
    public void Serialize_WritesNameAndValue()
    {
        var json = ToJson(new CustomizableInputParameter("name", "value"));

        AssertProperties(json, "name", "value");
        Assert.Equal("name", json.GetProperty("name").GetString());
        Assert.Equal("value", json.GetProperty("value").GetString());
    }

    [Fact]
    public void Name_ThrowsWhenNullOrEmpty()
    {
        Assert.Throws<ArgumentNullException>(() => new CustomizableInputParameter(null!, "value"));
        Assert.Throws<ArgumentException>(() => new CustomizableInputParameter("", "value"));
    }

    [Fact]
    public void Value_ThrowsWhenNullOrEmpty()
    {
        Assert.Throws<ArgumentNullException>(() => new CustomizableInputParameter("name", null!));
        Assert.Throws<ArgumentException>(() => new CustomizableInputParameter("name", ""));
    }
}
