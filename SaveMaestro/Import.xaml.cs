using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows;
using SaveMaestro;
using SaveMaestroFTP;
using SaveMaestroSocket;
using System.IO;
using Microsoft.Win32;
using System.Threading;
using Ookii.Dialogs.Wpf;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using static System.Net.WebRequestMethods;

namespace Import
{
    public partial class ImportWindow
    {
        config config = new config();

        public ImportWindow()
        {
            InitializeComponent();
            readconfig();
            upload_modified.Visibility = Visibility.Collapsed;
            folderpath.Visibility = Visibility.Collapsed;
            clear_path.Visibility = Visibility.Collapsed;
            selectfolder.Visibility = Visibility.Collapsed;

        }

        public void readconfig()
        {
            try
            {
                if (System.IO.File.Exists("config.json"))
                {
                    String json = System.IO.File.ReadAllText("config.json");

                    config = JsonConvert.DeserializeObject<config>(json);
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
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

        private async void begin_import_Click(object sender, RoutedEventArgs e)
        {
            socket s_import = new socket();
            FTP f_import = new FTP();
            string accountid = accountid_import.Text;

            if (filepaths.Items.Count == 2 && s_import.checkaccid(accountid) == true)
            {

                string host = config.ip;
                int s_port = config.s_port;
                int f_port = config.f_port;

                string randomString = s_import.random_gen();
                string mpath = config.mount_path + $"/{randomString}";
                string upath1 = config.upload_path;
                List<string> files = new List<string>();

                async Task cleanup(string delfiles, string randomString1)
                {

                    await f_import.rmdir(mpath, host, f_port);

                    if (delfiles != null)
                    {
                        await f_import.deletefiles2(delfiles, host, f_port);
                    }
                    else
                    {
                        await f_import.rmdir(upath1, host, f_port);
                        await f_import.mkdir(upath1, host, f_port);
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
                        await f_import.mkdir(mpath, host, f_port);
                        UpdateTerminal("Created mountdir");

                        await f_import.uploadsaves2(upath1, files, host, f_port);
                        UpdateTerminal("Uploaded saves");

                        string response = s_import.socket_dump(mpath, savename, host, s_port);
                        UpdateTerminal($"Dump response: {response}");

                        Task<string> title_id = f_import.param_io(mpath, accountid, host, f_port, randomString);
                        string titleid = await title_id;
                        UpdateTerminal("Param processes complete");

                        Directory.Delete("temp", true);

                        string dlpath = System.IO.Path.Combine("temp", randomString);

                        await f_import.downloadfolder(dlpath, mpath, host, f_port);
                        string fullpath_edit = System.IO.Path.GetFullPath(dlpath);
                        UpdateTerminal($"Downloaded decrypted contents\nGo to this path and modify the save how you want\n(Remember to have the files in the same name)\n{dlpath}");

                        updatewidgets(savename, titleid, randomString);

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

        private void updatewidgets(string savename, string titleid, string randomString)
        {
            Dispatcher.Invoke(() => { accountid_import.Visibility = Visibility.Collapsed; });
            Dispatcher.Invoke(() => { filepaths.Visibility = Visibility.Collapsed; });
            Dispatcher.Invoke(() => { clear_paths.Visibility = Visibility.Collapsed; });
            Dispatcher.Invoke(() => { begin_import.Visibility = Visibility.Collapsed; });
            Dispatcher.Invoke(() => { selectfiles.Visibility = Visibility.Collapsed; });
            Dispatcher.Invoke(() => { ID_bar.Visibility = Visibility.Collapsed; });

            Dispatcher.Invoke(() => { upload_modified.Visibility = Visibility.Visible; });
            Dispatcher.Invoke(() => { folderpath.Visibility = Visibility.Visible; });
            Dispatcher.Invoke(() => { clear_path.Visibility = Visibility.Visible; });
            Dispatcher.Invoke(() => { selectfolder.Visibility = Visibility.Visible; });

            Dispatcher.Invoke(() => { savename_grab.Text = savename; });
            Dispatcher.Invoke(() => { randomstring_grab.Text = randomString; });
            Dispatcher.Invoke(() => { titleid_grab.Text = titleid; });
        }
        private void UpdateTerminal(string message)
        {
            // Update the UI on the UI thread
            Dispatcher.Invoke(() => { terminal_import.Text = message; });
        }

        private void clear_paths_Click(object sender, RoutedEventArgs e)
        {
            filepaths.Items.Clear();
        }

        private async void upload_modified_Click(object sender, RoutedEventArgs e)
        {

            void cleanwidgets()
            {
                Dispatcher.Invoke(() => { accountid_import.Visibility = Visibility.Visible; });
                Dispatcher.Invoke(() => { filepaths.Visibility = Visibility.Visible; });
                Dispatcher.Invoke(() => { clear_paths.Visibility = Visibility.Visible; });
                Dispatcher.Invoke(() => { begin_import.Visibility = Visibility.Visible; });
                Dispatcher.Invoke(() => { selectfiles.Visibility = Visibility.Visible; });
                Dispatcher.Invoke(() => { ID_bar.Visibility = Visibility.Visible; });

                Dispatcher.Invoke(() => { upload_modified.Visibility = Visibility.Collapsed; });
                Dispatcher.Invoke(() => { folderpath.Visibility = Visibility.Collapsed; });
                Dispatcher.Invoke(() => { clear_path.Visibility = Visibility.Collapsed; });
                Dispatcher.Invoke(() => { selectfolder.Visibility = Visibility.Collapsed; });

                Dispatcher.Invoke(() => { savename_grab.Text = string.Empty; });
                Dispatcher.Invoke(() => { randomstring_grab.Text = string.Empty; });
                Dispatcher.Invoke(() => { titleid_grab.Text = string.Empty; });
            }
            
            if (folderpath.Items.Count == 1)
            {

                socket s_import = new socket();
                FTP f_import = new FTP();

                string host = config.ip;
                int s_port = config.s_port;
                int f_port = config.f_port;

                string randomString = randomstring_grab.Text;
                string savename = savename_grab.Text;
                string mpath = config.mount_path + $"/{randomString}";
                string upath1 = config.upload_path;
                string delfiles = upath1 + $"/{savename}";
                string accountid = accountid_import.Text;
                string titleid = titleid_grab.Text;
                List<string> files = new List<string>();
                
                foreach (string item in folderpath.Items)
                {
                    files.Add(item);
                }

                string targetfolder = files[0];
            
                async Task cleanup(string delfiles1, string randomString1)
                {

                    await f_import.rmdir(mpath, host, f_port);

                    if (delfiles1 != null)
                    {
                        await f_import.deletefiles2(delfiles1, host, f_port);
                    }
                    else
                    {
                        await f_import.rmdir(upath1, host, f_port);
                        await f_import.mkdir(upath1, host, f_port);
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
                    await f_import.uploadfolder(targetfolder, mpath, host, f_port);
                    UpdateTerminal("Uploaded folder");

                    string response = s_import.socket_update(mpath, savename, host, s_port);
                    UpdateTerminal($"Update response: {response}");

                    string finalpath = System.IO.Path.Combine(randomString, "PS4", "SAVEDATA", accountid, titleid);
                    Console.WriteLine(finalpath);

                    await f_import.downloadfiles2(finalpath, delfiles, host, f_port);
                    UpdateTerminal("Downloaded saves\nDone!");

                    string fullpath = System.IO.Path.GetFullPath(finalpath);
                    UpdateTerminal($"Operation completed successfully.\n{fullpath}");

                    await cleanup(delfiles, randomString);
                    cleanwidgets();

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}\nAttempting cleanup...");
                    cleanwidgets();
                    await cleanup(null, null);
                    
                }

            }

            else
            {
                MessageBox.Show("Error, make sure you select 1 folder");
            }
        }

        private void selectfolder_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog folderDialog = new VistaFolderBrowserDialog();
            folderDialog.Description = "Select a folder:";

            if (folderDialog.ShowDialog(this) == true)
            {
                string selectedFolderPath = folderDialog.SelectedPath;
                folderpath.Items.Add(selectedFolderPath);
            }

        }

        private void clear_path_Click(object sender, RoutedEventArgs e)
        {
            folderpath.Items.Clear();
        }
    }
}