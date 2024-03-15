using System.Net.Sockets;
using System.Text.Json;
using Azure;
using Azure.AI.Translation.Text;
using Fleck;
using lib;
using socketAPIFirst.Dtos;

namespace socketAPIFirst.ClientRequest;

public class ClientWantsToGetLangueges: BaseEventHandler<ClientWantsToGetLanguegesDto>
{


    public override async Task Handle(ClientWantsToGetLanguegesDto dto, IWebSocketConnection socket)
    {
        string apikey = Environment.GetEnvironmentVariable("tapikey");
        string region = Environment.GetEnvironmentVariable("tregion");
        TextTranslationClient ttc = new TextTranslationClient(new AzureKeyCredential(apikey),
            new Uri("https://api.cognitive.microsofttranslator.com"));
        Response<GetLanguagesResult> response = await ttc.GetLanguagesAsync().ConfigureAwait(false);
        GetLanguagesResult languages = response.Value;
        Dictionary<string, string> listofLanguages = new ();
        List<string> listofNames = new List<string>();
        foreach (var translationLanguage in languages.Translation)
        {
            listofLanguages.TryAdd(translationLanguage.Value.Name, translationLanguage.Key);
            StateService.languages.TryAdd(socket.ConnectionInfo.Id, listofLanguages);
            listofNames.Add(translationLanguage.Value.Name);
        }

        socket.Send(JsonSerializer.Serialize(new ServerReturnsListOfLanguageNames()
        {names = listofNames}));
    }
}