using System.Text.Json.Serialization;
using SlackBlockKit.Elements.RichText;

namespace SlackBlockKit.Elements;

/// <summary>
/// Represents a Salesforce data field element that renders as a Salesforce data field reference.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/salesforce-data-field-element">Slack Salesforce Data Field Element Documentation</seealso>
public class SalesforceDataFieldElement : Element, IRichTextSubElement
{
    public SalesforceDataFieldElement(
        string salesforceRecordId,
        string? salesforceFieldLabel = null,
        string? salesforceFieldApiName = null,
        bool? salesforceIncludeFieldLabel = null,
        RichTextStyle? style = null)
    {
        SalesforceRecordId = salesforceRecordId;
        SalesforceFieldLabel = salesforceFieldLabel;
        SalesforceFieldApiName = salesforceFieldApiName;
        SalesforceIncludeFieldLabel = salesforceIncludeFieldLabel;
        Style = style;
    }

    [JsonPropertyName("salesforce_record_id")]
    public string SalesforceRecordId
    {
        get;
        set => field = ThrowIfNull(value);
    }

    [JsonPropertyName("salesforce_field_label")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SalesforceFieldLabel
    {
        get;
        set => field = ThrowIfEmpty(value);
    }

    [JsonPropertyName("salesforce_field_api_name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SalesforceFieldApiName
    {
        get;
        set => field = ThrowIfEmpty(value);
    }

    [JsonPropertyName("salesforce_include_field_label")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? SalesforceIncludeFieldLabel { get; set; }

    [JsonPropertyName("style")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public RichTextStyle? Style { get; set; }

    public override string Type => "salesforce_data_field";
}