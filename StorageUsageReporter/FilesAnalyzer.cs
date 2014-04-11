using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Wintellect.PowerCollections;
using Directory = Alphaleonis.Win32.Filesystem.Directory;
using Path = Alphaleonis.Win32.Filesystem.Path;

namespace StorageUsageReporter
{
    /// <summary>
    /// Process storage location and generate file metadata and basic statistics.
    /// </summary>
    internal class FilesAnalyzer
    {
        public long TotalSize { get; set; }

        public long TotalNumber { get; set; }

        public long AverageSize { get; set; }

        public long MedianSize { get; set; }

        public IEnumerable<FileMetadata> ProcessPath(IEnumerable<string> paths, bool sanitize, bool rollup, bool rollupRecursive)
        {
            return CollectStatistics(
                rollupRecursive?
                    paths.SelectMany(path => GetMetadataRollupRecursive(path, sanitize)) :
                    rollup ? 
                        paths.SelectMany(path => GetMetadataRollup(path, sanitize)) : 
                        paths.SelectMany(path => GetMetadataFlat(path, sanitize)));
        }

        private IEnumerable<FileMetadata> CollectStatistics(IEnumerable<FileMetadata> fileMetadata)
        {
            var fileSizes = new OrderedBag<long>();

            foreach (var info in fileMetadata)
            {
                TotalSize += info.FileSize;
                TotalNumber++;

                fileSizes.Add(info.FileSize);

                yield return info;
            }

            AverageSize = TotalSize / TotalNumber;
            MedianSize = fileSizes[fileSizes.Count/2];
        }

        private IEnumerable<FileMetadata> GetMetadataFlat(string path, bool sanitize)
        {
            return
                from info in Directory.EnumerateFileSystemInfos(path, "*", SearchOption.AllDirectories, continueOnAccessError: true)
                where info.SystemInfo.IsFile
                let directoryName = Path.GetDirectoryName(info.FullName)
                select new FileMetadata
                {
                    FileName = sanitize ? info.Name.ToHashedText() : info.Name,
                    FileExtension = Path.GetExtension(info.Name).TrimStart('.'),
                    DirectoryPath = sanitize ? directoryName.ToHashedText() : directoryName,
                    FileSize = info.SystemInfo.FileSize,
                    CreationTimeUtc = info.CreationTimeUtc,
                    LastWriteTimeUtc = info.LastWriteTimeUtc,
                    LastAccessTimeUtc = info.LastAccessTimeUtc,
                };
        }

        private IEnumerable<FileMetadata> GetMetadataRollup(string path, bool sanitize)
        {
            var initialFile = Directory
                .EnumerateFileSystemInfos(path, "*", SearchOption.TopDirectoryOnly, continueOnAccessError: true)
                .FirstOrDefault(info => info.SystemInfo.IsFile);

            if (initialFile != null)
            {
                yield return Directory
                    .EnumerateFileSystemInfos(path, "*", SearchOption.TopDirectoryOnly, continueOnAccessError: true)
                    .Where(info => info.SystemInfo.IsFile)
                    .Aggregate(new FileMetadata
                        {
                            FileName = string.Empty,
                            FileExtension = string.Empty,
                            DirectoryPath = sanitize ? 
                                Path.GetDirectoryName(initialFile.FullName).ToHashedText() : 
                                Path.GetDirectoryName(initialFile.FullName),
                            FileSize = 0,
                            CreationTimeUtc = DateTime.MinValue,
                            LastWriteTimeUtc = DateTime.MinValue,
                            LastAccessTimeUtc = DateTime.MinValue,
                        },
                        (info, file) => new FileMetadata
                        {
                            FileName = info.FileName,
                            FileExtension = info.FileExtension,
                            DirectoryPath = info.DirectoryPath,
                            FileSize = info.FileSize + file.SystemInfo.FileSize,
                            CreationTimeUtc =
                                file.CreationTimeUtc > info.CreationTimeUtc
                                    ? file.CreationTimeUtc
                                    : info.CreationTimeUtc,
                            LastWriteTimeUtc =
                                file.LastWriteTimeUtc > info.LastWriteTimeUtc
                                    ? file.LastWriteTimeUtc
                                    : info.LastWriteTimeUtc,
                            LastAccessTimeUtc =
                                file.LastAccessTimeUtc > info.LastAccessTimeUtc
                                    ? file.LastAccessTimeUtc
                                    : info.LastAccessTimeUtc,
                        });
            }

            foreach (var directory in Directory
                .EnumerateFileSystemInfos(path, "*", SearchOption.TopDirectoryOnly, continueOnAccessError: true)
                .Where(info => info.SystemInfo.IsDirectory))
            {
                foreach (var rollup in GetMetadataRollup(directory.FullName, sanitize))
                {
                    yield return rollup;
                }
            }
        }

