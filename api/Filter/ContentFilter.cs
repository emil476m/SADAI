using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Text.Json;

namespace api.Filter;

public class ContentFilter
{
    public record RequestModel(string text, List<string> categories, string outputType)
    {
        public override string ToString()
        {
            return $"{{ text = {text}, categories = {categories}, outputType = {outputType} }}";
        }
    }

    public async Task verifyMessage(string message)
    {
        int toxicLevl = 2;

        HttpClient client = new HttpClient();

        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post,
            "https://toxisityfilter.cognitiveservices.azure.com/contentsafety/text:analyze?api-version=2023-10-01");

        request.Headers.Add("accept", "application/json");
        request.Headers.Add("Ocp-Apim-Subscription-Key", Environment.GetEnvironmentVariable("ContentFilterKey"));

        var req = new RequestModel(message, new List<String>() { "Hate", "Sexual", "SelfHarm", "Violence" },
            "FourSeverityLevels");

        request.Content = new StringContent(JsonSerializer.Serialize(req));

        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        HttpResponseMessage response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        var obj = JsonSerializer.Deserialize<ContentFilterResponse>(responseBody);

        var isToxic = obj.categoriesAnalysis.Count(e => e.severity > toxicLevl) >= 1;
        if (isToxic)
        {
            Console.Write("This is Toxic");
            throw new ValidationException("This speech is not allowed");
        }
    }
}

public class BlocklistsMatch
{
    public string blocklistName { get; set; }
    public string blocklistItemId { get; set; }
    public string blocklistItemText { get; set; }
}

public class CategoriesAnalysis
{
    public string category { get; set; }
    public int severity { get; set; }
}

public class ContentFilterResponse
{
    public List<object> blocklistsMatch { get; set; }
    public List<CategoriesAnalysis> categoriesAnalysis { get; set; }
}