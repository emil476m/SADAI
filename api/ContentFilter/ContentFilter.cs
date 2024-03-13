using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using lib;

namespace socketAPIFirst.ContentFilter;

public class ContentFilter
{
    public record RequestModel(string text, List<string> categories, string outputType)
    {
        public override string ToString()
        {
            return $"{{ text = {text}, categories = {categories}, outputType = {outputType} }}";
        }
    }

    private async Task verifyMessage(string message)
    {
        HttpClient client = new HttpClient();

        HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://contenfilter.cognitiveservices.azure.com/contentsafety/text:analyze?api-version=2023-10-01");
        
        requestMessage.Headers.Add("accept", "application/json");
        requestMessage.Headers.Add("Ocp-Apim-Subscription-Key", Environment.GetEnvironmentVariable("ContentFilterKey"));

        var req = new RequestModel(message, new List<string>() { "Hate", "Sexual", "SelfHarm", "Violence" }, "FourSeverityLevels");

        requestMessage.Content = new StringContent(JsonSerializer.Serialize(req));
        requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        HttpResponseMessage response = await client.SendAsync(requestMessage);
        response.EnsureSuccessStatusCode();
        string responsebody = await response.Content.ReadAsStringAsync();
        var obj = JsonSerializer.Deserialize<ContentFilterResponse>(responsebody);
        var isToxic = obj.categoriesAnalysis.Count(e => e.severity > 1) >= 1;
        if (isToxic)
        {
            Console.Write("This is Toxic");
            throw new ValidationException("This speech is not allowed");
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
        public List<BlocklistsMatch> blocklistsMatch { get; set; }
        public List<CategoriesAnalysis> categoriesAnalysis { get; set; }
    }
}