namespace SlackBlockKit.Tests.Elements;

public class FeedbackButtonObjectTests
{
    [Fact]
    public void StringConstructor_WrapsText()
    {
        var button = new FeedbackButtonObject("Good", "positive", "Thumbs up");

        Assert.Equal("Good", button.Text.Text);
        Assert.Equal("positive", button.Value);
        Assert.Equal("Thumbs up", button.AccessibilityLabel);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new FeedbackButtonObject(new PlainTextObject("Good"), "positive", "Thumbs up"));

        AssertProperties(json, "text", "value", "accessibility_label");
        Assert.Equal("plain_text", json.GetProperty("text").GetProperty("type").GetString());
        Assert.Equal("Thumbs up", json.GetProperty("accessibility_label").GetString());
    }

    [Fact]
    public void Serialize_OmitsAccessibilityLabelWhenNull()
    {
        AssertProperties(ToJson(new FeedbackButtonObject("Good", "positive")), "text", "value");
    }

    [Fact]
    public void Text_ThrowsWhenNullOrTooLong()
    {
        Assert.Throws<ArgumentNullException>(() => new FeedbackButtonObject((PlainTextObject)null!, "v"));
        Assert.NotNull(new FeedbackButtonObject(Chars(75), "v"));
        Assert.Throws<ArgumentException>(() => new FeedbackButtonObject(Chars(76), "v"));
    }

    [Fact]
    public void Value_ThrowsWhenNullEmptyOrTooLong()
    {
        Assert.Throws<ArgumentNullException>(() => new FeedbackButtonObject("Good", null!));
        Assert.Throws<ArgumentException>(() => new FeedbackButtonObject("Good", ""));
        Assert.NotNull(new FeedbackButtonObject("Good", Chars(2000)));
        Assert.Throws<ArgumentException>(() => new FeedbackButtonObject("Good", Chars(2001)));
    }

    [Fact]
    public void AccessibilityLabel_ThrowsWhenTooLong()
    {
        Assert.NotNull(new FeedbackButtonObject("Good", "v", Chars(75)));
        Assert.Throws<ArgumentException>(() => new FeedbackButtonObject("Good", "v", Chars(76)));
    }

    [Fact]
    public void RoundTrip_PreservesValues()
    {
        var button = RoundTrip(new FeedbackButtonObject("Good", "positive", "Thumbs up"));

        Assert.Equal("Good", button.Text.Text);
        Assert.Equal("positive", button.Value);
        Assert.Equal("Thumbs up", button.AccessibilityLabel);
    }
}
