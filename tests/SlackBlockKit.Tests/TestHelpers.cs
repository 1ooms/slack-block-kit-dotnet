namespace SlackBlockKit.Tests;

/// <summary>
/// Shared helpers for asserting on the JSON produced by <see cref="SlackSerializer"/>.
/// </summary>
public static class TestHelpers
{
    /// <summary>Serializes <paramref name="value"/> with <see cref="SlackSerializer"/> and returns the JSON root.</summary>
    public static JsonElement ToJson(object value)
    {
        using var doc = JsonDocument.Parse(SlackSerializer.Serialize(value));
        return doc.RootElement.Clone();
    }

    /// <summary>Serializes <paramref name="value"/> and deserializes it back as <typeparamref name="T"/>.</summary>
    public static T RoundTrip<T>(T value) where T : class
    {
        return SlackSerializer.Deserialize<T>(SlackSerializer.Serialize(value))!;
    }

    /// <summary>Creates a string of <paramref name="length"/> characters.</summary>
    public static string Chars(int length)
    {
        return new string('a', length);
    }

    /// <summary>Asserts that the JSON object has no property named <paramref name="name"/>.</summary>
    public static void AssertNoProperty(JsonElement json, string name)
    {
        Assert.False(json.TryGetProperty(name, out _), $"Expected no '{name}' property in {json}");
    }

    /// <summary>Asserts that the JSON object contains exactly the given property names.</summary>
    public static void AssertProperties(JsonElement json, params string[] names)
    {
        var actual = json.EnumerateObject().Select(p => p.Name).OrderBy(n => n).ToArray();
        Assert.Equal(names.OrderBy(n => n).ToArray(), actual);
    }

    /// <summary>Returns <c>json[name].text</c>, the text of a nested text object.</summary>
    public static string? TextOf(JsonElement json, string name)
    {
        return json.GetProperty(name).GetProperty("text").GetString();
    }

    public static OptionObject Option(string value = "value") => new($"Text {value}", value);

    public static ConfirmationDialogObject Confirm() => new("Title", "Text", "Yes", "No");

    public static ImageElement Image() => ImageElement.FromImageUrl("alt", "https://example.com/image.png");

    public static RichTextBlock RichTextBlockOf(string text = "text") =>
        new([new RichTextSectionElement(new TextElement(text))]);
}
