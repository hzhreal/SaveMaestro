using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using System.Windows;
using System.Windows.Threading;

namespace CustomCrypto
{
    public partial class CustomCryptoWindow
    {
        public static class KEYS
        {
            public static class R_STAR
            {
                public static byte[] ENCRYPTION_KEY = new byte[]
                {
                    0x16, 0x85, 0xFF, 0xA3, 0x8D, 0x01, 0x0F, 0x0D, 0xFE, 0x66, 0x1C, 0xF9, 0xB5, 0x57, 0x2C, 0x50,
                    0x0D, 0x80, 0x26, 0x48, 0xDB, 0x37, 0xB9, 0xED, 0x0F, 0x48, 0xC5, 0x73, 0x42, 0xC0, 0x22, 0xF5
                };
            }
            public static class BL3
            {
                public static byte[] SAVEGAME_PREFIX = new byte[]
                {
                    0xd1, 0x7b, 0xbf, 0x75, 0x4c, 0xc1, 0x80, 0x30, 0x37, 0x92, 0xbd, 0xd0, 0x18, 0x3e, 0x4a, 0x5f,
                    0x43, 0xa2, 0x46, 0xa0, 0xed, 0xdb, 0x2d, 0x9f, 0x56, 0x5f, 0x8b, 0x3d, 0x6e, 0x73, 0xe6, 0xb8
                };
                public static byte[] SAVEGAME_XOR = new byte[]
                {
                    0xfb, 0xfd, 0xfd, 0x51, 0x3a, 0x5c, 0xdb, 0x20, 0xbb, 0x5e, 0xc7, 0xaf, 0x66, 0x6f, 0xb6, 0x9a,
                    0x9a, 0x52, 0x67, 0x0f, 0x19, 0x5d, 0xd3, 0x84, 0x15, 0x19, 0xc9, 0x4a, 0x79, 0x67, 0xda, 0x6d
                };
                public static byte[] PROFILE_PREFIX = new byte[]
                {
                    0xad, 0x1e, 0x60, 0x4e, 0x42, 0x9e, 0xa9, 0x33, 0xb2, 0xf5, 0x01, 0xe1, 0x02, 0x4d, 0x08, 0x75,
                    0xb1, 0xad, 0x1a, 0x3d, 0xa1, 0x03, 0x6b, 0x1a, 0x17, 0xe6, 0xec, 0x0f, 0x60, 0x8d, 0xb4, 0xf9
                };
                public static byte[] PROFILE_XOR = new byte[]
                {
                    0xba, 0x0e, 0x86, 0x1d, 0x58, 0xe1, 0x92, 0x21, 0x30, 0xd6, 0xcb, 0xf0, 0xd0, 0x82, 0xd5, 0x58,
                    0x36, 0x12, 0xe1, 0xf6, 0x39, 0x44, 0x88, 0xea, 0x4e, 0xfb, 0x04, 0x74, 0x07, 0x95, 0x3a, 0xa2
                };
                public static string SAVEGAME_STRING = "OakSaveGame";
                public static string PROFILE_STRING = "BP_DefaultOakProfile_C";
            }
        }
        public class Operations
        {
            public class BL3
            {
                public static void SaveGameEncryptFile(string filepath, string filename, byte[] SAVEGAME_PREFIX, byte[] SAVEGAME_XOR)
                {

                }

                public static void SaveGameDecryptFile(string filepath, string filename, byte[] SAVEGAME_PREFIX, byte[] SAVEGAME_XOR)
                {
                    
                }

                public static void ProfileEncryptFile(string filepath, string filename, byte[] PROFILE_PREFIX, byte[] PROFILE_XOR)
                {

                }

                public static void ProfileDecryptFile(string filepath, string filename, byte[] PROFILE_PREFIX, byte[] PROFILE_XOR)
                {

                }

                public static string searchData(string filepath)
                {
                    string[] lines = File.ReadAllLines(filepath);

                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (lines[i].Contains(KEYS.BL3.SAVEGAME_STRING)) // .sav found
                        {
                            return "SAVEGAME";
                        }

                        else if (lines[i].Contains(KEYS.BL3.PROFILE_STRING)) // profile found
                        {
                            return "PROFILE";
                        }

                    }

