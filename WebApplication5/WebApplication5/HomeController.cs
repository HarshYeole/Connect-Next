using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Windows.Forms;
using QRCoder;

namespace HelloWorld
{
    public partial class Form1 : Form
    {
        bool sidebarExpand;
        NetworkInterface networkInterface;

        public Form1()
        {
            InitializeComponent();
        }
        private void sidebarTimer_Tick(object sender, EventArgs e)
        {
            //Set the min and max size of sidebar panel

            if (sidebarExpand)
            {
                //if sidebar is expand, minimize
                sidebar.Width -= 10;
                if (sidebar.Width == sidebar.MinimumSize.Width)
                {
                    sidebarExpand = false;
                    sidebarTimer.Stop();
                }
            }
            else
            {
                sidebar.Width += 10;
                if (sidebar.Width == sidebar.MaximumSize.Width)
                {
                    sidebarExpand = true;
                    sidebarTimer.Stop();
                }
            }
        }

        private void menuButton_Click(object sender, EventArgs e)
        {
            //set timer interval to lowest to make it smoother
            sidebarTimer.Start();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string Host_Name = Dns.GetHostName();
            textBox1.Text = Host_Name;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (networkInterface != null)
            {
                // Get the IPv4 address of the active network interface
                string ipAddress = networkInterface.GetIPProperties()
                    .UnicastAddresses.FirstOrDefault(ip => ip.Address.AddressFamily == AddressFamily.InterNetwork)?.Address.ToString();

                // Get the MAC address of the active network interface
                string macAddress = BitConverter.ToString(networkInterface.GetPhysicalAddress().GetAddressBytes());

                // Display the IP address and MAC address in the respective text boxes
                textBox2.Text = ipAddress;
                textBox3.Text = macAddress;
            }
            else
            {
                // If no active network interface is found, display an error message
                MessageBox.Show("No active WiFi network interface found.");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //QR Generator
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(textBox1.Text + '\n' + textBox2.Text + '\n' + textBox3.Text, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            pictureBox1.Image = qrCode.GetGraphic(50);

            //For Saving QR as Image
            string initialDIR = @"C:\Users\Sayali Dongre\Desktop\Selec_Control\QR_images";//save image directory path
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PNG Image|*.png";
            saveFileDialog.Title = "Save QR Code as Image";
            saveFileDialog.FileName = "QRCode.png";
            saveFileDialog.InitialDirectory = initialDIR;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image.Save(saveFileDialog.FileName);
                MessageBox.Show("QR Code saved successfully.");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Get the active WiFi network interface
            networkInterface = NetworkInterface.GetAllNetworkInterfaces()
                .FirstOrDefault(ni => ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 && ni.OperationalStatus == OperationalStatus.Up);
        }
    }
}