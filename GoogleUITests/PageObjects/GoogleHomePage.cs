using Microsoft.Playwright;
using FluentAssertions;

namespace GoogleUITests.PageObjects;

public class GoogleHomePage
{
    private readonly IPage _page;
    private readonly ILocator _searchBox;
    private readonly ILocator _searchButton;
    private readonly ILocator _googleLogo;
    private readonly ILocator _searchResults;

    public GoogleHomePage(IPage page)
    {
        _page = page;
        _searchBox = _page.Locator("[name='q']");
        _searchButton = _page.Locator("[name='btnK']").First;
        _googleLogo = _page.Locator("img[alt*='Google']");
        _searchResults = _page.Locator("#search");
    }

    public async Task NavigateToAsync()
    {
        await _page.GotoAsync("https://www.google.com");
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    public async Task SearchForAsync(string searchTerm)
    {
        await _searchBox.FillAsync(searchTerm);
        await _searchBox.PressAsync("Enter");
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    public async Task ClickSearchButtonAsync()
    {
        await _searchButton.ClickAsync();
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    public async Task<bool> IsGoogleLogoVisibleAsync()
    {
        return await _googleLogo.IsVisibleAsync();
    }

    public async Task<bool> IsSearchBoxPresentAsync()
    {
        return await _searchBox.IsVisibleAsync();
    }

    public async Task<bool> IsSearchButtonPresentAsync()
    {
        return await _searchButton.IsVisibleAsync();
    }

    public async Task<bool> AreSearchResultsVisibleAsync()
    {
        await _searchResults.WaitForAsync(new LocatorWaitForOptions { Timeout = 10000 });
        return await _searchResults.IsVisibleAsync();
    }

    public async Task<string> GetPageTitleAsync()
    {
        return await _page.TitleAsync();
    }

    public async Task VerifyPageTitleContainsAsync(string expectedText)
    {
        var title = await GetPageTitleAsync();
        title.Should().Contain(expectedText, $"Page title should contain '{expectedText}'");
    }
}