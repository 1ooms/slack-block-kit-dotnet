namespace SlackBlockKit.Tests.Elements;

public class DateFormatTests
{
    [Theory]
    [InlineData(DateFormat.DateNum, "{date_num}")]
    [InlineData(DateFormat.DateSlash, "{date_slash}")]
    [InlineData(DateFormat.DateLong, "{date_long}")]
    [InlineData(DateFormat.DateLongFull, "{date_long_full}")]
    [InlineData(DateFormat.DateLongPretty, "{date_long_pretty}")]
    [InlineData(DateFormat.Date, "{date}")]
    [InlineData(DateFormat.DatePretty, "{date_pretty}")]
    [InlineData(DateFormat.DateShort, "{date_short}")]
    [InlineData(DateFormat.DateShortPretty, "{date_short_pretty}")]
    [InlineData(DateFormat.DayDividerPretty, "{day_divider_pretty}")]
    [InlineData(DateFormat.Time, "{time}")]
    [InlineData(DateFormat.TimeSecs, "{time_secs}")]
    [InlineData(DateFormat.Ago, "{ago}")]
    public void Tokens_MatchSlackSyntax(string token, string expected)
    {
        Assert.Equal(expected, token);
    }
}
