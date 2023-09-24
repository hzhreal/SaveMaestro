using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SaveMaestroSocket;
using SaveMaestroFTP;
using SaveMaestro;
using Newtonsoft.Json;
using System.Threading;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics.Contracts;

namespace Resign
{
    public partial class ResignWindow
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

        public ResignWindow()
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

        private async void begin_resign_Click(object sender, RoutedEventArgs e)
        {
            socket s_resign = new socket();
            FTP f_resign = new FTP();
            string accountid = accountid_resign.Text;

            if (filepaths.Items.Count == 2 && s_resign.checkaccid(accountid) == true)
            {
                string host = config.ip;
                int s_port = config.s_port;
                int f_port = config.f_port;

                string randomString = s_resign.random_gen();
                string mpath = config.mount_path + $"/{randomString}";
                string upath1 = config.upload_path;
                List<string> files = new List<string>();

                async Task cleanup(string delfiles, string randomString1)
                {

                    await f_resign.rmdir(mpath, host, f_port);

                    if (delfiles != null)
                    {
                        await f_resign.deletefiles2(delfiles, host, f_port);
                    }
                    else
                    {
                        await f_resign.rmdir(upath1, host, f_port);
                        await f_resign.mkdir(upath1, host, f_port);
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
                        await f_resign.mkdir(mpath, host, f_port);
                        UpdateTerminal("Created mountdir");

                        await f_resign.uploadsaves2(upath1, files, host, f_port);
                        UpdateTerminal("Uploaded saves");

                        string response = s_resign.socket_dump(mpath, savename, host, s_port);
                        UpdateTerminal($"Dump response: {response}");

                        Task<string> title_id = f_resign.param_io(mpath, accountid, host, f_port, randomString);
                        string titleid = await title_id;
                        UpdateTerminal("Param processes complete");

                        string finalpath = System.IO.Path.Combine(randomString, "PS4", "SAVEDATA", accountid, titleid);

                        Console.WriteLine(finalpath);

                        string response1 = s_resign.socket_update(mpath, savename, host, s_port);
                        UpdateTerminal($"Update response: {response1}");

                        await f_resign.downloadfiles2(finalpath, upath, host, f_port);
                        UpdateTerminal("Downloaded saves\nDone!");

                        await cleanup(delfiles, randomString);

                        string fullpath = System.IO.Path.GetFullPath(finalpath);
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
                MessageBox.Show("Error, make sure you select 2 files and input an account ID in a valid format");
            }
           
        }

        private void UpdateTerminal(string message)
        {
            // Update the UI on the UI thread
            Dispatcher.Invoke(() => { terminal_resign.Text = message; });
        }

        private void clear_paths_Click(object sender, RoutedEventArgs e)
        {
            filepaths.Items.Clear();
        }
    }

}