                    return "ERROR";
                }
            }
            public class R_STAR
            {
                public static void EncryptFile(string filepath, string filename, byte[] ENCRYPTION_KEY, string start_offset)
                {

                    int offset = Convert.ToInt32(start_offset, 16);

                    using (Aes cipher = Aes.Create())
                    {
                        cipher.Key = ENCRYPTION_KEY;
                        cipher.Mode = CipherMode.ECB;
                        cipher.Padding = PaddingMode.None;

                        using (var encryptor = cipher.CreateEncryptor())
                        {
                            byte[] original_bytes = File.ReadAllBytes(filepath); // read whole file
                            byte[] dataToEncrypt = new byte[original_bytes.Length - offset]; // create new byte without the part that does not need encrypting
                            Array.Copy(original_bytes, offset, dataToEncrypt, 0, dataToEncrypt.Length); // copy the bytes that need encrypting

                            byte[] encryptedData = PerformCryptography(encryptor, dataToEncrypt); // encrypt

                            using (FileStream f = new FileStream("enc_" + filename, FileMode.Create, FileAccess.Write))
                            {
                                f.Write(original_bytes, 0, original_bytes.Length); // write original file
                                f.Seek(offset, SeekOrigin.Begin); // seek to encrypted data start offset
                                f.Write(encryptedData, 0, encryptedData.Length); // write encrypted data
                            }
                        }
                    }

                    Console.WriteLine($"Encrypted {filepath}, saved to enc_{filename}");
                    MessageBox.Show($"Encrypted {filepath}");
                }

                public static void DecryptFile(string filepath, string filename, byte[] ENCRYPTION_KEY, string start_offset)
                {
                    int offset = Convert.ToInt32(start_offset, 16);

                    using (Aes cipher = Aes.Create())
                    {
                        cipher.Key = ENCRYPTION_KEY;
                        cipher.Mode = CipherMode.ECB;
                        cipher.Padding = PaddingMode.None;

                        using (var decryptor = cipher.CreateDecryptor())
                        {
                            byte[] original_bytes = File.ReadAllBytes(filepath); // read whole file
                            byte[] dataToDecrypt = new byte[original_bytes.Length - offset]; // create new byte without the part that does not need decrypting
                            Array.Copy(original_bytes, offset, dataToDecrypt, 0, dataToDecrypt.Length); // copy the bytes that need decrypting

                            byte[] encryptedData = PerformCryptography(decryptor, dataToDecrypt); // decrypt

                            using (FileStream f = new FileStream("dec_" + filename, FileMode.Create, FileAccess.Write))
                            {
                                f.Write(original_bytes, 0, original_bytes.Length); // write original file
                                f.Seek(offset, SeekOrigin.Begin); // seek to decrypted data start offset
                                f.Write(encryptedData, 0, encryptedData.Length); // write decrypted data
                            }
                        }

                    }

                    Console.WriteLine($"Decrypted {filepath}, saved to dec_{filename}");
                    MessageBox.Show($"Decrypted {filepath}");
                }

            }
        }

        public CustomCryptoWindow()
        {
            InitializeComponent();
        }

