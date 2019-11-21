using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace VpnTray.Driver.Cisco.AnyConnect
{
    public static class VpnCli
    {
        public static string VpnCliPath { get; set; } = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
            "Cisco",
            "Cisco AnyConnect Secure Mobility Client",
            "vpncli.exe");

        private static object _lock = new object();

        private static Task Run(string args, Action<string> action = null, Stream input = null)
        {
            lock (_lock)
            {
                var psi = new ProcessStartInfo
                {
                    FileName = VpnCliPath,
                    Arguments = args,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = action != null,
                    RedirectStandardInput = input != null
                };
                var process = new Process { StartInfo = psi };
                process.Start();
                if (action != null)
                {
                    process.OutputDataReceived += (s, e) =>
                    {
                        if (e.Data != null)
                        {
                            action(e.Data);
                        }
                    };
                    process.BeginOutputReadLine();
                }
                if (input != null)
                {
                    using (var reader = new StreamReader(input))
                    {
                        while (!reader.EndOfStream)
                        {
                            process.StandardInput.WriteLine(reader.ReadLine());
                        }
                    }
                }
                process.WaitForExit();
            }
            return Task.CompletedTask;
        }

        public static async Task<VpnStats> GetStats()
        {
            var stats = new VpnStats();
            Action<string> action = s =>
            {
                var line = s.Trim();
                if (line.StartsWith(">> state"))
                {
                    stats.State = line.Split(new[] { ':' }, 2)[1].TrimStart();
                }
                else if (line.StartsWith(">> notice: Connected to"))
                {
                    stats.Server = line.Substring(">> notice: Connected to".Length).TrimStart().TrimEnd('.');
                }
                else if (line.StartsWith("Client Address (IPv4)"))
                {
                    stats.Ip = line.Split(new[] { ':' }, 2)[1].TrimStart();
                }
            };
            await Run("stats", action);
            return stats;
        }

        public static async Task Connect(string name, string credsFile)
        {
            using (var input = File.OpenRead(credsFile))
            {
                await Run($"connect {name} -s", input: input);
            }
        }

        public static async Task Disconnect()
        {
            await Run("disconnect");
        }
    }
}
