using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Directory = Alphaleonis.Win32.Filesystem.Directory;
using Path = Alphaleonis.Win32.Filesystem.Path;

namespace StorageUsageReporter
{
    internal class FilesAnalyzer
    {
        public long TotalSize { get; set; }

        public long TotalNumber { get; set; }

        public IEnumerable<FileMetadata> ProcessPath(string path, bool sanitize, bool rollup)
        {
            return CollectStatistics(
                rollup ? 
                    GetMetadataRollup(path, sanitize) : 
                    GetMetadataFlat(path, sanitize));
        }

        private IEnumerable<FileMetadata> CollectStatistics(IEnumerable<FileMetadata> fileMetadata)
        {
            foreach (var info in fileMetadata)
            {
                TotalSize += info.FileSize;
                TotalNumber++;

                yield return info;
            }
        }

        private IEnumerable<FileMetadata> GetMetadataFlat(string path, bool sanitize)
        {
            var santizedDirectoryName = new ConcurrentDictionary<string, string>();

            return
                from info in Directory.EnumerateFileSystemInfos(path, "*", SearchOption.AllDirectories, continueOnAccessError: true)
                where info.SystemInfo.IsFile
                let directoryName = Path.GetDirectoryName(info.FullName)
                select new FileMetadata
                {
                    FileName = sanitize ? Path.GetRandomFileName() : info.Name,
                    DirectoryPath = sanitize ?
                        santizedDirectoryName.GetOrAdd(directoryName, fullName => Guid.NewGuid().ToString()) :
                        directoryName,
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
                            DirectoryPath = sanitize ? Guid.NewGuid().ToString() : Path.GetDirectoryName(initialFile.FullName),
                            FileSize = 0,
                            CreationTimeUtc = DateTime.MinValue,
                            LastWriteTimeUtc = DateTime.MinValue,
                            LastAccessTimeUtc = DateTime.MinValue,
                        },
                        (info, file) => new FileMetadata
                        {
                            FileName = info.FileName,
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
    }
}
