using CommandLine;

namespace wtf.cluster.FDSPacker
{
    [Verb("unpack")]
    class UnpackOptions
    {
        public UnpackOptions(bool noUnknown, bool quiet, string inputFile, string outputDir)
        {
            NoUnknown = noUnknown;
            Quiet = quiet;
            InputFile = inputFile;
            OutputDir = outputDir;
        }

        [Option('u', "no-unknown", Default = false)]
        public bool NoUnknown { get; }
        [Option('q', "quiet", Default = false)]
        public bool Quiet { get; }
        [Value(0, Required = true)]
        public string InputFile { get; }
        [Value(1, Required = true)]
        public string OutputDir { get; }
    }
}
