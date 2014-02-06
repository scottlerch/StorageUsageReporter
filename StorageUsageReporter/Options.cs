using Alphaleonis.Win32.Filesystem;
using CommandLine;
using CommandLine.Text;
using System;

namespace StorageUsageReporter
{
    /// <summary>
    /// Command line options.
    /// </summary>
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
            return
                Environment.NewLine +
                HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current)) +
                @"For more information visit https://github.com/scottlerch/StorageUsageReporter" +
                Environment.NewLine;
        }

        public string GetValidationErrors()
        {
            foreach (var path in SourcePaths)
            {
                try
                {
                    if (!Directory.Exists(path))
                    {
                        return string.Format("The location '{0}' does not exist!", path);
                    }
                }
                catch (Exception ex)
                {
                    return string.Format("Unable to read from '{0}'! ({1})", path, ex.Message);
                }
            }

            try
            {
                File.WriteAllText(OutputPath, string.Empty);
            }
            catch (Exception ex)
            {
                return string.Format("Unable to write to '{0}'! ({1})", OutputPath, ex.Message);
            }

            return string.Empty;
        }
    }
}
