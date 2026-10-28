using System.Text.Json.Serialization;

namespace SlackBlockKit.Blocks;

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