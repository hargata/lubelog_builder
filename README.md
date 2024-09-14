# LubeLog Builder
## Cross Platform Compiler and Packager for LubeLogger
![image](https://github.com/user-attachments/assets/04f9ef75-d054-4e34-b9ed-68888aedeb0a)
## Notes:
- This is an internal tool that we have made public so that users can run LubeLogger on bare-metal(without Docker) outside of Windows.
- This being an internal tool, considerations for the UI was not a priority.
- As stated, this was an internal tool and hence there was not a whole lot of testing in environments outside of our own.
- You should at the very least be comfortable with C# if this app does not run on the first go.
- No guarantees that the output will run on your system.
- No support will be provided, use at your own risk.
## Pre-requisites
- A Windows Machine with .NET 8 Installed(non-negotiable!)
- Optional: Visual Studio 2022 or some other .NET C# IDE like Rider(Highly recommended but you can also do `dotnet build`)
## How to Use
1. Clone this repo
2. Build the source code
3. Download the Source Code of the latest LubeLogger Release(it will be downloaded in a file name similar to `lubelog-n.n.n.zip`
4. Extract the zip file.
5. Run the Builder
6. Select the Source Code folder(the one you just extracted from the zip file)
7. Select the target architecture, by default only Windows is selected
8. Decide if you want to do a self-contained build(will allow the app to run on systems without installing .NET 8)
9. Click Build
## Dependencies
- [Avalonia](https://github.com/avaloniaui/avalonia)
