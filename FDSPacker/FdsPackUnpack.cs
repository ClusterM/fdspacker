using com.clusterrr.Famicom.Containers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text.Json;
using System.Text.RegularExpressions;
using wtf.cluster.FDSPacker.JsonTypes;

namespace wtf.cluster.FDSPacker
{
    public static class FdsPackUnpack
    {
        const string DISK_INFO_FILE = "diskinfo.json";

        static JsonSerializerSettings jsonOptions = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Include,
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            },
            Formatting = Formatting.Indented
        };

        // Unpack a .fds file
        public static void Unpack(UnpackOptions options)
        {
            var write = (string text) => { if (!options.Quiet) Console.Write(text); };
            var writeLine = (string text) => { if (!options.Quiet) Console.WriteLine(text); };

            write($"Reading {options.InputFile}... ");
            var fds = FdsFile.FromFile(options.InputFile);
            writeLine("OK");
            Directory.CreateDirectory(options.OutputDir);

            // Copy data to a JSON object
            var root = new FdsJsonRoot();
            var invalidCharsPattern = $"[{Regex.Escape(new string(Path.GetInvalidFileNameChars()))}]";
            for (var sideId = 0; sideId < fds.Sides.Count; sideId++)
            {
                var side = fds.Sides[sideId];
                var outSide = new FdsJsonSide();
                outSide.WriteUnknown = !options.NoUnknown;
                var sideDirName = $"side_{sideId + 1}";
                var sideFullPath = Path.Combine(options.OutputDir, sideDirName);
                Directory.CreateDirectory(sideFullPath);
                CopyProperties(side, outSide);

                // Process every file
                var usedFiles = new HashSet<string>();
                foreach (var file in side.Files)
                {
                    var outFile = new FdsJsonFile();
                    CopyProperties(file, outFile);
                    // Avoid filename duplication
                    var name = Regex.Replace(file.FileName.ToLower(), invalidCharsPattern, "_");
                    var altName = name;
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
                    write($"Saving {file.FileName} as {outFile.Data}... ");
                    File.WriteAllBytes(targetPath, file.Data.ToArray());
                    writeLine("OK");

                    outSide.Files.Add(outFile);
                }

                root.Sides.Add(outSide);
            }

            // Save JSON
            var targetFile = Path.Combine(options.OutputDir, DISK_INFO_FILE);
            var json = JsonConvert.SerializeObject(root, jsonOptions);
            write($"Saving {targetFile}... ");
            File.WriteAllText(targetFile, json);
            writeLine("OK");
        }

        // Pack a .fds file
        public static void Pack(PackOptions options)
        {
            var write = (string text) => { if (!options.Quiet) Console.Write(text); };
            var writeLine = (string text) => { if (!options.Quiet) Console.WriteLine(text); };

            var inputFile = options.InputFile;
            if (Directory.Exists(inputFile))
                inputFile = Path.Combine(inputFile, DISK_INFO_FILE);
            write($"Reading {inputFile}... ");
            var jsonData = File.ReadAllText(inputFile);
            var root = JsonConvert.DeserializeObject<FdsJsonRoot>(jsonData, jsonOptions);
            if (root == null) throw new InvalidDataException("Invalid input JSON file");
            writeLine($"OK");

            // Copy data from a JSON object
            var output = new FdsFile();
            foreach (var side in root.Sides)
            {
                var outputSide = new FdsDiskSide();
                CopyProperties(side, outputSide);
                foreach (var file in side.Files)
                {
                    if (string.IsNullOrEmpty(file.Data))
                        throw new InvalidDataException("\'data\' field missed in JSON file");
                    var outputFile = new FdsDiskFile();
                    CopyProperties(file, outputFile);
                    var dataPath = Path.Combine(Path.GetDirectoryName(inputFile)!, file.Data);
                    write($"Reading {file.Data}... ");
                    outputFile.Data = File.ReadAllBytes(dataPath);
                    writeLine($"OK");
                    outputSide.Files.Add(outputFile);
                }

                output.Sides.Add(outputSide);
            }

            write($"Saving {options.OutputFile}... ");
            output.Save(options.OutputFile, options.UseHeader);
            writeLine($"OK");

            writeLine($"Done.");
        }

        // Easy way to copy properties between types
        public static void CopyProperties(object source, object destination)
        {
            if (source == null || destination == null) return;

            var sourceType = source.GetType();
            var destinationType = destination.GetType();

            foreach (var sourceProperty in sourceType.GetProperties())
            {
                var destinationProperty = destinationType.GetProperty(sourceProperty.Name);
                if (destinationProperty == null)
                    continue;
                if (destinationProperty.PropertyType != sourceProperty.PropertyType)
                    continue;
                destinationProperty.SetValue(destination, sourceProperty.GetValue(source));
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
}
