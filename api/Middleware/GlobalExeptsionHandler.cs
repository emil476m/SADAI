using System.Text.Json;
using Fleck;
using lib;
using Serilog;
using socketAPIFirst;

namespace api.Middleware;

public static class GlobalExeptsionHandler
{
    public static void Handle(this Exception exception, IWebSocketConnection socket, string? message)
    {
        Log.Error(exception, "this Wass caught in gloabal handler");
        socket.Send(JsonSerializer.Serialize(new ServerSendsErrorMessageToClient()
        {
            recivedMessage = message,
            errorMessage = exception.Message
        }));
    }
}

public class ServerSendsErrorMessageToClient : BaseDto
{
    public string recivedMessage { get; set; }
    public string errorMessage { get; set; }
}