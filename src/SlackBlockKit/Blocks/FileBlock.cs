using System.Text.Json.Serialization;

namespace SlackBlockKit.Blocks;

/// <summary>
/// Represents a file block (<c>type: "file"</c>) that displays a remote file shared into Slack.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/blocks/file-block">Slack File Block Documentation</seealso>
public class FileBlock : Block
{
    public FileBlock(string externalId, string source = "remote", string? blockId = null)
    {
        ExternalId = externalId;
        Source = source;
        BlockId = blockId;
    }

    public override string Type => "file";

    [JsonPropertyName("external_id")]
    public string ExternalId
    {
        get;
        set => field = ThrowIfNull(value);
    }

    [JsonPropertyName("source")]
    public string Source
    {
        get;
        set => field = ThrowIfNull(value);
    }
}