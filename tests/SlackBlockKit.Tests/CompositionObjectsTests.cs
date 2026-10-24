using System.Text.Json;
using SlackBlockKit.CompositionObjects;
using SlackBlockKit.TextObjects;

namespace SlackBlockKit.Tests;

public class CompositionObjectsTests
{
    [Fact]
    public void ConfirmationDialog_SerializesCorrectly()
    {
        var dialog = new ConfirmationDialogObject(
            "Confirm Action",
            "Are you sure you want to proceed?",
            "Yes, proceed",
            "No, cancel",
            ConfirmationDialogStyle.Danger
        );

        var json = SlackSerializer.Serialize(dialog);
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        Assert.Equal("Confirm Action", root.GetProperty("title").GetProperty("text").GetString());
        Assert.Equal("plain_text", root.GetProperty("title").GetProperty("type").GetString());
        Assert.Equal("Are you sure you want to proceed?", root.GetProperty("text").GetProperty("text").GetString());
        Assert.Equal("Yes, proceed", root.GetProperty("confirm").GetProperty("text").GetString());
        Assert.Equal("No, cancel", root.GetProperty("deny").GetProperty("text").GetString());
        Assert.Equal("danger", root.GetProperty("style").GetString());
    }

    [Fact]
    public void ConfirmationDialog_Validation()
    {
        // Title exceeds 100
        Assert.Throws<ArgumentException>(() =>
            new ConfirmationDialogObject(new string('a', 101), "Text", "Confirm", "Cancel"));

        // Text exceeds 300
        Assert.Throws<ArgumentException>(() =>
            new ConfirmationDialogObject("Title", new string('a', 301), "Confirm", "Cancel"));

        // Confirm text exceeds 30
        Assert.Throws<ArgumentException>(() =>
            new ConfirmationDialogObject("Title", "Text", new string('a', 31), "Cancel"));

        // Deny text exceeds 30
        Assert.Throws<ArgumentException>(() =>
            new ConfirmationDialogObject("Title", "Text", "Confirm", new string('a', 31)));
    }

    [Fact]
    public void OptionObject_SerializesCorrectly()
    {
        var option = new OptionObject(
            "Option Label",
            "opt_val_1",
            "A short description",
            "https://example.com"
        );

        var json = SlackSerializer.Serialize(option);
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        Assert.Equal("Option Label", root.GetProperty("text").GetProperty("text").GetString());
        Assert.Equal("opt_val_1", root.GetProperty("value").GetString());
        Assert.Equal("A short description", root.GetProperty("description").GetProperty("text").GetString());
        Assert.Equal("https://example.com", root.GetProperty("url").GetString());
    }

    [Fact]
    public void OptionObject_Validation()
    {
        // Text null throws
        Assert.Throws<ArgumentNullException>(() =>
            new OptionObject((PlainTextObject)null!, "val"));

        // Text exceeds 75
        Assert.Throws<ArgumentException>(() =>
            new OptionObject(new string('a', 76), "val"));

        // Value null throws
        Assert.Throws<ArgumentNullException>(() =>
            new OptionObject(new PlainTextObject("Text"), null!));

        // Value empty throws
        Assert.Throws<ArgumentException>(() =>
            new OptionObject("Text", ""));

        // Value exceeds 75
        Assert.Throws<ArgumentException>(() =>
            new OptionObject("Text", new string('a', 76)));

        // Description exceeds 75
        Assert.Throws<ArgumentException>(() =>
            new OptionObject("Text", "val", description: new string('a', 76)));

        // Url empty throws
        Assert.Throws<ArgumentException>(() =>
            new OptionObject("Text", "val", url: ""));

        // Url exceeds 3000
        Assert.Throws<ArgumentException>(() =>
            new OptionObject("Text", "val", url: new string('a', 3001)));

        // Property setters
        var option = new OptionObject("Text", "val");
        option.Text = new PlainTextObject("Updated Text");
        Assert.Equal("Updated Text", option.Text.Text);
        option.Value = "new_val";
        Assert.Equal("new_val", option.Value);
        option.Description = new PlainTextObject("desc");
        Assert.Equal("desc", option.Description?.Text);
        option.Url = "https://example.com/new";
        Assert.Equal("https://example.com/new", option.Url);
    }

