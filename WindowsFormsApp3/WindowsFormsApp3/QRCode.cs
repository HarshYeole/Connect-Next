using System;

namespace WindowsFormsApp3
{
    internal class QRCode
    {
        private QRCodeData data;

        public QRCode(QRCodeData data)
        {
            this.data = data;
        }

        internal object GetGraphics(int v)
        {
            throw new NotImplementedException();
        }
    }
}