$gitRootPath = $(git rev-parse --show-toplevel)

function Invoke-Deploy {
    $kubernetesConfigPath = "$($gitRootPath)/kubernetes.yaml"
    $kubernetesConfig = Get-Content $kubernetesConfigPath `
    | Foreach-Object { $_ `
            -Replace '{{ NAMESPACE }}', $env:NAMESPACE `
            -Replace '{{ BERRY_IMAGE }}', $env:BERRY_IMAGE `
            -Replace '{{ HOST_NAME }}', $env:HOST_NAME `
            -Replace '{{ PUBLIC_CERT_SECRET_NAME }}', $env:PUBLIC_CERT_SECRET_NAME `
            -Replace '{{ ANTHROPIC_API_KEY }}', $env:ANTHROPIC_API_KEY `
            -Replace '{{ ADO_TOKEN }}', $env:ADO_TOKEN `
            -Replace '{{ ADO_ORGANIZATION }}', $env:ADO_ORGANIZATION `
            -Replace '{{ ADO_PROJECT }}', $env:ADO_PROJECT `
            -Replace '{{ ADO_WEBHOOK_SECRET }}', $env:ADO_WEBHOOK_SECRET `
            -Replace '{{ ADO_REPOSITORY_ID }}', $env:ADO_REPOSITORY_ID `
            -Replace '{{ ADO_IDENTITY_ID }}', $env:ADO_IDENTITY_ID `
            -Replace '{{ ADO_IDENTITY_DISPLAY_NAME }}', $env:ADO_IDENTITY_DISPLAY_NAME `
            -Replace '{{ GOOGLE_USER_NAME }}', $env:GOOGLE_USER_NAME `
            -Replace '{{ GOOGLE_APPLICATION_NAME }}', $env:GOOGLE_APPLICATION_NAME `
            -Replace '{{ GOOGLE_ACCESS_KEY_JSON }}', $env:GOOGLE_ACCESS_KEY_JSON
    }
    Write-Output $kubernetesConfig
}

Invoke-Deploy