using System.Text;
using Fleck;
using lib;
using Newtonsoft.Json;
using socketAPIFirst.Dtos;
using JsonSerializer = System.Text.Json.JsonSerializer;


namespace socketAPIFirst.ClientRequest;

public class ClientWantsAIResponse: BaseEventHandler<ClientWantsAIResponseDto>
{
    private static readonly HttpClient client = new HttpClient();
    public static string endPointURL = "https://api.openai.com/v1/chat/completions";
    public static string apiKey = Environment.GetEnvironmentVariable("aikey");
    public static string AITopic = "You are an intelligent assistant who always answers questions with a joke";
    
    public override async Task Handle(ClientWantsAIResponseDto dto, IWebSocketConnection socket)
    {
        
        var payload = new
        {
            model = "gpt-3.5-turbo",
            max_tokens = 256,
            temperature = 0.9,
            messages = new[]
            {
                new { role = "system", content = $"{AITopic}"},
                new { role = "user", content = $"{dto.message}" }
            }
        };

        string jsonPayload = JsonSerializer.Serialize(payload);

        var request = new HttpRequestMessage(HttpMethod.Post, endPointURL);
        request.Headers.Add("Authorization", $"Bearer {apiKey}");
        request.Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        var httpResponse = await client.SendAsync(request);
        string responseContent = await httpResponse.Content.ReadAsStringAsync();
        
        ChatApiResponse chatResponse = JsonConvert.DeserializeObject<ChatApiResponse>(responseContent);
        string responseText = chatResponse.Choices[0].Message.Content;
        
        var echo = new ServerRespondsToUser()
        {
            message = "ChatBot: " + responseText,
            isUser = false
        };
        var messageToClient = JsonConvert.SerializeObject(echo);

        socket.Send(messageToClient);
    }
}

public class Message
{
    public string Content { get; set; }
}
    
public class Choice
{
    public Message Message { get; set; }
}
    
public class ChatApiResponse
{
    public List<Choice> Choices { get; set; }
}