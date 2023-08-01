using System;
using System.Diagnostics;
using System.Net;
using System.Drawing;
using QRCoder;
using System.Net.NetworkInformation;
using System.IO;
using SnmpSharpNet;

class Program
{
    static void Main(string[] args)
    {
        // Router information
        string routerIpAddress = "192.168.4.1";
        string routerCommunity = "public";

        // Network credentials for router access
        NetworkCredential credentials = new NetworkCredential("192.168.4.1", "iotwifi123");
        WebClient client = new WebClient();
        client.Credentials = credentials;
        client.DownloadString("http://" + routerIpAddress + "/#deviceinfo");
        Process.Start("http://" + routerIpAddress + "/#deviceinfo");

        // Retrieve the IP address and MAC address of the Wi-Fi network interface
        NetworkInterface wifiInterface = GetWifiInterface();
        string ipAddress = wifiInterface.GetIPProperties().UnicastAddresses[1].Address.ToString();
        string macAddress = wifiInterface.GetPhysicalAddress().ToString();

        // Generate the QR code data
        string ssid = "S2W:A4:E5:7C:E1:79:A8";
        string pass = "iotwifi123";
        string qrCodeText = string.Format("IP Address: {0}\nMAC Address: {1}\nssid: {2}\npassword: {3}", ipAddress, macAddress,ssid,pass);
        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrCodeText, QRCodeGenerator.ECCLevel.Q);

        // Generate the QR code image
        QRCode qrCode = new QRCode(qrCodeData);
        Bitmap qrCodeImage = qrCode.GetGraphic(20);

        // Save the QR code image to a file
        string filePath = @"D:\Selec\qrcode1.png";
        using (FileStream stream = new FileStream(filePath, FileMode.Create))
        {
            qrCodeImage.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
        }

        Console.WriteLine("QR code saved to: " + filePath);

        // Get router software version
        string softwareVersion = GetRouterSoftwareVersion(routerIpAddress, routerCommunity);
        Console.WriteLine("Router Software Version: " + softwareVersion);

        // Get router hardware version
        string hardwareVersion = GetRouterHardwareVersion(routerIpAddress, routerCommunity);
        Console.WriteLine("Router Hardware Version: " + hardwareVersion);
    }

    static NetworkInterface GetWifiInterface()
    {
        // Retrieve the list of network interfaces
        NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();

        // Print the details of all network interfaces
        foreach (NetworkInterface intf in interfaces)
        {
            Console.WriteLine("Interface Name: " + intf.Name);
            Console.WriteLine("Interface Type: " + intf.NetworkInterfaceType);
            Console.WriteLine("Operational Status: " + intf.OperationalStatus);
            Console.WriteLine();
        }

        // Find the Wi-Fi network interface
        foreach (NetworkInterface intf in interfaces)
        {
            if (intf.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 && intf.OperationalStatus == OperationalStatus.Up)
            {
                return intf;
            }
        }

        throw new Exception("Wi-Fi interface not found");
    }

    static string GetRouterSoftwareVersion(string ipAddress, string community)
    {
        var agentParameters = new AgentParameters(SnmpVersion.Ver1, new OctetString("public"));
        var target = new UdpTarget((IPAddress)new IpAddress("192.168.0.105"), 161, 2000, 1);

        var pdu = new Pdu(PduType.Get);
        pdu.VbList.Add("1.3.6.1.2.1.1.1.0"); // sysDescr OID

        var response = target.Request(pdu, agentParameters);

        if (response != null && response.Pdu.VbList.Count > 0)
        {
            var version = response.Pdu.VbList[0].Value.ToString();
            return version;
        }

        return "Unknown";
    }

    static string GetRouterHardwareVersion(string ipAddress, string community)
    {
        var agentParameters = new AgentParameters(SnmpVersion.Ver1, new OctetString("public"));
        var target = new UdpTarget((IPAddress)new IpAddress("192.168.0.105"), 161, 2000, 1);

        var pdu = new Pdu(PduType.Get);
        pdu.VbList.Add("1.3.6.1.2.1.47.1.1.1.1.13.1"); // entPhysicalSoftwareRev OID

        var response = target.Request(pdu, agentParameters);

        if (response != null && response.Pdu.VbList.Count > 0)
        {
            var version = response.Pdu.VbList[0].Value.ToString();
            return version;
        }

        return "Unknown";
    }
}