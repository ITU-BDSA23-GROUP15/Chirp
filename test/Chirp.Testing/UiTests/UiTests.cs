using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace Application.Testing;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class UITests : PageTest
{
    [Test]
    public async Task EndToEndUiTest()
    {
        
            await Page.GotoAsync("http://localhost:5273/");
            // await page.getByText("Public Timeline You need to").click();
            // await page.getByRole("link", { name: "log in" }).click();
            // await page.getByLabel("Username or email address").click();
            // await page.getByLabel("Username or email address").fill("macj@itu.dk");
            // await page.getByLabel("Username or email address").press("Tab");
            // await page.getByLabel("Password").fill("Ons24nov99");
            // await page.getByLabel("Password").press("Enter");
            // await page.locator("#Text").click();
            // await page.locator("#Text").fill("Omg I am UI testing");
            // await page.getByRole("button", { name: "Share" }).click();
            // await page.getByRole("link", { name: "my timeline" }).click();
            // await page.getByRole("link", { name: "public timeline" }).click();
            // await page.getByRole("link", { name: "about me" }).click();
            // await page.getByRole("link", { name: "my timeline" }).click();
            // await page.getByRole("link", { name: "public timeline" }).click();
            // await page.locator("li").filter({ hasText: "Follow Jacqualine Gilcoine — 08/01/2023 13:17:39 Starbuck now is what we hear" }).getByRole("button").click();
            // await page.getByRole("link", { name: "about me" }).click();
            // await page.getByRole("link", { name: "my timeline" }).click();
            // await page.getByRole("link", { name: "logout [Noerklit]" }).click();   
    }
}
// import { test, expect } from '@playwright/test';

// test('test', async ({ page }) => {
//   await page.goto('http://localhost:5273/');
//   await page.getByText('Public Timeline You need to').click();
//   await page.getByRole('link', { name: 'log in' }).click();
//   await page.getByLabel('Username or email address').click();
//   await page.getByLabel('Username or email address').fill('macj@itu.dk');
//   await page.getByLabel('Username or email address').press('Tab');
//   await page.getByLabel('Password').fill('Ons24nov99');
//   await page.getByLabel('Password').press('Enter');
//   await page.locator('#Text').click();
//   await page.locator('#Text').fill('Omg I am UI testing');
//   await page.getByRole('button', { name: 'Share' }).click();
//   await page.getByRole('link', { name: 'my timeline' }).click();
//   await page.getByRole('link', { name: 'public timeline' }).click();
//   await page.getByRole('link', { name: 'about me' }).click();
//   await page.getByRole('link', { name: 'my timeline' }).click();
//   await page.getByRole('link', { name: 'public timeline' }).click();
//   await page.locator('li').filter({ hasText: 'Follow Jacqualine Gilcoine — 08/01/2023 13:17:39 Starbuck now is what we hear' }).getByRole('button').click();
//   await page.getByRole('link', { name: 'about me' }).click();
//   await page.getByRole('link', { name: 'my timeline' }).click();
//   await page.getByRole('link', { name: 'logout [Noerklit]' }).click();
// });