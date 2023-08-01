using System;
using System.Net;
using System.IO;
using System.Text;
using System.Web;
using System.Drawing;
using QRCoder;

namespace RouterQRCodeGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            // Set up the TP-Link router URL and credentials
            string routerUrl = "http://192.168.0.1/";
            string username = "admin";
            string password = "admin";

            // Create a WebRequest to access the router webpage
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(routerUrl);
            request.Method = "GET";

            // Set up the basic authentication credentials
            string authInfo = username + ":" + password;
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
            request.Headers["Authorization"] = "Basic " + authInfo;

            // Send the WebRequest and get the WebResponse
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseHtml = reader.ReadToEnd();

            // Extract the necessary device information from the HTML response
            string ssid = HttpUtility.HtmlDecode(GetTagValue(responseHtml, "ssid"));
            string deviceName = HttpUtility.HtmlDecode(GetTagValue(responseHtml, "device_name"));
            string macId = HttpUtility.HtmlDecode(GetTagValue(responseHtml, "mac"));
            string ipAddress = HttpUtility.HtmlDecode(GetTagValue(responseHtml, "ip"));
            string hwVersion = HttpUtility.HtmlDecode(GetTagValue(responseHtml, "hwversion"));
            string swVersion = HttpUtility.HtmlDecode(GetTagValue(responseHtml, "swversion"));

            // Generate a QR code based on the captured device information
            string qrData = string.Format("SSID: {0}\nDevice Name: {1}\nMAC ID: {2}\nIP Address: {3}\nHW Version: {4}\nSW Version: {5}",
                ssid, deviceName, macId, ipAddress, hwVersion, swVersion);
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrData, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);

            // Save the QR code image to a file
            string fileName = "router_qr_code.png";
            qrCodeImage.Save(fileName, System.Drawing.Imaging.ImageFormat.Png);
            Console.WriteLine("QR code image saved to {0}", fileName);

            // Clean up the WebResponse
            reader.Close();
            dataStream.Close();
            response.Close();
        }

        // Helper function to extract the value of a tag from an HTML string
        private static string GetTagValue(string html, string tag)
        {
            string tagStart = "<" + tag + ">";
            string tagEnd = "</" + tag + ">";
            int start = html.IndexOf(tagStart) + tagStart.Length;
            int end = html.IndexOf(tagEnd, start);
            return html.Substring(start, end - start);
        }
    }
}