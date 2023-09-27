# SaveMaestro
Application to resign, decrypt and re-region PS4 saves.

# Features
- Resigning
- Decrypting
- Modifiyng/importing the decrypted save folder
- Re-regioning
- Multiple windows doing their own processes (dont go too crazy with this)
- Account ID converter from username

# Tutorial
1. Download the pkg file and install it on your PS4.
2. Download the config.ini file from https://github.com/Team-Alua/cecie.nim/blob/main/examples/config.ini and edit it with your desired socket port and upload folder.
3. Upload the config.ini file to /data/cecie on your PS4.
4. Now run SaveMaestro.exe.
5. Put in the PS4 IP, ports and paths and click save. A json file will be created so you only have to do this once.
6. Now choose any operation.

# Disclaimers
- Saves created using this application will work on SaveWizard as long as you copy it from your PS4.
- Custom save encryption is not yet handled.
- Make sure you have .NET Framework 4.7.2 installed.
- Remember to not have the same folder for mount and upload. Have them in different paths, for example /data/example/mount & /data/example/upload.
# No jailbroken PS4?
- Join my discord where I have a bot that can do the same operations, free to use and often hosted. https://discord.gg/fHfmjaCXtb.

# Credits
- https://github.com/Team-Alua/cecie.nim for creating the homebrew app that makes this possible.
