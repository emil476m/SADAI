using System.Data;
using System.Text.Json;
using Azure;
using Azure.AI.Translation.Text;
using Fleck;
using lib;
using socketAPIFirst.Dtos;

namespace socketAPIFirst.ClientRequest;

public class ClientWantsToTranslateText : BaseEventHandler<ClientWantsToTranslateTextDto>
{
    public override async Task Handle(ClientWantsToTranslateTextDto dto, IWebSocketConnection socket)
    {
        try
        {
            var lan = StateService.languages[socket.ConnectionInfo.Id];
            var to = lan[dto.toLan];
            var from = lan[dto.fromLan];
            string apikey = Environment.GetEnvironmentVariable("tapikey");
            string region = Environment.GetEnvironmentVariable("tregion");
            TextTranslationClient ttc = new TextTranslationClient(new AzureKeyCredential(apikey), region);

            Response<IReadOnlyList<TranslatedTextItem>> response =
                await ttc.TranslateAsync(to, dto.text, from).ConfigureAwait(false);
            IReadOnlyList<TranslatedTextItem> translations = response.Value;
            TranslatedTextItem translation = translations.FirstOrDefault();

            var echo = new ServerRespondsToUser()
            {
                message = translation?.Translations?.FirstOrDefault()?.Text,
                isUser = false
            };
            var messageToClient = JsonSerializer.Serialize(echo);

            socket.Send(messageToClient);
        }
        catch (Exception e)
        {
            throw new Exception("Failed to translate please try again");
        }
    }
}