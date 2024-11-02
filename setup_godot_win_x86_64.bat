@echo off
if exist "%~dp0\.godot_install\" rmdir /S /Q ".godot_install"
mkdir ".godot_install"
cd ".godot_install"
echo Downloading Godot v4.3 .NET...
powershell -Command "$ProgressPreference = 'SilentlyContinue'; Invoke-WebRequest https://github.com/godotengine/godot/releases/download/4.3-stable/Godot_v4.3-stable_mono_win64.zip -OutFile Godot.zip"
echo Unzipping...
powershell -Command "$ProgressPreference = 'SilentlyContinue'; Expand-Archive Godot.zip"
move /Y Godot\Godot_v4.3-stable_mono_win64\GodotSharp .
move /Y Godot\Godot_v4.3-stable_mono_win64\Godot_v4.3-stable_mono_win64.exe .
rmdir /S /Q "Godot"
del "Godot.zip"
ren Godot_v4.3-stable_mono_win64.exe Godot.exe