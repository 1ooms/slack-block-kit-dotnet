using System.Text.Json.Serialization;
using SlackBlockKit.Blocks;

namespace SlackBlockKit.Surfaces;

/// <summary>
/// Represents an App Home tab view (<c>type: "home"</c>), published with the <c>views.publish</c> Web API method.
/// </summary>
/// <seealso href="https://docs.slack.dev/surfaces/app-home">Slack App Home Documentation</seealso>
/// <seealso href="https://docs.slack.dev/reference/views/home-tab-views">Slack Home Tab View Reference</seealso>
public class HomeSurface : ViewSurface
{
    [JsonConstructor]
    public HomeSurface(
        List<Block> blocks,
        string? privateMetadata = null,
        string? callbackId = null,
        string? externalId = null)
    {
        Blocks = blocks;
        PrivateMetadata = privateMetadata;
        CallbackId = callbackId;
        ExternalId = externalId;
    }

    public HomeSurface(params Block[] blocks)
        : this(blocks.ToList())
    {
    }

    public override string Type => "home";
}