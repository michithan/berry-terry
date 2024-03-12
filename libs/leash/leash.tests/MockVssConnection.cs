using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;

namespace leash.tests;

public class MockVssConnection(Uri baseUrl, VssCredentials credentials) : VssConnection(baseUrl, credentials)
{
}