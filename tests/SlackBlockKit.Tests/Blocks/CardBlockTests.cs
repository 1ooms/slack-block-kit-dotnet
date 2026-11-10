namespace SlackBlockKit.Tests.Blocks;

public class CardBlockTests
{
    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var hero = Image();
        var actions = new List<ButtonElement> { new("Go") };

        var block = new CardBlock(new PlainTextObject("Title"), new PlainTextObject("Subtitle"),
            new MarkdownTextObject("Body"), new PlainTextObject("Subtext"), hero, null, SlackIcon.Star, actions,
            "card_1");

        Assert.Equal("Title", block.Title!.Text);
        Assert.Equal("Subtitle", block.Subtitle!.Text);
        Assert.Equal("Body", block.Body!.Text);
        Assert.Equal("Subtext", block.Subtext!.Text);
        Assert.Same(hero, block.HeroImage);
        Assert.Null(block.Icon);
        Assert.Equal(SlackIcon.Star, block.SlackIcon!.Name);
        Assert.Same(actions, block.Actions);
        Assert.Equal("card_1", block.BlockId);
        Assert.Equal("card", block.Type);
    }

    [Fact]
    public void Serialize_WritesAllFieldsWithNestedDiscriminators()
    {
        var json = ToJson(new CardBlock(new PlainTextObject("Title"), new PlainTextObject("Subtitle"),
            new MarkdownTextObject("Body"), new PlainTextObject("Subtext"), Image(), null, SlackIcon.Star,
            [new ButtonElement("Go")], "card_1"));

        AssertProperties(json, "type", "title", "subtitle", "body", "subtext", "hero_image", "slack_icon",
            "actions", "block_id");
        Assert.Equal("card", json.GetProperty("type").GetString());
        Assert.Equal("image", json.GetProperty("hero_image").GetProperty("type").GetString());
        Assert.Equal("icon", json.GetProperty("slack_icon").GetProperty("type").GetString());
        Assert.Equal("star", json.GetProperty("slack_icon").GetProperty("name").GetString());
        Assert.Equal("button", json.GetProperty("actions")[0].GetProperty("type").GetString());
    }

    [Fact]
    public void Serialize_WithIcon_WritesImageElement()
    {
        var json = ToJson(new CardBlock(title: new PlainTextObject("Title"), icon: Image()));

        Assert.Equal("image", json.GetProperty("icon").GetProperty("type").GetString());
        AssertNoProperty(json, "slack_icon");
    }

    [Fact]
    public void Serialize_OmitsNullFields()
    {
        AssertProperties(ToJson(new CardBlock(body: new PlainTextObject("Body"))), "type", "body");
    }

    [Fact]
    public void Constructor_ThrowsWithoutRequiredContent()
    {
        Assert.Throws<ArgumentException>(() => new CardBlock());
        Assert.Throws<ArgumentException>(() => new CardBlock(subtitle: new PlainTextObject("Subtitle")));
        Assert.ThrowsAny<ArgumentException>(() => new CardBlock(actions: []));
    }

    [Fact]
    public void Constructor_AcceptsEachRequiredContentAlone()
    {
        Assert.NotNull(new CardBlock(title: new PlainTextObject("T")));
        Assert.NotNull(new CardBlock(body: new PlainTextObject("B")));
        Assert.NotNull(new CardBlock(heroImage: Image()));
        Assert.NotNull(new CardBlock(actions: [new ButtonElement("Go")]));
    }

    [Fact]
    public void Constructor_ThrowsWhenBothIconsProvided()
    {
        Assert.Throws<ArgumentException>(() =>
            new CardBlock(title: new PlainTextObject("T"), icon: Image(), slackIcon: SlackIcon.Star));
    }

    [Fact]
    public void Setters_EnforceRulesAfterConstruction()
    {
        var block = new CardBlock(title: new PlainTextObject("T"), icon: Image());

        Assert.Throws<ArgumentException>(() => block.SlackIcon = SlackIcon.Star);
        Assert.Throws<ArgumentException>(() => block.Title = null);

        block.Icon = null;
        block.SlackIcon = SlackIcon.Star;
        block.Body = new PlainTextObject("B");
        block.Title = null;

        Assert.Null(block.Icon);
        Assert.Equal(SlackIcon.Star, block.SlackIcon!.Name);
        Assert.Null(block.Title);
        Assert.Throws<ArgumentException>(() => block.Body = null);
    }

    [Fact]
    public void Initializer_ThrowsWhenBothIconsEndUpSet()
    {
        Assert.Throws<ArgumentException>(() =>
            new CardBlock(title: new PlainTextObject("T"), icon: Image()) { SlackIcon = SlackIcon.Star });
        Assert.Throws<ArgumentException>(() =>
            new CardBlock(title: new PlainTextObject("T"), slackIcon: SlackIcon.Star) { Icon = Image() });
    }

    [Fact]
    public void Initializer_AllowsSwappingOrClearingIcons()
    {
        var block = new CardBlock(title: new PlainTextObject("T")) { SlackIcon = SlackIcon.Star };
        Assert.Equal(SlackIcon.Star, block.SlackIcon!.Name);

        Assert.Null(new CardBlock(title: new PlainTextObject("T"), icon: Image()) { Icon = null }.Icon);
    }

    [Fact]
    public void Initializer_ThrowsWhenRequiredContentIsCleared()
    {
        Assert.Throws<ArgumentException>(() => new CardBlock(title: new PlainTextObject("T")) { Title = null });
        Assert.Throws<ArgumentException>(() => new CardBlock(body: new PlainTextObject("B")) { Body = null });
        Assert.Throws<ArgumentException>(() => new CardBlock(heroImage: Image()) { HeroImage = null });
        Assert.Throws<ArgumentException>(() => new CardBlock(actions: [new ButtonElement("Go")]) { Actions = null });
    }

    [Fact]
    public void Initializer_AllowsClearingOneRequiredContentWhenAnotherRemains()
    {
        var block = new CardBlock(title: new PlainTextObject("T"), body: new PlainTextObject("B")) { Title = null };

        Assert.Null(block.Title);
        Assert.Equal("B", block.Body!.Text);
    }

    [Fact]
    public void Deserialize_RoundTripsCard()
    {
        var json = SlackSerializer.Serialize(new CardBlock(title: new PlainTextObject("T"), slackIcon: SlackIcon.Star));
        var block = SlackSerializer.Deserialize<CardBlock>(json)!;

        Assert.Equal("T", block.Title!.Text);
        Assert.Equal(SlackIcon.Star, block.SlackIcon!.Name);
    }

    [Fact]
    public void TextLengths_AreValidated()
    {
        Assert.NotNull(new CardBlock(new PlainTextObject(Chars(150)), new PlainTextObject(Chars(150)),
            new PlainTextObject(Chars(200)), new PlainTextObject(Chars(200))));
        Assert.Throws<ArgumentException>(() => new CardBlock(title: new PlainTextObject(Chars(151))));
        Assert.Throws<ArgumentException>(() => new CardBlock(body: new PlainTextObject(Chars(201))));
        Assert.Throws<ArgumentException>(() =>
            new CardBlock(title: new PlainTextObject("T"), subtitle: new PlainTextObject(Chars(151))));
        Assert.Throws<ArgumentException>(() =>
            new CardBlock(title: new PlainTextObject("T"), subtext: new PlainTextObject(Chars(201))));
    }

    [Fact]
    public void Actions_AllowsUpTo3()
    {
        var actions = Enumerable.Range(0, 3).Select(_ => new ButtonElement("Go")).ToList();

        Assert.Equal(3, new CardBlock(actions: actions).Actions!.Count);

        actions.Add(new ButtonElement("Go"));
        Assert.Throws<ArgumentException>(() => new CardBlock(actions: actions));
    }
}
