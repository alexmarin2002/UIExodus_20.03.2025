using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace TestingUI3
{
    public class FileSharing : IDisposable
    {
        // Enum to define the indices for different settings
        public enum SettingIndex
        {
            Status = 0,
            ShotWhileMoving = 1,
            PixelRange = 2,
            ShootingMethod = 3,
            ColorDetection = 4,
            ShootingDelay = 5,
            SelectedKey = 6,
            WeaponDelayKey = 7,
            WeaponDelayVandal = 8,
            WeaponDelayGuardian = 9,
            WeaponDelaySheriff = 10,
            WeaponDelayPhantom = 11,
            WeaponDelayGhost = 12,
            WeaponDelayNone = 13,
            AudioRange = 14
        }

        // File path for settings storage
        private readonly string settingsFilePath;
        private readonly Dictionary<SettingIndex, int> settingValues;
        private readonly object fileLock = new object();
        private bool isDisposed = false;
        private FileSystemWatcher watcher;
        private DateTime lastWriteTime = DateTime.MinValue;

        public FileSharing(string filePath = null)
        {
            // Use default file path if none provided
            settingsFilePath = filePath ?? Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "TestingUI3",
                "settings.txt");

            // Create directory if it doesn't exist
            Directory.CreateDirectory(Path.GetDirectoryName(settingsFilePath));

            // Initialize dictionary for settings
            settingValues = new Dictionary<SettingIndex, int>();

            // Load existing settings or create default
            if (File.Exists(settingsFilePath))
            {
                LoadSettingsFromFile();
            }
            else
            {
                InitializeDefaultValues();
                SaveSettingsToFile();
            }

            // Set up file watcher to detect changes from C++ application
            SetupFileWatcher();
        }

        // Initialize default settings values
        private void InitializeDefaultValues()
        {
            // Status off by default
            settingValues[SettingIndex.Status] = 0;

            // ShotWhileMoving off by default
            settingValues[SettingIndex.ShotWhileMoving] = 5;

            // Default pixel range 5 (mapped to 15)
            settingValues[SettingIndex.PixelRange] = 15;

            // Default to Press Button
            settingValues[SettingIndex.ShootingMethod] = 12;

            // Default to Red
            settingValues[SettingIndex.ColorDetection] = 7;

            // Default to Vandal
            settingValues[SettingIndex.ShootingDelay] = 5;

            // Default weapon delays
            settingValues[SettingIndex.WeaponDelayVandal] = 6;
            settingValues[SettingIndex.WeaponDelayGuardian] = 11;
            settingValues[SettingIndex.WeaponDelaySheriff] = 16;
            settingValues[SettingIndex.WeaponDelayPhantom] = 20;
            settingValues[SettingIndex.WeaponDelayGhost] = 25;
            settingValues[SettingIndex.WeaponDelayNone] = 30;

            // Default to no keys assigned
            settingValues[SettingIndex.SelectedKey] = 0;
            settingValues[SettingIndex.WeaponDelayKey] = 0;

            //Default Audio Range
            settingValues[SettingIndex.AudioRange] = 5;

        }

        // Load settings from the file
        private void LoadSettingsFromFile()
        {
            lock (fileLock)
            {
                try
                {
                    string[] lines = File.ReadAllLines(settingsFilePath);

                    foreach (string line in lines)
                    {
                        // Skip empty lines and comments
                        if (string.IsNullOrWhiteSpace(line) || line.TrimStart().StartsWith("#"))
                            continue;

                        string[] parts = line.Split('=');
                        if (parts.Length == 2)
                        {
                            string key = parts[0].Trim();
                            string value = parts[1].Trim();

                            if (Enum.TryParse<SettingIndex>(key, out SettingIndex index) &&
                                int.TryParse(value, out int intValue))
                            {
                                settingValues[index] = intValue;
                            }
                        }
                    }

                    // Record when we last read the file
                    lastWriteTime = File.GetLastWriteTime(settingsFilePath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading settings from file: {ex.Message}");

                    // If loading fails, initialize with defaults
                    InitializeDefaultValues();
                }
            }
        }

        // Save settings to the file
        private void SaveSettingsToFile()
        {
            lock (fileLock)
            {
                try
                {
                    // Temporarily disable file watcher to prevent triggering our own events
                    if (watcher != null)
                        watcher.EnableRaisingEvents = false;

                    using (StreamWriter writer = new StreamWriter(settingsFilePath, false, Encoding.UTF8))
                    {
                        writer.WriteLine("# TestingUI3 Settings File");
                        writer.WriteLine("# Format: SettingName=Value");
                        writer.WriteLine("# Last Updated: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        writer.WriteLine();

                        foreach (var kvp in settingValues)
                        {
                            writer.WriteLine($"{kvp.Key}={kvp.Value}");
                        }
                    }

                    // Record when we last wrote to the file
                    lastWriteTime = File.GetLastWriteTime(settingsFilePath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving settings to file: {ex.Message}");
                }
                finally
                {
                    // Re-enable file watcher
                    if (watcher != null)
                        watcher.EnableRaisingEvents = true;
                }
            }
        }

        // Set up file watcher to detect external changes
        private void SetupFileWatcher()
        {
            try
            {
                watcher = new FileSystemWatcher(Path.GetDirectoryName(settingsFilePath))
                {
                    Filter = Path.GetFileName(settingsFilePath),
                    NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.CreationTime,
                    EnableRaisingEvents = true
                };

                watcher.Changed += OnFileChanged;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting up file watcher: {ex.Message}");
            }
        }

        // Handle file change event
        private void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            try
            {
                // Avoid processing the same change multiple times
                DateTime currentWriteTime = File.GetLastWriteTime(settingsFilePath);

                if (currentWriteTime == lastWriteTime)
                    return;

                // Wait a moment to ensure the file is not locked
                Thread.Sleep(100);

                // Reload settings
                LoadSettingsFromFile();

                Console.WriteLine("Settings file was modified externally. Reloaded settings.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error handling file change: {ex.Message}");
            }
        }

        // Update a setting value
        public void UpdateSetting(SettingIndex index, int value)
        {
            lock (fileLock)
            {
                if (settingValues.ContainsKey(index) && settingValues[index] == value)
                    return; // No change needed

                // Update in memory
                settingValues[index] = value;

                // Save to file
                SaveSettingsToFile();

                Console.WriteLine($"Updated setting {index} to {value}");
            }
        }

        // Read a setting value
        public int ReadSetting(SettingIndex index)
        {
            lock (fileLock)
            {
                if (settingValues.TryGetValue(index, out int value))
                    return value;

                return 0; // Default if not found
            }
        }

        // Get the file path (for C++ program to know where to look)
        public string GetSettingsFilePath()
        {
            return settingsFilePath;
        }

        // Implement IDisposable to clean up resources
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    if (watcher != null)
                    {
                        watcher.Changed -= OnFileChanged;
                        watcher.Dispose();
                        watcher = null;
                    }
                }

                isDisposed = true;
            }
        }

        ~FileSharing()
        {
            Dispose(false);
        }
    }
}