    [Fact]
    public void OptionGroupObject_SerializesCorrectly()
    {
        var group = new OptionGroupObject(
            "Group A",
            [
                new OptionObject("Opt 1", "v1"),
                new OptionObject("Opt 2", "v2")
            ]
        );

        var json = SlackSerializer.Serialize(group);
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        Assert.Equal("Group A", root.GetProperty("label").GetProperty("text").GetString());
        Assert.Equal(2, root.GetProperty("options").GetArrayLength());
    }

    [Fact]
    public void OptionGroupObject_Validation()
    {
        // Label null throws
        Assert.Throws<ArgumentNullException>(() =>
            new OptionGroupObject((PlainTextObject)null!, [new OptionObject("Opt", "v")]));

        // Label exceeds 75
        Assert.Throws<ArgumentException>(() =>
            new OptionGroupObject(new string('a', 76), [new OptionObject("Opt", "v")]));

        // Options null throws
        Assert.Throws<ArgumentNullException>(() =>
            new OptionGroupObject("Label", null!));

        // Options empty throws
        Assert.Throws<ArgumentException>(() =>
            new OptionGroupObject("Label", []));

        // Options exceed 100
        var tooManyOptions = Enumerable.Range(1, 101)
            .Select(i => new OptionObject($"Opt {i}", $"v{i}"))
            .ToList();

        Assert.Throws<ArgumentException>(() =>
            new OptionGroupObject("Label", tooManyOptions));

        // Property setters
        var group = new OptionGroupObject("Label", [new OptionObject("Opt", "v")]);
        group.Label = "Updated Label";
        Assert.Equal("Updated Label", group.Label.Text);
        group.Options = [new OptionObject("New Opt", "v2")];
        Assert.Single(group.Options);
    }

    [Fact]
    public void ConversationFilterObject_SerializesCorrectly()
    {
        var filter = new ConversationFilterObject(
            include: [ConversationType.Public, ConversationType.Private],
            excludeExternalSharedChannels: true,
            excludeBotUsers: false
        );

        var json = SlackSerializer.Serialize(filter);
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        Assert.Equal(2, root.GetProperty("include").GetArrayLength());
        Assert.Equal("public", root.GetProperty("include")[0].GetString());
        Assert.Equal("private", root.GetProperty("include")[1].GetString());
        Assert.True(root.GetProperty("exclude_external_shared_channels").GetBoolean());
        Assert.False(root.GetProperty("exclude_bot_users").GetBoolean());
    }

    [Fact]
    public void ConversationFilterObject_Validation()
    {
        // Throws when no fields provided
        Assert.Throws<ArgumentException>(() => new ConversationFilterObject());

        // Throws when include is empty
        Assert.Throws<ArgumentException>(() => new ConversationFilterObject(include: []));

        // Valid with only excludeBotUsers
        var filter = new ConversationFilterObject(excludeBotUsers: true);
        Assert.True(filter.ExcludeBotUsers);

        // Valid with all 4 types using enum
        var allTypesFilter = new ConversationFilterObject(
            include: [ConversationType.Im, ConversationType.Mpim, ConversationType.Private, ConversationType.Public]);
        Assert.Equal(4, allTypesFilter.Include!.Count);

        // Property setters
        filter.Include = [ConversationType.Im];
        Assert.Single(filter.Include);

        // Setting Include to empty list throws
        Assert.Throws<ArgumentException>(() => filter.Include = []);

        // Setting ExcludeExternalSharedChannels
        filter.ExcludeExternalSharedChannels = false;
        Assert.False(filter.ExcludeExternalSharedChannels);

        // Setting ExcludeBotUsers
        filter.ExcludeBotUsers = true;
        Assert.True(filter.ExcludeBotUsers);

        // Setting all properties to null throws
        Assert.Throws<ArgumentException>(() =>
        {
            filter.Include = null;
            filter.ExcludeExternalSharedChannels = null;
            filter.ExcludeBotUsers = null;
        });
    }

