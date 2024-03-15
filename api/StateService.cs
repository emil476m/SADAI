using Fleck;

namespace socketAPIFirst;

public class WebSocketMetaData(IWebSocketConnection connection)
{
    public IWebSocketConnection Connection { get; set; } = connection;
}

public static class StateService
{
    public static Dictionary<Guid, WebSocketMetaData> WsConections = new();

    public static Dictionary<Guid, Dictionary<string, string>> languages
        = new();
    public static bool AddConection(IWebSocketConnection ws)
    {
        return WsConections.TryAdd(ws.ConnectionInfo.Id, new WebSocketMetaData(ws));
    }


}