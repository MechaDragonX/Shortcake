using DiscordRPC;
using System;
using System.Diagnostics;
using System.Linq;

namespace Shortcake
{
    class Strawberry : Presence
    {
        public override void Initialize()
        {
            client = new DiscordRpcClient("835722093513408532");

            if(!Process.GetProcesses().Where(x => x.ProcessName.StartsWith("strawberry")).Any())
            {
                Console.WriteLine("Strawberry was not found! Is it open?");
                return;
            }
            Process = Process.GetProcesses().Where(x => x.ProcessName.StartsWith("strawberry")).ToList()[0];
            windowTitle = Process.MainWindowTitle;

            client.OnReady += (sender, e) => { };
            client.OnPresenceUpdate += (sender, e) => { };

            try
            {
                client.Initialize();
                Console.WriteLine("Successfully connected to client!");
            }
            catch(Exception e)
            {
                Console.WriteLine($"Connection to client was not successful!\nERROR: {e.Message}");
                return;
            }

            try { SetNewPresence(); }
            catch(Exception e)
            {
                Console.WriteLine($"Setting presence was not successful!\nERROR: {e.Message}");
                return;
            }
        }
        public override void Update()
        {
            client.OnPresenceUpdate += (sender, e) => { };
            client.Invoke();
            OnUpdate();
        }
        public override void Deinitialize()
        {
            client.ClearPresence();
            client.Dispose();
        }

        public override void OnUpdate()
        {
            Process process;
            try
            {
                process = Process.GetProcesses().Where(x => x.ProcessName.StartsWith("strawberry")).ToList()[0];
            }
            catch(Exception) { return; }

            if (process.MainWindowTitle != windowTitle)
            {
                Process = process;
                windowTitle = Process.MainWindowTitle;
                SetNewPresence();
            }
        }
        public override void SetNewPresence()
        {
            string details;
            try
            {
                if(!windowTitle.Contains(" - "))
                    details = "Sifting through records";
                else
                    details = windowTitle;
            }
            catch (Exception) { return; }

            string status;
            try
            {
                status = "Listening to Music";
            }
            catch(Exception) { return; }

            try
            {
                client.SetPresence(new RichPresence
                {
                    Details = details,
                    State = status,
                    Timestamps = new Timestamps(DateTime.UtcNow),
                    Assets = new Assets()
                    {
                        LargeImageKey = "shortcake",
                        LargeImageText = "Strawberry"
                    }
                });
                Console.WriteLine("Presence successfully set!");
            }
            catch (Exception)
            {
                Console.WriteLine("Presence was not set successfully!");
                return;
            }
        }
    }
}
