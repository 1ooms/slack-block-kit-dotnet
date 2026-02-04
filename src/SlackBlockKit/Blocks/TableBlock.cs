using System.Text.Json.Serialization;
using SlackBlockKit.TextObjects;

namespace SlackBlockKit.Blocks;

public class TableBlock : Block
{
    private const int MaxRows = 100;
    private const int MaxColumns = 20;

    public TableBlock(List<List<ITableTextObject>> rows)
    {
        Rows = rows;
    }

    public override string Type => "table";

    [JsonPropertyName("rows")]
    public List<List<ITableTextObject>> Rows
    {
        get;
        set
        {
            if (value == null)
                throw new ArgumentException("Rows cannot be null.", nameof(value));
            if (Rows.Count > MaxRows)
                throw new ArgumentException($"Table cannot have more than {MaxRows} rows.", nameof(value));
            if (value.Any(row => row.Count > MaxColumns))
                throw new ArgumentException($"Table cannot have more than {MaxColumns} columns.", nameof(value));
            field = value;
        }
    }

    [JsonPropertyName("column_settings")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<ColumnSetting>? ColumnSettings
    {
        get;
        set
        {
            if (value != null && value.Count > MaxColumns)
                throw new ArgumentException($"Column settings cannot have more than {MaxColumns} columns.",
                    nameof(value));

            field = value;
        }
    }

    public class ColumnSetting
    {
        [JsonPropertyName("align")] public string? Align;

        [JsonPropertyName("is_wrapped")] public bool? IsWrapped;

        public ColumnSetting(string? align = null, bool? isWrapped = null)
        {
            if (align != null && align != "left" && align != "center" && align != "right")
                throw new ArgumentException("Align must be one of 'left', 'center', or 'right'.", nameof(align));

            Align = align;
            IsWrapped = isWrapped;
        }
    }
}