using System.Text.Json.Serialization;

namespace SlackBlockKit.Elements;

/// <summary>
/// Represents a file input element that allows users to upload files.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/file-input-element">Slack File Input Element Documentation</seealso>
public class FileInputElement : ActionElement, IInputElement
{
    private const int MaxAllowedFiles = 10;

    public FileInputElement(
        string? actionId = null,
        List<string>? filetypes = null,
        int? maxFiles = null)
    {
        ActionId = actionId;
        Filetypes = filetypes;
        MaxFiles = maxFiles;
    }

    public override string Type => "file_input";

    [JsonPropertyName("filetypes")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string>? Filetypes { get; set; }

    [JsonPropertyName("max_files")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MaxFiles
    {
        get;
        set
        {
            if (value != null && (value < 1 || value > MaxAllowedFiles))
                throw new ArgumentException($"MaxFiles must be between 1 and {MaxAllowedFiles}.", nameof(value));
            field = value;
        }
    }
}