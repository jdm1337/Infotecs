using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1
{
    internal class CopyMaster
    {
        private readonly IConfiguration _configuration;

        public CopyMaster(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Execute()
        {
            var sourcePathsList = GetSourcePaths();
            var targetPath = GetTargetPath();

            var timestampDirectoryPath = CreateTimestampDirectory(targetPath);
            Copy(sourcePathsList, timestampDirectoryPath);
        }

        private void Copy(List<string> sourcePathsList, string timestampDirectoryPath)
        {
            string fileName;
            string destFile;
            foreach (var sourcePath in sourcePathsList)
            {
                if (Directory.Exists(sourcePath))
                {
                    string[] files = Directory.GetFiles(sourcePath);

                    foreach (string file in files)
                    {
                        fileName = Path.GetFileName(file);
                        destFile = Path.Combine(timestampDirectoryPath, fileName);
                        File.Copy(file, destFile, true);
                    }
                }
                else
                {
                    Console.WriteLine("Source path does not exist!");
                }
            }
        }

        private string GetTargetPath()
        {
            return _configuration.GetSection("TargetPath").Value;
        }

        private string CreateTimestampDirectory(string targetPath)
        {
            string timespan = DateTime.UtcNow.ToLocalTime().ToString("yyyy-MM-dd_HH_mm");
            var timestampDirectoryPath = Path.Combine(targetPath, timespan);

            var directoryInfo = Directory.CreateDirectory(timestampDirectoryPath);
            return directoryInfo.FullName;
            
        }

        private List<string> GetSourcePaths()
        {
            return _configuration.GetSection("SourcePaths").Get<List<string>>();
        }
    }
}
