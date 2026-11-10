namespace SlackBlockKit.Tests;

public class ValidationTests
{
    [Fact]
    public void Validation_ThrowIfNull_ThrowsOnNullAndReturnsNonNull()
    {
        string? nullString = null;
        Assert.Throws<ArgumentNullException>(() => Validation.ThrowIfNull(nullString));

        var validObj = new object();
        Assert.Same(validObj, Validation.ThrowIfNull(validObj));
    }

    [Fact]
    public void Validation_PropagatesPropertyNameAsParamName()
    {
        var nullEx = Assert.Throws<ArgumentNullException>(() => Validation.ThrowIfNull<string>(null, "Prop"));
        Assert.Equal("Prop", nullEx.ParamName);

        var ex = Assert.Throws<ArgumentException>(() => Validation.ThrowIfEmpty("", "Prop"));
        Assert.Equal("Prop", ex.ParamName);
        Assert.StartsWith("Prop cannot be an empty string", ex.Message);
    }

    [Fact]
    public void Validation_ThrowIfEmpty_AllowsNullAndNonEmpty()
    {
        Assert.Null(Validation.ThrowIfEmpty(null));
        Assert.Equal("valid", Validation.ThrowIfEmpty("valid"));
    }

    [Fact]
    public void Validation_ThrowIfEmpty_ThrowsOnEmptyString()
    {
        var ex = Assert.Throws<ArgumentException>(() => Validation.ThrowIfEmpty(""));
        Assert.Contains("cannot be an empty string", ex.Message);
    }

    [Fact]
    public void Validation_String_ThrowIfNullOrEmpty_WorksCorrectly()
    {
        Assert.Throws<ArgumentNullException>(() => Validation.ThrowIfNullOrEmpty((string?)null));
        Assert.Throws<ArgumentException>(() => Validation.ThrowIfNullOrEmpty(""));
        Assert.Equal("valid", Validation.ThrowIfNullOrEmpty("valid"));
    }

    [Fact]
    public void Validation_String_ThrowIfExceedsLength_WorksCorrectly()
    {
        Assert.Null(Validation.ThrowIfExceedsLength((string?)null, 5));
        Assert.Equal("", Validation.ThrowIfExceedsLength("", 5));
        Assert.Equal("12345", Validation.ThrowIfExceedsLength("12345", 5));

        var ex = Assert.Throws<ArgumentException>(() => Validation.ThrowIfExceedsLength("123456", 5));
        Assert.Contains("cannot exceed 5 characters", ex.Message);
    }

    [Fact]
    public void Validation_ThrowIfEmptyOrExceedsLength_WorksCorrectly()
    {
        Assert.Null(Validation.ThrowIfEmptyOrExceedsLength(null, 10));
        Assert.Equal("abc", Validation.ThrowIfEmptyOrExceedsLength("abc", 10));

        var emptyEx = Assert.Throws<ArgumentException>(() => Validation.ThrowIfEmptyOrExceedsLength("", 10));
        Assert.Contains("cannot be an empty string", emptyEx.Message);

        var lengthEx = Assert.Throws<ArgumentException>(() => Validation.ThrowIfEmptyOrExceedsLength("toolongstring", 5));
        Assert.Contains("cannot exceed 5 characters", lengthEx.Message);
    }

    [Fact]
    public void Validation_ThrowIfNullOrEmptyOrExceedsLength_WorksCorrectly()
    {
        Assert.Throws<ArgumentNullException>(() => Validation.ThrowIfNullOrEmptyOrExceedsLength(null, 10));

        var emptyEx = Assert.Throws<ArgumentException>(() => Validation.ThrowIfNullOrEmptyOrExceedsLength("", 10));
        Assert.Contains("cannot be an empty string", emptyEx.Message);

        var lengthEx = Assert.Throws<ArgumentException>(() => Validation.ThrowIfNullOrEmptyOrExceedsLength("toolongstring", 5));
        Assert.Contains("cannot exceed 5 characters", lengthEx.Message);

        Assert.Equal("valid", Validation.ThrowIfNullOrEmptyOrExceedsLength("valid", 10));
    }

    [Fact]
    public void Validation_String_ThrowIfNullOrExceedsLength_WorksCorrectly()
    {
        string? nullString = null;
        Assert.Throws<ArgumentNullException>(() => Validation.ThrowIfNullOrExceedsLength(nullString, 10));

        var ex = Assert.Throws<ArgumentException>(() => Validation.ThrowIfNullOrExceedsLength("123456", 5));
        Assert.Contains("cannot exceed 5 characters", ex.Message);

        Assert.Equal("", Validation.ThrowIfNullOrExceedsLength("", 5));
        Assert.Equal("abc", Validation.ThrowIfNullOrExceedsLength("abc", 5));
    }

