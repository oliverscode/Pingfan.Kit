using System.IO;
using System.IO.Compression;

namespace Pingfan.Kit
{
    public class GZip
    {
        public static MemoryStream Decompress(Stream stream)
        {
            stream.Position = 0L;
            using (var gZipStream = new GZipStream(stream, CompressionMode.Decompress, leaveOpen: true))
            {
                var memoryStream = new MemoryStream();
                gZipStream.CopyTo(memoryStream);
                return memoryStream;
            }
        }

        public static byte[] Decompress(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            {
                return Decompress(stream).ToArray();
            }
            
        }

        public static MemoryStream Compress(Stream stream)
        {
            stream.Position = 0L;
            MemoryStream memoryStream = new MemoryStream();
            using (var destination = new GZipStream(memoryStream, CompressionMode.Compress, leaveOpen: true))
            {
                stream.CopyTo(destination);
                return memoryStream;
            }
           
        }

        public static byte[] Compress(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            {
                return Compress(stream).ToArray();
            }
           
        }
    }
}