    [Fact]
    public void DispatchActionConfiguration_SerializesCorrectly()
    {
        var config = new DispatchActionConfigurationObject([TriggerActions.OnEnterPressed, TriggerActions.OnCharacterEntered]);
        var json = SlackSerializer.Serialize(config);
        using var doc = JsonDocument.Parse(json);
        var actions = doc.RootElement.GetProperty("trigger_actions_on");
        Assert.Equal(2, actions.GetArrayLength());
        Assert.Equal("on_enter_pressed", actions[0].GetString());
        Assert.Equal("on_character_entered", actions[1].GetString());
    }

    [Fact]
    public void DispatchActionConfiguration_ThrowsOnEmptyList()
    {
        Assert.Throws<ArgumentException>(() => new DispatchActionConfigurationObject([]));
    }

    [Fact]
    public void SlackFileObject_SerializesAndValidates()
    {
        var fileById = SlackFileObject.FromId("F012345678");
        var jsonId = SlackSerializer.Serialize(fileById);
        using var docId = JsonDocument.Parse(jsonId);
        Assert.Equal("F012345678", docId.RootElement.GetProperty("id").GetString());

        var fileByUrl = SlackFileObject.FromUrl("https://files.slack.com/test.png");
        var jsonUrl = SlackSerializer.Serialize(fileByUrl);
        using var docUrl = JsonDocument.Parse(jsonUrl);
        Assert.Equal("https://files.slack.com/test.png", docUrl.RootElement.GetProperty("url").GetString());

        // Regex pattern validation for ID (^[F][A-Z0-9]{8,}$)
        Assert.Throws<ArgumentException>(() => SlackFileObject.FromId("F12345")); // too short
        Assert.Throws<ArgumentException>(() => SlackFileObject.FromId("f012345678")); // lowercase
        Assert.Throws<ArgumentException>(() => SlackFileObject.FromId("C012345678")); // does not start with F

        // Neither provided
        Assert.Throws<ArgumentException>(() => new SlackFileObject());

        // Both provided
        Assert.Throws<ArgumentException>(() => new SlackFileObject("F012345678", "https://example.com"));
    }

    [Fact]
    public void SlackIconObject_SerializesAndValidates()
    {
        var icon = new SlackIconObject(SlackIcon.Rocket);
        var json = SlackSerializer.Serialize(icon);
        using var doc = JsonDocument.Parse(json);
        Assert.Equal("icon", doc.RootElement.GetProperty("type").GetString());
        Assert.Equal("rocket", doc.RootElement.GetProperty("name").GetString());

        // Hyphenated names
        var thumbsUp = new SlackIconObject(SlackIcon.ThumbsUp);
        var thumbsUpJson = SlackSerializer.Serialize(thumbsUp);
        using var thumbsDoc = JsonDocument.Parse(thumbsUpJson);
        Assert.Equal("thumbs-up", thumbsDoc.RootElement.GetProperty("name").GetString());

        var deserialized = SlackSerializer.Deserialize<SlackIconObject>(thumbsUpJson);
        Assert.NotNull(deserialized);
        Assert.Equal(SlackIcon.ThumbsUp, deserialized.Name);

        var caretLeft = new SlackIconObject(SlackIcon.CaretLeft);
        var caretJson = SlackSerializer.Serialize(caretLeft);
        using var caretDoc = JsonDocument.Parse(caretJson);
        Assert.Equal("caret-left", caretDoc.RootElement.GetProperty("name").GetString());

        // Implicit operator
        SlackIconObject implicitIcon = SlackIcon.Warning;
        Assert.Equal(SlackIcon.Warning, implicitIcon.Name);

        // Property setter
        icon.Name = SlackIcon.Check;
        Assert.Equal(SlackIcon.Check, icon.Name);
    }

