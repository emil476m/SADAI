using System.Reflection;
using System.Text.Json;
using Fleck;
using lib;
using Serilog;
using api.Middleware;
using socketAPIFirst;

public static class StartUp
{
    public static void Main(String[] args)
    {
        var app = StatUp(args);
        app.Run();
    }

    public static WebApplication StatUp(String[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(
                outputTemplate: "\n{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}\n")
            .CreateLogger();

        var server = new WebSocketServer("ws://0.0.0.0:8181");
        var builder = WebApplication.CreateBuilder(args);
        var clientEventHandler = builder.FindAndInjectClientEventHandlers(Assembly.GetExecutingAssembly());

        var app = builder.Build();

        server.Start(ws =>
        {
            ws.OnOpen = () =>
            {
                StateService.AddConection(ws);
            };

            ws.OnClose = () =>
            {
                StateService.WsConections.Remove(ws.ConnectionInfo.Id);
            };


            ws.OnMessage = async message =>
            {
                try
                {
                    await app.InvokeClientEventHandler(clientEventHandler, ws, message);
                }
                catch (Exception e)
                {
                    e.Handle(ws, e.Message);
                }
            };
        });

        return app;
    }
}