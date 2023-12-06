using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace Application.Testing;

[Parallelizable(ParallelScope.Self)]
[TestFixture]

// All tests in this class has been generated using codegen (https://playwright.dev/dotnet/docs/codegen) 
public class UiTests : PageTest
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
        await Page.GotoAsync("http://localhost:5273");

        await Page.GetByRole(AriaRole.Link, new() { Name = "log in" }).ClickAsync();

        await Page.GetByLabel("Username or email address").FillAsync("macj@itu.dk");

        await Page.GetByLabel("Username or email address").PressAsync("Tab");

        await Page.GetByLabel("Password").FillAsync("Ons24nov99");

        await Page.GetByLabel("Password").PressAsync("Enter");

        await Page.Locator("#Text").ClickAsync();

        await Page.Locator("#Text").FillAsync("Hej virker denne test?");

        await Page.GetByRole(AriaRole.Button, new() { Name = "Share" }).ClickAsync();

        await Page.GetByRole(AriaRole.Link, new() { Name = "my timeline" }).ClickAsync();

        await Page.GetByRole(AriaRole.Link, new() { Name = "public timeline" }).ClickAsync();

        await Page.GetByRole(AriaRole.Link, new() { Name = "about me" }).ClickAsync();

        await Page.GetByRole(AriaRole.Link, new() { Name = "public timeline" }).ClickAsync();

        await Page.Locator("li").Filter(new() { HasText = "Follow DanielSMillard — 12/06" }).GetByRole(AriaRole.Button).ClickAsync();

        await Page.GetByRole(AriaRole.Link, new() { Name = "my timeline" }).ClickAsync();

        await Page.Locator("li").Filter(new() { HasText = "Unfollow DanielSMillard Teeeeeeest — 12/06/2023 11:27:" }).GetByRole(AriaRole.Button).ClickAsync();

        await Page.Locator("div").Filter(new() { HasText = "my timeline | public timeline" }).Nth(1).ClickAsync();

        await Page.GetByRole(AriaRole.Link, new() { Name = "public timeline" }).ClickAsync();

        await Page.GetByRole(AriaRole.Link, new() { Name = "Next page" }).ClickAsync();

    }
}