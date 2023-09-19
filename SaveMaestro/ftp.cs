using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SaveMaestroFTP
{
   public static class FTP
    {
        public static void mkdir(string host, int port, string mountpath)
        {
            Uri ftpUri = new Uri($"ftp://{host}:{port}/{mountpath}");

            try
            {
                FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(ftpUri);
                ftpRequest.Method = WebRequestMethods.Ftp.MakeDirectory;

                using (FtpWebResponse ftpResponse = (FtpWebResponse)ftpRequest.GetResponse())
                {
                    Console.WriteLine($"Directory Created. Status: {ftpResponse.StatusDescription}");
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public static void uploadsaves2(string host, int port, string mountpath, string savepath)
        {
            string[] savepaths = new string[]
            {
                savepath,
                savepath + ".bin"

            };

        }
    }
}
