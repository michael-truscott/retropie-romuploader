using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RetroPieRomUploader
{
    public interface IRomFileManager
    {
        string[] GetFilesForConsole(string console);
        void MoveFileToConsoleDir(string srcFilePath, string targetFileName, string console);
        bool RomFileExists(string console, string romFile);
        string GetRomFilePath(string console, string romFile);
        void DeleteRomFile(string console, string romFile);
    }

    public class RomFileManager : IRomFileManager
    {
        private readonly ILogger<RomFileManager> _logger;
        private readonly IConfiguration _configuration;

        private string _romDirectory;

        public RomFileManager(ILogger<RomFileManager> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;

            _romDirectory = _configuration.GetValue<string>("RomDirectory");
        }
        
        public string[] GetFilesForConsole(string console)
        {
            return Directory.GetFiles(Path.Combine(_romDirectory, console));
        }

        public bool RomFileExists(string console, string romFile)
        {
            return File.Exists(Path.Combine(_romDirectory, console, romFile));
        }

        public void MoveFileToConsoleDir(string srcFilePath, string targetFileName, string console)
        {
            var consoleDir = Path.Combine(_romDirectory, console);
            File.Move(srcFilePath, Path.Combine(consoleDir, targetFileName));
        }

        public string GetRomFilePath(string console, string romFile)
        {
            return Path.Combine(_romDirectory, console, romFile);
        }

        public void DeleteRomFile(string console, string romFile)
        {
            var path = GetRomFilePath(console, romFile);
            File.Delete(path);
        }
    }
}