        private class FileMetaDataNode
        {
            public FileMetadata FileMetadata { get; set; }

            public Dictionary<string, FileMetaDataNode> Children { get; private set; }

            public FileMetaDataNode()
            {
                Children = new Dictionary<string, FileMetaDataNode>();
            }
        }

        private static readonly string[] Separator = {"\\"};

        private IEnumerable<FileMetadata> GetMetadataRollupRecursive(string path, bool sanitize)
        {
            var directoryRollups = GetMetadataRollup(path, sanitize).ToList();

            // Build tree so we can efficiently roll-up recursively in O(n log n) time
            var root = new FileMetaDataNode();

            foreach (var directoryRollup in directoryRollups)
            {
                var current = root;
                var parts = directoryRollup.DirectoryPath.Split(Separator, StringSplitOptions.RemoveEmptyEntries);

                foreach (var part in parts)
                {
                    FileMetaDataNode node;
                    if (current.Children.TryGetValue(part, out node))
                    {
                        current = node;
                    }
                    else
                    {
                        var temp = new FileMetaDataNode();
                        current.Children.Add(part, temp);
                        current = temp;
                    }
                }

                current.FileMetadata = directoryRollup;
            }

            // Aggregate recursively at each level in-place
            AggregateMetaDataNodes(root);

            return directoryRollups;
        }

        private FileMetadata AggregateMetaDataNodes(FileMetaDataNode node)
        {
            if (node.FileMetadata == null)
            {
                node.FileMetadata = new FileMetadata
                {
                    FileName = string.Empty,
                    FileExtension = string.Empty,
                    DirectoryPath = string.Empty,
                    FileSize = 0,
                    CreationTimeUtc = DateTime.MinValue,
                    LastWriteTimeUtc = DateTime.MinValue,
                    LastAccessTimeUtc = DateTime.MinValue,
                };
            }

            if (node.Children.Count == 0)
            {
                return node.FileMetadata;
            }

            foreach (var child in node.Children.Values)
            {
                var fileMetaData = AggregateMetaDataNodes(child);

                node.FileMetadata.FileSize += fileMetaData.FileSize;
                node.FileMetadata.CreationTimeUtc =
                    node.FileMetadata.CreationTimeUtc > fileMetaData.CreationTimeUtc
                        ? node.FileMetadata.CreationTimeUtc
                        : fileMetaData.CreationTimeUtc;
                node.FileMetadata.LastWriteTimeUtc =
                    node.FileMetadata.LastWriteTimeUtc > fileMetaData.LastWriteTimeUtc
                        ? node.FileMetadata.LastWriteTimeUtc
                        : fileMetaData.LastWriteTimeUtc;
                node.FileMetadata.LastAccessTimeUtc =
                    node.FileMetadata.LastAccessTimeUtc > fileMetaData.LastAccessTimeUtc
                        ? node.FileMetadata.LastAccessTimeUtc
                        : fileMetaData.LastAccessTimeUtc;
            }

            return node.FileMetadata;
        }
    }
}
