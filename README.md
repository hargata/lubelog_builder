# LubeLog Builder
## Cross Platform Compiler and Packager for LubeLogger
![image](https://github.com/user-attachments/assets/908e8f98-f941-4c74-b286-f4a38c392605)
## Notes:
- This is an internal tool that we have made public so that users can run LubeLogger on bare-metal(without Docker) outside of Windows.
- This being an internal tool, considerations for the UI was not a priority.
- As stated, this was an internal tool and hence there was not a whole lot of testing in environments outside of our own.
- You should at the very least be comfortable with C# if this app does not run on the first go.
- No guarantees that the output will run on your system.
- No support will be provided, use at your own risk.
## Pre-requisites
- A machine with .NET 8 SDK Installed(non-negotiable!)
- Optional: Visual Studio 2022 or some other .NET C# IDE like Rider(Highly recommended but you can also do `dotnet build`)
## How to Use
1. Download source code in the Release section
2. Build the source code(either in VS or command line)
3. Download the Source Code of the latest LubeLogger Release(it will be downloaded in a file name similar to `lubelog-n.n.n.zip`
4. Extract the zip file.
5. Run the Builder
6. Select the Source Code folder(the one you just extracted from the zip file)
7. Select the target architecture, by default only Windows is selected
8. If not building in Windows, make sure you use the right shell app to run the command
9. Decide if you want to do a self-contained build(will allow the app to run on systems without installing .NET 8)
10. Click Build
11. The app will output a zip file containing the executable and all of the dependencies required to run on the targeted system.
## Dependencies
- [Avalonia](https://github.com/avaloniaui/avalonia)
- [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
