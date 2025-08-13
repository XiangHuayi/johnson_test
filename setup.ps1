#!/usr/bin/env pwsh

# Google Test Automation Suite Setup Script
# This script helps set up the testing environment and run tests

param(
    [Parameter(Mandatory=$false)]
    [ValidateSet("setup", "test", "ui", "api", "install-playwright", "clean")]
    [string]$Action = "setup",
    
    [Parameter(Mandatory=$false)]
    [switch]$Headless = $true,
    
    [Parameter(Mandatory=$false)]
    [string]$Category = ""
)

function Write-Header {
    param([string]$Message)
    Write-Host "`n" -ForegroundColor Green
    Write-Host "=" * 60 -ForegroundColor Green
    Write-Host $Message -ForegroundColor Green
    Write-Host "=" * 60 -ForegroundColor Green
}

function Write-Step {
    param([string]$Message)
    Write-Host "`n🔧 $Message" -ForegroundColor Cyan
}

function Write-Success {
    param([string]$Message)
    Write-Host "✅ $Message" -ForegroundColor Green
}

function Write-Error {
    param([string]$Message)
    Write-Host "❌ $Message" -ForegroundColor Red
}

function Test-DotNetInstalled {
    try {
        $version = dotnet --version
        Write-Success ".NET SDK version $version is installed"
        return $true
    }
    catch {
        Write-Error ".NET SDK is not installed. Please install .NET 8.0 SDK or later."
        return $false
    }
}

function Install-PlaywrightBrowsers {
    Write-Step "Installing Playwright CLI tool..."
    try {
        dotnet tool install --global Microsoft.Playwright.CLI 2>$null
        Write-Success "Playwright CLI installed"
    }
    catch {
        Write-Host "⚠️  Playwright CLI might already be installed" -ForegroundColor Yellow
    }
    
    Write-Step "Installing Playwright browsers..."
    try {
        playwright install chromium
        Write-Success "Chromium browser installed"
    }
    catch {
        Write-Error "Failed to install Playwright browsers"
        return $false
    }
    return $true
}

function Restore-Dependencies {
    Write-Step "Restoring NuGet packages..."
    try {
        dotnet restore
        Write-Success "Dependencies restored"
        return $true
    }
    catch {
        Write-Error "Failed to restore dependencies"
        return $false
    }
}

function Build-Solution {
    Write-Step "Building solution..."
    try {
        dotnet build --configuration Release
        Write-Success "Solution built successfully"
        return $true
    }
    catch {
        Write-Error "Failed to build solution"
        return $false
    }
}

function Run-Tests {
    param(
        [string]$Project = "",
        [string]$TestCategory = "",
        [bool]$ShowBrowser = $false
    )
    
    $env:HEADLESS = if ($ShowBrowser) { "false" } else { "true" }
    
    $testCommand = "dotnet test"
    
    if ($Project) {
        $testCommand += " $Project"
    }
    
    $testCommand += " --configuration Release --logger console --logger trx"
    
    if ($TestCategory) {
        $testCommand += " --filter Category=$TestCategory"
    }
    
    Write-Step "Running tests with command: $testCommand"
    Write-Host "Browser mode: $(if ($ShowBrowser) { 'Visible' } else { 'Headless' })" -ForegroundColor Yellow
    
    try {
        Invoke-Expression $testCommand
        Write-Success "Tests completed"
    }
    catch {
        Write-Error "Tests failed or encountered errors"
    }
}

function Clean-Artifacts {
    Write-Step "Cleaning build artifacts and test results..."
    
    $foldersToClean = @(
        "bin", "obj", "TestResults", "screenshots", "videos", 
        "GoogleUITests/bin", "GoogleUITests/obj", "GoogleUITests/TestResults",
        "GoogleAPITests/bin", "GoogleAPITests/obj", "GoogleAPITests/TestResults"
    )
    
    foreach ($folder in $foldersToClean) {
        if (Test-Path $folder) {
            Remove-Item $folder -Recurse -Force
            Write-Host "Removed $folder" -ForegroundColor Yellow
        }
    }
    
    Write-Success "Cleanup completed"
}

# Main execution
Write-Header "Google Test Automation Suite - Setup & Execution Script"

switch ($Action.ToLower()) {
    "setup" {
        Write-Header "Setting up the test environment..."
        
        if (-not (Test-DotNetInstalled)) {
            exit 1
        }
        
        if (-not (Restore-Dependencies)) {
            exit 1
        }
        
        if (-not (Build-Solution)) {
            exit 1
        }
        
        if (-not (Install-PlaywrightBrowsers)) {
            exit 1
        }
        
        Write-Success "Setup completed successfully!"
        Write-Host "`nYou can now run tests using:" -ForegroundColor Green
        Write-Host "  .\setup.ps1 -Action test          # Run all tests" -ForegroundColor White
        Write-Host "  .\setup.ps1 -Action ui            # Run UI tests only" -ForegroundColor White
        Write-Host "  .\setup.ps1 -Action api           # Run API tests only" -ForegroundColor White
        Write-Host "  .\setup.ps1 -Action ui -Headless:`$false  # Run UI tests with visible browser" -ForegroundColor White
    }
    
    "test" {
        Write-Header "Running all tests..."
        Run-Tests -ShowBrowser (-not $Headless) -TestCategory $Category
    }
    
    "ui" {
        Write-Header "Running UI tests..."
        Run-Tests -Project "GoogleUITests/GoogleUITests.csproj" -ShowBrowser (-not $Headless) -TestCategory $Category
    }
    
    "api" {
        Write-Header "Running API tests..."
        Run-Tests -Project "GoogleAPITests/GoogleAPITests.csproj" -TestCategory $Category
    }
    
    "install-playwright" {
        Write-Header "Installing Playwright browsers..."
        Install-PlaywrightBrowsers
    }
    
    "clean" {
        Write-Header "Cleaning artifacts..."
        Clean-Artifacts
    }
    
    default {
        Write-Error "Unknown action: $Action"
        Write-Host "Available actions: setup, test, ui, api, install-playwright, clean" -ForegroundColor Yellow
        exit 1
    }
}

Write-Host "`n🎉 Script execution completed!" -ForegroundColor Green