using System.Diagnostics;
using System.Security.Principal;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;

public class ProtocolHandlerService(ILogger<ProtocolHandlerService> logger)
{
    private const string ProtocolName = "mycustomprotocolhandler";

    public void HandleProtocol(string url)
    {
        try
        {
            logger.LogInformation("Processing URL: {Url}", url);
            var uri = new Uri(url);
            var action = uri.Host.ToLower();
            var path = Uri.UnescapeDataString(uri.AbsolutePath.TrimStart('/')).Replace('/', '\\');

            switch (action)
            {
                case "openfile":
                    if (File.Exists(path))
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = path,
                            UseShellExecute = true
                        });
                        logger.LogInformation("File opened: {Path}", path);
                    }
                    else
                    {
                        logger.LogError("File not found: {Path}", path);
                        MessageBox.Show($"File not found: {path}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;

                case "openfolder":
                    if (Directory.Exists(path))
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = "explorer.exe",
                            Arguments = $"\"{path}\"",
                            UseShellExecute = false
                        });
                        logger.LogInformation("Folder opened: {Path}", path);
                    }
                    else
                    {
                        logger.LogError("Folder not found: {Path}", path);
                        MessageBox.Show($"Folder not found: {path}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;

                default:
                    logger.LogError("Invalid action: {Action}", action);
                    MessageBox.Show($"Invalid action: {action}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error handling protocol");
            MessageBox.Show($"Error handling protocol: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    public void RegisterProtocolHandler()
    {
        var exePath = $"\"{Environment.ProcessPath}\" \"%1\"";
        
        try
        {
            using var key = Registry.ClassesRoot.CreateSubKey(ProtocolName);
            key.SetValue("", $"URL:{ProtocolName} Protocol");
            key.SetValue("URL Protocol", "");

            using (var defaultIcon = key.CreateSubKey("DefaultIcon"))
            {
                defaultIcon.SetValue("", exePath);
            }

            using (var commandKey = key.CreateSubKey(@"shell\open\command"))
            {
                commandKey.SetValue("", exePath);
            }

            logger.LogInformation("Protocol handler registered successfully");
            MessageBox.Show("Protocol handler registered successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to register protocol handler");
            MessageBox.Show($"Failed to register protocol handler: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    public static bool IsAdministrator()
    {
        var identity = WindowsIdentity.GetCurrent();
        var principal = new WindowsPrincipal(identity);
        return principal.IsInRole(WindowsBuiltInRole.Administrator);
    }
}