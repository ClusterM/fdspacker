using CommandLine;

namespace wtf.cluster.FDSPacker
{
    [Verb("pack")]
    class PackOptions
    {
        public PackOptions(bool useHeader, string inputFile, string outputFile)
        {
            UseHeader = useHeader;
            InputFile = inputFile;
            OutputFile = outputFile;
        }

        [Option('d', "header", Default = false)]
        public bool UseHeader { get; }
        [Value(0, Required = true)]
        public string InputFile { get; }
        [Value(1, Required = true)]
        public string OutputFile { get; }
    }
}
