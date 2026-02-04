namespace SlackBlockKit.TextObjects;

public class MarkdownTextObject(string text) : TextObject(text)
{
    public override string Type => "mrkdwn";
}