namespace SlackBlockKit.Tests.Surfaces;

public class ModalSurfaceTests
{
    [Fact]
    public void TextObjectConstructor_SetsProperties()
    {
        var title = new PlainTextObject("Title");
        List<IModalBlock> blocks = [new HeaderBlock("Header")];

        var modal = new ModalSurface(title, blocks, new PlainTextObject("Submit"), new PlainTextObject("Close"),
            "callback", "metadata", true, false, "external", true);

        Assert.Same(title, modal.Title);
        Assert.Same(blocks, modal.Blocks);
        Assert.Equal("Submit", modal.Submit!.Text);
        Assert.Equal("Close", modal.Close!.Text);
        Assert.Equal("callback", modal.CallbackId);
        Assert.Equal("metadata", modal.PrivateMetadata);
        Assert.True(modal.ClearOnClose);
        Assert.False(modal.NotifyOnClose);
        Assert.Equal("external", modal.ExternalId);
        Assert.True(modal.SubmitDisabled);
        Assert.Equal("modal", modal.Type);
    }

    [Fact]
    public void StringConstructor_WrapsText()
    {
        var modal = new ModalSurface("Title", [], "Submit", "Close", "callback");

        Assert.Equal("Title", modal.Title.Text);
        Assert.Equal("Submit", modal.Submit!.Text);
        Assert.Equal("Close", modal.Close!.Text);
        Assert.Equal("callback", modal.CallbackId);
    }

    [Fact]
    public void Serialize_WritesAllFields()
    {
        var json = ToJson(new ModalSurface("Title", [new SectionBlock("Text")], "Submit", "Close", "callback",
            "metadata", true, true, "external", true));

        AssertProperties(json, "type", "title", "blocks", "submit", "close", "callback_id", "private_metadata",
            "clear_on_close", "notify_on_close", "external_id", "submit_disabled");
        Assert.Equal("modal", json.GetProperty("type").GetString());
        Assert.Equal("Title", TextOf(json, "title"));
        Assert.Equal("plain_text", json.GetProperty("submit").GetProperty("type").GetString());
        Assert.Equal("section", json.GetProperty("blocks")[0].GetProperty("type").GetString());
    }

    [Fact]
    public void Serialize_OmitsNullFields()
    {
        AssertProperties(ToJson(new ModalSurface("Title", [])), "type", "title", "blocks");
    }

    [Fact]
    public void Title_ThrowsWhenNullOrTooLong()
    {
        Assert.Throws<ArgumentNullException>(() => new ModalSurface((PlainTextObject)null!, []));
        Assert.NotNull(new ModalSurface(Chars(24), []));
        Assert.Throws<ArgumentException>(() => new ModalSurface(Chars(25), []));
    }

    [Fact]
    public void Blocks_ThrowsWhenNullOrTooMany()
    {
        Assert.Throws<ArgumentNullException>(() => new ModalSurface("Title", null!));
        Assert.NotNull(new ModalSurface("Title", Enumerable.Range(0, 100).Select(_ => (IModalBlock)new DividerBlock()).ToList()));
        Assert.Throws<ArgumentException>(() =>
            new ModalSurface("Title", Enumerable.Range(0, 101).Select(_ => (IModalBlock)new DividerBlock()).ToList()));
    }

    [Fact]
    public void Buttons_ThrowWhenTooLong()
    {
        Assert.NotNull(new ModalSurface("Title", [], Chars(24), Chars(24)));
        Assert.Throws<ArgumentException>(() => new ModalSurface("Title", [], submit: Chars(25)));
        Assert.Throws<ArgumentException>(() => new ModalSurface("Title", [], close: Chars(25)));
    }

    [Fact]
    public void Identifiers_ThrowWhenTooLong()
    {
        Assert.NotNull(new ModalSurface("Title", [], callbackId: Chars(255), privateMetadata: Chars(3000)));
        Assert.Throws<ArgumentException>(() => new ModalSurface("Title", [], callbackId: Chars(256)));
        Assert.Throws<ArgumentException>(() => new ModalSurface("Title", [], privateMetadata: Chars(3001)));
    }

    [Fact]
    public void RoundTrip_PreservesValues()
    {
        var modal = RoundTrip(new ModalSurface("Title", [new HeaderBlock("Header")], "Submit", "Close", "callback",
            "metadata", true, false, "external", true));

        Assert.Equal("Title", modal.Title.Text);
        Assert.Equal("Header", Assert.IsType<HeaderBlock>(Assert.Single(modal.Blocks)).Text.Text);
        Assert.Equal("Submit", modal.Submit!.Text);
        Assert.Equal("Close", modal.Close!.Text);
        Assert.Equal("callback", modal.CallbackId);
        Assert.Equal("metadata", modal.PrivateMetadata);
        Assert.True(modal.ClearOnClose);
        Assert.False(modal.NotifyOnClose);
        Assert.Equal("external", modal.ExternalId);
        Assert.True(modal.SubmitDisabled);
    }

    [Fact]
    public void Deserialize_ReadsSlackPayloadWithTypeAfterOtherProperties()
    {
        const string json =
            """{"id":"V123","team_id":"T123","title":{"type":"plain_text","text":"Title"},"blocks":[{"type":"divider"}],"type":"modal","callback_id":"callback"}""";

        var modal = SlackSerializer.Deserialize<ModalSurface>(json)!;

        Assert.Equal("Title", modal.Title.Text);
        Assert.IsType<DividerBlock>(Assert.Single(modal.Blocks));
        Assert.Equal("callback", modal.CallbackId);
    }
}
