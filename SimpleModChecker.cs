﻿// Simple Mod Checker Plus
// https://github.com/qstar-inc/cities2-SimpleModChecker
// StarQ 2024

using Colossal.PSI.Environment;
using Colossal.Serialization.Entities;
using Game.PSI;
using Game.SceneFlow;
using Game;
using System.Collections.Generic;

namespace SimpleModCheckerPlus
{
    public partial class ModNotification : GameSystemBase
    {
        public Mod _mod;
        private int count;
        public List<string> loadedMods = [];

        public ModNotification(Mod mod)
        {
            _mod = mod;
        }

        protected override void OnCreate()
        {
            base.OnCreate();
            CheckMod();
            //CleanFolders();
        }

        protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode)
        {
            base.OnGameLoadingComplete(purpose, mode);

            if (mode.IsGameOrEditor())
            {
                RemoveNotification();
            }
            else
            {
                if (Mod.Setting.ShowNotif)
                {
                    SendNotification(count);
                }
            }
        }
        protected override void OnUpdate()
        {
        }

        private void CheckMod()
        {
            count = 0;

            foreach (var modInfo in GameManager.instance.modManager)
            {
                string modName = modInfo.asset.name;
                if (!loadedMods.Contains(modName))
                {
                    loadedMods.Add(modName);
                    count += 1;
                    Mod.log.Info($"Loaded: {modName}");
                }
            }
            Mod.log.Info($"Total mod(s): {count}");
            if (Mod.Setting.ShowNotif)
            {
                SendNotification(count);
            }
        }

        public void SendNotification(int count)
        {
            var modstext = "mod";
            if (count < 2)
            {
                modstext += "";
            }
            else
            {
                modstext += "s";
            }

            NotificationSystem.Push("starq-mod-check",
                        title: Mod.ModName,
                        text: $"Loaded {count} {modstext}",
                        onClicked: () => System.Diagnostics.Process.Start($"{EnvPath.kUserDataPath}/Logs/{Mod.logFileName}.log"));
        }

        public void RemoveNotification()
        {
            NotificationSystem.Pop("mod-check");
        }
    }
}


//                                        SCRAPED IDEA
//private static void CleanFolders()
//{
//    string rootFolderPath = $"{EnvPath.kUserDataPath}/.cache/Mods/mods_subscribed";
//    string[] subdirectories = Directory.GetDirectories(rootFolderPath);

//    // Dictionary to store release numbers and their corresponding versions
//    Dictionary<int, List<int>> releaseVersions = new Dictionary<int, List<int>>();

//    // Iterate through each subdirectory
//    foreach (string directoryPath in subdirectories)
//    {
//        DirectoryInfo directory = new DirectoryInfo(directoryPath);
//        string directoryName = directory.Name;

//        // Split folder name into release number and version
//        string[] parts = directoryName.Split('_');
//        if (parts.Length != 2)
//        {
//            Mod.log.Info($"Invalid folder name format: {directoryName}. Underscore Errors! Skipping!");
//            continue;
//        }

//        if (!int.TryParse(parts[0], out int releaseNumber) || !int.TryParse(parts[1], out int version))
//        {
//            Mod.log.Info($"Invalid folder name format: {directoryName}. Number Errors! Skipping!");
//            continue;
//        }

//        // Add release number and version to dictionary
//        if (!releaseVersions.ContainsKey(releaseNumber))
//        {
//            releaseVersions[releaseNumber] = new List<int>();
//        }
//        releaseVersions[releaseNumber].Add(version);
//    }

//    int deleteCount = 0;
//    long sizeCount = 0;

//    // Iterate through release numbers and versions to find and delete older versions
//    foreach (var kvp in releaseVersions)
//    {
//        int releaseNumber = kvp.Key;
//        List<int> versions = kvp.Value;

//        // Sort versions in descending order
//        versions.Sort((x, y) => y.CompareTo(x));

//        // Get the latest version
//        int latestVersion = versions.First();

//        // Delete folders with older versions
//        foreach (var version in versions.Skip(1))
//        {
//            string folderToDelete = Path.Combine(rootFolderPath, $"{releaseNumber}_{version}");
//            long size = GetDirectorySize(folderToDelete);
//            Directory.Delete(folderToDelete, true); // Delete folder and its contents
//            Mod.log.Info($"Deleted folder: {folderToDelete} ({size} bytes)");
//            deleteCount++;
//            //Mod.log.Info(deleteCount);
//            sizeCount += size;
//            //Mod.log.Info(sizeCount);
//        }
//    }

//    if (deleteCount > 0)
//    {
//        Mod.log.Info($"Delete count: {deleteCount}");
//        var deletedModsText = "mod";
//        if (deleteCount < 2)
//        {
//            deletedModsText += "";
//        }
//        else
//        {
//            deletedModsText += "s";
//        }

//        NotificationSystem.Pop("deleted-check",
//            delay: 10f,
//            title: Mod.ModName,
//            text: $"Removed {deleteCount} old version.",
//            onClicked: () => System.Diagnostics.Process.Start($"{EnvPath.kUserDataPath}/Logs/{Mod.logFileName}.log")
//        );
//        Mod.log.Info($"Total saved {sizeCount/1024/1024} Mb off of {deleteCount} mods");
//    }
//}

//static long GetDirectorySize(string path)
//{
//    long size = 0;

//    try
//    {
//        // Get the size of all files in the directory
//        foreach (var file in Directory.EnumerateFiles(path))
//        {
//            size += new FileInfo(file).Length;
//        }

//        // Recursively get the size of all subdirectories
//        foreach (var dir in Directory.EnumerateDirectories(path))
//        {
//            size += GetDirectorySize(dir);
//        }
//    }
//    catch (UnauthorizedAccessException)
//    {
//        // Ignore any unauthorized access exceptions
//    }

//    return size;
//}
//                                        END OF SCRAPED IDEA