    [Fact]
    public void Validation_TextObject_ThrowIfExceedsLength_WorksCorrectly()
    {
        PlainTextObject? nullText = null;
        Assert.Null(Validation.ThrowIfExceedsLength(nullText, 10));

        var validText = new PlainTextObject("short");
        Assert.Same(validText, Validation.ThrowIfExceedsLength(validText, 10));

        var tooLongText = new PlainTextObject("12345678901");
        var ex = Assert.Throws<ArgumentException>(() => Validation.ThrowIfExceedsLength(tooLongText, 10));
        Assert.Contains("cannot exceed 10 characters", ex.Message);
    }

    [Fact]
    public void Validation_TextObject_ThrowIfNullOrExceedsLength_WorksCorrectly()
    {
        PlainTextObject? nullText = null;
        Assert.Throws<ArgumentNullException>(() => Validation.ThrowIfNullOrExceedsLength(nullText, 10));

        var tooLongText = new PlainTextObject("12345678901");
        var ex = Assert.Throws<ArgumentException>(() => Validation.ThrowIfNullOrExceedsLength(tooLongText, 10));
        Assert.Contains("cannot exceed 10 characters", ex.Message);

        var validText = new PlainTextObject("valid");
        Assert.Same(validText, Validation.ThrowIfNullOrExceedsLength(validText, 10));
    }

    [Fact]
    public void Validation_Collection_ThrowIfEmpty_WorksCorrectly()
    {
        List<string>? nullList = null;
        Assert.Null(Validation.ThrowIfEmpty(nullList));

        var nonEmptyList = new List<string> { "item" };
        Assert.Same(nonEmptyList, Validation.ThrowIfEmpty(nonEmptyList));

        var emptyList = new List<string>();
        var ex = Assert.Throws<ArgumentException>(() => Validation.ThrowIfEmpty(emptyList));
        Assert.Contains("cannot be empty", ex.Message);
    }

    [Fact]
    public void Validation_Collection_ThrowIfNullOrEmpty_WorksCorrectly()
    {
        List<string>? nullList = null;
        Assert.Throws<ArgumentNullException>(() => Validation.ThrowIfNullOrEmpty(nullList));

        var emptyList = new List<string>();
        var ex = Assert.Throws<ArgumentException>(() => Validation.ThrowIfNullOrEmpty(emptyList));
        Assert.Contains("cannot be empty", ex.Message);

        var validList = new List<string> { "a", "b" };
        Assert.Same(validList, Validation.ThrowIfNullOrEmpty(validList));
    }

    [Fact]
    public void Validation_Collection_ThrowIfExceedsCount_WorksCorrectly()
    {
        List<string>? nullList = null;
        Assert.Null(Validation.ThrowIfExceedsCount(nullList, 5));

        var list = new List<int> { 1, 2, 3 };
        Assert.Same(list, Validation.ThrowIfExceedsCount(list, 3));

        var ex = Assert.Throws<ArgumentException>(() => Validation.ThrowIfExceedsCount(list, 2));
        Assert.Contains("cannot contain more than 2 items", ex.Message);
    }

    [Fact]
    public void Validation_Collection_ThrowIfEmptyOrExceedsCount_WorksCorrectly()
    {
        List<int>? nullList = null;
        Assert.Null(Validation.ThrowIfEmptyOrExceedsCount(nullList, 5));

        var emptyList = new List<int>();
        var emptyEx = Assert.Throws<ArgumentException>(() => Validation.ThrowIfEmptyOrExceedsCount(emptyList, 5));
        Assert.Contains("cannot be empty", emptyEx.Message);

        var tooLongList = new List<int> { 1, 2, 3, 4 };
        var lengthEx = Assert.Throws<ArgumentException>(() => Validation.ThrowIfEmptyOrExceedsCount(tooLongList, 3));
        Assert.Contains("cannot contain more than 3 items", lengthEx.Message);

        var validList = new List<int> { 1, 2 };
        Assert.Same(validList, Validation.ThrowIfEmptyOrExceedsCount(validList, 3));
    }

    [Fact]
    public void Validation_Collection_ThrowIfNullOrEmptyOrExceedsCount_WorksCorrectly()
    {
        List<int>? nullList = null;
        Assert.Throws<ArgumentNullException>(() => Validation.ThrowIfNullOrEmptyOrExceedsCount(nullList, 5));

        var emptyList = new List<int>();
        var emptyEx = Assert.Throws<ArgumentException>(() => Validation.ThrowIfNullOrEmptyOrExceedsCount(emptyList, 5));
        Assert.Contains("cannot be empty", emptyEx.Message);

        var tooLongList = new List<int> { 1, 2, 3, 4 };
        var lengthEx = Assert.Throws<ArgumentException>(() => Validation.ThrowIfNullOrEmptyOrExceedsCount(tooLongList, 3));
        Assert.Contains("cannot contain more than 3 items", lengthEx.Message);

        var validList = new List<int> { 1, 2 };
        Assert.Same(validList, Validation.ThrowIfNullOrEmptyOrExceedsCount(validList, 3));
    }

