using Newtonsoft.Json;
using SaveMaestro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using Microsoft.Win32;
using System.Threading;
using SaveMaestroFTP;
using SaveMaestroSocket;

namespace Reregion
{
    public partial class ReregionWindow
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

        public ReregionWindow()
        {
            InitializeComponent();
            readconfig();
            selectfiles1.Visibility = Visibility.Collapsed;
            clear_paths1.Visibility = Visibility.Collapsed;
            begin_reregion2.Visibility = Visibility.Collapsed;
            filepaths1.Visibility = Visibility.Collapsed;
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

        private void UpdateTerminal(string message)
        {
            // Update the UI on the UI thread
            Dispatcher.Invoke(() => { terminal_reregion.Text = message; });
        }

        private void clear_paths_Click(object sender, RoutedEventArgs e)
        {
            filepaths.Items.Clear();
        }

        private void selectfiles1_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            if (ofd.ShowDialog() == true)
            {
                foreach (string item in ofd.FileNames)
                {
                    filepaths1.Items.Add(item);
                }
            }
        }

        private void clear_paths1_Click(object sender, RoutedEventArgs e)
        {
            filepaths1.Items.Clear();
        }

        private async void begin_reregion2_Click(object sender, RoutedEventArgs e)
        {
            void cleanwidgets()
            {
                Dispatcher.Invoke(() => { accountid_reregion.Visibility = Visibility.Visible; });
                Dispatcher.Invoke(() => { filepaths.Visibility = Visibility.Visible; });
                Dispatcher.Invoke(() => { clear_paths.Visibility = Visibility.Visible; });
                Dispatcher.Invoke(() => { begin_reregion.Visibility = Visibility.Visible; });
                Dispatcher.Invoke(() => { selectfiles.Visibility = Visibility.Visible; });
                Dispatcher.Invoke(() => { ID_bar.Visibility = Visibility.Visible; });

                Dispatcher.Invoke(() => { begin_reregion2.Visibility = Visibility.Collapsed; });
                Dispatcher.Invoke(() => { filepaths1.Visibility = Visibility.Collapsed; });
                Dispatcher.Invoke(() => { clear_paths1.Visibility = Visibility.Collapsed; });
                Dispatcher.Invoke(() => { selectfiles1.Visibility = Visibility.Collapsed; });

                Dispatcher.Invoke(() => { savename_grab.Text = string.Empty; });
                Dispatcher.Invoke(() => { randomstring_grab.Text = string.Empty; });
                Dispatcher.Invoke(() => { titleid_grab.Text = string.Empty; });
            }

            if (filepaths1.Items.Count == 2)
            {

                socket s_reregion = new socket();
                FTP f_reregion = new FTP();

                string host = config.ip;
                int s_port = config.s_port;
                int f_port = config.f_port;

                string randomString = randomstring_grab.Text;
                string mpath = config.mount_path + $"/{randomString}";
                string upath1 = config.upload_path;
                string accountid = accountid_reregion.Text;
                string titleid = titleid_grab.Text;
                List<string> files = new List<string>();

                async Task cleanup(string delfiles1, string randomString1)
                {

                    await f_reregion.rmdir(mpath, host, f_port);

                    if (delfiles1 != null)
                    {
                        await f_reregion.deletefiles2(delfiles1, host, f_port);
                    }
                    else
                    {
                        await f_reregion.rmdir(upath1, host, f_port);
                        await f_reregion.mkdir(upath1, host, f_port);
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
                    foreach (string item in filepaths1.Items)
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
                        await f_reregion.mkdir(mpath, host, f_port);
                        UpdateTerminal("Created mountdir");

                        await f_reregion.uploadsaves2(upath1, files, host, f_port);
                        UpdateTerminal("Uploaded saves");

                        string response = s_reregion.socket_dump(mpath, savename, host, s_port);
                        UpdateTerminal($"Dump response: {response}");

                        await f_reregion.reregion(randomString, mpath, accountid, titleid, host, f_port);
                        UpdateTerminal("Re-regioned and resigned");
                        Directory.Delete(randomString, true);

                        string response1 = s_reregion.socket_update(mpath, savename, host, s_port);
                        UpdateTerminal($"Update response: {response1}");

                        string finalpath = System.IO.Path.Combine(randomString, "PS4", "SAVEDATA", accountid, titleid);

                        Console.WriteLine(finalpath);

                        await f_reregion.downloadfiles2(finalpath, upath, host, f_port);
                        UpdateTerminal("Downloaded saves\nDone!");

                        // xenoverse 2
                        if (titleid == "CUSA05088" || titleid == "CUSA05350" || titleid == "CUSA04904" || titleid == "CUSA05085" || titleid == "CUSA05774")
                        {
                           if (Directory.Exists(finalpath))
                            {
                                string[] savefiles = Directory.GetFiles(finalpath);
                                foreach (string savefile in savefiles)
                                {
                                    Console.WriteLine($"Renaming {savefile}");
                                    string folderSavefile = Path.GetDirectoryName(savefile);
                                    Console.WriteLine(folderSavefile);
                                    if (savefile.EndsWith(".bin"))
                                    {
                                        string newSavename = Path.Combine(folderSavefile, titleid + "01.bin");
                                        Console.WriteLine($"Newname {newSavename}");
                                        Directory.Move(savefile, newSavename);
                                    }
                                    else
                                    {
                                        string newSavename = Path.Combine(folderSavefile, titleid + "01");
                                        Console.WriteLine($"Newname {newSavename}");
                                        Directory.Move(savefile, newSavename);
                                    }
                                    
                                }
                            }
                           else
                            {
                                throw new Exception($"Could not find {finalpath}");
                            }
                        }

                        await cleanup(delfiles, randomString);

                        string fullpath = System.IO.Path.GetFullPath(finalpath);
                        UpdateTerminal($"Operation completed successfully.\n{fullpath}");

                        cleanwidgets();


                    }, cts.Token);

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
                MessageBox.Show("Error, make sure you select 2 files");
            }
        }
    

