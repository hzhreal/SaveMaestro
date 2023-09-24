# SaveMaestro
Application to resign, decrypt and re-region PS4 saves.

# Features
- Resigning
- Decrypting
- Modifiyng/importing the decrypted save folder
- Re-regioning
- Multiple windows doing their own processes. Dont go too crazy with this and make sure the ftp server can handle multiple processes. For example https://github.com/hippie68/ps4-ftp/releases/tag/v1.08a.
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
- Custom save encryption are not yet handled.
- Make sure to use the latest goldhen beta payload for the ftp server, or use hippie68's. This is essential for the ftp code to function.

# No jailbroken PS4?
- Join my discord where I have a bot that can do the same operations, free to use and often hosted and we also have a bot for ps3 avatars. https://discord.gg/fHfmjaCXtb.

# Credits
- https://github.com/Team-Alua/cecie.nim for creating the homebrew app that makes this possible
