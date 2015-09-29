using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Huffman;

namespace HuffmanLibrary
{
    public class HuffmanLibrary : MarshalByRefObject, IPlugin
    {
        public static Dictionary<byte, int> lettreOcc(byte[] text)
        {
            Dictionary<byte, int> fDictOcc = new Dictionary<byte, int>();
            for (int i = 0; i < text.Length; i++)
            {
                byte currentByte = text[i];
                if (fDictOcc.ContainsKey(currentByte))
                {
                    fDictOcc[currentByte]++;
                }
                else
                {
                    fDictOcc.Add(currentByte, 1);
                }
            }
            return fDictOcc;
        }

        public static void iteration(ref List<Noeud> pNodelist)
        {
            // sort
            pNodelist = pNodelist.OrderBy(n => n.getValue()).ToList();
            /*// affichons
            Console.WriteLine("---------");
            foreach (Noeud kikou in pNodelist)
            {
                Console.WriteLine("{0} - {1}",Convert.ToChar(kikou.getLetter()), kikou.getValue());
            }*/
            // add children to parent
            Noeud parent = new Noeud(pNodelist[0].getValue() + pNodelist[1].getValue(), pNodelist[0], pNodelist[1]);
            // add new to list
            pNodelist.Add(parent);
            // add parent to children
            pNodelist[0].setParent(parent);
            pNodelist[1].setParent(parent);
            // set if right or left
            pNodelist[0].setIsRight(false);
            pNodelist[1].setIsRight(true);
            // remove old 2 from list
            pNodelist.RemoveAt(0);
            pNodelist.RemoveAt(0);
        }

        bool IPlugin.Compress(ref HuffmanData data)
        {
            Dictionary<byte, int> dictOcc = lettreOcc(data.uncompressedData);
            Dictionary<byte, List<bool>> huffcode = new Dictionary<byte, List<bool>>();
            List<Noeud> nodelist = new List<Noeud>();
            List<Noeud> leaflist = new List<Noeud>();
            List<bool> finalList = new List<bool>();
            
            // convert dictOcc to List<KVP> for huffman data
            data.frequency = dictOcc.ToList();

            foreach (var pair in dictOcc)
            {
                Noeud n = new Noeud(pair.Value, pair.Key);
                nodelist.Add(n);
                leaflist.Add(n);
            }

            while (nodelist.Count > 1)
            {
                iteration(ref nodelist);
            }

            foreach (Noeud leaf in leaflist)
            {
                byte currentLetter = leaf.getLetter();
                List<bool> code = new List<bool>();
                Noeud n = leaf;
                while (n.getParent() != null)
                {
                    code.Insert(0, n.getIsRight());
                    n = n.getParent();
                }
                huffcode.Add(currentLetter, code);
            }

            foreach (KeyValuePair<byte, List<bool>> pair in huffcode)
            {
                Console.WriteLine("{0} - {1}", Convert.ToChar(pair.Key), pair.Value);
            }

            data.sizeOfUncompressedData = data.uncompressedData.Length;

            for (int i = 0; i < data.sizeOfUncompressedData; i++)
            {
                finalList.AddRange(huffcode[data.uncompressedData[i]]);
            }

            // zero padding
            // il faut ajouter (8 - finalList.Count % 8) zeros ou 0 si c'est deja un multiple de 8
            int nbZeros = (finalList.Count % 8 == 0) ? 0 : (8 - finalList.Count % 8);
            while (nbZeros > 0)
            {
                finalList.Add(false);
                nbZeros--;
            }

            // determiner la taille de la compressed byte array
            int finalsize = finalList.Count / 8;
            // boucle de remplissage de la byte array
            for (int j = 0; j < finalsize; j++)
            {
                // une iteration pour un byte
                byte b = 0;
                for (int i = 0; i < 8; i++)
                {
                    b = (byte)(b | Convert.ToByte(finalList[0]));
                    finalList.RemoveAt(0);
                    if (i < 7)
                    {
                        b = (byte)(b << 1);
                    }
                }
                data.compressedData[j] = b;
            }

            return true;
        }

        bool IPlugin.Decompress(ref HuffmanData data)
        {
            throw new NotImplementedException();
        }

        string IPlugin.PluginName
        {
            get { throw new NotImplementedException(); }
        }
    }
}
