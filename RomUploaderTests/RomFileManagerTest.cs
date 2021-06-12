using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RetroPieRomUploader;
using System.Collections.Generic;
using System.IO;

namespace RomUploaderTests
{
    [TestClass]
    public class RomFileManagerTest
    {
        const string ROMS_DIR = "./test_roms";

        static string[] CONSOLE_DIRS =
        {
            "snes", "nes", "n64", "psx"
        };

        static Dictionary<string, string[]> FILENAME_MAP = new Dictionary<string, string[]>
        {
            { "snes", new[] {"doom.rom", "toystory.rom", "mortalkombat.rom"} },
            { "nes", new[] {"excitebike.rom", "megaman.rom", "castlevania.rom"} },
            { "n64", new[] {"mario64.rom", "zelda_ocarina.rom", "banjo.rom"} },
            { "psx", new[] {"spyro.rom", "crash.rom", "mgs.rom"} },
        };

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            if (Directory.Exists(ROMS_DIR))
                Directory.Delete(ROMS_DIR, true);

            Directory.CreateDirectory(ROMS_DIR);

            foreach (var entry in FILENAME_MAP)
            {
                var consoleDir = Path.Combine(ROMS_DIR, entry.Key);
                Directory.CreateDirectory(consoleDir);
            }
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            if (Directory.Exists(ROMS_DIR))
                Directory.Delete(ROMS_DIR, true);
        }

        [TestMethod]
        public void TestMoveAndGetFiles()
        {
            var logger = Mock.Of<ILogger<RomFileManager>>();
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "RomDirectory", ROMS_DIR},
                })
                .Build();
            var romManager = new RomFileManager(logger, configuration);

            foreach (var entry in FILENAME_MAP)
            {
                foreach (var file in entry.Value)
                {
                    var srcFilePath = Path.GetTempFileName();
                    File.WriteAllText(srcFilePath, "blah asdf");
                    romManager.MoveFileToConsoleDir(srcFilePath, file, entry.Key);
                }
            }

            foreach (var entry in FILENAME_MAP)
            {
                var filePaths = romManager.GetFilesForConsole(entry.Key);
                Assert.AreEqual(entry.Value.Length, filePaths.Length);
                foreach (var file in filePaths)
                    CollectionAssert.Contains(entry.Value, Path.GetFileName(file));
            }

            Assert.IsTrue(romManager.RomFileExists("snes", "doom.rom"));
            Assert.IsTrue(romManager.RomFileExists("nes", "excitebike.rom"));
            Assert.IsTrue(romManager.RomFileExists("psx", "mgs.rom"));

            Assert.IsFalse(romManager.RomFileExists("snes", "asdf.rom"));
            Assert.IsFalse(romManager.RomFileExists("nes", "tyuhfjukdgyhjfghj.rom"));
            Assert.IsFalse(romManager.RomFileExists("psx", "rg45g456h.rom"));
        }
    }
}
