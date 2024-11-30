using System.Diagnostics;
using System.Security.Principal;
using Microsoft.Win32;

const string ProtocolName = "mycustomprotocolhandler";
const bool DEBUG_MODE = true;  // Set to false for production

if (args.Length > 0)
{
    HandleProtocol(args[0]);
}
else if (IsAdministrator())
{
    RegisterProtocolHandler();
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("Protocol handler registered successfully!");
    Console.ResetColor();
    PauseIfDebug();
}
else
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Please run as administrator to register the protocol handler.");
    Console.ResetColor();
    PauseIfDebug();
    return 1;
}

return 0;

void HandleProtocol(string url)
{
    try
    {
        Console.WriteLine($"Received URL: {url}");
        
        var uri = new Uri(url);
        var action = uri.Host.ToLower();
        
        // Convert the path to Windows format
        var path = Uri.UnescapeDataString(uri.AbsolutePath.TrimStart('/'))
            .Replace('/', '\\');

        Console.WriteLine($"Action: {action}");
        Console.WriteLine($"Path: {path}");

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
                }
                else
                {
                    var error = $"File not found: {path}";
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(error);
                    Console.ResetColor();
                    PauseIfDebug();
                }
                break;

            case "openfolder":
                if (Directory.Exists(path))
                {
                    var startInfo = new ProcessStartInfo
                    {
                        FileName = "explorer.exe",
                        Arguments = $"\"{path}\"",
                        UseShellExecute = false
                    };
                    
                    Console.WriteLine($"Opening folder with command: {startInfo.FileName} {startInfo.Arguments}");
                    Process.Start(startInfo);
                }
                else
                {
                    var error = $"Folder not found: {path}";
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(error);
                    Console.ResetColor();
                    PauseIfDebug();
                }
                break;

            default:
                var invalidAction = $"Invalid action: {action}";
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(invalidAction);
                Console.ResetColor();
                PauseIfDebug();
                break;
        }
    }
    catch (Exception ex)
    {
        var error = $"Error handling protocol: {ex.Message}";
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(error);
        Console.ResetColor();
        PauseIfDebug();
    }
}

void RegisterProtocolHandler()
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

        var success = $"Registered protocol handler: {ProtocolName}://";
        Console.WriteLine(success);
        Console.WriteLine($"Handler path: {exePath}");
    }
    catch (Exception ex)
    {
        var error = $"Failed to register protocol handler: {ex.Message}";
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(error);
        Console.ResetColor();
        PauseIfDebug();
    }
}

bool IsAdministrator()
{
    var identity = WindowsIdentity.GetCurrent();
    var principal = new WindowsPrincipal(identity);
    return principal.IsInRole(WindowsBuiltInRole.Administrator);
}

void PauseIfDebug()
{
    if (DEBUG_MODE)
    {
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }
}