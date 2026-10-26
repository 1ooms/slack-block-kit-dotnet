using System.Text.Json.Serialization;

namespace SlackBlockKit.Elements;

/// <summary>
/// Base class for AI citation details objects used in citation elements.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/citation-element">Slack Citation Element Documentation</seealso>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "citation_type")]
[JsonDerivedType(typeof(FileCitationDetails), "file")]
[JsonDerivedType(typeof(ExternalCitationDetails), "external")]
[JsonDerivedType(typeof(WebCitationDetails), "web")]
[JsonDerivedType(typeof(MessageCitationDetails), "message")]
[JsonDerivedType(typeof(MemoryCitationDetails), "memory")]
public abstract class CitationDetails
{
    [JsonIgnore] public abstract string CitationType { get; }
}

/// <summary>
/// Represents file citation details for an AI citation element.
/// </summary>
public class FileCitationDetails : CitationDetails
{
    public FileCitationDetails(string fileId, string? descriptor = null)
    {
        FileId = fileId;
        Descriptor = descriptor;
    }

    public override string CitationType => "file";

    [JsonPropertyName("file_id")]
    public string FileId
    {
        get;
        set => field = ThrowIfNull(value);
    }

    [JsonPropertyName("descriptor")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Descriptor { get; set; }
}

/// <summary>
/// Represents external application citation details for an AI citation element.
/// </summary>
public class ExternalCitationDetails : CitationDetails
{
    public ExternalCitationDetails(string appName, string appIconUrl)
    {
        AppName = appName;
        AppIconUrl = appIconUrl;
    }

    public override string CitationType => "external";

    [JsonPropertyName("app_name")]
    public string AppName
    {
        get;
        set => field = ThrowIfNull(value);
    }

    [JsonPropertyName("app_icon_url")]
    public string AppIconUrl
    {
        get;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            field = value;
        }
    }
}

/// <summary>
/// Represents web source citation details for an AI citation element.
/// </summary>
public class WebCitationDetails : CitationDetails
{
    public WebCitationDetails(string displayName, string? title = null, string? snippet = null)
    {
        DisplayName = displayName;
        Title = title;
        Snippet = snippet;
    }

    public override string CitationType => "web";

    [JsonPropertyName("display_name")]
    public string DisplayName
    {
        get;
        set => field = ThrowIfNull(value);
    }

    [JsonPropertyName("title")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Title { get; set; }

    [JsonPropertyName("snippet")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Snippet { get; set; }
}

/// <summary>
/// Represents message citation details for an AI citation element.
/// </summary>
public class MessageCitationDetails : CitationDetails
{
    public MessageCitationDetails(string channel, string messageTs)
    {
        Channel = channel;
        MessageTs = messageTs;
    }

    public override string CitationType => "message";

    [JsonPropertyName("channel")] public string Channel { get; set; }

    [JsonPropertyName("message_ts")]
    public string MessageTs
    {
        get;
        set => field = ThrowIfNull(value);
    }
}

/// <summary>
/// Represents memory citation details for an AI citation element.
/// </summary>
public class MemoryCitationDetails : CitationDetails
{
    public MemoryCitationDetails(string memoryId)
    {
        MemoryId = memoryId;
    }

    public override string CitationType => "memory";

    [JsonPropertyName("memory_id")]
    public string MemoryId
    {
        get;
        set => field = ThrowIfNull(value);
    }
}