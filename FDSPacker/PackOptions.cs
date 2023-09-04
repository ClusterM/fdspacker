using CommandLine;

namespace wtf.cluster.FDSPacker
{
    [Verb("pack")]
    public class PackOptions
    {
        public PackOptions(bool useHeader, bool quiet, string inputFile, string outputFile)
        {
            UseHeader = useHeader;
            Quiet = quiet;
            InputFile = inputFile;
            OutputFile = outputFile;
        }

        [Option('d', "header", Default = false)]
        public bool UseHeader { get; }
        [Option('q', "quiet", Default = false)]
        public bool Quiet { get; }
        [Value(0, Required = true)]
        public string InputFile { get; }
        [Value(1, Required = true)]
        public string OutputFile { get; }
    }
}
