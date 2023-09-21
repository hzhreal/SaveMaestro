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

namespace SaveMaestroFTP
{
   public class FTP
    {
        public void mkdir(string path)
        {
            config configFTP = new config();
            string host = configFTP.ip;
            int port = configFTP.f_port;
            
            try
            {
              using (var ftp = new FtpClient(host, port))
              {
                    ftp.Connect();
                    ftp.CreateDirectory(path);
              }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            
            
        }

        public void uploadsaves2(string mountpath, string savepath)
        {
            config configFTP = new config();
            string host = configFTP.ip;
            int port = configFTP.f_port;

            try
            {

                using (var ftp = new FtpClient(host, port))
                {
                    ftp.Connect();
                    ftp.UploadFiles(
                        new[] {
                            savepath,
                            savepath + ".bin"
                        }, 
                        mountpath);
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }

        }

        public void rmdir(string path)
        {
            config configFTP = new config();
            string host = configFTP.ip;
            int port = configFTP.f_port;

            try
            {

                using (var ftp = new FtpClient(host, port))
                {
                    ftp.Connect();
                    ftp.DeleteDirectory(path);

                }
        
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        public void param_io(string mountpath, string account_id)
        {
            config configFTP = new config();
            string host = configFTP.ip;
            int port = configFTP.f_port;
            string param_local = Path.Combine("temp", "param", "param.sfo");
            string path = mountpath + "/sce_sys/param.sfo";
            ulong accid = Convert.ToUInt64(account_id, 16);
            long offset = 0x15C;

            try
            {

                using (var ftp = new FtpClient(host, port))
                {
                    ftp.Connect();
                    ftp.DownloadFile(param_local, path, FtpLocalExists.Overwrite, FtpVerify.Retry);
                }

                using (FileStream param = new FileStream(param_local, FileMode.Open, FileAccess.ReadWrite))
                {
                    param.Seek(offset, SeekOrigin.Begin);
                    byte[] byteValue = BitConverter.GetBytes(accid);

                    if (BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(byteValue);
                    }

                    param.Write(byteValue, 0, byteValue.Length);
                }

                using (var ftp = new FtpClient(host, port))
                {
                    ftp.Connect();
                    ftp.UploadFile(param_local, path, FtpRemoteExists.Overwrite, true, FtpVerify.Retry);
                }

            }

            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        
        }
    


        public void downloadfiles2(string localpath, string savepath)
        {
            config configFTP = new config();
            string host = configFTP.ip;
            int port = configFTP.f_port;

            try
            {
                using (var ftp = new FtpClient(host, port))
                {
                    ftp.Connect();
                    ftp.DownloadFiles(localpath,
                    new[] {
                        savepath,
                        savepath + ".bin"
                    });
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        
        }   
            


    }   

}