    [Fact]
    public void TriggerAndWorkflowObject_SerializesCorrectly()
    {
        var workflow = new WorkflowObject("https://slack.com/shortcuts/Ft0123/xyz",
            new Dictionary<string, string>
            {
                { "param_a", "val_a" },
                { "param_b", "val_b" }
            });

        var json = SlackSerializer.Serialize(workflow);
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        var trigger = root.GetProperty("trigger");
        Assert.Equal("https://slack.com/shortcuts/Ft0123/xyz", trigger.GetProperty("url").GetString());

        var paramsArray = trigger.GetProperty("customizable_input_parameters");
        Assert.Equal(2, paramsArray.GetArrayLength());
        Assert.Equal("param_a", paramsArray[0].GetProperty("name").GetString());
        Assert.Equal("val_a", paramsArray[0].GetProperty("value").GetString());

        // Secondary constructor taking List<CustomizableInputParameter>
        var paramsList = new List<CustomizableInputParameter>
        {
            new("p1", "v1")
        };
        var workflowFromList = new WorkflowObject("https://slack.com/shortcuts/Ft0123/xyz", paramsList);
        Assert.NotNull(workflowFromList.Trigger);
        Assert.Equal("https://slack.com/shortcuts/Ft0123/xyz", workflowFromList.Trigger.Url);
        Assert.Single(workflowFromList.Trigger.CustomizableInputParameters!);

        // Workflow property setter
        workflowFromList.Trigger = new TriggerObject("https://slack.com/shortcuts/Ft0123/abc");
        Assert.Equal("https://slack.com/shortcuts/Ft0123/abc", workflowFromList.Trigger.Url);

        // Trigger property setters
        var trig = workflowFromList.Trigger;
        trig.Url = "https://slack.com/shortcuts/Ft0123/updated";
        Assert.Equal("https://slack.com/shortcuts/Ft0123/updated", trig.Url);
        trig.CustomizableInputParameters = [new CustomizableInputParameter("key", "val")];
        Assert.Single(trig.CustomizableInputParameters);

        // CustomizableInputParameter property setters
        var param = trig.CustomizableInputParameters[0];
        param.Name = "new_key";
        param.Value = "new_val";
        Assert.Equal("new_key", param.Name);
        Assert.Equal("new_val", param.Value);
    }

    [Fact]
    public void TriggerObject_ThrowsWhenParametersExceed10()
    {
        var tooMany = Enumerable.Range(1, 11)
            .ToDictionary(i => $"key_{i}", i => $"val_{i}");

        Assert.Throws<ArgumentException>(() =>
            new TriggerObject("https://example.com", tooMany));

        var trig = new TriggerObject("https://example.com");
        var tooManyList = Enumerable.Range(1, 11)
            .Select(i => new CustomizableInputParameter($"key_{i}", $"val_{i}"))
            .ToList();
        Assert.Throws<ArgumentException>(() => trig.CustomizableInputParameters = tooManyList);
    }

    [Fact]
    public void TextObjects_SerializesAndValidates()
    {
        // PlainTextObject
        var plain = new PlainTextObject("Plain text", emoji: true);
        var plainJson = SlackSerializer.Serialize(plain);
        using var plainDoc = JsonDocument.Parse(plainJson);
        Assert.Equal("plain_text", plainDoc.RootElement.GetProperty("type").GetString());
        Assert.Equal("Plain text", plainDoc.RootElement.GetProperty("text").GetString());
        Assert.True(plainDoc.RootElement.GetProperty("emoji").GetBoolean());

        plain.Emoji = false;
        Assert.False(plain.Emoji);

        // Implicit operator
        PlainTextObject implicitPlain = "Implicit text";
        Assert.Equal("Implicit text", implicitPlain.Text);

        plain.Text = "Updated text";
        Assert.Equal("Updated text", plain.Text);

        // MarkdownTextObject
        var mrkdwn = new MarkdownTextObject("*Bold*", verbatim: true);
        var mrkdwnJson = SlackSerializer.Serialize(mrkdwn);
        using var mrkdwnDoc = JsonDocument.Parse(mrkdwnJson);
        Assert.Equal("mrkdwn", mrkdwnDoc.RootElement.GetProperty("type").GetString());
        Assert.Equal("*Bold*", mrkdwnDoc.RootElement.GetProperty("text").GetString());
        Assert.True(mrkdwnDoc.RootElement.GetProperty("verbatim").GetBoolean());

        mrkdwn.Verbatim = false;
        Assert.False(mrkdwn.Verbatim);

        // Implicit operator
        MarkdownTextObject implicitMrkdwn = "_Italic_";
        Assert.Equal("_Italic_", implicitMrkdwn.Text);

        // TextObject ToString
        Assert.Equal("Updated text", plain.ToString());
        Assert.Equal("*Bold*", mrkdwn.ToString());
    }
}
