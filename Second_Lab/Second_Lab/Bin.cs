using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Second_Lab
{
    public class Bin
    {
        public static int LenX, LenY, LenZ;
        public static short[] array;

        public Bin()
        {
        }

        public void ReadBin(string name)
        {
            if (File.Exists(name))
            {
                BinaryReader reader = new BinaryReader(File.Open(name, FileMode.Open));
                LenX = reader.ReadInt32();
                LenY = reader.ReadInt32();
                LenZ = reader.ReadInt32();

                array = new short[LenX * LenY * LenZ];
                for (int i = 0; i < LenX * LenY * LenZ; i++)
                    array[i] = reader.ReadInt16();  
            }
        }
    }
}
