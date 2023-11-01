using Microsoft.Win32;
using Newtonsoft.Json;
using SaveMaestro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Threading;
using SaveMaestroSocket;
using SaveMaestroFTP;

namespace Decrypt
{
    public partial class DecryptWindow
    {


        config config = new config();

        public void readconfig()
        {
            try
            {
                if (File.Exists("config.json"))
                {
                    String json = File.ReadAllText("config.json");

                    config = JsonConvert.DeserializeObject<config>(json);
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        public DecryptWindow()
        {
            InitializeComponent();
            readconfig();
        }

        private void selectfiles_Click(object sender, RoutedEventArgs e)
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

        private async void begin_decrypt_Click(object sender, RoutedEventArgs e)
        {
            if (filepaths.Items.Count == 2)
            {
                socket s_decrypt = new socket();
                FTP f_decrypt = new FTP();

                string host = config.ip;
                int s_port = config.s_port;
                int f_port = config.f_port;

                string randomString = s_decrypt.random_gen();
                string mpath = config.mount_path + $"/{randomString}";
                string upath1 = config.upload_path;
                List<string> files = new List<string>();

                async Task cleanup(string delfiles, string randomString1)
                {

                    await f_decrypt.rmdir(mpath, host, f_port);

                    if (delfiles != null)
                    {
                        await f_decrypt.deletefiles2(delfiles, host, f_port);
                    }
                    else
                    {
                        await f_decrypt.rmdir(upath1, host, f_port);
                        await f_decrypt.mkdir(upath1, host, f_port);
                    }

                    if (randomString1 != null)
                    {
                        string tempdir = System.IO.Path.Combine("temp", randomString1);

                        if (Directory.Exists(tempdir))
                        {
                            Directory.Delete(tempdir, true);
                        }
                    }

                    else
                    {
                        if (Directory.Exists("temp"))
                        {
                            Directory.Delete("temp", true);
                        }
                    }
                }

                try
                {
                    // Gather files and setup paths

                    foreach (string item in filepaths.Items)
                    {
                        files.Add(item);
                    }


                    string pathex = files[0];
                    string localdir = System.IO.Path.GetDirectoryName(pathex);

                    string savename = System.IO.Path.GetFileName(pathex);

                    if (savename.EndsWith(".bin"))
                    {
                        savename = System.IO.Path.GetFileNameWithoutExtension(savename);
                    }

                    string savepath = mpath + $"/{savename}";
                    string delfiles = upath1 + $"/{savename}";

                    string upath = config.upload_path + $"/{savename}";

                    // Start the operation and update UI accordingly

                    UpdateTerminal("Operation started...");

                    // Create a CancellationTokenSource to handle cancellation if needed
                    CancellationTokenSource cts = new CancellationTokenSource();

                    // Use Task.Run to run the long-running operations in the background
                    await Task.Run(async () =>
                    {
                        // Perform time-consuming operations here
                        await f_decrypt.mkdir(mpath, host, f_port);
                        UpdateTerminal("Created mountdir");

                        await f_decrypt.uploadsaves2(upath1, files, host, f_port);
                        UpdateTerminal("Uploaded saves");

                        string response = s_decrypt.socket_dump(mpath, savename, host, s_port);
                        UpdateTerminal($"Dump response: {response}");

                        await f_decrypt.downloadfolder(randomString, mpath, host, f_port);
                        UpdateTerminal("Downloaded decrypted contents");

                        string titleid = f_decrypt.obtain_titleid(randomString);

                        System.IO.Directory.Move(randomString, $"decrypted_{titleid}_{savename}_({randomString})");

                        await cleanup(delfiles, randomString);

                        string fullpath = System.IO.Path.GetFullPath($"decrypted_{titleid}_{savename}_({randomString})");
                        UpdateTerminal($"Operation completed successfully.\n{fullpath}");

                    }, cts.Token);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}\nAttempting cleanup...");
                    await cleanup(null, null);
                }
            }

            else
            {
                MessageBox.Show("Error, make sure you select 2 files");
            }
            
        }


        private void UpdateTerminal(string message)
        {
            // Update the UI on the UI thread
            Dispatcher.Invoke(() => { terminal_decrypt.Text = message; });
        }

        private void clear_paths_Click(object sender, RoutedEventArgs e)
        {
            filepaths.Items.Clear();
        }
    }
}
