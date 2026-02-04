using System.Text.Json.Serialization;

namespace SlackBlockKit.Blocks;

// IMPORTANT NOTE: this block is broken in the Slack API as of 2025-12-04, see GitHub issue https://github.com/slackapi/slack-github-action/issues/440
public class MarkdownBlock(string text) : Block
{
    [JsonPropertyName("text")] public string Text { get; set; } = text;

    public override string Type => "markdown";
}