        private void selectfiles_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            if (ofd.ShowDialog() == true)
            {
                foreach (string item in ofd.FileNames)
                {
                    filepaths.Items.Add(item);
                }
            }
        }

        private void clear_paths_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            filepaths.Items.Clear();
        }

        private void gta_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string start_offset = "0x114";

            if (operation.Text == "Encryption" && filepaths.Items.Count >= 1)
            {
                foreach (string filepath in filepaths.Items)
                {
                    try
                    {
                        string filename = Path.GetFileName(filepath);

                        Operations.R_STAR.EncryptFile(filepath, filename, KEYS.R_STAR.ENCRYPTION_KEY, start_offset);
                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex}");
                    }
                }
            }

            else if (operation.Text == "Decryption" && filepaths.Items.Count >= 1)
            {
                foreach (string filepath in filepaths.Items)
                {
                    try
                    {
                        string filename = Path.GetFileName(filepath);

                        Operations.R_STAR.DecryptFile(filepath, filename, KEYS.R_STAR.ENCRYPTION_KEY, start_offset);
                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex}");
                    }
                }
            }


            else
            {
                MessageBox.Show("Pick an operation and atleast 1 file");
            }
        }

        private void rdr2_Click(object sender, System.Windows.RoutedEventArgs e)
        {

            string start_offset = "0x120";


            if (operation.Text == "Encryption" && filepaths.Items.Count >= 1)
            {
                foreach (string filepath in filepaths.Items)
                {
                    try
                    {
                        string filename = Path.GetFileName(filepath);

                        Operations.R_STAR.EncryptFile(filepath, filename, KEYS.R_STAR.ENCRYPTION_KEY, start_offset);
                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex}");
                    }
                }
            }

            else if (operation.Text == "Decryption" && filepaths.Items.Count >= 1)
            {
                foreach (string filepath in filepaths.Items)
                {
                    try
                    {
                        string filename = Path.GetFileName(filepath);

                        Operations.R_STAR.DecryptFile(filepath, filename, KEYS.R_STAR.ENCRYPTION_KEY, start_offset);
                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex}");
                    }
                }
            }

            else
            {
                MessageBox.Show("Pick an operation and atleast 1 file");
            }

        }

        private void bl3_Click(object sender, RoutedEventArgs e)
        {
            if (operation.Text == "Encryption" && filepaths.Items.Count >= 1)
            {
                foreach (string filepath in filepaths.Items)
                {
                    try
                    {
                        string filename = Path.GetFileName(filepath);

                        if (Operations.BL3.searchData(filepath) == "SAVEGAME")
                        {
                            Operations.BL3.SaveGameEncryptFile(filepath, filename, KEYS.BL3.SAVEGAME_PREFIX, KEYS.BL3.SAVEGAME_XOR);
                        }

                        else if (Operations.BL3.searchData(filepath) == "PROFILE")
                        {
                            Operations.BL3.ProfileEncryptFile(filepath, filename, KEYS.BL3.PROFILE_PREFIX, KEYS.BL3.PROFILE_XOR);
                        }

                        else
                        {
                            MessageBox.Show("Error: Could not determine if the file is a profile or savegame");
                        }

                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex}");
                    }
                }
            }

            else if (operation.Text == "Decryption" && filepaths.Items.Count >= 1)
            {
                foreach (string filepath in filepaths.Items)
                {
                    try
                    {
                        string filename = Path.GetFileName(filepath);

                        if (Operations.BL3.searchData(filepath) == "SAVEGAME")
                        {
                            Operations.BL3.SaveGameDecryptFile(filepath, filename, KEYS.BL3.SAVEGAME_PREFIX, KEYS.BL3.SAVEGAME_XOR);
                        }

                        else if (Operations.BL3.searchData(filepath) == "PROFILE")
                        {
                            Operations.BL3.ProfileDecryptFile(filepath, filename, KEYS.BL3.PROFILE_PREFIX, KEYS.BL3.PROFILE_XOR);
                        }

                        else
                        {
                            MessageBox.Show("Error: Could not determine if the file is a profile or savegame");
                        }

                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex}");
                    }
                }
            }

            else
            {
                MessageBox.Show("Pick an operation and atleast 1 file");
            }

            
        }

        private void o_decrypt_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            UpdateOperation("Decryption");
        }

        private void o_encrypt_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            UpdateOperation("Encryption");
        }

        private void UpdateOperation(string message)
        {
            Dispatcher.Invoke(() => { operation.Text = message; });
        }

        private static byte[] PerformCryptography(ICryptoTransform cryptoTransform, byte[] data)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(data, 0, data.Length);
                    cryptoStream.FlushFinalBlock();
                    return memoryStream.ToArray();
                }
            }
        }

    }
}