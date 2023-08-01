using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using QRCoder;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace RouterQRCodeGenerator
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Define the router URL and login credentials
            var routerUrl = "http://192.168.0.1/";
            var username = "admin";
            var password = "password123";

            // Create an HttpClient instance
            var httpClient = new HttpClient();

            // Send a GET request to the router URL to obtain the session cookie
            var initialResponse = await httpClient.GetAsync(routerUrl);
            initialResponse.EnsureSuccessStatusCode();
            var cookies = initialResponse.Headers.GetValues("Set-Cookie");
            var sessionCookie = new Cookie("session_id", cookies[0].Split(';')[0], "/", "192.168.0.1");

            // Prepare the login request payload
            var payload = new Dictionary<string, string>
            {
                { "admin", username },
                { "admin", password },
                { "remember_me", "on" }
            };
            var content = new FormUrlEncodedContent(payload);

            // Send a POST request to the login page to obtain the authentication cookie
            var loginUrl = "http://192.168.0.1/TSOCMTDAYUJLIUGA/userRpm/Index.htm";
            var loginRequest = new HttpRequestMessage(HttpMethod.Post, loginUrl);
            loginRequest.Headers.Add("Cookie", sessionCookie.ToString());
            loginRequest.Content = content;
            var loginResponse = await httpClient.SendAsync(loginRequest);
            loginResponse.EnsureSuccessStatusCode();
            cookies = loginResponse.Headers.GetValues("Set-Cookie");
            var authCookie = new Cookie("auth", cookies[0].Split(';')[0], "/", "192.168.0.1");

            // Send a GET request to the router status page with the authentication cookie
            var statusUrl = "http://192.168.0.1/status.cgi";
            var statusRequest = new HttpRequestMessage(HttpMethod.Get, statusUrl);
            statusRequest.Headers.Add("Cookie", $"{sessionCookie.Name}={sessionCookie.Value}; {authCookie.Name}={authCookie.Value}");
            var statusResponse = await httpClient.SendAsync(statusRequest);
            statusResponse.EnsureSuccessStatusCode();
            var statusContent = await statusResponse.Content.ReadAsStringAsync();

            // Use HtmlAgilityPack to extract the device data
            var doc = new HtmlDocument();
            doc.LoadHtml(statusContent);

            var deviceNameNode = doc.DocumentNode.SelectSingleNode("//*[@id=\"status_internet\"]/div/form[1]/div[1]/div[2]/label");
            var deviceName = deviceNameNode?.InnerText;

            var deviceData = $"{deviceName}";

            // Generate the QR code image
            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(deviceData, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new QRCode(qrCodeData);
            var qrCodeImage = qrCode.GetGraphic(10);

            // Save the QR code image to a file
            var fileName = "qrcode.png";
            using (var stream = new FileStream(fileName, FileMode.Create))
            {
                qrCodeImage.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            }

            // Display the QR code image in a picture box
            var pictureBox = new PictureBox();
            pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox.Image = qrCodeImage;

            // Show the picture box in a form
            var form = new Form();
            _ = form.Controls;
        }
    }
}