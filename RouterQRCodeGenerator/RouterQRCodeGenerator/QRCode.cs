using QRCoder;

namespace RouterQRCodeGenerator
{
    internal class QRCode
    {
        private QRCodeData qrCodeData;

        public QRCode(QRCodeData qrCodeData)
        {
            this.qrCodeData = qrCodeData;
        }

        internal Bitmap GetGraphic(int v)
        {
            throw new NotImplementedException();
        }
    }
}