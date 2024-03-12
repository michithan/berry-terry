$gitRootPath = $(git rev-parse --show-toplevel)

function Invoke-Test {
    $cwd = Get-Location
    try {
        $testProjectLocations = Get-ChildItem -Path $gitRootPath  -Recurse -Include *.tests.csproj
        foreach ($testProjectLocation in $testProjectLocations) {
            Set-Location $testProjectLocation.Directory
            "-----------------------------------------" | Write-Host
            "Testing $($testProjectLocation.Directory)" | Write-Host
            "-----------------------------------------" | Write-Host
            Write-Host
            dotnet test
            if ($LASTEXITCODE -ne 0) {
                throw "Tests failed in $($testProjectLocation.Directory)"
            }
        }
    }
    finally {
        $cwd | Set-Location
    }
}

Invoke-Test