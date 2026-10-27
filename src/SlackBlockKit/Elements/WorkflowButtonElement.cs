using System.Text.Json.Serialization;
using SlackBlockKit.CompositionObjects;
using SlackBlockKit.CompositionObjects.TextObjects;

namespace SlackBlockKit.Elements;

/// <summary>
/// Represents a workflow button element that allows users to run a link trigger with customizable inputs.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/workflow-button-element">Slack Workflow Button Element Documentation</seealso>
public class WorkflowButtonElement : ActionElement, IActionsElement, ISectionElement
{
    private const int MaxTextLength = 75;
    private const int MaxAccessibilityLabelLength = 75;

    public WorkflowButtonElement(
        PlainTextObject text,
        WorkflowObject workflow,
        string actionId,
        ButtonElement.ButtonStyle? style = null,
        string? accessibilityLabel = null)
    {
        Text = text;
        Workflow = workflow;
        ActionId = actionId;
        Style = style;
        AccessibilityLabel = accessibilityLabel;
    }

    public WorkflowButtonElement(
        string text,
        string actionId,
        string triggerUrl,
        ButtonElement.ButtonStyle? style = null)
        : this(new PlainTextObject(text), new WorkflowObject(triggerUrl), actionId, style)
    {
    }

    public override string Type => "workflow_button";

    [JsonPropertyName("text")]
    public PlainTextObject Text
    {
        get;
        set => field = ThrowIfNullOrExceedsLength(value, MaxTextLength);
    }

    [JsonPropertyName("workflow")]
    public WorkflowObject Workflow
    {
        get;
        set => field = ThrowIfNull(value);
    }

    [JsonPropertyName("style")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ButtonElement.ButtonStyle? Style { get; set; }

    [JsonPropertyName("accessibility_label")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AccessibilityLabel
    {
        get;
        set => field = ThrowIfExceedsLength(value, MaxAccessibilityLabelLength);
    }
}