using System.Text.Json.Serialization;
using SlackBlockKit.Elements;
using SlackBlockKit.TextObjects;

namespace SlackBlockKit.Blocks;

public class InputBlock : Block
{
    private const int MaxLabelLength = 2000;
    private const int MaxHintLength = 2000;

    public InputBlock(PlainTextObject label, IInputElement element, string blockId, bool? dispatchAction = null,
        PlainTextObject? hint = null, bool? optional = null)
    {
        Label = label;
        Element = element;
        BlockId = blockId;
        DispatchAction = dispatchAction;
        Hint = hint;
        Optional = optional;
    }

    public override string Type => "input";

    [JsonPropertyName("label")]
    public PlainTextObject Label
    {
        get;
        set
        {
            if (value.Text.Length > MaxLabelLength)
                throw new ArgumentException($"Label text cannot be longer than {MaxLabelLength} characters.",
                    nameof(Label));

            field = value;
        }
    }

    [JsonPropertyName("element")] public IInputElement Element { get; set; }

    [JsonPropertyName("dispatch_action")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? DispatchAction { get; set; }

    [JsonPropertyName("block_id")] public new string BlockId { get; set; }

    [JsonPropertyName("hint")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public PlainTextObject? Hint
    {
        get;
        set
        {
            if (value != null && value.Text.Length > MaxHintLength)
                throw new ArgumentException($"Hint text cannot be longer than {MaxHintLength} characters.",
                    nameof(Hint));

            field = value;
        }
    }

    [JsonPropertyName("optional")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Optional { get; set; }
}