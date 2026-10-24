using System.Text.Json.Serialization;

namespace SlackBlockKit.CompositionObjects;

/// <summary>
/// Specifies the type of conversation to include in a conversation filter.
/// </summary>
public enum ConversationType
{
    Im,
    Mpim,
    Private,
    Public
}

/// <summary>
/// Provides a filter for conversation lists in a conversations select menu.
/// <para>At least one filter property must be supplied.</para>
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/composition-objects/conversation-filter">Slack Conversation Filter Documentation</seealso>
public class ConversationFilterObject : CompositionObject
{
    private List<ConversationType>? _include;
    private bool? _excludeExternalSharedChannels;
    private bool? _excludeBotUsers;

    public ConversationFilterObject(
        List<ConversationType>? include = null,
        bool? excludeExternalSharedChannels = null,
        bool? excludeBotUsers = null)
    {
        ValidateAtLeastOne(include, excludeExternalSharedChannels, excludeBotUsers);
        ValidateInclude(include);

        _include = include;
        _excludeExternalSharedChannels = excludeExternalSharedChannels;
        _excludeBotUsers = excludeBotUsers;
    }

    [JsonPropertyName("include")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<ConversationType>? Include
    {
        get => _include;
        set
        {
            ValidateInclude(value);
            _include = value;
            ValidateAtLeastOne(_include, _excludeExternalSharedChannels, _excludeBotUsers);
        }
    }

    [JsonPropertyName("exclude_external_shared_channels")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExcludeExternalSharedChannels
    {
        get => _excludeExternalSharedChannels;
        set
        {
            _excludeExternalSharedChannels = value;
            ValidateAtLeastOne(_include, _excludeExternalSharedChannels, _excludeBotUsers);
        }
    }

    [JsonPropertyName("exclude_bot_users")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExcludeBotUsers
    {
        get => _excludeBotUsers;
        set
        {
            _excludeBotUsers = value;
            ValidateAtLeastOne(_include, _excludeExternalSharedChannels, _excludeBotUsers);
        }
    }

    private static void ValidateInclude(List<ConversationType>? include)
    {
        if (include != null && include.Count == 0)
            throw new ArgumentException("The 'include' list cannot be empty when provided.", nameof(include));
    }

    private static void ValidateAtLeastOne(List<ConversationType>? include, bool? excludeExternalSharedChannels,
        bool? excludeBotUsers)
    {
        if (include == null && excludeExternalSharedChannels == null && excludeBotUsers == null)
        {
            throw new ArgumentException(
                "At least one filter property ('include', 'excludeExternalSharedChannels', or 'excludeBotUsers') must be provided.");
        }
    }
}
