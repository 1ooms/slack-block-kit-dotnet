using SlackBlockKit.Blocks;
using SlackBlockKit.CompositionObjects;
using SlackBlockKit.CompositionObjects.TextObjects;
using SlackBlockKit.Elements;
using SlackBlockKit.Elements.RichText;

namespace SlackBlockKit;

/// <summary>
/// Static factory with concise helper methods for creating every Slack Block Kit layout block
/// (e.g. <c>BlockKit.Header("Hello")</c>, <c>BlockKit.Divider()</c>) without calling the block constructors directly.
/// </summary>
/// <seealso href="https://docs.slack.dev/reference/block-kit/blocks">Slack Blocks Documentation</seealso>
public static class BlockKit
{
    public static ActionsBlock Actions(params IActionsElement[] elements)
    {
        return new ActionsBlock(elements);
    }

    public static ActionsBlock Actions(List<IActionsElement> elements, string? blockId = null)
    {
        return new ActionsBlock(elements, blockId);
    }

    public static AlertBlock Alert(string text, AlertLevel level = AlertLevel.Default, string? blockId = null)
    {
        return new AlertBlock(text, level, blockId);
    }

    public static CardBlock Card(TextObject? title = null, TextObject? subtitle = null, TextObject? body = null,
        TextObject? subtext = null, ImageElement? heroImage = null, ImageElement? icon = null,
        SlackIconObject? slackIcon = null, List<ButtonElement>? actions = null, string? blockId = null)
    {
        return new CardBlock(title, subtitle, body, subtext, heroImage, icon, slackIcon, actions, blockId);
    }

    public static CarouselBlock Carousel(params CardBlock[] cards)
    {
        return new CarouselBlock(cards);
    }

    public static CarouselBlock Carousel(List<CardBlock> cards, string? blockId = null)
    {
        return new CarouselBlock(cards, blockId);
    }

    public static ContainerBlock Container(List<Block> childBlocks, PlainTextObject? title = null,
        TextObject? subtitle = null, string? blockId = null, ContainerWidth? width = null, ImageElement? icon = null,
        bool? isCollapsible = null, bool? defaultCollapsed = null, bool? hasHeaderDivider = null)
    {
        return new ContainerBlock(childBlocks, title, null, subtitle, blockId, width, icon, isCollapsible,
            defaultCollapsed, hasHeaderDivider);
    }

    public static ContextBlock Context(params IContextElement[] elements)
    {
        return new ContextBlock(elements);
    }

    public static ContextBlock Context(List<IContextElement> elements, string? blockId = null)
    {
        return new ContextBlock(elements, blockId);
    }

    public static ContextActionsBlock ContextActions(params IContextActionsElement[] elements)
    {
        return new ContextActionsBlock(elements);
    }

    public static ContextActionsBlock ContextActions(List<IContextActionsElement> elements, string? blockId = null)
    {
        return new ContextActionsBlock(elements, blockId);
    }

    public static DataTableBlock DataTable(string caption, List<List<IDataTableCell>> rows, int? pageSize = null,
        int? rowHeaderColumnIndex = null, string? blockId = null)
    {
        return new DataTableBlock(caption, rows, pageSize, rowHeaderColumnIndex, blockId);
    }

    public static DataVisualizationBlock DataVisualization(string title, Chart chart, string? blockId = null)
    {
        return new DataVisualizationBlock(title, chart, blockId);
    }

    public static DividerBlock Divider(string? blockId = null)
    {
        return new DividerBlock(blockId);
    }

    public static FileBlock File(string externalId, string source = "remote", string? blockId = null)
    {
        return new FileBlock(externalId, source, blockId);
    }

    public static HeaderBlock Header(string text, string? blockId = null, int? level = null)
    {
        return new HeaderBlock(text, blockId, level);
    }

    public static ImageBlock Image(string imageUrl, string altText, string? title = null, string? blockId = null)
    {
        return ImageBlock.FromImageUrl(altText, imageUrl, title, blockId);
    }

    public static InputBlock Input(string label, IInputElement element, string? blockId = null, string? hint = null,
        bool? optional = null, bool? dispatchAction = null)
    {
        return new InputBlock(label, element, blockId, dispatchAction, hint, optional);
    }

    public static MarkdownBlock Markdown(string text, string? blockId = null)
    {
        return new MarkdownBlock(text, blockId);
    }

    public static PlanBlock Plan(string title, params TaskCardBlock[] tasks)
    {
        return new PlanBlock(title, tasks);
    }

    public static PlanBlock Plan(string title, List<TaskCardBlock> tasks, string? blockId = null)
    {
        return new PlanBlock(title, tasks, blockId);
    }

    public static RichTextBlock RichText(params RichTextElement[] elements)
    {
        return new RichTextBlock(elements.ToList());
    }

    public static RichTextBlock RichText(List<RichTextElement> elements, string? blockId = null)
    {
        return new RichTextBlock(elements, blockId);
    }

    public static SectionBlock Section(string markdownText, ISectionElement? accessory = null, string? blockId = null)
    {
        return new SectionBlock(markdownText, accessory, blockId);
    }

    public static SectionBlock Section(TextObject text, List<TextObject>? fields = null,
        ISectionElement? accessory = null, string? blockId = null)
    {
        return new SectionBlock(text, fields, accessory, null, blockId);
    }

    public static TableBlock Table(List<List<ITableCell>> rows, List<TableBlock.ColumnSetting>? columnSettings = null,
        string? blockId = null)
    {
        return new TableBlock(rows, columnSettings, blockId);
    }

    public static TaskCardBlock TaskCard(string taskId, string title, TaskCardStatus status,
        RichTextBlock? details = null, RichTextBlock? output = null, List<UrlSourceElement>? sources = null,
        SlackIcon? icon = null, bool? hideTitle = null, string? blockId = null)
    {
        return new TaskCardBlock(taskId, title, status, details, output, sources, icon, hideTitle, blockId);
    }

    public static VideoBlock Video(string altText, string title, string thumbnailUrl, string videoUrl,
        string? titleUrl = null, string? providerName = null, string? blockId = null)
    {
        return new VideoBlock(altText, title, thumbnailUrl, videoUrl, titleUrl, providerName, blockId);
    }
}

public static class Text
{
    public static PlainTextObject Plain(string text, bool? emoji = null)
    {
        return new PlainTextObject(text, emoji);
    }

    public static MarkdownTextObject Markdown(string text, bool? verbatim = null)
    {
        return new MarkdownTextObject(text, verbatim);
    }
}