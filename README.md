RetroPieRomUploader
==================
A web-based ROM file manager for the RetroPie game emulation system.
---------------------

RetroPieRomUploader enables you to upload rom files from your PC to your <a href="https://retropie.org.uk/">RetroPie</a> installation directly from your web browser, without having to install an SSH or SCP/FTP client.

To add new rom files, simply go to the <a asp-page="./Roms/Index">Roms</a> page, create a new rom file entry and upload the file.

To refresh the available roms in RetroPie, restart EmulationStation if it is open (press start button -&gt; Quit -&gt; Restart EmulationStation). The new files should appear in EmulationStation after it finishes restarting.

Installation & Configuration
------------

RetroPie Server Prerequisites:
- RetroPie running on a Raspberry Pi 3+ or other system
- NET Core 3.1 Runtime (if running as a framework-dependent app)

Build Prerequisites:
- NET Core 3.1 SDK

Building & Running
----------
- On a PC with the .NET Core SDK installed, open a terminal/command line to the project folder and run `dotnet publish -c Release`.
  - To publish a self-contained app, run `dotnet publish -c Release -r linux-arm --self-contained true`.
- Copy the generated /publish folder to the RetroPie system.
- Optional: Set the rom directory by modifying the "RomDirectory" property in appsettings.linux.json (default is <b>"/home/pi/RetroPie/roms"</b>).
- On the RetroPie system, navigate to the app folder and run the command `dotnet RetroPieRomUploader.dll --urls="http://*:5000` to start the app and listen on all interfaces on port 5000.
- In a web browser, navigate to "http://[retropie-ip]:5000" (where <b>[retropie-ip]</b> is the address of your RetroPie system).
- Upload some roms!
