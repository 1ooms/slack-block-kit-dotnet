namespace SlackBlockKit.Tests.CompositionObjects;

public class TriggerObjectTests
{
    private const string Url = "https://slack.com/shortcuts/Ft0123/abc";

    [Fact]
    public void Constructor_SetsProperties()
    {
        var trigger = new TriggerObject(Url, [new CustomizableInputParameter("name", "value")]);

        Assert.Equal(Url, trigger.Url);
        Assert.Single(trigger.CustomizableInputParameters!);
    }

    [Fact]
    public void DictionaryConstructor_ConvertsToParameters()
    {
        var trigger = new TriggerObject(Url, new Dictionary<string, string> { ["a"] = "1", ["b"] = "2" });

        Assert.Equal(["a", "b"], trigger.CustomizableInputParameters!.Select(p => p.Name));
        Assert.Equal(["1", "2"], trigger.CustomizableInputParameters!.Select(p => p.Value));
    }

    [Fact]
    public void Serialize_WritesUrlAndParameters()
    {
        var json = ToJson(new TriggerObject(Url, [new CustomizableInputParameter("name", "value")]));

        AssertProperties(json, "url", "customizable_input_parameters");
        Assert.Equal(Url, json.GetProperty("url").GetString());
        var parameter = json.GetProperty("customizable_input_parameters")[0];
        Assert.Equal("name", parameter.GetProperty("name").GetString());
        Assert.Equal("value", parameter.GetProperty("value").GetString());
    }

    [Fact]
    public void Serialize_OmitsParametersWhenNull()
    {
        AssertProperties(ToJson(new TriggerObject(Url)), "url");
    }

    [Fact]
    public void Url_ThrowsWhenNullOrEmpty()
    {
        Assert.Throws<ArgumentNullException>(() => new TriggerObject(null!));
        Assert.Throws<ArgumentException>(() => new TriggerObject(""));
    }

    [Fact]
    public void Parameters_ThrowsWhenEmpty()
    {
        Assert.Throws<ArgumentException>(() => new TriggerObject(Url, new List<CustomizableInputParameter>()));
        Assert.Throws<ArgumentException>(() => new TriggerObject(Url, new Dictionary<string, string>()));
    }

    [Fact]
    public void Parameters_AllowsUpTo10()
    {
        var parameters = Enumerable.Range(0, 10).Select(i => new CustomizableInputParameter($"p{i}", "v")).ToList();

        Assert.Equal(10, new TriggerObject(Url, parameters).CustomizableInputParameters!.Count);

        parameters.Add(new CustomizableInputParameter("extra", "v"));
        Assert.Throws<ArgumentException>(() => new TriggerObject(Url, parameters));
    }

    [Fact]
    public void RoundTrip_PreservesValues()
    {
        var trigger = RoundTrip(new TriggerObject("https://example.com/trigger",
            new Dictionary<string, string> { ["name"] = "value" }));

        Assert.Equal("https://example.com/trigger", trigger.Url);
        var parameter = Assert.Single(trigger.CustomizableInputParameters!);
        Assert.Equal("name", parameter.Name);
        Assert.Equal("value", parameter.Value);
    }
}
