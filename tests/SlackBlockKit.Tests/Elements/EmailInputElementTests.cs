namespace SlackBlockKit.Tests.Elements;

public class EmailInputElementTests
{
    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var dispatch = new DispatchActionConfigurationObject([TriggerActions.OnEnterPressed]);
        var element = new EmailInputElement("action", "user@example.com", dispatch, true, "Email");

        Assert.Equal("action", element.ActionId);
        Assert.Equal("user@example.com", element.InitialValue);
        Assert.Same(dispatch, element.DispatchActionConfig);
        Assert.True(element.FocusOnLoad);
        Assert.Equal("Email", element.Placeholder!.Text);
        Assert.Equal("email_text_input", element.Type);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new EmailInputElement("action", "user@example.com",
            new DispatchActionConfigurationObject([TriggerActions.OnCharacterEntered]), true, "Email"));

        AssertProperties(json, "type", "action_id", "initial_value", "dispatch_action_config", "focus_on_load",
            "placeholder");
        Assert.Equal("email_text_input", json.GetProperty("type").GetString());
        Assert.Equal("user@example.com", json.GetProperty("initial_value").GetString());
        Assert.Equal("on_character_entered",
            json.GetProperty("dispatch_action_config").GetProperty("trigger_actions_on")[0].GetString());
    }

    [Fact]
    public void Serialize_OmitsNullFields()
    {
        AssertProperties(ToJson(new EmailInputElement()), "type");
    }

    [Theory]
    [InlineData("user@example.com")]
    [InlineData("first.last@sub.example.org")]
    [InlineData("user-name@example.co")]
    public void InitialValue_AcceptsValidEmails(string email)
    {
        Assert.Equal(email, new EmailInputElement(initialValue: email).InitialValue);
    }

    [Theory]
    [InlineData("")]
    [InlineData("not-an-email")]
    [InlineData("user@")]
    [InlineData("@example.com")]
    [InlineData("user@example")]
    [InlineData("user@example.toolongtld")]
    public void InitialValue_ThrowsOnInvalidEmails(string email)
    {
        Assert.Throws<ArgumentException>(() => new EmailInputElement(initialValue: email));
    }

    [Fact]
    public void Placeholder_ThrowsWhenTooLong()
    {
        Assert.NotNull(new EmailInputElement(placeholder: Chars(150)));
        Assert.Throws<ArgumentException>(() => new EmailInputElement(placeholder: Chars(151)));
    }
}
