using System.Text.Json.Serialization;
using SlackBlockKit.CompositionObjects.TextObjects;
using SlackBlockKit.Elements;

namespace SlackBlockKit.Blocks;

/// <summary>
/// Represents an input block (<c>type: "input"</c>) that collects user input with a label, hint, and an interactive input element.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/blocks/input-block">Slack Input Block Documentation</seealso>
public class InputBlock : Block
{
    private const int MaxLabelLength = 2000;
    private const int MaxHintLength = 2000;

    [JsonConstructor]
    public InputBlock(
        PlainTextObject label,
        IInputElement element,
        string? blockId = null,
        bool? dispatchAction = null,
        PlainTextObject? hint = null,
        bool? optional = null)
    {
        Label = label;
        Element = element;
        BlockId = blockId;
        DispatchAction = dispatchAction;
        Hint = hint;
        Optional = optional;
    }

    public InputBlock(
        string label,
        IInputElement element,
        string? blockId = null,
        bool? dispatchAction = null,
        string? hint = null,
        bool? optional = null)
        : this(
            new PlainTextObject(label),
            element,
            blockId,
            dispatchAction,
            hint != null ? new PlainTextObject(hint) : null,
            optional)
    {
    }

    public override string Type => "input";

    [JsonPropertyName("label")]
    public PlainTextObject Label
    {
        get;
        set => field = ThrowIfNullOrExceedsLength(value, MaxLabelLength);
    }

    [JsonPropertyName("element")]
    public IInputElement Element
    {
        get;
        set => field = ThrowIfNull(value);
    }

    [JsonPropertyName("dispatch_action")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? DispatchAction { get; set; }

    [JsonPropertyName("hint")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public PlainTextObject? Hint
    {
        get;
        set => field = ThrowIfExceedsLength(value, MaxHintLength);
    }

    [JsonPropertyName("optional")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Optional { get; set; }
}