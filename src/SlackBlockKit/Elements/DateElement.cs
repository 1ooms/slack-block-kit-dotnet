using System.Text.Json.Serialization;
using SlackBlockKit.Elements.RichText;

namespace SlackBlockKit.Elements;

/// <summary>
/// Provides standard format tokens for Slack <see cref="DateElement"/>.
/// A format string can be one of these tokens or a custom template combining tokens and text
/// (e.g., <c>$"{DateFormat.DateLong} at {DateFormat.Time}"</c>).
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/date-element">Slack Date Element Documentation</seealso>
public static class DateFormat
{
    /// <summary>
    /// Shows date as YYYY-MM-DD (e.g., 2014-02-18).
    /// </summary>
    public const string DateNum = "{date_num}";

    /// <summary>
    /// Shows date as DD/MM/YYYY (e.g., 18/02/2014), subject to local preferences.
    /// </summary>
    public const string DateSlash = "{date_slash}";

    /// <summary>
    /// Long-form sentence including the day-of-week, e.g., 'Monday, February 18th, 2014'.
    /// </summary>
    public const string DateLong = "{date_long}";

    /// <summary>
    /// Long-form sentence without the day-of-week, e.g., 'February 18, 2014'.
    /// </summary>
    public const string DateLongFull = "{date_long_full}";

    /// <summary>
    /// Shows 'yesterday', 'today', or 'tomorrow', otherwise uses the {date_long} format.
    /// </summary>
    public const string DateLongPretty = "{date_long_pretty}";

    /// <summary>
    /// Same as {date_long_full}, but without the year (e.g., 'February 18').
    /// </summary>
    public const string Date = "{date}";

    /// <summary>
    /// Shows 'today', 'yesterday', or 'tomorrow' if applicable; otherwise falls back to {date}.
    /// </summary>
    public const string DatePretty = "{date_pretty}";

    /// <summary>
    /// Shows short month names without day-of-week, e.g., 'Feb 18, 2014'.
    /// </summary>
    public const string DateShort = "{date_short}";

    /// <summary>
    /// Shows 'today', 'yesterday', or 'tomorrow' if applicable; otherwise falls back to {date_short}.
    /// </summary>
    public const string DateShortPretty = "{date_short_pretty}";

    /// <summary>
    /// Shows 'today', 'yesterday', or 'tomorrow' if applicable. If in the current year,
    /// uses {date_long} without the year; otherwise falls back to full {date_long}.
    /// </summary>
    public const string DayDividerPretty = "{day_divider_pretty}";

    /// <summary>
    /// Time-of-day portion using 12 or 24-hour clock preferences, e.g., '2:34 PM' or '14:34'.
    /// </summary>
    public const string Time = "{time}";

    /// <summary>
    /// Time-of-day portion including seconds, e.g., '2:34:56 PM' or '14:34:56'.
    /// </summary>
    public const string TimeSecs = "{time_secs}";

    /// <summary>
    /// A human-readable relative period, e.g., '3 minutes ago', '2 days ago'.
    /// </summary>
    public const string Ago = "{ago}";
}

/// <summary>
/// Represents a date element that displays a formatted, localized date.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/block-elements/date-element">Slack Date Element Documentation</seealso>
public class DateElement : Element, IRichTextSubElement
{
    [JsonConstructor]
    public DateElement(
        long timestamp,
        string format,
        string? timezone = null,
        string? url = null,
        string? fallback = null,
        RichTextStyle? style = null)
    {
        Timestamp = timestamp;
        Format = format;
        Timezone = timezone;
        Url = url;
        Fallback = fallback;
        Style = style;
    }

    public DateElement(
        DateTimeOffset dateTime,
        string format,
        string? timezone = null,
        string? url = null,
        string? fallback = null,
        RichTextStyle? style = null)
        : this(dateTime.ToUnixTimeSeconds(), format, timezone, url, fallback, style)
    {
    }

    [JsonPropertyName("timestamp")] public long Timestamp { get; set; }

    [JsonPropertyName("format")]
    public string Format
    {
        get;
        set => field = ThrowIfNullOrEmpty(value);
    }

    [JsonPropertyName("timezone")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Timezone { get; set; }

    [JsonPropertyName("url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Url { get; set; }

    [JsonPropertyName("fallback")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Fallback { get; set; }

    [JsonPropertyName("style")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public RichTextStyle? Style { get; set; }

    public override string Type => "date";
}