using System.Text.Json.Serialization;

namespace SlackBlockKit.CompositionObjects;

/// <summary>
/// Defines an object containing workflow information for a workflow button.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/composition-objects/workflow">Slack Workflow Object Documentation</seealso>
public class WorkflowObject : CompositionObject
{
    public WorkflowObject(TriggerObject trigger)
    {
        Trigger = trigger;
    }
    
    public WorkflowObject(string triggerUrl, List<CustomizableInputParameter>? parameters = null)
        : this(new TriggerObject(triggerUrl, parameters))
    {
    }

    public WorkflowObject(string triggerUrl, Dictionary<string, string> parameters)
        : this(new TriggerObject(triggerUrl, parameters))
    {
    }

    [JsonPropertyName("trigger")]
    public TriggerObject Trigger
    {
        get;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            field = value;
        }
    }
}