using System.Text.Json.Serialization;

namespace SlackBlockKit.CompositionObjects;

/// <summary>
/// Defines a trigger object used in workflow buttons to invoke Slack workflows.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/composition-objects/trigger">Slack Trigger Object Documentation</seealso>
public class TriggerObject : CompositionObject
{
    private const int MaxParameters = 10;

    public TriggerObject(string url, List<CustomizableInputParameter>? customizableInputParameters = null)
    {
        Url = url;
        CustomizableInputParameters = customizableInputParameters;
    }

    public TriggerObject(string url, Dictionary<string, string> parameters)
        : this(url, parameters?.Select(kv => new CustomizableInputParameter(kv.Key, kv.Value)).ToList())
    {
    }

    [JsonPropertyName("url")]
    public string Url 
    { 
        get;
        set
        { 
            ThrowIfNullOrEmpty(value);
            field = value;
        } 
    }

    [JsonPropertyName("customizable_input_parameters")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<CustomizableInputParameter>? CustomizableInputParameters
    {
        get;
        set
        {
            if (value != null)
            {
                ThrowIfEmpty(value);
                ThrowIfExceedsCount(value, MaxParameters);
            }
            field = value;
        }
    }
}

/// <summary>
/// Defines an input parameter passed to a customizable workflow trigger.
/// </summary>
public class CustomizableInputParameter
{
    public CustomizableInputParameter(string name, string value)
    {
        Name = name;
        Value = value;
    }

    [JsonPropertyName("name")]
    public string Name
    {
        get;
        set => field = ThrowIfNullOrEmpty(value);
    }

    [JsonPropertyName("value")]
    public string Value
    {
        get;
        set => field = ThrowIfNullOrEmpty(value);
    }
}