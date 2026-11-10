namespace SlackBlockKit.Tests.Elements;

public class UrlInputElementTests
{
    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var dispatch = new DispatchActionConfigurationObject([TriggerActions.OnEnterPressed]);

        var element = new UrlInputElement("action", "https://example.com", dispatch, true, "URL");

        Assert.Equal("action", element.ActionId);
        Assert.Equal("https://example.com", element.InitialValue);
        Assert.Same(dispatch, element.DispatchActionConfig);
        Assert.True(element.FocusOnLoad);
        Assert.Equal("URL", element.Placeholder!.Text);
        Assert.Equal("url_text_input", element.Type);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new UrlInputElement("action", "https://example.com",
            new DispatchActionConfigurationObject([TriggerActions.OnEnterPressed]), true, "URL"));

        AssertProperties(json, "type", "action_id", "initial_value", "dispatch_action_config", "focus_on_load",
            "placeholder");
        Assert.Equal("url_text_input", json.GetProperty("type").GetString());
        Assert.Equal("https://example.com", json.GetProperty("initial_value").GetString());
    }

    [Fact]
    public void Serialize_OmitsNullFields()
    {
        AssertProperties(ToJson(new UrlInputElement()), "type");
    }

    [Theory]
    [InlineData("http://example.com")]
    [InlineData("https://example.com/path?query=1#hash")]
    [InlineData("https://sub.example.co.uk:8080/")]
    public void InitialValue_AcceptsHttpAndHttpsUrls(string url)
    {
        Assert.Equal(url, new UrlInputElement(initialValue: url).InitialValue);
    }

    [Theory]
    [InlineData("")]
    [InlineData("example.com")]
    [InlineData("ftp://example.com")]
    [InlineData("mailto:user@example.com")]
    [InlineData("/relative/path")]
    [InlineData("https://")]
    public void InitialValue_ThrowsOnInvalidUrls(string url)
    {
        Assert.Throws<ArgumentException>(() => new UrlInputElement(initialValue: url));
    }

    [Fact]
    public void Placeholder_ThrowsWhenTooLong()
    {
        Assert.NotNull(new UrlInputElement(placeholder: Chars(150)));
        Assert.Throws<ArgumentException>(() => new UrlInputElement(placeholder: Chars(151)));
    }
}
