using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Task1
{
    internal class CopyMaster
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<CopyMaster> _logger;

        public CopyMaster(IConfiguration configuration,
            ILogger<CopyMaster> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public void Execute()
        {
            var sourcePathsList = GetSourcePaths();
            var targetPath = GetTargetPath();

            var timestampDirectoryPath = CreateTimestampDirectory(targetPath);
            Console.WriteLine(string.IsNullOrEmpty(timestampDirectoryPath));
            if (string.IsNullOrEmpty(timestampDirectoryPath) ||
                string.IsNullOrEmpty(targetPath))
            {
                _logger.LogInformation("Application stoped. Press Ctrl + C to close the window");
                
            }
            Copy(sourcePathsList, timestampDirectoryPath);
        }

        private void Copy(List<string> sourcePathsList, string timestampDirectoryPath)
        {
            string fileName;
            string destinationFile;
            foreach (var sourcePath in sourcePathsList)
            {
                if (Directory.Exists(sourcePath))
                {
                    string[] files = Directory.GetFiles(sourcePath);
                    _logger.LogInformation($"Directory: {sourcePath} in progress");
                    foreach (string file in files)
                    {
                        fileName = Path.GetFileName(file);
                        destinationFile = Path.Combine(timestampDirectoryPath, fileName);
                        try
                        {
                            File.Copy(file, destinationFile, true);
                            _logger.LogDebug($"File: {fileName} successfully copied to {destinationFile}");
                        }
                        catch(UnauthorizedAccessException ex)
                        {
                            _logger.LogInformation($"Failed to copy file: {fileName}, access denied");
                        }
                    }
                    _logger.LogInformation($"Directory: {sourcePath} processed");
                }
                else
                {
                    _logger.LogInformation($"Source path: {sourcePath} does not exist");
                }
            }
        }

        private string GetTargetPath()
        {
            try
            {
                var targetPath = _configuration.GetSection("TargetPath").Value;
                return targetPath;
            }
            catch (Exception ex)
            {
                _logger.LogError("Enable to get path of target directory");
                return "";
            }
        }

        private string CreateTimestampDirectory(string targetPath)
        {
            string timespan = DateTime.UtcNow.ToLocalTime().ToString("yyyy-MM-dd_HH_mm");
            var timestampDirectoryPath = Path.Combine(targetPath, timespan);
            try
            {
                var directoryInfo = Directory.CreateDirectory(timestampDirectoryPath);
                return directoryInfo.FullName;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Enable to create timestampFolder, check target folder permissions and try again");
                return "";
            }

        }

        private List<string> GetSourcePaths()
        {
            return _configuration.GetSection("SourcePaths").Get<List<string>>();
        }
    }
}
