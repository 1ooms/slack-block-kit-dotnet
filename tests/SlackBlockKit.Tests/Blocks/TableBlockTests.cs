namespace SlackBlockKit.Tests.Blocks;

public class TableBlockTests
{
    private static List<List<ITableCell>> Rows() =>
    [
        [new RawTextCell("Name"), new RawTextCell("Score")],
        [new RawTextCell("Alice"), new RawNumberCell(9, "9")]
    ];

    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var rows = Rows();
        var settings = new List<TableBlock.ColumnSetting> { new(ColumnAlignment.Left) };

        var block = new TableBlock(rows, settings, "table_1");

        Assert.Same(rows, block.Rows);
        Assert.Same(settings, block.ColumnSettings);
        Assert.Equal("table_1", block.BlockId);
        Assert.Equal("table", block.Type);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new TableBlock(Rows(),
            [new TableBlock.ColumnSetting(ColumnAlignment.Center, true)], "table_1"));

        AssertProperties(json, "type", "rows", "column_settings", "block_id");
        Assert.Equal("table", json.GetProperty("type").GetString());
        Assert.Equal("raw_text", json.GetProperty("rows")[0][0].GetProperty("type").GetString());
        Assert.Equal("raw_number", json.GetProperty("rows")[1][1].GetProperty("type").GetString());
        Assert.Equal("center", json.GetProperty("column_settings")[0].GetProperty("align").GetString());
    }

    [Fact]
    public void Serialize_OmitsNullFields()
    {
        AssertProperties(ToJson(new TableBlock(Rows())), "type", "rows");
    }

    [Fact]
    public void Serialize_SupportsRichTextCells()
    {
        var json = ToJson(new TableBlock([[RichTextBlockOf("rich")]]));

        Assert.Equal("rich_text", json.GetProperty("rows")[0][0].GetProperty("type").GetString());
    }

    [Fact]
    public void Rows_ThrowsWhenNull()
    {
        Assert.Throws<ArgumentNullException>(() => new TableBlock(null!));
    }

    [Fact]
    public void Rows_ThrowsWhenARowIsEmpty()
    {
        Assert.Throws<ArgumentException>(() => new TableBlock([[]]));
    }

    [Fact]
    public void Rows_AllowsUpTo100()
    {
        var rows = Enumerable.Range(0, 100).Select(_ => new List<ITableCell> { new RawTextCell("x") }).ToList();

        Assert.Equal(100, new TableBlock(rows).Rows.Count);

        rows.Add([new RawTextCell("x")]);
        Assert.Throws<ArgumentException>(() => new TableBlock(rows));
    }

    [Fact]
    public void Columns_AllowsUpTo20()
    {
        var row = Enumerable.Range(0, 20).Select(_ => (ITableCell)new RawTextCell("x")).ToList();

        Assert.NotNull(new TableBlock([row]));

        row.Add(new RawTextCell("x"));
        Assert.Throws<ArgumentException>(() => new TableBlock([row]));
    }

    [Fact]
    public void ColumnSettings_AllowsUpTo20()
    {
        var settings = Enumerable.Range(0, 20).Select(_ => new TableBlock.ColumnSetting()).ToList();

        Assert.NotNull(new TableBlock(Rows(), settings));

        settings.Add(new TableBlock.ColumnSetting());
        Assert.Throws<ArgumentException>(() => new TableBlock(Rows(), settings));
    }
}
