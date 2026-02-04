namespace SlackBlockKit.TextObjects;

public class RichTextObject(string text) : TextObject(text), ITableTextObject
{
    public override string Type => "rich_text";
}