
## Setup Instructions

Download and install the [.NET SDK](https://dotnet.microsoft.com/en-us/download).

Run *setup_godot_win_x86_64.bat*. The correct Godot version will be downloaded to the *.godot_install* directory. (It needs to be here for the VSCode stuff to find it.)

Run *.godot_install/Godot.exe* to open the editor. You will start in the project manager - select 'Import Existing Project', then select this folder.

### VSCode
If you open this folder with [VSCode](https://code.visualstudio.com/) it will recommend the extensions you need for Godot and C#.

In Godot, you'll want to set the C# external editor so you can automatically switch to VSCode when you open a script from the editor. In the top bar of the Godot editor, go to 'Editor' -> 'Editor Settings', then scroll down until you find the 'Dotnet' -> 'Editor' section. Set 'External Editor' to 'Visual Studio Code' in the dropdown.