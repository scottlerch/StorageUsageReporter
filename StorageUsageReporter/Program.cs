using Alphaleonis.Win32.Filesystem;
using CommandLine;
using ServiceStack.Text;
using System;
using System.Diagnostics;

namespace StorageUsageReporter
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            var options = new Options();

            if (!Parser.Default.ParseArguments(args, options) || !ValidateArguments(options))
            {
                Environment.Exit(-1);
            }

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

        private static bool ValidateArguments(Options options)
        {
            var errors = options.GetValidationErrors();

            if (!string.IsNullOrEmpty(errors))
            {
                Console.WriteLine();
                Console.WriteLine(errors);
                return false;
            }

            return true;
        }
    }
}
