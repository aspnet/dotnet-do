using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DotNetDo.Helpers;
using Microsoft.Extensions.PlatformAbstractions;

namespace DotNetDo.BuildSystem.ManagedCode.DotNet
{
    public class DotNetCli
    {
        public static readonly DotNetCli Default = GetDefaultCli();

        public string BasePath { get; }

        public string DriverPath => Path.Combine(BasePath, "bin", OS.ExeName("dotnet"));

        public DotNetCli(string path)
        {
            BasePath = path;
        }

        public CommandSpec Command(IEnumerable<string> args) => new CommandSpec(DriverPath, args);

        public CommandSpec Command(params string[] args) => new CommandSpec(DriverPath, args);

        public CommandSpec Restore(IEnumerable<string> args) => new CommandSpec(DriverPath, GetArgs("restore", args));
        public CommandSpec Restore(params string[] args) => new CommandSpec(DriverPath, GetArgs("restore", args));

        public CommandSpec Build(IEnumerable<string> args) => new CommandSpec(DriverPath, GetArgs("build", args));
        public CommandSpec Build(params string[] args) => new CommandSpec(DriverPath, GetArgs("build", args));

        public CommandSpec Pack(IEnumerable<string> args) => new CommandSpec(DriverPath, GetArgs("pack", args));
        public CommandSpec Pack(params string[] args) => new CommandSpec(DriverPath, GetArgs("pack", args));

        public CommandSpec Run(IEnumerable<string> args) => new CommandSpec(DriverPath, GetArgs("run", args));
        public CommandSpec Run(params string[] args) => new CommandSpec(DriverPath, GetArgs("run", args));

        public CommandSpec Test(IEnumerable<string> args) => new CommandSpec(DriverPath, GetArgs("test", args));
        public CommandSpec Test(params string[] args) => new CommandSpec(DriverPath, GetArgs("test", args));

        private static IEnumerable<string> GetArgs(string command, IEnumerable<string> args) => Enumerable.Concat(new[] { command }, args);

        public bool Exists()
        {
            return File.Exists(DriverPath);
        }

        public string GetVersion()
        {
            var versionFile = Path.Combine(BasePath, ".version");
            var lines = File.ReadAllLines(versionFile);
            return $"{lines[1]} (commit {lines[0]})";
        }

        private static DotNetCli GetDefaultCli()
        {
            // Hacky!
            var cliPath = Environment.GetEnvironmentVariable("DOTNET_CLI_PATH");
            if (!string.IsNullOrEmpty(cliPath))
            {
                return new DotNetCli(cliPath);
            }
            else if (PlatformServices.Default.Runtime.OperatingSystemPlatform == Platform.Windows)
            {
                return new DotNetCli(Path.Combine(
                    Environment.GetEnvironmentVariable("LOCALAPPDATA"),
                    "Microsoft",
                    "dotnet",
                    "cli"));
            }
            else
            {
                return new DotNetCli("/usr/local/share/dotnet/cli");
            }
        }
    }
}
