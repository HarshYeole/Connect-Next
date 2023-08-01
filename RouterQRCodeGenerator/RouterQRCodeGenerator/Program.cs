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
        client.DownloadString("http://192.168.0.1/WGSVZZIABMSSQAAA/userRpm/Index.htm");
        Process.Start("http://192.168.0.1/WGSVZZIABMSSQAAA/userRpm/Index.htm");

        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        QRCodeData qrCodeData = qrGenerator.CreateQrCode("Wi-Fi device details", QRCodeGenerator.ECCLevel.Q);
        QRCode qrCode = new QRCode(qrCodeData);
        Bitmap qrCodeImage = qrCode.GetGraphic(20);
        qrCodeImage.Save("qrcode.png", System.Drawing.Imaging.ImageFormat.Png);
    }
        
}