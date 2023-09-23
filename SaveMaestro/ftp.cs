using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Peers;
using FluentFTP;
using SaveMaestro;
using System.IO;
using System.Windows;
using System.Security;
using FluentFTP.Helpers;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Reflection;
using System.IO.Pipes;
using System.Threading;

namespace SaveMaestroFTP
{
   public class FTP
    {
        public async Task mkdir(string path, string host, int port)
        {
            
            try
            {
                var token = new CancellationToken();
                using (AsyncFtpClient ftp = new AsyncFtpClient(host, port))
              {
                    await ftp.Connect(token);
                    await ftp.CreateDirectory(path, token);
                    Console.WriteLine($"{host}:{port}\tCreated {path}");
              }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            
            
        }

        public async Task uploadsaves2(string mountpath, List<string> savepath, string host, int port)
        {

            string filePath1 = savepath[0].ToString();
            string filePath2 = savepath[1].ToString();

            try
            {
                var token = new CancellationToken();
                using (AsyncFtpClient ftp = new AsyncFtpClient(host, port))
                {
                    await ftp.Connect(token);
                    await ftp.UploadFiles(
                        new[] {
                            filePath1,
                            filePath2
                        }, 
                        mountpath, token: token);
                }

                Console.WriteLine($"{host}:{port}\tUploaded {filePath1}, {filePath2}");
            }   

            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }

        }

        public async Task rmdir(string path, string host , int port)
        {

            try
            {
                var token = new CancellationToken();

                using (AsyncFtpClient ftp = new AsyncFtpClient(host, port))
                {
                    await ftp.Connect(token);
                    await ftp.DeleteDirectory(path, token);
                    Console.WriteLine($"{host}:{port}\tDeleted {path}");

                }
        
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        public async Task<string> param_io(string mountpath, string account_id, string host, int port, string randomString)
        {
            string param_local = System.IO.Path.Combine("temp", randomString, "param", "param.sfo");
            string path = mountpath + "/sce_sys/param.sfo";
            long offset = 0x15C; // account id offset
            ulong accid = Convert.ToUInt64(account_id, 16);
            long offset1 = 0xA9C; // one of the title id offsets
            string titleid = "";

            

            try
            {
                var token = new CancellationToken();

                using (AsyncFtpClient ftp = new AsyncFtpClient(host, port))
                {
                    await ftp.Connect(token);
                    await ftp.DownloadFile(param_local, path, FtpLocalExists.Overwrite, FtpVerify.Retry, token: token);
                    Console.WriteLine($"{host}:{port}\tDownloaded {param_local}");
                }

                using (FileStream param = new FileStream(param_local, FileMode.Open, FileAccess.ReadWrite))
                {
                    //resigning
                    param.Seek(offset, SeekOrigin.Begin);
                    byte[] bytes = BitConverter.GetBytes(accid);
                    
                    param.Write(bytes, 0, bytes.Length);

                    //reading title id
                    byte[] bytes1 = new byte[9];
                    param.Seek(offset1, SeekOrigin.Begin);
                    param.Read(bytes1, 0, 9);
                    
                    titleid = Encoding.UTF8.GetString(bytes1);

                    Console.WriteLine($"Obtained title id: {titleid}");
                }

                Console.WriteLine($"Wrote {account_id} to local param file");

                using (AsyncFtpClient ftp = new AsyncFtpClient(host, port))
                {
                    await ftp.Connect(token);
                    await ftp.UploadFile(param_local, path, FtpRemoteExists.Overwrite, true, FtpVerify.Retry, token: token);
                    Console.WriteLine($"{host}:{port}\tUploaded {path}");
                }

                return titleid;

            }

            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return null;
            }
        
        }

        public string obtain_titleid(string randomString)
        {
            string param_path = System.IO.Path.Combine(randomString, "sce_sys", "param.sfo");
            long offset1 = 0xA9C; // one of the title id offsets
        

            try
            {
                using (FileStream param = new FileStream(param_path, FileMode.Open, FileAccess.Read))
                {
                    byte[] bytes1 = new byte[9];
                    param.Seek(offset1, SeekOrigin.Begin);
                    param.Read(bytes1, 0, 9);

                    string titleid = Encoding.UTF8.GetString(bytes1);

                    Console.WriteLine($"Obtained title id: {titleid}");
                    return titleid;
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return null;
            }
        }



        public async Task downloadfiles2(string localpath, string savepath, string host, int port)
        { 
            try
            {
                var token = new CancellationToken();
                using (AsyncFtpClient ftp = new AsyncFtpClient(host, port))
                {
                    await ftp.Connect(token);
                    await ftp.DownloadFiles(localpath,
                    new[] {
                        savepath,
                        savepath + ".bin"
                    }, FtpLocalExists.Overwrite, token: token);
                }

                Console.WriteLine($"{host}:{port}\tDownloaded {savepath}, {savepath + ".bin"}");
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        
        }

        public async Task deletefiles2(string filepath, string host, int port)
        {
            try
            {
                var token = new CancellationToken();

                using (AsyncFtpClient ftp = new AsyncFtpClient(host, port))
                {
                    await ftp.Connect(token);
                    await ftp.DeleteFile(filepath, token);
                    await ftp.DeleteFile(filepath + ".bin", token);
                }
            }

            catch(Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        public async Task downloadfolder(string randomString, string mpath, string host, int port)
        {
            try
            {
                var token = new CancellationToken();

                using (AsyncFtpClient ftp = new AsyncFtpClient(host, port))
                {
                    await ftp.Connect(token);
                    await ftp.DownloadDirectory(randomString, mpath, FtpFolderSyncMode.Update, token: token);
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        public async Task uploadfolder(string lpath, string mpath, string host, int port)
        {

            try
            {
                var token = new CancellationToken();

                using (AsyncFtpClient ftp = new AsyncFtpClient(host, port))
                {
                    await ftp.Connect(token);
                    await ftp.UploadDirectory(lpath, mpath, FtpFolderSyncMode.Update, FtpRemoteExists.Overwrite, token: token);
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

    }   

}
