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

        private RomFileManager CreateRomFileManager()
        {
            var logger = Mock.Of<ILogger<RomFileManager>>();
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "RomDirectory", ROMS_DIR},
                })
                .Build();
            return new RomFileManager(logger, configuration);
        }

        private void CreateDummyFiles(RomFileManager romManager)
        {
            foreach (var entry in FILENAME_MAP)
            {
                foreach (var file in entry.Value)
                {
                    var srcFilePath = romManager.GetRomFilePath(entry.Key, file);
                    File.WriteAllText(srcFilePath, "blah asdf");
                }
            }
        }

        private void DeleteFolderContents()
        {
            // cleanup files
            foreach (var entry in FILENAME_MAP)
            {
                foreach (var file in Directory.GetFiles(Path.Combine(ROMS_DIR, entry.Key)))
                    File.Delete(file);
            }
        }

        [TestMethod]
        public void TestCreateAndGetFiles()
        {
            var romManager = CreateRomFileManager();
            CreateDummyFiles(romManager);

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

            DeleteFolderContents();
        }

        [TestMethod]
        public void TestDeleteFiles()
        {
            var romManager = CreateRomFileManager();
            CreateDummyFiles(romManager);

            foreach (var entry in FILENAME_MAP)
            {
                foreach (var file in entry.Value)
                {
                    Assert.IsTrue(romManager.RomFileExists(entry.Key, file));
                    romManager.DeleteRomFile(entry.Key, file);
                    Assert.IsFalse(romManager.RomFileExists(entry.Key, file));
                }
            }

            // contents should be empty anyway but make sure
            DeleteFolderContents();
        }

        [TestMethod]
        public void TestMoveFiles()
        {
            var romManager = CreateRomFileManager();
            CreateDummyFiles(romManager);

            var tests = new[]
            {
                new { sourceConsole = "snes", destConsole = "psx", romName = "doom.rom" },
                new { sourceConsole = "nes", destConsole = "snes", romName = "excitebike.rom" },
                new { sourceConsole = "psx", destConsole = "nes", romName = "mgs.rom" },
            };

            foreach (var test in tests)
            {
                Assert.IsTrue(romManager.RomFileExists(test.sourceConsole, test.romName));
                Assert.IsFalse(romManager.RomFileExists(test.destConsole, test.romName));

                romManager.MoveFileToConsoleDir(romManager.GetRomFilePath(test.sourceConsole, test.romName), test.romName, test.destConsole);

                Assert.IsFalse(romManager.RomFileExists(test.sourceConsole, test.romName));
                Assert.IsTrue(romManager.RomFileExists(test.destConsole, test.romName));
            }
        }
    }
}
