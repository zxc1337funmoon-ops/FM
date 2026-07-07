[Setup]
AppName=FMTool
AppVersion=1.0.0
AppPublisher=zxc1337 for FunMoon
AppCopyright=Copyright (c) 2026 zxc1337
VersionInfoVersion=1.0.0.0
DefaultDirName={autopf}\FMTool
DefaultGroupName=FMTool
UninstallDisplayIcon={app}\FMTool.exe
OutputDir=C:\FMTool\installer
OutputBaseFilename=FMTool-Setup-1.0.0
Compression=lzma2/max
SolidCompression=yes
ArchitecturesInstallIn64BitMode=x64compatible
SetupIconFile=C:\FMTool\Resources\app.ico
WizardStyle=modern

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"
Name: "russian"; MessagesFile: "compiler:Languages\Russian.isl"

[Files]
Source: "C:\FMTool\publish\FMTool-1.0.0.exe"; DestDir: "{app}"; DestName: "FMTool.exe"; Flags: ignoreversion

[Icons]
Name: "{group}\FMTool"; Filename: "{app}\FMTool.exe"; IconFilename: "{app}\FMTool.exe"
Name: "{group}\Uninstall FMTool"; Filename: "{uninstallexe}"
Name: "{commondesktop}\FMTool"; Filename: "{app}\FMTool.exe"; IconFilename: "{app}\FMTool.exe"

[Run]
Filename: "{app}\FMTool.exe"; Description: "Launch FMTool"; Flags: postinstall nowait skipifsilent unchecked
