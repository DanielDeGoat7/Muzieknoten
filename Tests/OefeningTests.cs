using Microsoft.Playwright;

namespace Piano.Tests;

[TestClass]
public class MuzieknotenTests
{
    private const string BaseUrl = "https://localhost:7059";

    [TestMethod]
    public async Task TrebleOefeningAntwoordCheck()
    {
        var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false
        });
        var page = await browser.NewPageAsync();

        await page.GotoAsync(BaseUrl);

        await page.ClickAsync("button:has-text('Inloggen')");

        await page.FillAsync("input[type='email']", "hallo@a.nl");
        await page.FillAsync("input[type='password']", "123456");
        await page.ClickAsync("button:has-text('Inloggen')");
        await page.WaitForURLAsync($"{BaseUrl}/Home/Index");

        await page.ClickAsync("a:has-text('Oefeningen')");
        await page.ClickAsync("button:has-text('Treble')");

        await page.WaitForSelectorAsync("#treble-note");
        var isVisible = await page.IsVisibleAsync("#treble-note");
        Assert.IsTrue(isVisible);

        await page.WaitForSelectorAsync("#treble-feedback");
        var startFeedback = (await page.TextContentAsync("#treble-feedback"))?.Trim() ?? string.Empty;
        Assert.AreEqual("Klik op de juiste noot...", startFeedback, "Begin feedback tekst klopt niet");

        await page.ClickAsync("button:has-text('C')");

        await Task.Delay(500);

        var nieuweFeedback = (await page.TextContentAsync("#treble-feedback"))?.Trim() ?? string.Empty;
        var heeftFeedback = nieuweFeedback.Contains("Correct! Goed gedaan.") || nieuweFeedback.Contains("Helaas! Het juiste antwoord was: ");

        Assert.IsTrue(heeftFeedback, "Geen feedback gegeven");

        await browser.CloseAsync();
    }

    [TestMethod]
    public async Task BassOefeningAntwoordCheck()
    {
        var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false
        });
        var page = await browser.NewPageAsync();

        await page.GotoAsync(BaseUrl);

        await page.ClickAsync("button:has-text('Inloggen')");

        await page.FillAsync("input[type='email']", "hallo@a.nl");
        await page.FillAsync("input[type='password']", "123456");
        await page.ClickAsync("button:has-text('Inloggen')");
        await page.WaitForURLAsync($"{BaseUrl}/Home/Index");

        await page.ClickAsync("a:has-text('Oefeningen')");
        await page.ClickAsync("button:has-text('Bass')");

        await page.WaitForSelectorAsync("#bass-note");
        var isVisible = await page.IsVisibleAsync("#bass-note");
        Assert.IsTrue(isVisible);

        await page.WaitForSelectorAsync("#bass-feedback");
        var startFeedback = (await page.TextContentAsync("#bass-feedback"))?.Trim() ?? string.Empty;
        Assert.AreEqual("Klik op de juiste noot...", startFeedback, "Begin feedback tekst klopt niet");

        await page.ClickAsync("button:has-text('C')");

        await Task.Delay(500);

        var nieuweFeedback = (await page.TextContentAsync("#bass-feedback"))?.Trim() ?? string.Empty;
        var heeftFeedback = nieuweFeedback.Contains("Correct! Goed gedaan.") || nieuweFeedback.Contains("Helaas! Het juiste antwoord was: ");

        Assert.IsTrue(heeftFeedback, "Geen feedback gegeven");

        await browser.CloseAsync();
    }
}