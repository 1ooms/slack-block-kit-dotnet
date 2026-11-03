using System.Text.Json.Serialization;

namespace SlackBlockKit.Blocks;

/// <summary>
/// Represents a Slack Table block (<c>type: "table"</c>).
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/blocks/table-block">Slack Table Block Documentation</seealso>
public class TableBlock : Block
{
    public enum ColumnAlignment
    {
        Left,
        Center,
        Right
    }

    private const int MaxRows = 100;
    private const int MaxColumns = 20;

    public TableBlock(List<List<ITableCell>> rows, List<ColumnSetting>? columnSettings = null,
        string? blockId = null)
    {
        Rows = rows;
        ColumnSettings = columnSettings;
        BlockId = blockId;
    }

    public override string Type => "table";

    [JsonPropertyName("rows")]
    public List<List<ITableCell>> Rows
    {
        get;
        set
        {
            ThrowIfNullOrExceedsCount(value, MaxRows);
            foreach (var row in value)
            {
                ThrowIfExceedsCount(row, MaxColumns);
            }

            field = value;
        }
    }

    [JsonPropertyName("column_settings")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<ColumnSetting>? ColumnSettings
    {
        get;
        set => field = ThrowIfExceedsCount(value, MaxColumns);
    }

    /// <summary>
    /// Configures column alignment and wrapping for a table block.
    /// </summary>
    /// <seealso href="https://docs.slack.dev/reference/block-kit/blocks/table-block/#schema-for-column_settings">Slack Table Column Settings Reference</seealso>
    public class ColumnSetting
    {
        public ColumnSetting(ColumnAlignment? align = null, bool? isWrapped = null)
        {
            Align = align;
            IsWrapped = isWrapped;
        }

        [JsonPropertyName("align")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ColumnAlignment? Align { get; set; }

        [JsonPropertyName("is_wrapped")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? IsWrapped { get; set; }
    }
}

/// <summary>
/// Represents a cell in a Slack Table block.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/blocks/table-block">Slack Table Block Reference</seealso>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(RawTextCell), "raw_text")]
[JsonDerivedType(typeof(RawNumberCell), "raw_number")]
public interface ITableCell
{
    [JsonIgnore] string Type { get; }
}

/// <summary>
/// Represents a raw text cell in a Slack Table block (<c>type: "raw_text"</c>).
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/blocks/data-table-block/#schema-raw-text">Slack Raw Text Cell Reference</seealso>
public class RawTextCell : ITableCell
{
    public RawTextCell(string text)
    {
        Text = text;
    }

    [JsonPropertyName("text")]
    public string Text
    {
        get;
        set => field = ThrowIfNullOrEmpty(value);
    }

    [JsonIgnore] public string Type => "raw_text";

    public static implicit operator RawTextCell(string text) => new(text);
}

/// <summary>
/// Represents a raw numeric cell in a Slack Table block (<c>type: "raw_number"</c>).
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/blocks/data-table-block/#schema-raw-number">Slack Raw Number Cell Reference</seealso>
/// <remarks>This type of cell seems to be currently broken in the Slack API (29/10/2026).</remarks>
public class RawNumberCell : ITableCell
{
    public RawNumberCell(double value, string text)
    {
        Value = value;
        Text = text;
    }

    [JsonPropertyName("value")]
    public double Value
    {
        get;
        set => field = ThrowIfNull(value);
    }

    [JsonPropertyName("text")]
    public string Text
    {
        get;
        set => field = ThrowIfNullOrEmpty(value);
    }

    [JsonIgnore] public string Type => "raw_number";
}