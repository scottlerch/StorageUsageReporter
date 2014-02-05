using Alphaleonis.Win32.Filesystem;
using CommandLine;
using CommandLine.Text;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace StorageUsageReporter
{
    internal class Options
    {
        [OptionArray('p', "paths", Required = true, HelpText = "Paths to perform recursive analysis.")]
        public string[] SourcePaths { get; set; }

        [Option('o', "output", Required = false, DefaultValue = "output.csv", HelpText = "Output file to store analysis.")]
        public string OutputPath { get; set; }

        [Option('s', "sanitize", Required = false, DefaultValue = false, HelpText = "Randomize identifiable names.")]
        public bool Sanitize { get; set; }

        [Option('r', "rollup", Required = false, DefaultValue = false, HelpText = "Rollup metadata to directory level.")]
        public bool Rollup { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }

    internal class Program
    {
        internal static void Main(string[] args)
        {
            var options = new Options();

            if (!Parser.Default.ParseArguments(args, options)) Environment.Exit(-1);

            var stopwatch = Stopwatch.StartNew();

            Console.WriteLine();
            Console.WriteLine("Starting at {0}...", DateTime.Now);

            var analyzer = new FilesAnalyzer();
            var metadata = analyzer.ProcessPath(options.SourcePaths, options.Sanitize, options.Rollup);

            using (var file = File.Create(options.OutputPath))
            {
                CsvSerializer.SerializeToStream(metadata, file);
            }

            Console.WriteLine("Completed in {0}! ", stopwatch.Elapsed);

            Console.WriteLine();
            Console.WriteLine("Locations analyzed: {0}", string.Join(";", options.SourcePaths));
            Console.WriteLine("Detailed output: {0}", options.OutputPath);

            Console.WriteLine();
            Console.WriteLine("Number: {0}  Size: {1}", analyzer.TotalNumber, analyzer.TotalSize.ToPrettySize());
        }
    }
}
