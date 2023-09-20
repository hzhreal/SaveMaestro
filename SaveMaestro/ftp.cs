using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FluentFTP;
using SaveMaestro;

namespace SaveMaestroFTP
{
   public class FTP
    {
        public void mkdir(string path)
        {
            config configFTP = new config();
            string host = configFTP.ip;
            int port = configFTP.f_port;
        }

        public void uploadsaves2(string mountpath, string savepath)
        {
            config configFTP = new config();
            string host = configFTP.ip;
            int port = configFTP.f_port;
        }

        public void rmdir(string path)
        {
            config configFTP = new config();
            string host = configFTP.ip;
            int port = configFTP.f_port;
        }
    }
}
