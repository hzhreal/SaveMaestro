using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using System.Windows;

namespace customcrypto
{
    public partial class customcryptoWindow
    {

        public customcryptoWindow()
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

            byte[] ENCRYPTION_KEY = new byte[]
            {
                 0x16, 0x85, 0xFF, 0xA3, 0x8D, 0x01, 0x0F, 0x0D, 0xFE, 0x66, 0x1C, 0xF9, 0xB5, 0x57, 0x2C, 0x50,
                 0x0D, 0x80, 0x26, 0x48, 0xDB, 0x37, 0xB9, 0xED, 0x0F, 0x48, 0xC5, 0x73, 0x42, 0xC0, 0x22, 0xF5
            };


            if (operation.Text == "Encryption")
            {
                foreach (string filepath in filepaths.Items)
                {
                    try
                    {
                        string filename = Path.GetFileName(filepath);

                        EncryptFile_r(filepath, filename, ENCRYPTION_KEY, start_offset);
                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex}");
                    }
                }
            }

            else if (operation.Text == "Decryption")
            {
                foreach (string filepath in filepaths.Items)
                {
                    try
                    {
                        string filename = Path.GetFileName(filepath);

                        DecryptFile_r(filepath, filename, ENCRYPTION_KEY, start_offset);
                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex}");
                    }
                }
            }


            else
            {
                MessageBox.Show("Pick an operation");
            }
        }

        private void rdr2_Click(object sender, System.Windows.RoutedEventArgs e)
        {

            string start_offset = "0x120";

            byte[] ENCRYPTION_KEY = new byte[]
            {
                 0x16, 0x85, 0xFF, 0xA3, 0x8D, 0x01, 0x0F, 0x0D, 0xFE, 0x66, 0x1C, 0xF9, 0xB5, 0x57, 0x2C, 0x50,
                 0x0D, 0x80, 0x26, 0x48, 0xDB, 0x37, 0xB9, 0xED, 0x0F, 0x48, 0xC5, 0x73, 0x42, 0xC0, 0x22, 0xF5
            };
               

            if (operation.Text == "Encryption")
            {
                foreach (string filepath in filepaths.Items)
                {
                    try
                    {
                        string filename = Path.GetFileName(filepath);

                        EncryptFile_r(filepath, filename, ENCRYPTION_KEY, start_offset);
                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex}");
                    }
                }
            }

            else if (operation.Text == "Decryption")
            {
                foreach (string filepath in filepaths.Items)
                {
                    try
                    {
                        string filename = Path.GetFileName(filepath);

                        DecryptFile_r(filepath, filename, ENCRYPTION_KEY, start_offset);
                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex}");
                    }
                }
            }

            else
            {
                MessageBox.Show("Pick an operation");
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


        static void DecryptFile_r(string filepath, string filename, byte[] ENCRYPTION_KEY, string start_offset)
        {
            int offset = Convert.ToInt32(start_offset,16);

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

        static void EncryptFile_r(string filepath, string filename, byte[] ENCRYPTION_KEY, string start_offset)
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