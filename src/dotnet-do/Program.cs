using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace DotNetDo.Command
{
    public class Program
    {
        public static int Main(string[] args)
        {
            // Check if the `init` task is being run
            if (args.Any(a => a.Equals("init", StringComparison.OrdinalIgnoreCase)))
            {
                Console.Error.WriteLine("The 'init' task is reserved for the 'init' command, which is not yet implemented.");
                return 1;
            }

            // Walk up the file system tree to find the project
            var currentDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());
            var candidate = Path.Combine(currentDirectory.FullName, "tasks", "project.json");
            while(!File.Exists(candidate) && currentDirectory != null)
            {
                currentDirectory = currentDirectory.Parent;
                candidate = Path.Combine(currentDirectory.FullName, "tasks", "project.json");
            }

            if(!File.Exists(candidate))
            {
                Console.Error.WriteLine("Failed to locate tasks file.");
                return 2;
            }

            // Now run the thing!
            // TODO: Convert to dotnet run :)
            // TODO: Argument escaping. Spaces man... Spaces.
            var psi = new ProcessStartInfo()
            {
                FileName = "dnx",
                Arguments = $"-p {candidate} run {string.Join(" ", args)}",
                WorkingDirectory = Directory.GetCurrentDirectory()
            };

            // In theory this isn't needed while we're DNX hosted, but when we move to dotnet run, IApplicationEnvironment won't be available.
#if DNX451
            psi.EnvironmentVariables["DOTNET_DO_PROJECT"] = candidate;
#else
            psi.Environment["DOTNET_DO_PROJECT"] = candidate;
#endif

            var process = Process.Start(psi);
            process.WaitForExit();
            return process.ExitCode;
        }
    }
}
