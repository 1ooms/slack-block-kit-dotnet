using System.Text.Json.Serialization;

namespace SlackBlockKit.TextObjects;

public class PlainTextObject : TextObject
{
    public PlainTextObject(string text, bool? emoji = true, bool? verbatim = false) : base(text)
    {
        Emoji = emoji;
        Verbatim = verbatim;
        ValidateText();
    }

    public override string Type => "plain_text";

    [JsonIgnore] public int MaxLength { get; set; } = 3000;

    [JsonPropertyName("emoji")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Emoji { get; set; }

    [JsonPropertyName("verbatim")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Verbatim { get; set; }

    public override string Text
    {
        get => base.Text;
        set
        {
            base.Text = value;
            ValidateText();
        }
    }

    private void ValidateText()
    {
        if (string.IsNullOrEmpty(Text)) throw new ArgumentException("Text cannot be null or empty.", nameof(Text));

        if (Text.Length > MaxLength)
            throw new ArgumentException($"Text cannot be longer than {MaxLength} characters.", nameof(Text));
    }
}