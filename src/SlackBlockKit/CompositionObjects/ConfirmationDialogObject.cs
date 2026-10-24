using System.Text.Json.Serialization;
using SlackBlockKit.TextObjects;

namespace SlackBlockKit.CompositionObjects;

/// <summary>
/// Specifies the button style for a confirmation dialog.
/// </summary>
public enum ConfirmationDialogStyle
{
    Primary,
    Danger
}

/// <summary>
/// Represents a confirmation dialog composition object that defines an alert dialog displayed before an action is taken.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/composition-objects/confirmation-dialog">Slack Confirmation Dialog Documentation</seealso>
public class ConfirmationDialogObject : CompositionObject
{
    private const int MaxTitleLength = 100;
    private const int MaxTextLength = 300;
    private const int MaxButtonTextLength = 30;

    public ConfirmationDialogObject(
        PlainTextObject title,
        TextObject text,
        PlainTextObject confirm,
        PlainTextObject deny,
        ConfirmationDialogStyle? style = null)
    {
        Title = title;
        Text = text;
        Confirm = confirm;
        Deny = deny;
        Style = style;
    }

    public ConfirmationDialogObject(
        string title,
        string text,
        string confirm,
        string deny,
        ConfirmationDialogStyle? style = null)
        : this(new PlainTextObject(title), new PlainTextObject(text), new PlainTextObject(confirm),
            new PlainTextObject(deny), style)
    {
    }

    [JsonPropertyName("title")]
    public PlainTextObject Title
    {
        get;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            if (value.Text.Length > MaxTitleLength)
                throw new ArgumentException($"Title cannot exceed {MaxTitleLength} characters.", nameof(value));
            field = value;
        }
    }

    [JsonPropertyName("text")]
    public TextObject Text
    {
        get;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            if (value.Text.Length > MaxTextLength)
                throw new ArgumentException($"Text cannot exceed {MaxTextLength} characters.", nameof(value));
            field = value;
        }
    }

    [JsonPropertyName("confirm")]
    public PlainTextObject Confirm
    {
        get;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            if (value.Text.Length > MaxButtonTextLength)
                throw new ArgumentException($"Confirm text cannot exceed {MaxButtonTextLength} characters.",
                    nameof(value));
            field = value;
        }
    }

    [JsonPropertyName("deny")]
    public PlainTextObject Deny
    {
        get;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            if (value.Text.Length > MaxButtonTextLength)
                throw new ArgumentException($"Deny text cannot exceed {MaxButtonTextLength} characters.",
                    nameof(value));
            field = value;
        }
    }

    [JsonPropertyName("style")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ConfirmationDialogStyle? Style { get; set; }
}
