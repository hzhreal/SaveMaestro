using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Runtime.ExceptionServices;
using System.IO;
using SaveMaestro;
using System.Configuration;
using System.Windows;

namespace SaveMaestroSocket
{
    public class socket
    {
        public String random_gen()
        {
            Random random = new Random();
            string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            StringBuilder randomString = new StringBuilder();

            for (int i = 0; i < 5; i++)
            {
                int index = random.Next(0, characters.Length);
                randomString.Append(characters[index]);
            }

            return randomString.ToString();
        }

        public void socket_dump(string random_string, string savename)
        {
            config configsocket = new config();

            string host = configsocket.ip;
            int port = configsocket.s_port;
            string mountpath = configsocket.mount_path;

            string mountpath_new = mountpath + random_string;

            string jsonRequest = $"{{\"RequestType\": \"rtDumpSave\", \"sourceSaveName\": \"{savename}\", \"targetFolder\": \"{mountpath_new}\", \"dumpOnly\": []}}\r\n";

            try
            {
                using (TcpClient client = new TcpClient(host, port))
                {
                    Console.WriteLine($"Connected to TCP server {host}:{port}");

                    NetworkStream stream = client.GetStream();

                    // Convert the JSON request string to bytes
                    byte[] requestData = Encoding.UTF8.GetBytes(jsonRequest);

                    // Send the JSON data to the server
                    stream.Write(requestData, 0, requestData.Length);

                    byte[] responseBuffer = new byte[1024];
                    int bytesRead = stream.Read(responseBuffer, 0, responseBuffer.Length);
                    string response = Encoding.UTF8.GetString(responseBuffer, 0, bytesRead);
                    Console.WriteLine(response);
                    MessageBox.Show($"Dump response: {response}");
                }
            }
            
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

        }

        public void socket_update(string random_string, string savename)
        {
            config configsocket = new config();

            string host = configsocket.ip;
            int port = configsocket.s_port;
            string mountpath = configsocket.mount_path;

            string mountpath_new = mountpath + random_string;

            string jsonRequest = $"{{\"RequestType\": \"rtUpdateSave\", \"sourceSaveName\": \"{savename}\", \"targetFolder\": \"{mountpath_new}\", \"UpdateOnly\": []}}\r\n";

            try
            {
                using (TcpClient client = new TcpClient(host, port))
                {
                    Console.WriteLine($"Connected to TCP server {host}:{port}");

                    NetworkStream stream = client.GetStream();

                    // Convert the JSON request string to bytes
                    byte[] requestData = Encoding.UTF8.GetBytes(jsonRequest);

                    // Send the JSON data to the server
                    stream.Write(requestData, 0, requestData.Length);

                    byte[] responseBuffer = new byte[1024];
                    int bytesRead = stream.Read(responseBuffer, 0, responseBuffer.Length);
                    string response = Encoding.UTF8.GetString(responseBuffer, 0, bytesRead);
                    Console.WriteLine(response);
                    MessageBox.Show($"Update response: {response}");
                }
            }

            
            catch(Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

    }   

}
