using System;
using System.Drawing;
using System.IO;

namespace IridiumTest.Util
{
    public class Img
    {
        public static byte[] Image2Bytes(String uri)
        {
            FileStream streams = new FileStream(uri, FileMode.Open, FileAccess.Read);
            BinaryReader reader = new BinaryReader(streams);
            Byte[] imagens = reader.ReadBytes((int)streams.Length);
            reader.Close(); streams.Close();
            return imagens;
        }

        public static Image Bytes2Image(byte[] bytes)
        {
            if (bytes == null)
                return null;

            MemoryStream ms = new MemoryStream(bytes);
            Bitmap bm = null;
            try
            {
                bm = new Bitmap(ms);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return bm;
        }

    }
}
