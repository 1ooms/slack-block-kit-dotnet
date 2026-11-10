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
/// Base class for Slack Block Kit interactive and input elements that define an action_id.
/// </summary>
public abstract class ActionElement : Element
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

// The marker interfaces below list their allowed element types with JsonDerivedType, so deserializing an
// element type that isn't allowed in that position throws a JsonException instead of producing null.
// Only concrete types can be listed. ElementInterfaceDerivedTypesTests guards that each list is complete.

/// <summary>
/// Marks an element as valid for inclusion in an Actions block.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(ButtonElement), "button")]
[JsonDerivedType(typeof(CheckboxesElement), "checkboxes")]
[JsonDerivedType(typeof(DatePickerElement), "datepicker")]
[JsonDerivedType(typeof(DateTimePickerElement), "datetimepicker")]
[JsonDerivedType(typeof(OverflowMenuElement), "overflow")]
[JsonDerivedType(typeof(RadioButtonGroupElement), "radio_buttons")]
[JsonDerivedType(typeof(RichTextInputElement), "rich_text_input")]
[JsonDerivedType(typeof(TimePickerElement), "timepicker")]
[JsonDerivedType(typeof(WorkflowButtonElement), "workflow_button")]
[JsonDerivedType(typeof(StaticSelect), "static_select")]
[JsonDerivedType(typeof(ExternalSelect), "external_select")]
[JsonDerivedType(typeof(UsersSelect), "users_select")]
[JsonDerivedType(typeof(ConversationsSelect), "conversations_select")]
[JsonDerivedType(typeof(ChannelsSelect), "channels_select")]
[JsonDerivedType(typeof(MultiStaticSelect), "multi_static_select")]
[JsonDerivedType(typeof(MultiExternalSelect), "multi_external_select")]
[JsonDerivedType(typeof(MultiUsersSelect), "multi_users_select")]
[JsonDerivedType(typeof(MultiConversationsSelect), "multi_conversations_select")]
[JsonDerivedType(typeof(MultiChannelsSelect), "multi_channels_select")]
public interface IActionsElement;

/// <summary>
/// Marks an element as valid for inclusion in a ContextActions block.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(FeedbackButtonsElement), "feedback_buttons")]
[JsonDerivedType(typeof(IconButtonElement), "icon_button")]
public interface IContextActionsElement;

/// <summary>
/// Marks an element as valid for inclusion in an Input block.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(CheckboxesElement), "checkboxes")]
[JsonDerivedType(typeof(DatePickerElement), "datepicker")]
[JsonDerivedType(typeof(DateTimePickerElement), "datetimepicker")]
[JsonDerivedType(typeof(EmailInputElement), "email_text_input")]
[JsonDerivedType(typeof(FileInputElement), "file_input")]
[JsonDerivedType(typeof(NumberInputElement), "number_input")]
[JsonDerivedType(typeof(PlainTextInputElement), "plain_text_input")]
[JsonDerivedType(typeof(RadioButtonGroupElement), "radio_buttons")]
[JsonDerivedType(typeof(RichTextInputElement), "rich_text_input")]
[JsonDerivedType(typeof(TimePickerElement), "timepicker")]
[JsonDerivedType(typeof(UrlInputElement), "url_text_input")]
[JsonDerivedType(typeof(StaticSelect), "static_select")]
[JsonDerivedType(typeof(ExternalSelect), "external_select")]
[JsonDerivedType(typeof(UsersSelect), "users_select")]
[JsonDerivedType(typeof(ConversationsSelect), "conversations_select")]
[JsonDerivedType(typeof(ChannelsSelect), "channels_select")]
[JsonDerivedType(typeof(MultiStaticSelect), "multi_static_select")]
[JsonDerivedType(typeof(MultiExternalSelect), "multi_external_select")]
[JsonDerivedType(typeof(MultiUsersSelect), "multi_users_select")]
[JsonDerivedType(typeof(MultiConversationsSelect), "multi_conversations_select")]
[JsonDerivedType(typeof(MultiChannelsSelect), "multi_channels_select")]
public interface IInputElement;

/// <summary>
/// Marks an element as valid for inclusion as an accessory in a Section block.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(ButtonElement), "button")]
[JsonDerivedType(typeof(CheckboxesElement), "checkboxes")]
[JsonDerivedType(typeof(DatePickerElement), "datepicker")]
[JsonDerivedType(typeof(ImageElement), "image")]
[JsonDerivedType(typeof(OverflowMenuElement), "overflow")]
[JsonDerivedType(typeof(RadioButtonGroupElement), "radio_buttons")]
[JsonDerivedType(typeof(TimePickerElement), "timepicker")]
[JsonDerivedType(typeof(WorkflowButtonElement), "workflow_button")]
[JsonDerivedType(typeof(StaticSelect), "static_select")]
[JsonDerivedType(typeof(ExternalSelect), "external_select")]
[JsonDerivedType(typeof(UsersSelect), "users_select")]
[JsonDerivedType(typeof(ConversationsSelect), "conversations_select")]
[JsonDerivedType(typeof(ChannelsSelect), "channels_select")]
[JsonDerivedType(typeof(MultiStaticSelect), "multi_static_select")]
[JsonDerivedType(typeof(MultiExternalSelect), "multi_external_select")]
[JsonDerivedType(typeof(MultiUsersSelect), "multi_users_select")]
[JsonDerivedType(typeof(MultiConversationsSelect), "multi_conversations_select")]
[JsonDerivedType(typeof(MultiChannelsSelect), "multi_channels_select")]
public interface ISectionElement;

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