using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Shortcake
{
    class Program
    {
        private static void Main(string[] args)
        {
            Strawberry presence = new Strawberry();
            presence.Initialize();
            if(!Process.GetProcesses().Where(x => x.ProcessName == presence.Process.ProcessName).Any())
                return;
            while(true)
            {
                presence.Update();
                if(!Process.GetProcesses().Where(x => x.ProcessName == presence.Process.ProcessName).Any())
                {
                    presence.Deinitialize();
                    Console.WriteLine("Thanks for using Bheithir!");
                    return;
                }
            }
        }
    }
}
