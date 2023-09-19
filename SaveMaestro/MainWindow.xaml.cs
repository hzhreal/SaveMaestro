using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using System.IO;
using System.Xml.Serialization;
using SaveMaestroSocket;

namespace SaveMaestro
{
    public partial class MainWindow : Window
    {

        config configmain = new config();

        public MainWindow()
        {
            InitializeComponent();
            readconfig();
        }

        public class config
        {
            public String ip { get; set; }
            public int s_port { get; set; }
            public int f_port { get; set; }
            public String mount_path { get; set; }
            public String upload_path { get; set; }

        }

        public void readconfig()
        {
            try
            {
                String json = File.ReadAllText("config.json");

                configmain = JsonConvert.DeserializeObject<config>(json);
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void saveinput_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                configmain.ip = psip.Text;
                configmain.s_port = Convert.ToInt32(socketport.Text);
                configmain.f_port = Convert.ToInt32(ftpport.Text);
                configmain.upload_path = psuploadpath.Text;
                configmain.mount_path = mountpath.Text;


                String json = JsonConvert.SerializeObject(configmain, Formatting.Indented);

                File.WriteAllText("config.json", json);

                MessageBox.Show($"SAVED\nPS4 IP: {configmain.ip}\nSocket port: {configmain.s_port}\nFTP port: {configmain.f_port}\nPS4 upload path: {configmain.upload_path}\nPS4 mount path: {configmain.mount_path}");
            }
           

            catch (FormatException)
            {
                MessageBox.Show("Error, make sure ports are integers");
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
    }
}
