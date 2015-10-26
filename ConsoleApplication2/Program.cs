using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Huffman;

namespace HuffmanLibrary
{
    class Program
    {
        static void Main(string[] args)
        {
            string phrase = "gbonjourXgg";
            
            HuffmanData data = new HuffmanData();
            data.uncompressedData = Encoding.ASCII.GetBytes(phrase);
            //data.uncompressedData = new byte[] { 30, 0, 1, 21, 1, 2, 1, 48, 4, 54, 1, 2, 255 };
            //byte[] lol = new byte[] { 30, 0, 1, 21, 1, 2, 1, 48, 4, 54, 1, 2, 255 };
            HuffmanLibrary hufflib = new HuffmanLibrary();
            Console.WriteLine("*******************COMPRESSION*******************");
            hufflib.Compress(ref data);
            Dictionary<byte, int> dictOcc = HuffmanLibrary.lettreOcc(data.uncompressedData);
            // convert dictOcc to List<KVP> for huffman data
            //data.compressedData = new byte[] { 187,120,197,58};
            //data.frequency = dictOcc.ToList();
            //data.sizeOfUncompressedData = data.uncompressedData.Length;
            //data.compressedData = new byte[] {205, 186, 119, 131, 52 };
            Console.WriteLine("*******************DECOMPRESSION*******************");
            //hufflib.Decompress(ref data);
        }
    }
}
