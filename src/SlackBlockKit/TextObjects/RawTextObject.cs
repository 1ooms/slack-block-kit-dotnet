namespace SlackBlockKit.TextObjects;

public class RawTextObject(string text) : TextObject(text), ITableTextObject
{
    public override string Type => "raw_text";
}