$gitRootPath = $(git rev-parse --show-toplevel)

function Invoke-Publish {
    $berryAppPath = "$($gitRootPath)/apps/berry/berry"
    $cwd = Get-Location
    try {
        Set-Location $berryAppPath
        dotnet restore
        dotnet publish -c Release
        podman build -t $env:BERRY_IMAGE -f Dockerfile $berryAppPath
        if ($LASTEXITCODE -ne 0) {
            throw "Publish failed in $($berryAppPath)"
        }
        podman push $env:BERRY_IMAGE
        if ($LASTEXITCODE -ne 0) {
            throw "Publish failed in $($berryAppPath)"
        }
    }
    finally {
        $cwd | Set-Location
    }
}

Invoke-Publish