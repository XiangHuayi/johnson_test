using Microsoft.Playwright;
using TechTalk.SpecFlow;
using NUnit.Framework;

namespace GoogleUITests.Hooks;

[Binding]
public class PlaywrightHooks
{
    private static IPlaywright? _playwright;
    private static IBrowser? _browser;
    private IBrowserContext? _context;
    private IPage? _page;
    private readonly ScenarioContext _scenarioContext;

    public PlaywrightHooks(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [BeforeTestRun]
    public static async Task BeforeTestRun()
    {
        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = Environment.GetEnvironmentVariable("HEADLESS") != "false",
            SlowMo = 100
        });
    }

    [BeforeScenario]
    public async Task BeforeScenario()
    {
        if (_browser == null)
            throw new InvalidOperationException("Browser is not initialized");

        _context = await _browser.NewContextAsync(new BrowserNewContextOptions
        {
            ViewportSize = new ViewportSize { Width = 1920, Height = 1080 },
            RecordVideoDir = "videos/"
        });

        _page = await _context.NewPageAsync();
        _scenarioContext.Set(_page, "page");
        _scenarioContext.Set(_context, "context");
    }

    [AfterScenario]
    public async Task AfterScenario()
    {
        if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
        {
            if (_page != null)
            {
                var screenshot = await _page.ScreenshotAsync(new PageScreenshotOptions
                {
                    Path = $"screenshots/{TestContext.CurrentContext.Test.Name}_{DateTime.Now:yyyyMMdd_HHmmss}.png",
                    FullPage = true
                });
            }
        }

        if (_context != null)
        {
            await _context.CloseAsync();
        }
    }

    [AfterTestRun]
    public static async Task AfterTestRun()
    {
        if (_browser != null)
        {
            await _browser.CloseAsync();
        }
        _playwright?.Dispose();
    }
}