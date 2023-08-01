using System;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Net;


namespace WindowsFormsApp4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string Host_Name = Dns.GetHostName();
            textBox1.Text = Host_Name;
        }

        [Obsolete]
        private void button2_Click(object sender, EventArgs e)
        {
            string Host_Name = Dns.GetHostName();
            string IP_Address = Dns.GetHostByName(Host_Name).AddressList[0].ToString();
            textBox2.Text = IP_Address;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            {
                string macAddresses = string.Empty;

                foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (nic.OperationalStatus == OperationalStatus.Up)
                    {
                        macAddresses += nic.GetPhysicalAddress().ToString();
                        break;
                    }
                }
                textBox3.Text = macAddresses;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            QRCoder.QRCodeGenerator qr = new QRCoder.QRCodeGenerator();
            var data = qr.CreateQrCode((textBox1.Text) + "\n" + (textBox2.Text) + "\n" +(textBox3.Text), QRCoder.QRCodeGenerator.ECCLevel.L);
            var code = new QRCoder.QRCode(data); 
            pictureBox1.Image = code.GetGraphic(50);
        }

    }
}