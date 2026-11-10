namespace SlackBlockKit.Tests.CompositionObjects;

public class PlainTextOptionObjectTests
{
    [Fact]
    public void StringConstructor_WrapsTextAndDescription()
    {
        var option = new PlainTextOptionObject("Label", "value", "Description");

        Assert.Equal("Label", option.Text.Text);
        Assert.Equal("value", option.Value);
        Assert.Equal("Description", option.Description!.Text);
    }

    [Fact]
    public void StringConstructor_LeavesDescriptionNullWhenOmitted()
    {
        Assert.Null(new PlainTextOptionObject("Label", "value").Description);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new PlainTextOptionObject("Label", "value", "Description"));

        AssertProperties(json, "text", "value", "description");
        Assert.Equal("plain_text", json.GetProperty("text").GetProperty("type").GetString());
        Assert.Equal("Label", TextOf(json, "text"));
        Assert.Equal("value", json.GetProperty("value").GetString());
        Assert.Equal("plain_text", json.GetProperty("description").GetProperty("type").GetString());
        Assert.Equal("Description", TextOf(json, "description"));
    }

    [Fact]
    public void Serialize_OmitsOptionalFields()
    {
        AssertProperties(ToJson(new PlainTextOptionObject("Label", "value")), "text", "value");
    }

    [Fact]
    public void Text_ThrowsWhenNull()
    {
        Assert.Throws<ArgumentNullException>(() => new PlainTextOptionObject((PlainTextObject)null!, "value"));
    }

    [Fact]
    public void Text_ThrowsWhenTooLong()
    {
        Assert.NotNull(new PlainTextOptionObject(Chars(75), "value"));
        Assert.Throws<ArgumentException>(() => new PlainTextOptionObject(Chars(76), "value"));
    }

    [Fact]
    public void Value_ThrowsWhenNullEmptyOrTooLong()
    {
        Assert.Throws<ArgumentNullException>(() => new PlainTextOptionObject("Label", null!));
        Assert.Throws<ArgumentException>(() => new PlainTextOptionObject("Label", ""));
        Assert.NotNull(new PlainTextOptionObject("Label", Chars(75)));
        Assert.Throws<ArgumentException>(() => new PlainTextOptionObject("Label", Chars(76)));
    }

    [Fact]
    public void Description_ThrowsWhenTooLong()
    {
        Assert.NotNull(new PlainTextOptionObject("Label", "value", Chars(75)));
        Assert.Throws<ArgumentException>(() => new PlainTextOptionObject("Label", "value", Chars(76)));
    }

    [Fact]
    public void RoundTrip_PreservesValues()
    {
        var option = RoundTrip(new PlainTextOptionObject("Text", "value", "Description"));

        Assert.Equal("Text", option.Text.Text);
        Assert.Equal("value", option.Value);
        Assert.Equal("Description", option.Description!.Text);
    }
}
