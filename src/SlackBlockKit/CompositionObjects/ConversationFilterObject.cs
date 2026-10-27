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
    public ConversationFilterObject(
        List<ConversationType>? include = null,
        bool? excludeExternalSharedChannels = null,
        bool? excludeBotUsers = null)
    {
        ValidateAtLeastOne(include, excludeExternalSharedChannels, excludeBotUsers);

        Include = include;
        ExcludeExternalSharedChannels = excludeExternalSharedChannels;
        ExcludeBotUsers = excludeBotUsers;
    }

    [JsonPropertyName("include")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<ConversationType>? Include
    {
        get;
        set
        {
            ValidateAtLeastOne(value, ExcludeExternalSharedChannels, ExcludeBotUsers);
            field = ThrowIfEmpty(value);
        }
    }

    [JsonPropertyName("exclude_external_shared_channels")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExcludeExternalSharedChannels
    {
        get;
        set
        {
            ValidateAtLeastOne(Include, value, ExcludeBotUsers);
            field = value;
        }
    }

    [JsonPropertyName("exclude_bot_users")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExcludeBotUsers
    {
        get;
        set
        {
            ValidateAtLeastOne(Include, ExcludeExternalSharedChannels, ExcludeBotUsers);
            field = value;
        }
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