using System.IO;
using System.IO.Compression;

namespace Pingfan.Kit
{
    /// <summary>
    /// GZip 压缩工具类
    /// </summary>
    public static class GZip
    {
        /// <summary>
        /// 解压一个 GZip 压缩的流。
        /// </summary>
        /// <param name="stream">需要解压的压缩流。</param>
        /// <returns>包含解压数据的 MemoryStream 对象。</returns>
        public static MemoryStream Decompress(Stream stream)
        {
            stream.Position = 0L;
            using var gZipStream = new GZipStream(stream, CompressionMode.Decompress, leaveOpen: true);
            var memoryStream = new MemoryStream();
            gZipStream.CopyTo(memoryStream);
            memoryStream.Position = 0L; // 重置位置以便进一步读取
            return memoryStream;
        }

        /// <summary>
        /// 解压一个 GZip 压缩的字节数组。
        /// </summary>
        /// <param name="data">需要解压的压缩字节数组。</param>
        /// <returns>包含解压数据的字节数组。</returns>
        public static byte[] Decompress(byte[] data)
        {
            using var stream = new MemoryStream(data);
            return Decompress(stream).ToArray();
        }

        /// <summary>
        /// 使用 GZip 压缩一个流。
        /// </summary>
        /// <param name="stream">需要压缩的流。</param>
        /// <returns>包含压缩数据的 MemoryStream 对象。</returns>
        public static MemoryStream Compress(Stream stream)
        {
            stream.Position = 0L;
            var memoryStream = new MemoryStream();
            using (var destination = new GZipStream(memoryStream, CompressionMode.Compress, leaveOpen: true))
            {
                stream.CopyTo(destination);
                destination.Flush(); // 确保所有数据已被写入目标
            }
            memoryStream.Position = 0L; // 重置位置以便进一步读取
            return memoryStream;
        }

        /// <summary>
        /// 使用 GZip 压缩一个字节数组。
        /// </summary>
        /// <param name="data">需要压缩的字节数组。</param>
        /// <returns>包含压缩数据的字节数组。</returns>
        public static byte[] Compress(byte[] data)
        {
            using var stream = new MemoryStream(data);
            return Compress(stream).ToArray();
        }
    }
}
