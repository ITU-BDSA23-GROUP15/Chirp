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

        await Page.Locator("#Text").FillAsync("Can I cheep in this UI Test?");

        await Page.GetByRole(AriaRole.Button, new() { Name = "Share" }).ClickAsync();

        await Page.Locator("form").Filter(new() { HasText = "Share" }).ClickAsync();

        await Page.Locator("#Text").DblClickAsync();

        await Page.Locator("#Text").FillAsync("sssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssaaa");

        await Page.GetByRole(AriaRole.Link, new() { Name = "my timeline" }).ClickAsync();

        await Page.Locator("#Text").ClickAsync();

        await Page.Locator("#Text").FillAsync("My own timeline oh wauw");

        await Page.Locator("#Text").PressAsync("Enter");

        await Page.GetByRole(AriaRole.Link, new() { Name = "public timeline" }).ClickAsync();

        await Page.GetByRole(AriaRole.Link, new() { Name = "about me" }).ClickAsync();

        await Page.GetByRole(AriaRole.Link, new() { Name = "public timeline" }).ClickAsync();

        await Page.Locator("li").Filter(new() { HasText = "Follow Jacqualine Gilcoine — 08/01/2023 13:17:39 Starbuck now is what we hear" }).GetByRole(AriaRole.Link).Nth(1).ClickAsync();

        await Page.Locator("li").Filter(new() { HasText = "Follow Jacqualine Gilcoine — 08/01/2023 13:17:39 Starbuck now is what we hear" }).GetByRole(AriaRole.Button).ClickAsync();

        await Page.GetByRole(AriaRole.Link, new() { Name = "my timeline" }).ClickAsync();

        await Page.GetByRole(AriaRole.Link, new() { Name = "public timeline" }).ClickAsync();

        await Page.Locator("li").Filter(new() { HasText = "Unfollow Jacqualine Gilcoine — 08/01/2023 13:17:39 Starbuck now is what we hear" }).GetByRole(AriaRole.Button).ClickAsync();

        await Page.GetByRole(AriaRole.Link, new() { Name = "my timeline" }).ClickAsync();

        await Page.GetByRole(AriaRole.Link, new() { Name = "public timeline" }).ClickAsync();

    }
}