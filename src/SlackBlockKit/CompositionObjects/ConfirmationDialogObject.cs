using System.Text.Json.Serialization;
using SlackBlockKit.CompositionObjects.TextObjects;

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
        set => field = ThrowIfNullOrExceedsLength(value, MaxTitleLength);
    }

    [JsonPropertyName("text")]
    public TextObject Text
    {
        get;
        set => field = ThrowIfNullOrExceedsLength(value, MaxTextLength);
    }

    [JsonPropertyName("confirm")]
    public PlainTextObject Confirm
    {
        get;
        set => field = ThrowIfNullOrExceedsLength(value, MaxButtonTextLength);
    }

    [JsonPropertyName("deny")]
    public PlainTextObject Deny
    {
        get;
        set => field = ThrowIfNullOrExceedsLength(value, MaxButtonTextLength);
    }

    [JsonPropertyName("style")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ConfirmationDialogStyle? Style { get; set; }
}