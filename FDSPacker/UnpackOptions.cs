using CommandLine;


namespace wtf.cluster.FDSPacker
{
    [Verb("unpack")]
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
