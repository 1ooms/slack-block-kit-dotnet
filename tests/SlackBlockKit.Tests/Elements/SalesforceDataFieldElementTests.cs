namespace SlackBlockKit.Tests.Elements;

public class SalesforceDataFieldElementTests
{
    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var style = new RichTextStyle(bold: true);

        var element = new SalesforceDataFieldElement("001ABC", "Amount", "Amount__c", true, style);

        Assert.Equal("001ABC", element.SalesforceRecordId);
        Assert.Equal("Amount", element.SalesforceFieldLabel);
        Assert.Equal("Amount__c", element.SalesforceFieldApiName);
        Assert.True(element.SalesforceIncludeFieldLabel);
        Assert.Same(style, element.Style);
        Assert.Equal("salesforce_data_field", element.Type);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new SalesforceDataFieldElement("001ABC", "Amount", "Amount__c", true,
            new RichTextStyle(bold: true)));

        AssertProperties(json, "type", "salesforce_record_id", "salesforce_field_label",
            "salesforce_field_api_name", "salesforce_include_field_label", "style");
        Assert.Equal("salesforce_data_field", json.GetProperty("type").GetString());
    }

    [Fact]
    public void Serialize_OmitsNullFields()
    {
        AssertProperties(ToJson(new SalesforceDataFieldElement("001ABC")), "type", "salesforce_record_id");
    }

    [Fact]
    public void RecordId_ThrowsWhenNull()
    {
        Assert.Throws<ArgumentNullException>(() => new SalesforceDataFieldElement(null!));
    }

    [Fact]
    public void OptionalStrings_ThrowWhenEmpty()
    {
        Assert.Throws<ArgumentException>(() => new SalesforceDataFieldElement("001ABC", ""));
        Assert.Throws<ArgumentException>(() => new SalesforceDataFieldElement("001ABC", salesforceFieldApiName: ""));
    }
}
