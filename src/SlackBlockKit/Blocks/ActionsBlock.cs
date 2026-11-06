using System.Text.Json.Serialization;
using SlackBlockKit.Elements;

namespace SlackBlockKit.Blocks;

/// <summary>
/// Represents an actions block (<c>type: "actions"</c>) that holds interactive elements such as buttons, select menus, and date pickers.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/blocks/actions-block">Slack Actions Block Documentation</seealso>
public class ActionsBlock : Block
{
    private const int MaxElements = 25;

    [JsonConstructor]
    public ActionsBlock(List<IActionsElement> elements, string? blockId = null)
    {
        Elements = elements;
        BlockId = blockId;
    }

    public ActionsBlock(params IActionsElement[] elements)
        : this(elements.ToList())
    {
    }

    public override string Type => "actions";

    [JsonPropertyName("elements")]
    public List<IActionsElement> Elements
    {
        get;
        set => field = ThrowIfNullOrEmptyOrExceedsCount(value, MaxElements);
    }
}