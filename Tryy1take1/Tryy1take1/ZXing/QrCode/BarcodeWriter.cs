namespace ZXing.QrCode
{
    internal class BarcodeWriter
    {
        public BarcodeWriter()
        {
        }

        public BarcodeFormat Format { get; internal set; }
        public QrCodeEncodingOptions Options { get; internal set; }

        internal object Write(string deviceInfo)
        {
            throw new NotImplementedException();
        }
    }
}