using System;
using System.Diagnostics;
using System.Net;
using System.Drawing;
using QRCoder;

class Program
{
    static void Main(string[] args)
    {
        NetworkCredential credentials = new NetworkCredential("admin", "admin");
        WebClient client = new WebClient();
        client.Credentials = credentials;

        // Access the webpage of the router
        string routerIP = "http://192.168.0.1";
        string routerPage = "/WGSVZZIABMSSQAAA/userRpm/Index.htm";
        string routerUrl = routerIP + routerPage;
        client.DownloadString(routerUrl);
        Process.Start(routerUrl);

        // Get the IP and MAC address of the router
        IPAddress localIP = IPAddress.Parse("192.168.0.1");
        PhysicalAddress macAddr = PhysicalAddress.Parse("F4-F2-6D-CF-B9-C0");

        // Generate the QR code data
        string qrText = $"IP Address: {localIP}\nMAC Address: {macAddr}";
        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrText, QRCodeGenerator.ECCLevel.Q);

        // Generate the QR code image
        QRCode qrCode = new QRCode(qrCodeData);
        Bitmap qrCodeImage = qrCode.GetGraphic(20);

        // Save the QR code image to a file
        string filePath = @"D:\Selec\qrcode.png";
        using (FileStream stream = new FileStream(filePath, FileMode.Create))
        {
            qrCodeImage.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
        }

        Console.WriteLine("QR code saved to: " + filePath);
    }
}