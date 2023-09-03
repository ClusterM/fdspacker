using CommandLine;
using CommandLine.Text;
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
            try
            {
                var parser = new Parser(with => with.HelpWriter = null);
                var parserResult = parser.ParseArguments<PackOptions, UnpackOptions>(args);
                parserResult
                    .WithParsed<PackOptions>(options => {
                        if (!options.Quiet) PrintHeader();
                        FdsPackUnpack.Pack(options);
                     })
                    .WithParsed<UnpackOptions>(options => {
                        if (!options.Quiet) PrintHeader();
                        FdsPackUnpack.Unpack(options);
                     })
                    .WithNotParsed(errs =>
                    {
                        PrintHelp(errs);
                        Environment.Exit(1);
                    });
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine($"ERROR {ex.GetType()}: {ex.Message}{ex.StackTrace}");
#else
                Console.WriteLine($"ERROR: {ex.Message}");
#endif
                return 2;
            }
            return 0;
        }

        static void PrintHeader()
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
            Console.WriteLine("  (c) Alexey Cluster / https://cluster.wtf / cluster@cluster.wtf");
            Console.WriteLine("");
        }

        static void PrintHelp(IEnumerable<Error> errs)
        {
            PrintHeader();
            foreach (var err in errs)
            {
                if (err.Tag == ErrorType.NoVerbSelectedError) continue;
                Console.WriteLine($"ERROR: " + err.Tag switch
                {
                    ErrorType.UnknownOptionError => "unknown option",
                    ErrorType.MissingRequiredOptionError => "missing required option",
                    ErrorType.BadVerbSelectedError => "unknown command",
                    _ => $"can't parse command line: {err.Tag}"
                } + ".");
            }
            Console.WriteLine($"Usage:");
            Console.WriteLine($" {Path.GetFileName(Process.GetCurrentProcess().MainModule?.FileName)} pack [options] <diskinfo.json> <output.fds>");
            Console.WriteLine($"  Options:");
            Console.WriteLine($"   -d, --header       - write .fds file with header");
            Console.WriteLine($"   -q, --quiet        - do not print anythng to the console");
            Console.WriteLine($" {Path.GetFileName(Process.GetCurrentProcess().MainModule?.FileName)} unpack [options] <input.fds> <output directory>");
            Console.WriteLine($"  Options:");
            Console.WriteLine($"   -u, --no-unknown   - do not extract unknown fields");
            Console.WriteLine($"   -q, --quiet        - do not print anythng to the console");
        }
    }
}
