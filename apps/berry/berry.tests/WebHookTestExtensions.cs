using System.Reflection;
using System.Text;
using System.Text.Json;

namespace berry.tests;

public static class WebHookTestExtensions
{
    public static string LoadMockFile(string fileName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = $"berry.tests.mocks.{fileName}";

        using Stream? stream = assembly.GetManifestResourceStream(resourceName) ?? throw new Exception($"Resource {resourceName} not found");
        using StreamReader reader = new(stream, Encoding.UTF8);

        return reader.ReadToEnd();
    }

    public static JsonElement LoadJsonMockFile(string fileName)
    {
        string jsonFileContent = LoadMockFile(fileName);
        return JsonDocument.Parse(jsonFileContent).RootElement;
    }
}