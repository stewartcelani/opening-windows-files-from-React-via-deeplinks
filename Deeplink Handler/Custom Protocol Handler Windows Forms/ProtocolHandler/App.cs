using Microsoft.Extensions.Logging;

public class App(ILogger<App> logger, ProtocolHandlerService protocolHandler)
{
    public Task RunAsync(string[] args)
    {
        logger.LogInformation("Starting protocol handler");

        if (args.Length > 0)
        {
            protocolHandler.HandleProtocol(args[0]);
        }
        else if (ProtocolHandlerService.IsAdministrator())
        {
            protocolHandler.RegisterProtocolHandler();
        }
        else
        {
            MessageBox.Show("Please run as administrator to register the protocol handler.", 
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        return Task.CompletedTask;
    }
}