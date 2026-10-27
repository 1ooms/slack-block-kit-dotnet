using System.Text.Json.Serialization;
using SlackBlockKit.Elements.RichText;

namespace SlackBlockKit.Elements;

/// <summary>
/// Represents a workflow mention element that renders as a link to a workflow.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/workflow-mention-element">Slack Workflow Mention Element Documentation</seealso>
public class WorkflowMentionElement : Element, IRichTextSubElement
{
    public WorkflowMentionElement(
        string workflowId,
        string functionTriggerId,
        string text,
        string? url = null,
        string? channelId = null,
        string? ts = null,
        RichTextStyle? style = null)
    {
        WorkflowId = workflowId;
        FunctionTriggerId = functionTriggerId;
        Text = text;
        Url = url;
        ChannelId = channelId;
        Ts = ts;
        Style = style;
    }

    [JsonPropertyName("workflow_id")]
    public string WorkflowId
    {
        get;
        set => field = ThrowIfNullOrEmpty(value);
    }

    [JsonPropertyName("function_trigger_id")]
    public string FunctionTriggerId
    {
        get;
        set => field = ThrowIfNullOrEmpty(value);
    }

    [JsonPropertyName("text")]
    public string Text
    {
        get;
        set => field = ThrowIfNullOrEmpty(value);
    }

    [JsonPropertyName("url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Url
    {
        get;
        set => field = ThrowIfEmpty(value);
    }

    [JsonPropertyName("channel_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ChannelId
    {
        get;
        set => field = ThrowIfEmpty(value);
    }

    [JsonPropertyName("ts")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Ts
    {
        get;
        set => field = ThrowIfEmpty(value);
    }

    [JsonPropertyName("style")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public RichTextStyle? Style { get; set; }

    public override string Type => "workflow_mention";
}