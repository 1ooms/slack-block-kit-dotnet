namespace SlackBlockKit.Tests.Elements;

public class NumberInputElementTests
{
    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var dispatch = new DispatchActionConfigurationObject([TriggerActions.OnEnterPressed]);
        var element = new NumberInputElement(true, "action", "5.5", "1", "10", dispatch, true, "Amount");

        Assert.True(element.IsDecimalAllowed);
        Assert.Equal("action", element.ActionId);
        Assert.Equal("5.5", element.InitialValue);
        Assert.Equal("1", element.MinValue);
        Assert.Equal("10", element.MaxValue);
        Assert.Same(dispatch, element.DispatchActionConfig);
        Assert.True(element.FocusOnLoad);
        Assert.Equal("Amount", element.Placeholder!.Text);
        Assert.Equal("number_input", element.Type);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new NumberInputElement(true, "action", "5.5", "1", "10",
            new DispatchActionConfigurationObject([TriggerActions.OnEnterPressed]), true, "Amount"));

        AssertProperties(json, "type", "is_decimal_allowed", "action_id", "initial_value", "min_value",
            "max_value", "dispatch_action_config", "focus_on_load", "placeholder");
        Assert.Equal("number_input", json.GetProperty("type").GetString());
        Assert.True(json.GetProperty("is_decimal_allowed").GetBoolean());
        Assert.Equal("5.5", json.GetProperty("initial_value").GetString());
        Assert.Equal("1", json.GetProperty("min_value").GetString());
        Assert.Equal("10", json.GetProperty("max_value").GetString());
    }

    [Fact]
    public void Serialize_AlwaysWritesIsDecimalAllowed()
    {
        var json = ToJson(new NumberInputElement());

        AssertProperties(json, "type", "is_decimal_allowed");
        Assert.False(json.GetProperty("is_decimal_allowed").GetBoolean());
    }

    [Theory]
    [InlineData("42")]
    [InlineData("-42")]
    [InlineData("+42")]
    [InlineData("0")]
    public void IntegerMode_AcceptsIntegers(string value)
    {
        Assert.Equal(value, new NumberInputElement(initialValue: value).InitialValue);
    }

    [Theory]
    [InlineData("4.2")]
    [InlineData("-0.5")]
    public void IntegerMode_RejectsDecimals(string value)
    {
        Assert.Throws<ArgumentException>(() => new NumberInputElement(initialValue: value));
        Assert.Throws<ArgumentException>(() => new NumberInputElement(minValue: value));
        Assert.Throws<ArgumentException>(() => new NumberInputElement(maxValue: value));
    }

    [Theory]
    [InlineData("4.2")]
    [InlineData("-0.5")]
    [InlineData("10")]
    public void DecimalMode_AcceptsDecimalsAndIntegers(string value)
    {
        Assert.Equal(value, new NumberInputElement(true, initialValue: value).InitialValue);
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("")]
    [InlineData("1.")]
    [InlineData(".5")]
    [InlineData("1,5")]
    [InlineData("1e5")]
    [InlineData(" 1")]
    public void InvalidNumbers_Throw(string value)
    {
        Assert.Throws<ArgumentException>(() => new NumberInputElement(true, initialValue: value));
    }

    [Fact]
    public void MinValue_MustBeSmallerThanMaxValue()
    {
        Assert.Throws<ArgumentException>(() => new NumberInputElement(minValue: "10", maxValue: "10"));
        Assert.Throws<ArgumentException>(() => new NumberInputElement(minValue: "11", maxValue: "10"));
        Assert.NotNull(new NumberInputElement(minValue: "9", maxValue: "10"));
    }

    [Fact]
    public void InitialValue_MustBeWithinRange()
    {
        Assert.Throws<ArgumentException>(() => new NumberInputElement(initialValue: "0", minValue: "1"));
        Assert.Throws<ArgumentException>(() => new NumberInputElement(initialValue: "11", maxValue: "10"));
        Assert.NotNull(new NumberInputElement(initialValue: "1", minValue: "1", maxValue: "10"));
        Assert.NotNull(new NumberInputElement(initialValue: "10", minValue: "1", maxValue: "10"));
    }

    [Fact]
    public void Decimals_AreComparedNumerically()
    {
        Assert.Throws<ArgumentException>(() =>
            new NumberInputElement(true, initialValue: "1.05", minValue: "1.1"));
        Assert.NotNull(new NumberInputElement(true, initialValue: "1.15", minValue: "1.1", maxValue: "1.2"));
    }

    [Fact]
    public void Setters_ValidateAgainstCurrentState()
    {
        var element = new NumberInputElement(initialValue: "5", minValue: "1", maxValue: "10");

        Assert.Throws<ArgumentException>(() => element.InitialValue = "11");
        Assert.Throws<ArgumentException>(() => element.MinValue = "6");
        Assert.Throws<ArgumentException>(() => element.MaxValue = "4");
        Assert.Throws<ArgumentException>(() => element.MaxValue = "1");

        element.MaxValue = "20";
        element.InitialValue = "15";

        Assert.Equal("15", element.InitialValue);
    }

    [Fact]
    public void IsDecimalAllowed_CannotBeDisabledWhileDecimalsAreSet()
    {
        var element = new NumberInputElement(true, initialValue: "1.5");

        Assert.Throws<ArgumentException>(() => element.IsDecimalAllowed = false);
        Assert.True(element.IsDecimalAllowed);
    }

    [Fact]
    public void IsDecimalAllowed_CanBeEnabledLater()
    {
        var element = new NumberInputElement(initialValue: "1") { IsDecimalAllowed = true };

        element.InitialValue = "1.5";

        Assert.Equal("1.5", element.InitialValue);
    }

    [Fact]
    public void Placeholder_ThrowsWhenTooLong()
    {
        Assert.NotNull(new NumberInputElement(placeholder: Chars(150)));
        Assert.Throws<ArgumentException>(() => new NumberInputElement(placeholder: Chars(151)));
    }

    [Fact]
    public void RoundTrip_PreservesValues()
    {
        var element = RoundTrip(new NumberInputElement(true, "action", "2.5", "1", "3"));

        Assert.True(element.IsDecimalAllowed);
        Assert.Equal("action", element.ActionId);
        Assert.Equal("2.5", element.InitialValue);
        Assert.Equal("1", element.MinValue);
        Assert.Equal("3", element.MaxValue);
    }
}
