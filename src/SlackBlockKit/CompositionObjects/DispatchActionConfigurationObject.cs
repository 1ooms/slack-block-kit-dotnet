using System.Text.Json.Serialization;

namespace SlackBlockKit.CompositionObjects;

/// <summary>
/// Specifies the trigger actions for a Dispatch Action.
/// </summary>
public enum TriggerActions
{
    OnEnterPressed,
    OnCharacterEntered
}

/// <summary>
/// Determines when a plain text input element returns its value to your app backend.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/composition-objects/dispatch-action-configuration">Slack Dispatch Action Configuration Documentation</seealso>
public class DispatchActionConfigurationObject : CompositionObject
{
    public DispatchActionConfigurationObject(List<TriggerActions> triggerActionsOn)
    {
        TriggerActionsOn = triggerActionsOn;
    }

    [JsonPropertyName("trigger_actions_on")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<TriggerActions> TriggerActionsOn
    {
        get;
        set => field = ThrowIfNullOrEmpty(value);
    }
}
