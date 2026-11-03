using System.Text.Json.Serialization;
using SlackBlockKit.Elements;

namespace SlackBlockKit.Blocks;

/// <summary>
/// Represents a data table block (<c>type: "data_table"</c>) that displays paginated tabular data with interactive column and row features.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/blocks/data-table-block">Slack Data Table Block Documentation</seealso>
public class DataTableBlock : Block
{
    private const int MaxRows = 201;
    private const int MaxColumns = 20;

    public DataTableBlock(
        string caption,
        List<List<IDataTableCell>> rows,
        int? pageSize = null,
        int? rowHeaderColumnIndex = null,
        string? blockId = null)
    {
        Caption = caption;
        Rows = rows;
        PageSize = pageSize;
        RowHeaderColumnIndex = rowHeaderColumnIndex;
        BlockId = blockId;
    }

    public override string Type => "data_table";

    [JsonPropertyName("caption")] public string Caption { get; set; }

    [JsonPropertyName("rows")]
    public List<List<IDataTableCell>> Rows
    {
        get;
        set
        {
            ThrowIfNullOrEmptyOrExceedsCount(value, MaxRows);
            foreach (var row in value)
            {
                ThrowIfNullOrEmptyOrExceedsCount(row, MaxColumns);
            }

            foreach (var cell in value[0])
            {
                if (cell.GetType() != typeof(RawTextCell))
                {
                    throw new ArgumentException("First row (header) must only contain cells of type RawTextCell",
                        nameof(value));
                }
            }

            field = value;
        }
    }

    [JsonPropertyName("page_size")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? PageSize
    {
        get;
        set => field = ThrowIfNotBetween(value, 1, 100);
    }

    [JsonPropertyName("row_header_column_index")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? RowHeaderColumnIndex
    {
        get;
        set => field = ThrowIfNotBetween(value, 0, 19);
    }
}

/// <summary>
/// Represents a cell in a Slack Data Table block.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/blocks/data-table-block">Slack Data Table Block Reference</seealso>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(RawTextCell), "raw_text")]
[JsonDerivedType(typeof(RawNumberCell), "raw_number")]
[JsonDerivedType(typeof(RichTextBlock), "rich_text")]
[JsonDerivedType(typeof(ActionCell), "action_cell")]
public interface IDataTableCell
{
    [JsonIgnore] string Type { get; }
}

/// <summary>
/// Represents an action cell within a data table block containing an interactive element.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/blocks/data-table-block#schema-action-cell">Slack Action Cell Documentation</seealso>
public class ActionCell : IDataTableCell
{
    public ActionCell(ButtonElement element, IActionCellFallbackObject? fallback = null)
    {
        Element = element;
        Fallback = fallback;
    }

    [JsonPropertyName("element")]
    public ButtonElement Element
    {
        get;
        set => field = ThrowIfNull(value);
    }

    [JsonPropertyName("fallback")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IActionCellFallbackObject? Fallback { get; set; }

    [JsonPropertyName("type")] public string Type => "action_cell";
}

/// <summary>
/// Represents the fallback object within an action cell within a data table block.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/blocks/data-table-block#schema-action-cell">Slack Action Cell Documentation</seealso>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(RawTextCell), "raw_text")]
[JsonDerivedType(typeof(RawNumberCell), "raw_number")]
public interface IActionCellFallbackObject
{
    [JsonIgnore] string Type { get; }
}