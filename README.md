# Google Test Automation Suite

A comprehensive test automation framework for testing Google's web services using C#, NUnit, Playwright, SpecFlow, and RestSharp with CI/CD integration.

## 🏗️ Project Structure

```
GoogleTestSuite/
├── .github/
│   └── workflows/
│       └── test-automation.yml     # GitHub Actions CI/CD pipeline
├── GoogleUITests/                  # UI automation tests
│   ├── Features/
│   │   └── GoogleSearch.feature    # SpecFlow feature files
│   ├── StepDefinitions/
│   │   └── GoogleSearchStepDefinitions.cs
│   ├── PageObjects/
│   │   └── GoogleHomePage.cs       # Page Object Model
│   ├── Hooks/
│   │   └── PlaywrightHooks.cs      # Test setup/teardown
│   ├── GoogleUITests.csproj
│   └── specflow.json
├── GoogleAPITests/                 # API automation tests
│   ├── GoogleApiTests.cs
│   └── GoogleAPITests.csproj
├── GoogleTestSuite.sln
└── README.md
```

## 🛠️ Technologies Used

- **C# .NET 8.0** - Programming language and framework
- **NUnit** - Testing framework
- **Playwright** - Browser automation for UI tests
- **SpecFlow** - BDD framework for readable test scenarios
- **RestSharp** - HTTP client for API testing
- **FluentAssertions** - Assertion library
- **GitHub Actions** - CI/CD pipeline

## 🚀 Getting Started

### Prerequisites

- .NET 8.0 SDK or later
- Visual Studio 2022 or VS Code
- Git

### Installation

1. **Clone the repository:**
   ```bash
   git clone <your-repo-url>
   cd GoogleTestSuite
   ```

2. **Restore NuGet packages:**
   ```bash
   dotnet restore
   ```

3. **Install Playwright browsers:**
   ```bash
   dotnet tool install --global Microsoft.Playwright.CLI
   playwright install
   ```

4. **Build the solution:**
   ```bash
   dotnet build
   ```

## 🧪 Running Tests

### Run All Tests
```bash
dotnet test
```

### Run UI Tests Only
```bash
dotnet test GoogleUITests/GoogleUITests.csproj
```

### Run API Tests Only
```bash
dotnet test GoogleAPITests/GoogleAPITests.csproj
```

### Run Tests by Category
```bash
# Run smoke tests
dotnet test --filter Category=Smoke

# Run regression tests
dotnet test --filter Category=Regression

# Run UI tests
dotnet test --filter Category=UI

# Run API tests
dotnet test --filter Category=API
```

### Run Tests with Visible Browser (UI Tests)
```bash
set HEADLESS=false
dotnet test GoogleUITests/GoogleUITests.csproj
```

## 📊 Test Reports

Test results are automatically generated in TRX format. Screenshots and videos are captured for failed UI tests.

- **Screenshots:** `screenshots/` directory
- **Videos:** `videos/` directory
- **Test Results:** `TestResults/` directory

## 🔄 CI/CD Pipeline

The GitHub Actions workflow (`test-automation.yml`) includes:

- **Triggers:**
  - Push to main/develop branches
  - Pull requests to main
  - Daily scheduled runs at 6 AM UTC
  - Manual workflow dispatch

- **Jobs:**
  - API Tests (RestSharp)
  - UI Tests (Playwright + SpecFlow)
  - Test Report Generation
  - Notification of results

- **Artifacts:**
  - Test results (TRX files)
  - Screenshots (on failure)
  - Videos (on failure)
  - Code coverage reports

## 🧩 Test Scenarios

### UI Tests (SpecFlow + Playwright)
- ✅ Google homepage loads correctly
- ✅ Search functionality works
- ✅ Search results are displayed
- ✅ Page title updates after search
- ✅ Parameterized tests with different search terms

### API Tests (RestSharp)
- ✅ Google homepage returns 200 status
- ✅ Response contains expected content
- ✅ Performance testing (response time)
- ✅ Security headers validation
- ✅ Multiple endpoint accessibility

## 🔧 Configuration

### Environment Variables
- `HEADLESS`: Set to `false` to run UI tests with visible browser

### SpecFlow Configuration
Configuration is managed in `specflow.json` with:
- Binding culture settings
- Runtime dependencies
- Trace settings

## 📝 Best Practices Implemented

1. **Page Object Model (POM)** - Maintainable UI test structure
2. **BDD with SpecFlow** - Readable test scenarios
3. **Separation of Concerns** - UI and API tests in separate projects
4. **Comprehensive Reporting** - Screenshots, videos, and detailed logs
5. **CI/CD Integration** - Automated testing on code changes
6. **Parallel Execution** - Faster test execution
7. **Error Handling** - Robust test failure management
8. **Security Testing** - API security header validation

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests for new functionality
5. Ensure all tests pass
6. Submit a pull request

## 📄 License

This project is licensed under the MIT License.

## 🆘 Troubleshooting

### Common Issues

1. **Playwright browser not found:**
   ```bash
   playwright install chromium
   ```

2. **Tests failing in headless mode:**
   - Set `HEADLESS=false` to debug visually
   - Check screenshots in the `screenshots/` directory

3. **API tests timing out:**
   - Check network connectivity
   - Verify Google.com accessibility

4. **SpecFlow step definitions not found:**
   - Rebuild the solution
   - Check that step definitions match feature file steps

## 📞 Support

For questions or issues, please create an issue in the GitHub repository.