namespace SlackBlockKit.Tests.Blocks;

public class DataTableBlockTests
{
    private static List<List<IDataTableCell>> Rows() =>
    [
        [new RawTextCell("Name"), new RawTextCell("Score")],
        [new RawTextCell("Alice"), new RawNumberCell(9.5, "9.5")]
    ];

    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var rows = Rows();

        var block = new DataTableBlock("Scores", rows, 10, 0, "table_1");

        Assert.Equal("Scores", block.Caption);
        Assert.Same(rows, block.Rows);
        Assert.Equal(10, block.PageSize);
        Assert.Equal(0, block.RowHeaderColumnIndex);
        Assert.Equal("table_1", block.BlockId);
        Assert.Equal("data_table", block.Type);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new DataTableBlock("Scores", Rows(), 10, 0, "table_1"));

        AssertProperties(json, "type", "caption", "rows", "page_size", "row_header_column_index", "block_id");
        Assert.Equal("data_table", json.GetProperty("type").GetString());
        Assert.Equal("raw_text", json.GetProperty("rows")[0][0].GetProperty("type").GetString());
        Assert.Equal("raw_number", json.GetProperty("rows")[1][1].GetProperty("type").GetString());
    }

    [Fact]
    public void Serialize_OmitsNullFields()
    {
        AssertProperties(ToJson(new DataTableBlock("Scores", Rows())), "type", "caption", "rows");
    }

    [Fact]
    public void Serialize_SupportsAllCellTypes()
    {
        var json = ToJson(new DataTableBlock("Caption", [
            [new RawTextCell("A"), new RawTextCell("B"), new RawTextCell("C"), new RawTextCell("D")],
            [new RawTextCell("x"), new RawNumberCell(1, "1"), RichTextBlockOf(), new ActionCell(new ButtonElement("Go"))]
        ]));

        Assert.Equal(["raw_text", "raw_number", "rich_text", "action_cell"],
            json.GetProperty("rows")[1].EnumerateArray().Select(c => c.GetProperty("type").GetString()));
    }

    [Fact]
    public void Rows_ThrowsWhenNullOrEmpty()
    {
        Assert.Throws<ArgumentNullException>(() => new DataTableBlock("Caption", null!));
        Assert.Throws<ArgumentException>(() => new DataTableBlock("Caption", []));
        Assert.Throws<ArgumentException>(() => new DataTableBlock("Caption", [[]]));
    }

    [Fact]
    public void Rows_AllowsUpTo201()
    {
        var rows = Enumerable.Range(0, 201).Select(_ => new List<IDataTableCell> { new RawTextCell("x") }).ToList();

        Assert.Equal(201, new DataTableBlock("Caption", rows).Rows.Count);

        rows.Add([new RawTextCell("x")]);
        Assert.Throws<ArgumentException>(() => new DataTableBlock("Caption", rows));
    }

    [Fact]
    public void Columns_AllowsUpTo20()
    {
        var row = Enumerable.Range(0, 20).Select(_ => (IDataTableCell)new RawTextCell("x")).ToList();

        Assert.NotNull(new DataTableBlock("Caption", [row]));

        row.Add(new RawTextCell("x"));
        Assert.Throws<ArgumentException>(() => new DataTableBlock("Caption", [row]));
    }

    [Fact]
    public void HeaderRow_MustOnlyContainRawTextCells()
    {
        Assert.Throws<ArgumentException>(() => new DataTableBlock("Caption", [[new RawNumberCell(1, "1")]]));
        Assert.Throws<ArgumentException>(() => new DataTableBlock("Caption", [[RichTextBlockOf()]]));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    public void PageSize_AcceptsValuesBetween1And100(int value)
    {
        Assert.Equal(value, new DataTableBlock("Caption", Rows(), value).PageSize);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(101)]
    public void PageSize_ThrowsWhenOutOfRange(int value)
    {
        Assert.Throws<ArgumentException>(() => new DataTableBlock("Caption", Rows(), value));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(20)]
    public void RowHeaderColumnIndex_ThrowsWhenOutOfRange(int value)
    {
        Assert.Throws<ArgumentException>(() => new DataTableBlock("Caption", Rows(), rowHeaderColumnIndex: value));
    }

    [Fact]
    public void RowHeaderColumnIndex_AcceptsBoundaries()
    {
        Assert.Equal(19, new DataTableBlock("Caption", Rows(), rowHeaderColumnIndex: 19).RowHeaderColumnIndex);
    }

    [Fact]
    public void RoundTrip_PreservesCellTypes()
    {
        var block = RoundTrip(new DataTableBlock("Caption", [
            [new RawTextCell("A"), new RawTextCell("B"), new RawTextCell("C")],
            [new RawNumberCell(1, "1"), RichTextBlockOf(), new ActionCell(new ButtonElement("Go"), new RawTextCell("Go"))]
        ], 10, 0, "table_1"));

        Assert.Equal("A", Assert.IsType<RawTextCell>(block.Rows[0][0]).Text);
        Assert.Equal(1, Assert.IsType<RawNumberCell>(block.Rows[1][0]).Value);
        Assert.IsType<RichTextBlock>(block.Rows[1][1]);
        var action = Assert.IsType<ActionCell>(block.Rows[1][2]);
        Assert.IsType<RawTextCell>(action.Fallback);
        Assert.Equal(10, block.PageSize);
        Assert.Equal("table_1", block.BlockId);
    }
}
