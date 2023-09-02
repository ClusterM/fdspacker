using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using com.clusterrr.Famicom.Containers;
using CommandLine;
using CommandLine.Text;
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

                var parser = new Parser(with => with.HelpWriter = null);
                var parserResult = parser.ParseArguments<PackOptions, UnpackOptions>(args);
                parserResult
                    .WithParsed<PackOptions>(options => FdsPack(options))
                    .WithParsed<UnpackOptions>(options => FdsUnpack(options))
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

        static void DisplayHelp<T>(ParserResult<T> result, IEnumerable<Error> errs)
        {
            var helpText = HelpText.AutoBuild(result, h =>
            {
                var version = Assembly.GetExecutingAssembly()?.GetName()?.Version;
                var versionStr = $"{version?.Major}.{version?.Minor}{((version?.Build ?? 0) > 0 ? $"{(char)((byte)'a' + version!.Build)}" : "")}";
                h.AdditionalNewLineAfterOption = false;
                h.AddDashesToOption = true;
                h.AutoVersion = false;
                return HelpText.DefaultParsingErrorsHandler(result, h);
            }, e => e);
            Console.WriteLine(helpText);
        }

        static void PrintHelp(IEnumerable<Error> errs)
        {
            Console.WriteLine($"Usage:");
            Console.WriteLine($" {Path.GetFileName(Process.GetCurrentProcess().MainModule?.FileName)} pack [--header] <diskinfo.json> <output.fds>");
            Console.WriteLine($" {Path.GetFileName(Process.GetCurrentProcess().MainModule?.FileName)} unpack <input.fds> <output directory>");
        }

        static void FdsUnpack(UnpackOptions options)
        {
            var fds = FdsFile.FromFile(options.InputFile);
            Directory.CreateDirectory(options.OutputDir);

            // Copy data to JSON object
            var root = new FdsJsonRoot();
            for (var sideId = 0; sideId < fds.Sides.Count; sideId++)
            {
                var side = fds.Sides[sideId];
                var outSide = new FdsJsonSide();
                var sideDirName = $"side_{sideId + 1}";
                var sideFullPath = Path.Combine(options.OutputDir, sideDirName);
                Directory.CreateDirectory(sideFullPath);
                CopyPropertiesToJson(side, outSide);

                // Process every file
                var usedFiles = new HashSet<string>();
                foreach (var file in side.Files)
                {
                    var outFile = new FdsJsonFile();
                    CopyPropertiesToJson(file, outFile);
                    // Avoid filename duplication
                    var name = file.FileName;
                    var altName = name.ToLower();
                    int id = 1;
                    while (usedFiles.Contains(altName!))
                    {
                        id++;
                        altName = $"{name}_{id}";
                    }
                    usedFiles.Add(altName);
                    // Select extention
                    var ext = file.FileKind switch
                    {
                        FdsBlockFileHeader.Kind.Program => "prg",
                        FdsBlockFileHeader.Kind.Character => "chr",
                        FdsBlockFileHeader.Kind.NameTable => "nt",
                        _ => ".bin"
                    };
                    // Relative path
                    outFile.Data = Path.Combine(sideDirName, $"{altName}.{ext}")
                        .Replace("\\", "/"); // Unix-style
                    // Absolute path
                    var targetPath = Path.Combine(options.OutputDir, outFile.Data);
                    // Saving file data
                    File.WriteAllBytes(targetPath, file.Data.ToArray());

                    outSide.Files.Add(outFile);
                }

                root.Sides.Add(outSide);
            }

            // Save JSON
            var targetFile = Path.Combine(options.OutputDir, DISK_INFO_FILE);
            var jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.Never,
                ReadCommentHandling = JsonCommentHandling.Skip,
                Converters = { new JsonStringEnumConverter() },
                PropertyNamingPolicy = new SnakeCaseNamingPolicy()
            };
            var json = JsonSerializer.Serialize(root, jsonOptions);
            File.WriteAllText(targetFile, json);
        }

        static void FdsPack(PackOptions options)
        {
            var inputFile = options.InputFile;
            var jsonOptions = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.Never,
                ReadCommentHandling = JsonCommentHandling.Skip,
                Converters = { new JsonStringEnumConverter() },
                PropertyNamingPolicy = new SnakeCaseNamingPolicy()
            };
            var jsonData = File.ReadAllText(inputFile);
            var root = JsonSerializer.Deserialize<FdsJsonRoot>(jsonData, jsonOptions);

            var output = new FdsFile();
            foreach (var side in root.Sides)
            {
                var outputSide = new FdsDiskSide();

                CopyPropertiesFromJson(side, outputSide);
                foreach(var file in side.Files)
                {
                    var outputFile = new FdsDiskFile();
                    CopyPropertiesFromJson(file, outputFile);
                    var dataPath = Path.Combine(Path.GetDirectoryName(inputFile), file.Data);
                    outputFile.Data = File.ReadAllBytes(dataPath);
                    outputSide.Files.Add(outputFile);
                }

                output.Sides.Add(outputSide);
            }

            output.Save(options.OutputFile, options.UseHeader);
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
                if (destinationProperty != null && destinationProperty.Name != "Files")
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
                    } else
                    {
                        throw new InvalidDataException("Unknown data combination");
                    }
                }
            }
        }

        // Easy way to copy properties between types
        public static void CopyPropertiesFromJson(object source, object destination)
        {
            if (source == null || destination == null) return;

            var sourceType = source.GetType();
            var destinationType = destination.GetType();

            foreach (var sourceProperty in sourceType.GetProperties())
            {
                var destinationProperty = destinationType.GetProperty(sourceProperty.Name);
                if (destinationProperty != null && destinationProperty.Name != "Data" && destinationProperty.Name != "Files")
                {
                    var sourceValue = sourceProperty.GetValue(source);
                    if (sourceProperty.PropertyType == destinationProperty.PropertyType)
                    {                        
                        destinationProperty.SetValue(destination, sourceValue);
                    }
                    else if (sourceProperty.PropertyType == typeof(string) && destinationProperty.PropertyType.IsEnum)
                    {
                        object outputValue;
                        if (!Enum.TryParse(destinationProperty.PropertyType, sourceValue as string, true, out outputValue))
                            outputValue = (sourceValue as string).ParseHex();
                        if (outputValue == null) 
                            throw new InvalidDataException($"Invalid value \"{sourceValue}\" for \"{sourceProperty.Name}\"");
                        destinationProperty.SetValue(destination, outputValue);
                    }
                    else if (sourceProperty.PropertyType == typeof(string))
                    {
                        destinationProperty.SetValue(destination, (sourceValue as string).ParseHex());
                    }
                    else
                    {
                        throw new InvalidDataException("Unknown data combination");
                    }
                }
            }
        }

        private class SnakeCaseNamingPolicy : JsonNamingPolicy
        {
            public override string ConvertName(string name)
                => ToSnakeCase(name);
            private static string ToSnakeCase(string str)
                => Regex.Replace(str, "(?<!^)([A-Z])", "_$1").ToLower();
        }
    }

    [Verb("pack", HelpText = "Pack a .fds file.")]
    class PackOptions
    {
        public PackOptions(bool useHeader, string inputFile, string outputFile)
        {
            UseHeader = useHeader;
            InputFile = inputFile;
            OutputFile = outputFile;
        }

        [Option("head", Default = false)]
        public bool UseHeader { get; }
        [Value(0, Required = true)]
        public string InputFile { get; }
        [Value(1, Required = true)]
        public string OutputFile { get; }
    }

    [Verb("unpack", HelpText = "Unpack a .fds file.")]
    class UnpackOptions
    {
        [Value(0, Required = true)]
        public string InputFile { get; }
        [Value(1, Required = true)]
        public string OutputDir { get; }

        public UnpackOptions(string inputFile, string outputDir)
        {
            InputFile = inputFile;
            OutputDir = outputDir;
        }
    }
}
