using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using LubeLogger_Builder.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace LubeLogger_Builder.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        public async void BrowseForFolder(object source, RoutedEventArgs args)
        {
            var topLevel = TopLevel.GetTopLevel(this);
            var selectedFolder = await topLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions{
                Title = "Select LubeLogger Source Code Path",
                AllowMultiple = false
            });
            try
            {
                if (selectedFolder.Any() && !string.IsNullOrWhiteSpace(selectedFolder.First().Path.LocalPath))
                {
                    sourcePath.Text = selectedFolder.First().Path.LocalPath;
                    WriteToOutput($"Selected Folder: {sourcePath.Text}");
                }
            } catch (Exception ex)
            {
                WriteToOutput($"Error: {ex.Message}");
            }
        }
        public void WriteToOutput(string message)
        {
            outputText.Text += $"{DateTime.Now}: {message}\r\n";
        }
        public async void BuildLubeLogger(object source, RoutedEventArgs args)
        {
            var buildParams = new BuildParams
            {
                SourceFolder = sourcePath.Text ?? "",
                BuildSelfContained = buildSelfContained.IsChecked ?? false
            };
            if (buildWindowsCheck.IsChecked ?? false)
            {
                buildParams.TargetArchs.Add("win-x64");
            }
            if (buildLinuxCheck.IsChecked ?? false)
            {
                buildParams.TargetArchs.Add("linux-x64");
            }
            if (buildMacCheck.IsChecked ?? false)
            {
                buildParams.TargetArchs.Add("osx-x64");
            }
            if (!string.IsNullOrWhiteSpace(otherArch.Text))
            {
                var acceptableRIDS = new List<string>() { "win-x86", "win-arm64", "linux-musl-x64", "linux-musl-arm64", "linux-arm", "linux-arm64", "linux-bionic-arm64", "osx-arm64" };
                var otherArchs = otherArch.Text.Split(",");
                foreach(string rid in otherArchs)
                {
                    if (acceptableRIDS.Contains(rid))
                    {
                        buildParams.TargetArchs.Add(rid);
                    } else
                    {
                        WriteToOutput($"Invalid RID({rid}), possible RIDs: {string.Join(",", acceptableRIDS)}");
                        return;
                    }
                }
            }
            //validation
            if (string.IsNullOrWhiteSpace(buildParams.SourceFolder))
            {
                WriteToOutput($"Error: No Source Folder Selected!");
                return;
            }
            var buildPath = Path.Combine(buildParams.SourceFolder, "lubelog");
            if (!Directory.Exists(buildPath))
            {
                WriteToOutput("Error: Invalid Build Path Selected, lubelog folder not found");
            }
            var configPath = Path.Combine(buildParams.SourceFolder, "config");
            if (Directory.Exists(configPath))
            {
                Directory.CreateDirectory(configPath);
            }
            var versionNumber = 0;
            try
            {
                var folderName = Path.GetFileName(buildParams.SourceFolder);
                versionNumber = int.Parse(folderName.Split("-")[1].Replace(".", ""));
            } catch (Exception ex)
            {
                WriteToOutput($"Error:{ex.Message}");
            }
            if (versionNumber == default)
            {
                WriteToOutput($"Error: Unable to determine version number.");
                return;
            }
            if (!buildParams.TargetArchs.Any())
            {
                WriteToOutput($"Error: No OS Targeted!");
                return;
            }
            WriteToOutput($"RIDs to Build For: {string.Join(",", buildParams.TargetArchs)}");
            var executableName = executableFile.SelectionBoxItem.ToString();
            var commandTitle = "/c";
            if (executableName != "cmd.exe")
            {
                commandTitle = "-c";
            }
            //build for each arch
            foreach( var archCommand in buildParams.TargetArchs)
            {
                var selfContainedCommand = buildParams.BuildSelfContained ? "--self-contained" : "";
                var fullCommand = $"{commandTitle} dotnet publish -r {archCommand} {selfContainedCommand}";
                WriteToOutput($"Building for {archCommand}");
                await RunBuildCommand(executableName, fullCommand, buildPath);
                //check if folder exists.
                var archPath = Path.Combine(buildParams.SourceFolder, $"lubelog/bin/Release/net8.0/{archCommand}/publish");
                if (Directory.Exists(archPath))
                {
                    //make zip.
                    var destFilePath = Path.Combine(buildParams.SourceFolder, $"LubeLogger_v{versionNumber}_{archCommand.Replace("-", "_")}.zip");
                    if (File.Exists(destFilePath))
                    {
                        File.Delete(destFilePath);
                    }
                    WriteToOutput($"Creating Zip File for {archCommand}");
                    ZipFile.CreateFromDirectory(archPath, destFilePath);
                    WriteToOutput($"Created Zip File: {Path.GetFileName(destFilePath)}");
                    WriteToOutput($"Build Completed for {archCommand}");
                } else
                {
                    WriteToOutput($"Error: Build Failed for {archCommand}");
                }
            }
            //clean up
            WriteToOutput("Cleaning Up");
            var releaseFolder = Path.Combine(buildParams.SourceFolder, "lubelog/bin/Release/net8.0/");
            if (Directory.Exists(releaseFolder))
            {
                Directory.Delete(releaseFolder, true);
            }
            WriteToOutput("All Done");
        }
        private async Task RunBuildCommand(string executableName, string buildCommand, string buildPath)
        {
            var p = new Process();
            p.StartInfo.FileName = executableName;
            p.StartInfo.Arguments = buildCommand;
            p.StartInfo.WorkingDirectory = buildPath;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            await p.WaitForExitAsync();
        }
    }
}