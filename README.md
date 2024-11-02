
## Setup Instructions

Download and install the [.NET SDK](https://dotnet.microsoft.com/en-us/download).

Run *setup_godot_win_x86_64.bat*. The correct Godot version will be downloaded to the *.godot_install* directory. (It needs to be here for the VSCode stuff to find it.)

Run *.godot_install/Godot.exe* to open the editor. You will start in the project manager - select 'Import Existing Project', then select this folder.

> [!WARNING]
> If you're working on an Abertay machine, make sure the repository is cloned locally (to the C drive) rather than to the network M drive.
> You have less permissions working in the M drive, and it can break things.

### VSCode
If you open this folder with [VSCode](https://code.visualstudio.com/) it will recommend the extensions you need for Godot and C#.

> [!NOTE]
> In Godot, you'll want to set the C# external editor so you can automatically switch to VSCode when you open a script from the editor.
> 
> In the top bar of the Godot editor, go to 'Editor' -> 'Editor Settings', then scroll down until you find the 'Dotnet' -> 'Editor' section. Set 'External Editor' to 'Visual Studio Code' in the dropdown.

> [!TIP]
> You can use 'Godot Tools: Open workspace with Godot editor' from the VSCode command palette to open the editor.

> [!TIP]
> The godot-tools extension will bug you about not being able to access the language server if the editor isn't open - this only matters when dealing with GDScript, and can safely be ignored.
> 
> If you click the cog in the top right of the popup, it will give you the option to disable the alerts.
