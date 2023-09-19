using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Runtime.ExceptionServices;
using System.IO;

namespace SaveMaestroSocket
{
    public static class socket
    {
        public static String random_gen()
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

        public static void socket_dump(string host, int port, string savename, string mountpath, string random_string)
        {

            mountpath = mountpath + random_string;

            string jsonRequest = $"{{\"RequestType\": \"rtDumpSave\", \"sourceSaveName\": \"{savename}\", \"targetFolder\": \"{mountpath}\", \"dumpOnly\": []}}\r\n";

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
                }
            }
            
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

        }

        public static void socket_update(string host, int port, string savename, string mountpath, string random_string)
        {
            mountpath = mountpath + random_string;

            string jsonRequest = $"{{\"RequestType\": \"rtUpdateSave\", \"sourceSaveName\": \"{savename}\", \"targetFolder\": \"{mountpath}\", \"UpdateOnly\": []}}\r\n";

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
                }
            }

            
            catch(Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

    }   

}
