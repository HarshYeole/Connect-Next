using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net.NetworkInformation;
using ZXing;
using ZXing.Common;

class Program
{
    static void Main(string[] args)
    {
        // Get the device information of the router
        var router = NetworkInterface.GetAllNetworkInterfaces()[0];
        var macAddress = BitConverter.ToString(router.GetPhysicalAddress().GetAddressBytes()).Replace("-", ":");
        var ipAddress = router.GetIPProperties().UnicastAddresses[0].Address.ToString();

        // Create the QR code content with the device information
        var qrCodeContent = $"MAC Address: {macAddress}\nIP Address: {ipAddress}";

        // Generate the QR code bitmap
        var writer = new BarcodeWriterPixelData
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new EncodingOptions
            {
                Height = 300,
                Width = 300
            }
        };
        var pixelData = writer.Write(qrCodeContent);
        var bitmap = new Bitmap(pixelData.Width, pixelData.Height, PixelFormat.Format32bppRgb);
        var bitmapData = bitmap.LockBits(new Rectangle(0, 0, pixelData.Width, pixelData.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);
        try
        {
            System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
        }
        finally
        {
            bitmap.UnlockBits(bitmapData);
        }

        // Save the QR code bitmap to file
        bitmap.Save("qrcode.png", ImageFormat.Png);
    }
}


