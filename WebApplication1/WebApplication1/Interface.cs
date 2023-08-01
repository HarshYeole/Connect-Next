using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace WebApplication1
{
    public interface Interface
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
        }
    }
}
}
