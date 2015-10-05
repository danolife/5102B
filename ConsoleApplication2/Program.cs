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
            HuffmanData data = new HuffmanData();
            data.uncompressedData = new byte[] { 0, 0, 1, 21, 1, 2, 1, 48, 4, 54, 1, 2, 255 };
            HuffmanLibrary hufflib = new HuffmanLibrary();
            hufflib.Compress(ref data);
        }
    }
}
