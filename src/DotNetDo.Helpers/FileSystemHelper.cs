using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;
using Microsoft.Extensions.Logging;

namespace DotNetDo.Helpers
{
    public class FileSystemHelper
    {
        private readonly TaskRunContext _taskRunContext;
        private readonly ILogger _log;

        public DirectoryInfoBase Root => Dir(_taskRunContext.DoRoot);

        public FileSystemHelper(ILogger<FileSystemHelper> log, TaskRunContext taskRunContext)
        {
            _log = log;
            _taskRunContext = taskRunContext;
        }

        public string Resolve(params string[] relativePathSegments)
        {
            return Path.Combine(_taskRunContext.DoRoot, Path.Combine(relativePathSegments));
        }

        /// <summary>
        /// Deletes the specified path recursively. If the path is a directory, all files and 
        /// sub-directories will be deleted as well.
        /// </summary>
        /// <param name="path"></param>
        public void RmRf(string path)
        {
            path = Resolve(path);

            if(Directory.Exists(path))
            {
                _log.LogTrace("Deleting directory {0}", path);
                Directory.Delete(path, recursive: true);
            }
            else if(System.IO.File.Exists(path))
            {
                _log.LogTrace("Deleting file {0}", path);
                System.IO.File.Delete(path);
            }
            else
            {
                _log.LogDebug("Not deleting non-existant path {0}", path);
            }
        }

        public FileInfoBase File(string relativePath)
        {
            return new FileInfoWrapper(new FileInfo(Resolve(relativePath)));
        }

        public DirectoryInfoBase Dir(string relativePath)
        {
            return new DirectoryInfoWrapper(new DirectoryInfo(_taskRunContext.DoRoot));
        }

        /// <summary>
        /// Returns a list of files matching the specified globbing patterns. Patterns starting with
        /// `!` are interpreted as exclude patterns.
        /// </summary>
        public IEnumerable<FileInfoBase> Files(params string[] globs) => Files((IEnumerable<string>)globs);

        /// <summary>
        /// Returns a list of files matching the specified globbing patterns. Patterns starting with
        /// `!` are interpreted as exclude patterns.
        /// </summary>
        public IEnumerable<FileInfoBase> Files(IEnumerable<string> globs)
        {
            var matcher = new Matcher();
            var includePatterns = globs.Where(s => s.Length > 0 && s[0] != '!');
            var excludePatterns = globs.Where(s => s.Length > 0 && s[0] == '!').Select(s => s.Substring(1));
            matcher.AddIncludePatterns(includePatterns);
            matcher.AddExcludePatterns(excludePatterns);
            var results = matcher.Execute(Root);
            return results.Files.Select(f => File(f.Path));
        }
    }
}
