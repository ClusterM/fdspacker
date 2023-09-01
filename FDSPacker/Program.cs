using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using com.clusterrr.Famicom.Containers;
using wtf.cluster.FDSPacker.JsonTypes;

namespace wtf.cluster.FDSPacker
{
    internal class Program
    {
        public const string APP_NAME = "FDSPacker";
        public const string REPO_PATH = "https://github.com/ClusterM/fdspacker";
        public static DateTime BUILD_TIME = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(long.Parse(Properties.Resources.BuildTime.Trim()));
        const string DISK_INFO_FILE = "diskinfo.json";

        static int Main(string[] args)
        {
            try
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

                switch (args[0].ToLower())
                {
                    case "unpack":
                        FdsUnpack(args[1], args[2]);
                        break;
                    case "pack":
                        throw new NotImplementedException("Not done yet.");
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine($"ERROR {ex.GetType()}: {ex.Message}{ex.StackTrace}");
#else
                Console.WriteLine($"ERROR: {ex.Message}");
#endif
                return 1;
            }

            return 0;
        }

        static void PrintHelp()
        {
            Console.WriteLine($"Usage:");
            Console.WriteLine($" {Path.GetFileName(Process.GetCurrentProcess().MainModule?.FileName)} pack <diskinfo.json> <output.fds>");
            Console.WriteLine($" {Path.GetFileName(Process.GetCurrentProcess().MainModule?.FileName)} unpack <input.fds> <output directory>");
        }

        static void FdsUnpack(string fdsFile, string outputDir)
        {
            var fds = FdsFile.FromFile(fdsFile);
            Directory.CreateDirectory(outputDir);

            // Copy data to JSON object
            var root = new FdsJsonRoot();
            var usedFiles = new HashSet<string>();
            foreach (var side in fds.Sides)
            {
                var outSide = new FdsJsonSide();
                CopyPropertiesToJson(side, outSide);

                // Process every file
                foreach (var file in side.Files)
                {
                    var outFile = new FdsJsonFile();
                    CopyPropertiesToJson(file, outFile);
                    // Avoid filename duplication
                    var name = file.FileName;
                    var altName = name;
                    int id = 1;
                    while (usedFiles.Contains(altName!))
                    {
                        id++;
                        altName = $"{name}_{id}";
                    }
                    usedFiles.Add(altName);
                    outFile.Data = $"{altName}.bin";
                    var targetPath = Path.Combine(outputDir, outFile.Data);
                    File.WriteAllBytes(targetPath, file.Data.ToArray());
                    outSide.Files.Add(outFile);
                }

                root.Sides.Add(outSide);
            }

            // Save JSON
            var targetFile = Path.Combine(outputDir, DISK_INFO_FILE);
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.Never,
                ReadCommentHandling = JsonCommentHandling.Skip,
                Converters = { new JsonStringEnumConverter() },
                PropertyNamingPolicy = new SnakeCaseNamingPolicy()
            };
            var json = JsonSerializer.Serialize(root, options);
            File.WriteAllText(targetFile, json);
        }

        // Easy way to copy properties between types
        public static void CopyPropertiesToJson(object source, object destination)
        {
            if (source == null || destination == null) return;

            var sourceType = source.GetType();
            var destinationType = destination.GetType();

            foreach (var sourceProperty in sourceType.GetProperties())
            {
                var destinationProperty = destinationType.GetProperty(sourceProperty.Name);
                if (destinationProperty != null)
                {
                    if (destinationProperty.PropertyType == sourceProperty.PropertyType)
                    {
                        destinationProperty.SetValue(destination, sourceProperty.GetValue(source));
                    }
                    else if (destinationProperty.PropertyType == typeof(string) && sourceProperty.PropertyType.IsEnum)
                    {
                        destinationProperty.SetValue(destination, $"{(int)sourceProperty.GetValue(source)!}");
                    }
                    else if (destinationProperty.PropertyType == typeof(string))
                    {
                        destinationProperty.SetValue(destination, $"{sourceProperty.GetValue(source)}");
                    }
                }
            }
        }

        public class SnakeCaseNamingPolicy : JsonNamingPolicy
        {
            public override string ConvertName(string name)
                => ToSnakeCase(name);
            private static string ToSnakeCase(string str)
                => Regex.Replace(str, "(?<!^)([A-Z])", "_$1").ToLower();
        }
    }
}
