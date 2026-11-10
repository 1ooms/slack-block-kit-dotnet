namespace SlackBlockKit.Tests.Elements;

public class FeedbackButtonsElementTests
{
    private static FeedbackButtonsElement Create(string? actionId = null) =>
        new(new FeedbackButtonObject("Good", "positive"), new FeedbackButtonObject("Bad", "negative"), actionId);

    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var element = Create("feedback");

        Assert.Equal("Good", element.PositiveButton.Text.Text);
        Assert.Equal("negative", element.NegativeButton.Value);
        Assert.Equal("feedback", element.ActionId);
        Assert.Equal("feedback_buttons", element.Type);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(Create("feedback"));

        AssertProperties(json, "type", "positive_button", "negative_button", "action_id");
        Assert.Equal("feedback_buttons", json.GetProperty("type").GetString());
        Assert.Equal("Good", TextOf(json.GetProperty("positive_button"), "text"));
        Assert.Equal("positive", json.GetProperty("positive_button").GetProperty("value").GetString());
        Assert.Equal("negative", json.GetProperty("negative_button").GetProperty("value").GetString());
    }

    [Fact]
    public void Serialize_OmitsActionIdWhenNull()
    {
        AssertNoProperty(ToJson(Create()), "action_id");
    }

    [Fact]
    public void Buttons_ThrowWhenNull()
    {
        var button = new FeedbackButtonObject("Good", "positive");

        Assert.Throws<ArgumentNullException>(() => new FeedbackButtonsElement(null!, button));
        Assert.Throws<ArgumentNullException>(() => new FeedbackButtonsElement(button, null!));
    }
}
