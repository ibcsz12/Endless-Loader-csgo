using System;
using System.IO;
using Microsoft.Win32;

namespace Primordial
{
    public static class CsgoInstaller
    {
        public static bool InstallFilesForNeverlose()
        {
            try
            {
                string csgoPath = FindCsgoPath();

                if (string.IsNullOrEmpty(csgoPath) || !Directory.Exists(csgoPath))
                {
                    return false;
                }

                // 1. Распаковываем бинарники (.exe) в корень
                SaveResourceFile(Properties.Resources.csgo, Path.Combine(csgoPath, "csgo.exe"));
                SaveResourceFile(Properties.Resources.srcds, Path.Combine(csgoPath, "srcds.exe"));

                // 2. Создаем папку csgo_gc
                string gcFolderPath = Path.Combine(csgoPath, "csgo_gc");
                if (!Directory.Exists(gcFolderPath))
                {
                    Directory.CreateDirectory(gcFolderPath);
                }

                // 3. Распаковываем эмулятор и текстовые конфиги в csgo_gc
                SaveResourceFile(Properties.Resources.config, Path.Combine(gcFolderPath, "config.txt"));
                SaveResourceFile(Properties.Resources.csgo_gc, Path.Combine(gcFolderPath, "csgo_gc.dll"));
                SaveResourceFile(Properties.Resources.inventory, Path.Combine(gcFolderPath, "inventory.txt"));
                SaveResourceFile(Properties.Resources.price_sheet, Path.Combine(gcFolderPath, "price_sheet.txt"));
                SaveResourceFile(Properties.Resources.unusual_loot_lists, Path.Combine(gcFolderPath, "unusual_loot_list.txt"));

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static string FindCsgoPath()
        {
            string[] possiblePaths = new string[]
            {
                @"C:\Program Files (x86)\Steam\steamapps\common\Counter-Strike Global Offensive",
                @"C:\Program Files (x86)\Steam\steamapps\common\csgo legacy",
                @"C:\Program Files\Steam\steamapps\common\Counter-Strike Global Offensive",
                @"C:\Program Files\Steam\steamapps\common\csgo legacy",
                @"C:\Steam\steamapps\common\csgo legacy"
            };

            foreach (string path in possiblePaths)
            {
                if (Directory.Exists(path))
                    return path;
            }

            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Valve\Steam"))
                {
                    if (key != null)
                    {
                        string steamPath = key.GetValue("SteamPath") as string;
                        if (!string.IsNullOrEmpty(steamPath))
                        {
                            string legacyPath = Path.Combine(steamPath, @"steamapps\common\csgo legacy");
                            if (Directory.Exists(legacyPath)) return legacyPath;

                            string standardPath = Path.Combine(steamPath, @"steamapps\common\Counter-Strike Global Offensive");
                            if (Directory.Exists(standardPath)) return standardPath;
                        }
                    }
                }
            }
            catch { }

            return null;
        }

        // Для бинарных файлов (.exe, .dll)
        private static void SaveResourceFile(byte[] resourceBytes, string destinationPath)
        {
            if (resourceBytes != null)
            {
                File.WriteAllBytes(destinationPath, resourceBytes);
            }
        }

        // Для текстовых файлов (.txt)
        private static void SaveResourceFile(string resourceText, string destinationPath)
        {
            if (resourceText != null)
            {
                File.WriteAllText(destinationPath, resourceText);
            }
        }
    }
}