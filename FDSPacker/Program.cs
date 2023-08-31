using System.Diagnostics;
using System.Reflection;

namespace wtf.cluster.FDSPacker
{
    internal class Program
    {
        public const string APP_NAME = "FDSPacker";
        public const string REPO_PATH = "https://github.com/ClusterM/fdspacker";
        public static DateTime BUILD_TIME = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(long.Parse(Properties.Resources.BuildTime.Trim()));

        static int Main(string[] args)
        {
            var version = Assembly.GetExecutingAssembly()?.GetName()?.Version;
            var versionStr = $"{version?.Major}.{version?.Minor}{((version?.Build ?? 0) > 0 ? $"{(char)((byte)'a' + version!.Build)}" : "")}";
            Console.WriteLine($"{APP_NAME} " +
#if !INTERIM
                $"v{versionStr}"
#else
                "interim version"
#endif
#if DEBUG
                + " (debug)"
#endif
            );
#if INTERIM || DEBUG
            Console.WriteLine($"  Commit: {Properties.Resources.GitCommit} @ {REPO_PATH}");
            Console.WriteLine($"  Build time: {BUILD_TIME.ToLocalTime()}");
#endif
            Console.WriteLine("  (c) Alexey 'Cluster' / https://cluster.wtf / cluster@cluster.wtf");
            Console.WriteLine("");

            if (args.Length < 3 || args[0] == "help" || args[0] == "--help" || args[0] == "/?")
            {
                PrintHelp();
                return 0;
            }

            return 0;
        }

        static void PrintHelp()
        {
            Console.WriteLine($"Usage:");
            Console.WriteLine($" {Path.GetFileName(Process.GetCurrentProcess().MainModule?.FileName)} pack <diskinfo.json> <output.fds>");
            Console.WriteLine($" {Path.GetFileName(Process.GetCurrentProcess().MainModule?.FileName)} unpack <input.fds> <output directory>");
        }
    }
}
