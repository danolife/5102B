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
        /* lettreOcc calcule l'occurence de chaque lettre de notre message à compresser 
         * Retourne un dictionnaire<lettre, occurence> indiquant la fréquence de chaque lettre.
         */
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

        /* Iteration de création de l'arbre du code Huffmann*/
        public static void iteration(ref List<Noeud> pNodelist)
        {
            // sort
            pNodelist = pNodelist.OrderBy(n => n.getValue()).ToList();
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

        /* Retourne un dictionnaire contenant le code Huffmann de chaque lettre à partir de la table des Fréquences*/
        public static Dictionary<byte, List<bool>> Huffcreation(Dictionary<byte, int> tabFreq)
        {
            //Dictionary<byte, int> dictOcc = lettreOcc(data.uncompressedData);
            Dictionary<byte, List<bool>> huffcode = new Dictionary<byte, List<bool>>();
            List<Noeud> nodelist = new List<Noeud>();
            List<Noeud> leaflist = new List<Noeud>();
            //List<bool> finalList = new List<bool>();

            // convert dictOcc to List<KVP> for huffman data
            //data.frequency = tabFreq.ToList();

            foreach (var pair in tabFreq)
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

            return huffcode;
          
        
        }

        public bool Compress(ref HuffmanData data)
        {
            Dictionary<byte, int> dictOcc = lettreOcc(data.uncompressedData);
            /*Dictionary<byte, List<bool>> huffcode = new Dictionary<byte, List<bool>>();
            List<Noeud> nodelist = new List<Noeud>();
            List<Noeud> leaflist = new List<Noeud>();
            */
            List<bool> finalList = new List<bool>();
            
            // convert dictOcc to List<KVP> for huffman data
            data.frequency = dictOcc.ToList();
            data.sizeOfUncompressedData = data.uncompressedData.Length;
            /*
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
            } */

            foreach (KeyValuePair<byte, List<bool>> pair in Huffcreation(dictOcc))
            {
                string test = string.Join(",", pair.Value.ToArray());
                Console.WriteLine("{0} - {1}", Convert.ToChar(pair.Key), test);
            }

            

            for (int i = 0; i < data.sizeOfUncompressedData; i++)
            {
                finalList.AddRange(Huffcreation(dictOcc)[data.uncompressedData[i]]);
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
            data.compressedData = new byte[finalsize];
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
            for(int i=0;i<finalsize;i++){
                Console.WriteLine(data.compressedData[i]);
            }
            return true;
        }


        

        public bool Decompress(ref HuffmanData data)
        {
            // Convertie la list de freq en un dictionnaire
            Dictionary<byte,int> tableFreq = data.frequency.ToDictionary(pair => pair.Key, pair => pair.Value);
            // Reproduit le code Huffmann
            Dictionary<byte, List<bool>> codeHuff = Huffcreation(tableFreq);

            foreach (KeyValuePair<byte, List<bool>> pair in codeHuff)
            {
                string test = string.Join(",", pair.Value.ToArray());
                Console.WriteLine("{0} - {1}", Convert.ToChar(pair.Key), test);
            }
            
            // Crée un BitArray à partir du tableau de bytes
            
            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine(data.compressedData[i].ToString());
                
            }
            List<bool> bits = new List<bool>();

            for (int i = 0; i < data.compressedData.Length; i++ )
            {
                bits.AddRange(byteToBool(data.compressedData[i]).ToList());
            }
            
            StringBuilder sb = new StringBuilder();
            foreach (var b in bits)
            {
                sb.Append((bool)b ? "1" : "0");
            }
            Console.WriteLine(sb.ToString());


            List<bool> temp = new List<bool>();
            temp.Add(bits[0]);
            int count = 0;
            //List<byte> finalList = new List<byte>();
            //
            for(int i = 1; i< data.sizeOfUncompressedData; i++){
                
                foreach (KeyValuePair<byte, List<bool>> code in codeHuff)
                {
                    //Console.WriteLine("aymeric");
                    if (compare(temp,code.Value)) {
                        //finalList.Add(code.Key);
                        data.uncompressedData[count] = code.Key;
                        count++;
                        temp.Clear();
                        //Console.WriteLine("dany");
                    }
                }
                temp.Add(bits[i]);
            }
            Console.WriteLine("\n\nConversion :");
            for (int i = 0; i < data.sizeOfUncompressedData; i++)
            {               
                Console.WriteLine(Convert.ToChar(data.uncompressedData[i]));
            }
            return true;
        }

        string IPlugin.PluginName
        {
            get { return "MonPlugin"; }
        }

        public static bool compare(List<bool> listA, List<bool> listB){
            if(listA.Count != listB.Count){
                return false;
            }

            for (int i = 0; i < listA.Count; i++)
            {
                if(listA[i] != listB[i]){
                    return false;
                }
                
            }
            return true;
        }

        static IEnumerable<bool> GetBitsStartingFromLSB(byte b)
        {
            for (int i = 0; i < 8; i++)
            {
                yield return (b % 2 == 0) ? false : true;
                b = (byte)(b >> 1);
            }
        }

        static bool[] byteToBool(byte b) { 
            //List<bool> listBool = new List<bool>();
            bool[] tabBool = new bool[8] {false,false,false,false,false,false,false,false};
            //Console.WriteLine("B = " + b);
            int bInt = Convert.ToInt32(b);
            //Console.WriteLine("BInt = " + bInt);
            if (bInt - 128 >= 0) {
                bInt -= 128;
                tabBool[0] = true;
            }
            if (bInt - 64 >= 0)
            {
                bInt -= 64;
                tabBool[1] = true;
            }
            if (bInt - 32 >= 0)
            {
                bInt -= 32;
                tabBool[2] = true;
            }
            if (bInt - 16 >= 0)
            {
                bInt -= 16;
                tabBool[3] = true;
            }
            if (bInt - 8 >= 0)
            {
                bInt -= 8;
                tabBool[4] = true;
            }
            if (bInt - 4 >= 0)
            {
                bInt -= 4;
                tabBool[5] = true;
            }
            if (bInt - 2 >= 0)
            {
                bInt -= 2;
                tabBool[6] = true;
            }
            if (bInt - 1 >= 0)
            {
                bInt -= 1;
                tabBool[7] = true;
            }

            return tabBool;
        }
        
    }
}
