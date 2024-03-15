using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Text.Json;

namespace api.Filter;

public class ContentFilter
{
    private HttpClient http = new HttpClient();
    public record RequestModel(string text, List<string> categories, string outputType)
    {
        public override string ToString()
        {
            return $"{{ text = {text}, categories = {categories}, outputType = {outputType} }}";
        }
    }

    public async Task checkText(string message)
    {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://toxisityfilter.cognitiveservices.azure.com/contentsafety/text:analyze?api-version=2023-10-01");
        
        request.Headers.Add("accept", "application/json");
        request.Headers.Add("Ocp-Apim-Subscription-Key", Environment.GetEnvironmentVariable("aikey"));

        var req = new RequestModel(message, new List<string>
            { "Hate", "Sexual", "SelfHarm", "Violence" }, "FourSeverityLevels");

        request.Content = new StringContent(JsonSerializer.Serialize(req));
        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        
        HttpResponseMessage response = await http.SendAsync(request);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        var obj = JsonSerializer.Deserialize<ContentFilterResponce>(responseBody);

        var isToxic = obj.categoriesAnalysis.Count(e => e.severity > 1) >= 1;

        if (isToxic)
        {
            throw new ValidationException("Such language is not allowed");
        }
    }
}

public class CategoriesAnalysis
{
    public string category { get; set; }
    public int severity { get; set; }
}

public class ContentFilterResponce
{
    public List<object> blocklistsMatch { get; set; }
    public List<CategoriesAnalysis> categoriesAnalysis { get; set; }
}