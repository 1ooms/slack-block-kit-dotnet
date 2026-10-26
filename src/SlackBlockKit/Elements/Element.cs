using System.Text.Json;
using System.Text.Json.Serialization;
using SlackBlockKit.CompositionObjects.TextObjects;
using SlackBlockKit.Elements.MultiSelectMenu;
using SlackBlockKit.Elements.SelectMenu;

namespace SlackBlockKit.Elements;

/// <summary>
/// Base class for all Slack Block Kit elements.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements">Slack Block Elements Documentation</seealso>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(AttachmentMentionElement), "attachment_mention")]
[JsonDerivedType(typeof(BroadcastElement), "broadcast")]
[JsonDerivedType(typeof(ButtonElement), "button")]
[JsonDerivedType(typeof(CanvasElement), "canvas")]
[JsonDerivedType(typeof(CanvasMessageUnfurlElement), "canvas_message_unfurl")]
[JsonDerivedType(typeof(CanvasUserMentionElement), "canvas_user_mention")]
[JsonDerivedType(typeof(ChannelElement), "channel")]
[JsonDerivedType(typeof(CheckboxesElement), "checkboxes")]
[JsonDerivedType(typeof(CitationElement), "citation")]
[JsonDerivedType(typeof(ColorElement), "color")]
[JsonDerivedType(typeof(DateElement), "date")]
[JsonDerivedType(typeof(DatePickerElement), "datepicker")]
[JsonDerivedType(typeof(DateTimePickerElement), "datetimepicker")]
[JsonDerivedType(typeof(EmailInputElement), "email_text_input")]
[JsonDerivedType(typeof(EmojiElement), "emoji")]
[JsonDerivedType(typeof(FeedbackButtonsElement), "feedback_buttons")]
[JsonDerivedType(typeof(FileElement), "file")]
[JsonDerivedType(typeof(FileInputElement), "file_input")]
[JsonDerivedType(typeof(IconButtonElement), "icon_button")]
[JsonDerivedType(typeof(ImageElement), "image")]
[JsonDerivedType(typeof(LinkElement), "link")]
[JsonDerivedType(typeof(ListRecordElement), "list_record")]
[JsonDerivedType(typeof(MessageMentionElement), "message_mention")]
[JsonDerivedType(typeof(NumberInputElement), "number_input")]
[JsonDerivedType(typeof(OverflowMenuElement), "overflow")]
[JsonDerivedType(typeof(PlainTextInputElement), "plain_text_input")]
[JsonDerivedType(typeof(RadioButtonGroupElement), "radio_buttons")]
[JsonDerivedType(typeof(RichTextInputElement), "rich_text_input")]
[JsonDerivedType(typeof(SalesforceDataFieldElement), "salesforce_data_field")]
[JsonDerivedType(typeof(TagElement), "tag")]
[JsonDerivedType(typeof(TeamElement), "team")]
[JsonDerivedType(typeof(TextElement), "text")]
[JsonDerivedType(typeof(TimePickerElement), "timepicker")]
[JsonDerivedType(typeof(UrlInputElement), "url_text_input")]
[JsonDerivedType(typeof(UrlSourceElement), "url")]
[JsonDerivedType(typeof(UserElement), "user")]
[JsonDerivedType(typeof(UsergroupElement), "usergroup")]
[JsonDerivedType(typeof(WorkObjectMentionElement), "work_object_mention")]
[JsonDerivedType(typeof(WorkflowButtonElement), "workflow_button")]
[JsonDerivedType(typeof(WorkflowMentionElement), "workflow_mention")]
// Multi-Select menu Elements
[JsonDerivedType(typeof(MultiStaticSelect), "multi_static_select")]
[JsonDerivedType(typeof(MultiExternalSelect), "multi_external_select")]
[JsonDerivedType(typeof(MultiUsersSelect), "multi_users_select")]
[JsonDerivedType(typeof(MultiConversationsSelect), "multi_conversations_select")]
[JsonDerivedType(typeof(MultiChannelsSelect), "multi_channels_select")]
// Select menu Elements
[JsonDerivedType(typeof(StaticSelect), "static_select")]
[JsonDerivedType(typeof(ExternalSelect), "external_select")]
[JsonDerivedType(typeof(UsersSelect), "users_select")]
[JsonDerivedType(typeof(ConversationsSelect), "conversations_select")]
[JsonDerivedType(typeof(ChannelsSelect), "channels_select")]
public abstract class Element
{
    [JsonIgnore] public abstract string Type { get; }
}

/// <summary>
/// Defines an element that has an interactive or input action identifier.
/// </summary>
public interface IActionElement
{
    string? ActionId { get; set; }
}

/// <summary>
/// Base class for Slack Block Kit interactive and input elements that define an action_id.
/// </summary>
public abstract class ActionElement : Element, IActionElement
{
    private const int MaxActionIdLength = 255;

    [JsonPropertyName("action_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ActionId
    {
        get;
        set
        {
            if (value != null && value.Length > MaxActionIdLength)
                throw new ArgumentException($"ActionId cannot be longer than {MaxActionIdLength} characters.",
                    nameof(value));

            field = value;
        }
    }
}

/// <summary>
/// Marks an element as valid for inclusion in an Actions block.
/// </summary>
[JsonConverter(typeof(InterfaceElementConverter<IActionsElement>))]
public interface IActionsElement;

/// <summary>
/// Marks an element as valid for inclusion in a ContextActions block.
/// </summary>
[JsonConverter(typeof(InterfaceElementConverter<IContextActionsElement>))]
public interface IContextActionsElement;

/// <summary>
/// Marks an element as valid for inclusion in an Input block.
/// </summary>
[JsonConverter(typeof(InterfaceElementConverter<IInputElement>))]
public interface IInputElement;

/// <summary>
/// Marks an element as valid for inclusion as an accessory in a Section block.
/// </summary>
[JsonConverter(typeof(InterfaceElementConverter<ISectionElement>))]
public interface ISectionElement;

/// <summary>
/// Converter for interfaces whose implementors all derive from <see cref="Element"/>.
/// </summary>
/// <remarks>
/// <para>
/// Slack Block Kit specifies that blocks can only contain certain element types (e.g., an Actions block
/// can only contain interactive elements). Rather than duplicating dozens of <see cref="JsonDerivedTypeAttribute"/>
/// declarations across every marker interface (<see cref="IActionsElement"/>, <see cref="IInputElement"/>, etc.),
/// this converter delegates serialization and deserialization to the root <see cref="Element"/> polymorphic
/// type hierarchy, then casts the result to <typeparamref name="T"/>.
/// </para>
/// </remarks>
public class InterfaceElementConverter<T> : JsonConverter<T> where T : class
{
    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return JsonSerializer.Deserialize<Element>(ref reader, options) as T;
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, (Element)(object)value, options);
    }
}

/// <summary>
/// Marks an element or text object as valid for inclusion in a Context block.
/// </summary>
/// <remarks>
/// <para>
/// In Slack's Block Kit specification, a Context block accepts both
/// <c>image</c> elements (<see cref="ImageElement"/>) and text composition objects (<see cref="TextObject"/>).
/// The below interface applies <see cref="JsonDerivedTypeAttribute"/> directly for both, eliminating the need
/// for a custom converter.
/// </para>
/// </remarks>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(ImageElement), "image")]
[JsonDerivedType(typeof(PlainTextObject), "plain_text")]
[JsonDerivedType(typeof(MarkdownTextObject), "mrkdwn")]
public interface IContextElement;