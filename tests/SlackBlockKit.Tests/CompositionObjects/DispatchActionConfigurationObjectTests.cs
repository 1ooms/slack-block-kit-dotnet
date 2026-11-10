namespace SlackBlockKit.Tests.CompositionObjects;

public class DispatchActionConfigurationObjectTests
{
    [Fact]
    public void Serialize_WritesTriggerActionsAsSnakeCase()
    {
        var json = ToJson(new DispatchActionConfigurationObject(
            [TriggerActions.OnEnterPressed, TriggerActions.OnCharacterEntered]));

        AssertProperties(json, "trigger_actions_on");
        Assert.Equal(["on_enter_pressed", "on_character_entered"],
            json.GetProperty("trigger_actions_on").EnumerateArray().Select(e => e.GetString()));
    }

    [Fact]
    public void TriggerActionsOn_ThrowsWhenNull()
    {
        Assert.Throws<ArgumentNullException>(() => new DispatchActionConfigurationObject(null!));
    }

    [Fact]
    public void TriggerActionsOn_ThrowsWhenEmpty()
    {
        Assert.Throws<ArgumentException>(() => new DispatchActionConfigurationObject([]));
    }
}
