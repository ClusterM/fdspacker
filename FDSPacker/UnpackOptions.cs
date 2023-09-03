using CommandLine;


namespace wtf.cluster.FDSPacker
{
    [Verb("unpack")]
    class UnpackOptions
    {
        public UnpackOptions(bool noUnknown, string inputFile, string outputDir)
        {
            NoUnknown = noUnknown;
            InputFile = inputFile;
            OutputDir = outputDir;
        }

        [Option('u', "no-unknown", Default = false)]
        public bool NoUnknown { get; }
        [Value(0, Required = true)]
        public string InputFile { get; }
        [Value(1, Required = true)]
        public string OutputDir { get; }
    }
}