    [Fact]
    public void Validation_Collection_ThrowIfNullOrExceedsCount_WorksCorrectly()
    {
        List<int>? nullList = null;
        Assert.Throws<ArgumentNullException>(() => Validation.ThrowIfNullOrExceedsCount(nullList, 5));

        var emptyList = new List<int>();
        Assert.Same(emptyList, Validation.ThrowIfNullOrExceedsCount(emptyList, 5));

        var tooLongList = new List<int> { 1, 2, 3, 4 };
        var ex = Assert.Throws<ArgumentException>(() => Validation.ThrowIfNullOrExceedsCount(tooLongList, 3));
        Assert.Contains("cannot contain more than 3 items", ex.Message);

        var validList = new List<int> { 1, 2, 3 };
        Assert.Same(validList, Validation.ThrowIfNullOrExceedsCount(validList, 3));
    }

    [Fact]
    public void Validation_ThrowIfNotValidUrl_AllowsNullAndValidUrls()
    {
        Assert.Null(Validation.ThrowIfNotValidUrl(null));
        Assert.Equal("https://example.com", Validation.ThrowIfNotValidUrl("https://example.com"));
        Assert.Equal("http://localhost:5000/api", Validation.ThrowIfNotValidUrl("http://localhost:5000/api"));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("ftp://example.com")]
    [InlineData("not-a-url")]
    [InlineData("example.com")]
    [InlineData("http://")]
    public void Validation_ThrowIfNotValidUrl_ThrowsOnInvalidUrls(string invalidUrl)
    {
        var ex = Assert.Throws<ArgumentException>(() => Validation.ThrowIfNotValidUrl(invalidUrl));
        Assert.Contains("must be a valid URL", ex.Message);
    }

    [Fact]
    public void Validation_ThrowIfNotValidHttpsUrl_AllowsNullAndValidHttpsUrls()
    {
        Assert.Null(Validation.ThrowIfNotValidHttpsUrl(null));
        Assert.Equal("https://example.com/path?q=1", Validation.ThrowIfNotValidHttpsUrl("https://example.com/path?q=1"));
    }

    [Theory]
    [InlineData("")]
    [InlineData("http://example.com")]
    [InlineData("ftp://example.com")]
    [InlineData("example.com")]
    [InlineData("https://")]
    public void Validation_ThrowIfNotValidHttpsUrl_ThrowsOnInvalidUrls(string invalidUrl)
    {
        var ex = Assert.Throws<ArgumentException>(() => Validation.ThrowIfNotValidHttpsUrl(invalidUrl));
        Assert.Contains("must be a valid HTTPS URL", ex.Message);
    }

    [Fact]
    public void Validation_ThrowIfLessThan_WorksCorrectly()
    {
        Assert.Null(Validation.ThrowIfLessThan(null, 5));
        Assert.Equal(5, Validation.ThrowIfLessThan(5, 5));
        Assert.Equal(6, Validation.ThrowIfLessThan(6, 5));

        var ex = Assert.Throws<ArgumentException>(() => Validation.ThrowIfLessThan(4, 5));
        Assert.Contains("cannot be less than 5", ex.Message);
    }

    [Fact]
    public void Validation_ThrowIfGreaterThan_WorksCorrectly()
    {
        Assert.Null(Validation.ThrowIfGreaterThan(null, 10));
        Assert.Equal(10, Validation.ThrowIfGreaterThan(10, 10));
        Assert.Equal(9, Validation.ThrowIfGreaterThan(9, 10));

        var ex = Assert.Throws<ArgumentException>(() => Validation.ThrowIfGreaterThan(11, 10));
        Assert.Contains("cannot be greater than 10", ex.Message);
    }

    [Fact]
    public void Validation_ThrowIfNotBetween_WorksCorrectly()
    {
        var minMaxEx = Assert.Throws<ArgumentException>(() => Validation.ThrowIfNotBetween(5, 10, 2));
        Assert.Contains("minimum value cannot be greater than the maximum value", minMaxEx.Message);

        Assert.Null(Validation.ThrowIfNotBetween(null, 1, 5));
        Assert.Equal(1, Validation.ThrowIfNotBetween(1, 1, 5));
        Assert.Equal(5, Validation.ThrowIfNotBetween(5, 1, 5));

        var belowEx = Assert.Throws<ArgumentException>(() => Validation.ThrowIfNotBetween(0, 1, 5));
        Assert.Contains("must be between 1 and 5", belowEx.Message);

        var aboveEx = Assert.Throws<ArgumentException>(() => Validation.ThrowIfNotBetween(6, 1, 5));
        Assert.Contains("must be between 1 and 5", aboveEx.Message);
    }
}
