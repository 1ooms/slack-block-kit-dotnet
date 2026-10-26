using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using SlackBlockKit.CompositionObjects;
using SlackBlockKit.CompositionObjects.TextObjects;

namespace SlackBlockKit.Elements;

/// <summary>
/// Represents an email input element that allows users to enter an email address into a single-line field.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/email-input-element">Slack Email Input Element Documentation</seealso>
public partial class EmailInputElement : ActionElement, IInputElement
{
    private const int MaxPlaceholderLength = 150;

    public EmailInputElement(
        string? actionId = null,
        string? initialValue = null,
        DispatchActionConfigurationObject? dispatchActionConfig = null,
        bool? focusOnLoad = null,
        PlainTextObject? placeholder = null)
    {
        ActionId = actionId;
        InitialValue = initialValue;
        DispatchActionConfig = dispatchActionConfig;
        FocusOnLoad = focusOnLoad;
        Placeholder = placeholder;
    }

    public override string Type => "email_text_input";

    [JsonPropertyName("initial_value")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? InitialValue
    {
        get;
        set
        {
            if (value != null && !EmailRegex().IsMatch(value))
                throw new ArgumentException(
                    $@"Invalid Email '{value}'. Email must match regex pattern '^[\w-\.]+@([\w-]+\.)+[\w-]{{2,4}}$'.",
                    nameof(value));
            field = value;
        }
    }

    [JsonPropertyName("dispatch_action_config")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DispatchActionConfigurationObject? DispatchActionConfig { get; set; }

    [JsonPropertyName("focus_on_load")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? FocusOnLoad { get; set; }

    [JsonPropertyName("placeholder")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public PlainTextObject? Placeholder
    {
        get;
        set => field = ThrowIfExceedsLength(value, MaxPlaceholderLength);
    }

    [GeneratedRegex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$")]
    private static partial Regex EmailRegex();
}