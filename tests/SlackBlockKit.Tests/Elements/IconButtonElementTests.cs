namespace SlackBlockKit.Tests.Elements;

public class IconButtonElementTests
{
    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var confirm = Confirm();
        var element = new IconButtonElement(IconButtonIcon.Trash, "Delete", "action", "value", confirm, ["U1"],
            "Delete item");

        Assert.Equal(IconButtonIcon.Trash, element.Icon);
        Assert.Equal("Delete", element.Text.Text);
        Assert.Equal("action", element.ActionId);
        Assert.Equal("value", element.Value);
        Assert.Same(confirm, element.Confirm);
        Assert.Equal(["U1"], element.VisibleToUserIds);
        Assert.Equal("Delete item", element.AccessibilityLabel);
        Assert.Equal("icon_button", element.Type);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new IconButtonElement(IconButtonIcon.Trash, "Delete", "action", "value", Confirm(),
            ["U1"], "Delete item"));

        AssertProperties(json, "type", "icon", "text", "action_id", "value", "confirm", "visible_to_user_ids",
            "accessibility_label");
        Assert.Equal("icon_button", json.GetProperty("type").GetString());
        Assert.Equal("trash", json.GetProperty("icon").GetString());
        Assert.Equal("Delete", TextOf(json, "text"));
    }

    [Fact]
    public void Serialize_OmitsNullFields()
    {
        AssertProperties(ToJson(new IconButtonElement(IconButtonIcon.Trash, "Delete")), "type", "icon", "text");
    }

    [Fact]
    public void Text_ThrowsWhenNull()
    {
        Assert.Throws<ArgumentNullException>(() => new IconButtonElement(IconButtonIcon.Trash, null!));
    }

    [Fact]
    public void Value_ThrowsWhenEmpty()
    {
        Assert.Throws<ArgumentException>(() => new IconButtonElement(IconButtonIcon.Trash, "Delete", value: ""));
    }

    [Fact]
    public void AccessibilityLabel_ThrowsWhenTooLong()
    {
        Assert.NotNull(new IconButtonElement(IconButtonIcon.Trash, "Delete", accessibilityLabel: Chars(75)));
        Assert.Throws<ArgumentException>(() =>
            new IconButtonElement(IconButtonIcon.Trash, "Delete", accessibilityLabel: Chars(76)));
    }
}