        private void updatewidgets(string savename, string titleid, string randomString)
        {
            Dispatcher.Invoke(() => { accountid_reregion.Visibility = Visibility.Collapsed; });
            Dispatcher.Invoke(() => { filepaths.Visibility = Visibility.Collapsed; });
            Dispatcher.Invoke(() => { clear_paths.Visibility = Visibility.Collapsed; });
            Dispatcher.Invoke(() => { begin_reregion.Visibility = Visibility.Collapsed; });
            Dispatcher.Invoke(() => { selectfiles.Visibility = Visibility.Collapsed; });
            Dispatcher.Invoke(() => { ID_bar.Visibility = Visibility.Collapsed; });

            Dispatcher.Invoke(() => { selectfiles1.Visibility = Visibility.Visible; });
            Dispatcher.Invoke(() => { filepaths1.Visibility = Visibility.Visible; });
            Dispatcher.Invoke(() => { clear_paths1.Visibility = Visibility.Visible; });
            Dispatcher.Invoke(() => { begin_reregion2.Visibility = Visibility.Visible; });

            Dispatcher.Invoke(() => { savename_grab.Text = savename; });
            Dispatcher.Invoke(() => { randomstring_grab.Text = randomString; });
            Dispatcher.Invoke(() => { titleid_grab.Text = titleid; });
        }

        private async void begin_reregion_Click(object sender, RoutedEventArgs e)
        {
            socket s_reregion = new socket();
            FTP f_reregion = new FTP();
            string accountid = accountid_reregion.Text;

            if (filepaths.Items.Count == 2 && s_reregion.checkaccid(accountid) == true)
            {
                string host = config.ip;
                int s_port = config.s_port;
                int f_port = config.f_port;

                string randomString = s_reregion.random_gen();
                string mpath = config.mount_path + $"/{randomString}";
                string upath1 = config.upload_path;
                List<string> files = new List<string>();

                async Task cleanup(string delfiles, string randomString1)
                {

                    await f_reregion.rmdir(mpath, host, f_port);

                    if (delfiles != null)
                    {
                        await f_reregion.deletefiles2(delfiles, host, f_port);
                    }
                    else
                    {
                        await f_reregion.rmdir(upath1, host, f_port);
                        await f_reregion.mkdir(upath1, host, f_port);
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
                        await f_reregion.mkdir(mpath, host, f_port);
                        UpdateTerminal("Created mountdir");

                        await f_reregion.uploadsaves2(upath1, files, host, f_port);
                        UpdateTerminal("Uploaded saves");

                        string response = s_reregion.socket_dump(mpath, savename, host, s_port);
                        UpdateTerminal($"Dump response: {response}");

                        await f_reregion.downloadfolder(randomString, mpath, host, f_port);
                        await f_reregion.deletefiles2(delfiles, host, f_port);
                        await f_reregion.rmdir(mpath, host, f_port);

                        string titleid = f_reregion.obtain_titleid(randomString);
                        UpdateTerminal("Obtained titleid and keystone\nUpload files from foreign region now");

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



    }
}
