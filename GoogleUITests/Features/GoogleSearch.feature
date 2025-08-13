Feature: Google Search Functionality
	As a user
	I want to search on Google
	So that I can find relevant information

@ui @smoke
Scenario: Successful search on Google homepage
	Given I navigate to Google homepage
	When I search for "Playwright testing"
	Then I should see search results
	And the page title should contain "Playwright testing"

@ui @regression
Scenario: Google homepage loads correctly
	Given I navigate to Google homepage
	Then the Google logo should be visible
	And the search box should be present
	And the search button should be present

@ui @regression
Scenario Outline: Search with different terms
	Given I navigate to Google homepage
	When I search for "<searchTerm>"
	Then I should see search results
	And the page title should contain "<searchTerm>"

Examples:
	| searchTerm |
	| C# programming |
	| Selenium WebDriver |
	| Test automation |