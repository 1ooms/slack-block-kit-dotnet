using System.Text.Json.Serialization;
using SlackBlockKit.CompositionObjects;
using SlackBlockKit.CompositionObjects.TextObjects;

namespace SlackBlockKit.Elements;

/// <summary>
/// Represents a URL input element that allows users to enter a URL into a single-line field.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/url-input-element">Slack URL Input Element Documentation</seealso>
public class UrlInputElement : ActionElement, IInputElement
{
    private const int MaxPlaceholderLength = 150;

    public UrlInputElement(
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

    public override string Type => "url_text_input";

    [JsonPropertyName("initial_value")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? InitialValue
    {
        get;
        set => field = ThrowIfNotValidUrl(value);
    }

    [JsonPropertyName("placeholder")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public PlainTextObject? Placeholder
    {
        get;
        set => field = ThrowIfExceedsLength(value, MaxPlaceholderLength);
    }

    [JsonPropertyName("dispatch_action_config")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DispatchActionConfigurationObject? DispatchActionConfig { get; set; }

    [JsonPropertyName("focus_on_load")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? FocusOnLoad { get; set; }
}