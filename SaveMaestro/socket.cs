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
using Resign;
using System.Runtime.Remoting.Messaging;
using System.CodeDom;

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

        public string socket_dump(string mountpath_new, string savename, string host, int port)
        {

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
                    return response;
                }

            }
            
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return "None";
            }

        }

        public string socket_update(string mountpath_new, string savename, string host, int port)
        {

            string jsonRequest = $"{{\"RequestType\": \"rtUpdateSave\", \"targetSaveName\": \"{savename}\", \"sourceFolder\": \"{mountpath_new}\", \"selectOnly\": []}}\r\n";

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
                    return response;
                }
            }

            
            catch(Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return "None";
            }
        }

    }   

}
