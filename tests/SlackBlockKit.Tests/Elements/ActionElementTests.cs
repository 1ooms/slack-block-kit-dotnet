namespace SlackBlockKit.Tests.Elements;

public class ActionElementTests
{
    private class TestActionElement : ActionElement
    {
        public override string Type => "test";
    }

    [Fact]
    public void ActionId_DefaultsToNull()
    {
        Assert.Null(new TestActionElement().ActionId);
    }

    [Fact]
    public void ActionId_AllowsUpTo255Characters()
    {
        var element = new TestActionElement { ActionId = Chars(255) };

        Assert.Equal(255, element.ActionId!.Length);
    }

    [Fact]
    public void ActionId_ThrowsWhenLongerThan255Characters()
    {
        Assert.Throws<ArgumentException>(() => new TestActionElement { ActionId = Chars(256) });
    }

    [Fact]
    public void ActionId_CanBeResetToNull()
    {
        var element = new TestActionElement { ActionId = "action" };

        element.ActionId = null;

        Assert.Null(element.ActionId);
    }

    [Fact]
    public void ActionId_SerializesAsSnakeCase()
    {
        var json = ToJson(new ButtonElement("Click", "my_action"));

        Assert.Equal("my_action", json.GetProperty("action_id").GetString());
    }

    [Fact]
    public void ActionId_OmittedWhenNull()
    {
        AssertNoProperty(ToJson(new ButtonElement("Click")), "action_id");
    }
}
