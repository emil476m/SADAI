using System.Text.Json;
using Fleck;
using lib;
using socketAPIFirst.Dtos;

namespace socketAPIFirst.ClientRequest;

public class ClientWantsToTextServer : BaseEventHandler<ClientWantsToTextServeDto>
{
    public override Task Handle(ClientWantsToTextServeDto dto, IWebSocketConnection socket)
    {
        var echo = new ClientWantsToTextServeDto()
        {
            message = "echo: " + dto.message,
            isUser = false
        };
        var messageToClient = JsonSerializer.Serialize(echo);

        socket.Send(messageToClient);
        
        return Task.CompletedTask;
    }
}