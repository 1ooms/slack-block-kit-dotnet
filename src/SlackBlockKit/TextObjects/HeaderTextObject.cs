namespace SlackBlockKit.TextObjects;

public class HeaderTextObject(string text) : TextObject(text)
{
    public override string Type => "plain_text";
}