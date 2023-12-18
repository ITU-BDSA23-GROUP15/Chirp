using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using Microsoft.Extensions.Configuration;

namespace Chirp.UI.Testing;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class UITests : PageTest
{
    [Test]
    // This test is simply run to make sure PlayWright is running correctly, should always pass
    public async Task HomepageHasPlaywrightInTitleAndGetStartedLinkLinkingtoTheIntroPage()
    {
        await Page.GotoAsync("https://playwright.dev");

        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync(new Regex("Playwright"));

        // create a locator
        var getStarted = Page.GetByRole(AriaRole.Link, new() { Name = "Get started" });

        // Expect an attribute "to be strictly equal" to the value.
        await Expect(getStarted).ToHaveAttributeAsync("href", "/docs/intro");

        // Click the get started link.
        await getStarted.ClickAsync();

        // Expects the URL to contain intro.
        await Expect(Page).ToHaveURLAsync(new Regex(".*intro"));
    }

    [Test]
    public async Task MyTest()
    {
        var config = new ConfigurationBuilder().AddUserSecrets<UITests>().Build();
        var UserName = config["UserName"];
        var Password = config["Password"];

        await Page.GotoAsync("http://localhost:5273/");

        await Page.GetByRole(AriaRole.Link, new() { Name = "login" }).ClickAsync();

        await Page.GetByLabel("Username or email address").ClickAsync();

        await Page.GetByLabel("Username or email address").FillAsync(UserName);

        await Page.GetByLabel("Password").ClickAsync();

        await Page.GetByLabel("Password").FillAsync(Password);

        await Page.GetByRole(AriaRole.Button, new() { Name = "Sign in", Exact = true }).ClickAsync();

        await Page.Locator("#Text").ClickAsync();

        await Page.Locator("#Text").FillAsync("This is a UI test, does this work?");

        await Page.GetByRole(AriaRole.Button, new() { Name = "Share" }).ClickAsync();

        await Page.GetByRole(AriaRole.Link, new() { Name = "my timeline" }).ClickAsync();

        await Page.GetByRole(AriaRole.Link, new() { Name = "Next page" }).ClickAsync();

        await Page.GetByRole(AriaRole.Link, new() { Name = "public timeline" }).ClickAsync();

        await Page.GetByRole(AriaRole.Link, new() { Name = "Next page" }).ClickAsync();

        await Page.GetByRole(AriaRole.Link, new() { Name = "about me" }).ClickAsync();

        void Page_Dialog_EventHandler(object sender, IDialog dialog)
        {
            Console.WriteLine($"Dialog message: {dialog.Message}");
            dialog.AcceptAsync();
            Page.Dialog -= Page_Dialog_EventHandler;
        }
        Page.Dialog += Page_Dialog_EventHandler;
        await Page.Locator("li").Filter(new() { HasText = "This is a UI test, does this work?" }).GetByRole(AriaRole.Button).ClickAsync();

        await Page.GetByRole(AriaRole.Link, new() { Name = "my timeline" }).ClickAsync();

        await Page.GetByRole(AriaRole.Link, new() { Name = "public timeline" }).ClickAsync();

        await Page.Locator("li").Filter(new() { HasText = "Follow Jacqualine Gilcoine — 08/01/2023 13:17:39 Starbuck now is what we hear" }).GetByRole(AriaRole.Button).ClickAsync();

        await Page.Locator("li").Filter(new() { HasText = "Unfollow Jacqualine Gilcoine — 08/01/2023 13:17:39 Starbuck now is what we hear" }).GetByRole(AriaRole.Link).Nth(1).ClickAsync();

        await Page.GetByRole(AriaRole.Link, new() { Name = "about me" }).ClickAsync();

        await Page.GetByRole(AriaRole.Button, new() { Name = "Unfollow" }).ClickAsync();

        await Page.GetByRole(AriaRole.Link, new() { Name = "my timeline" }).ClickAsync();

    }
}