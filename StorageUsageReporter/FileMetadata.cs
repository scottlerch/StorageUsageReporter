using System;

namespace StorageUsageReporter
{
    internal class FileMetadata
    {
        public string DirectoryPath { get; set; }

        public string FileName { get; set; }

        public string FileExtension { get; set; }

        public long FileSize { get; set; }

        public DateTime CreationTimeUtc { get; set; }

        public DateTime LastAccessTimeUtc { get; set; }

        public DateTime LastWriteTimeUtc { get; set; }
    }
}
