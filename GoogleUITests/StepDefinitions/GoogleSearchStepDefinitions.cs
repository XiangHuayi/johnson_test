using Microsoft.Playwright;
using TechTalk.SpecFlow;
using FluentAssertions;
using GoogleUITests.PageObjects;

namespace GoogleUITests.StepDefinitions;

[Binding]
public class GoogleSearchStepDefinitions
{
    private readonly ScenarioContext _scenarioContext;
    private GoogleHomePage _googleHomePage = null!;
    private IPage _page = null!;

    public GoogleSearchStepDefinitions(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [Given(@"I navigate to Google homepage")]
    public async Task GivenINavigateToGoogleHomepage()
    {
        _page = _scenarioContext.Get<IPage>("page");
        _googleHomePage = new GoogleHomePage(_page);
        await _googleHomePage.NavigateToAsync();
    }

    [When(@"I search for ""(.*)""")]
    public async Task WhenISearchFor(string searchTerm)
    {
        await _googleHomePage.SearchForAsync(searchTerm);
    }

    [Then(@"I should see search results")]
    public async Task ThenIShouldSeeSearchResults()
    {
        var resultsVisible = await _googleHomePage.AreSearchResultsVisibleAsync();
        resultsVisible.Should().BeTrue("Search results should be visible");
    }

    [Then(@"the page title should contain ""(.*)""")]
    public async Task ThenThePageTitleShouldContain(string expectedText)
    {
        await _googleHomePage.VerifyPageTitleContainsAsync(expectedText);
    }

    [Then(@"the Google logo should be visible")]
    public async Task ThenTheGoogleLogoShouldBeVisible()
    {
        var logoVisible = await _googleHomePage.IsGoogleLogoVisibleAsync();
        logoVisible.Should().BeTrue("Google logo should be visible");
    }

    [Then(@"the search box should be present")]
    public async Task ThenTheSearchBoxShouldBePresent()
    {
        var searchBoxPresent = await _googleHomePage.IsSearchBoxPresentAsync();
        searchBoxPresent.Should().BeTrue("Search box should be present");
    }

    [Then(@"the search button should be present")]
    public async Task ThenTheSearchButtonShouldBePresent()
    {
        var searchButtonPresent = await _googleHomePage.IsSearchButtonPresentAsync();
        searchButtonPresent.Should().BeTrue("Search button should be present");
    }
}