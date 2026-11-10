namespace SlackBlockKit.Tests.Elements;

public class OptionValidationTests
{
    private static List<OptionGroupObject> Groups(params string[] values) =>
        [new OptionGroupObject("Group", values.Select(v => Option(v)).ToList())];

    [Fact]
    public void ValidateOptionsAndOptionGroups_ThrowsWhenNeitherProvided()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            OptionValidation.ValidateOptionsAndOptionGroups(null, null, "Target"));

        Assert.Contains("for Target", ex.Message);
    }

    [Fact]
    public void ValidateOptionsAndOptionGroups_ThrowsWhenBothProvided()
    {
        Assert.Throws<ArgumentException>(() =>
            OptionValidation.ValidateOptionsAndOptionGroups([Option()], Groups("a")));
    }

    [Fact]
    public void ValidateOptionsAndOptionGroups_AcceptsEitherOne()
    {
        OptionValidation.ValidateOptionsAndOptionGroups([Option()], null);
        OptionValidation.ValidateOptionsAndOptionGroups(null, Groups("a"));
    }

    [Fact]
    public void ValidateOptionsAndOptionGroups_OmitsTargetWhenNull()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            OptionValidation.ValidateOptionsAndOptionGroups(null, null));

        Assert.DoesNotContain(" for ", ex.Message);
    }

    [Fact]
    public void ValidateInitialOption_AllowsNull()
    {
        OptionValidation.ValidateInitialOption([Option()], null, null);
    }

    [Fact]
    public void ValidateInitialOption_AcceptsMatchingOptionInOptionsOrGroups()
    {
        OptionValidation.ValidateInitialOption([Option("a")], null, Option("a"));
        OptionValidation.ValidateInitialOption(null, Groups("a", "b"), Option("b"));
    }

    [Fact]
    public void ValidateInitialOption_ThrowsWhenNotFound()
    {
        Assert.Throws<ArgumentException>(() =>
            OptionValidation.ValidateInitialOption([Option("a")], null, Option("b")));
        Assert.Throws<ArgumentException>(() =>
            OptionValidation.ValidateInitialOption(null, Groups("a"), Option("b")));
    }

    [Fact]
    public void ValidateInitialOptions_ValidatesEveryOption()
    {
        OptionValidation.ValidateInitialOptions([Option("a"), Option("b")], null, [Option("a"), Option("b")]);
        OptionValidation.ValidateInitialOptions([Option("a")], null, null);

        Assert.Throws<ArgumentException>(() =>
            OptionValidation.ValidateInitialOptions([Option("a")], null, [Option("a"), Option("c")]));
    }

    [Fact]
    public void ContainsOption_ReturnsFalseWithoutOptions()
    {
        Assert.False(OptionValidation.ContainsOption(null, null, Option()));
    }

    [Fact]
    public void OptionMatches_MatchesSameInstance()
    {
        var option = Option();

        Assert.True(OptionValidation.OptionMatches(option, option));
    }

    [Fact]
    public void OptionMatches_ComparesValueAndText()
    {
        Assert.True(OptionValidation.OptionMatches(new TextOptionObject("A", "1"), new TextOptionObject("A", "1")));
        Assert.False(OptionValidation.OptionMatches(new TextOptionObject("A", "1"), new TextOptionObject("A", "2")));
        Assert.False(OptionValidation.OptionMatches(new TextOptionObject("A", "1"), new TextOptionObject("B", "1")));
    }

    [Fact]
    public void OptionMatches_ComparesTextType()
    {
        Assert.False(OptionValidation.OptionMatches(
            new TextOptionObject(new PlainTextObject("A"), "1"), new TextOptionObject(new MarkdownTextObject("A"), "1")));
        Assert.True(OptionValidation.OptionMatches(
            new TextOptionObject(new MarkdownTextObject("A"), "1"), new TextOptionObject(new MarkdownTextObject("A"), "1")));
    }

    [Fact]
    public void OptionMatches_ComparesDescription()
    {
        Assert.True(OptionValidation.OptionMatches(new TextOptionObject("A", "1", "d"), new TextOptionObject("A", "1", "d")));
        Assert.False(OptionValidation.OptionMatches(new TextOptionObject("A", "1", "d"), new TextOptionObject("A", "1", "e")));
        Assert.False(OptionValidation.OptionMatches(new TextOptionObject("A", "1", "d"), new TextOptionObject("A", "1")));
        Assert.False(OptionValidation.OptionMatches(new TextOptionObject("A", "1"), new TextOptionObject("A", "1", "d")));
    }

    [Fact]
    public void InitialOption_ThrowsWhenOnlyDescriptionDiffers()
    {
        Assert.Throws<ArgumentException>(() => new StaticSelect(
            options: [new PlainTextOptionObject("A", "1", "d")], initialOption: new PlainTextOptionObject("A", "1", "other")));
        Assert.Throws<ArgumentException>(() => new RadioButtonGroupElement(
            [new TextOptionObject("A", "1", "d")], initialOption: new TextOptionObject("A", "1")));
        Assert.Throws<ArgumentException>(() => new CheckboxesElement(
            [new TextOptionObject("A", "1", "d")], initialOptions: [new TextOptionObject("A", "1")]));
    }
}
