using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

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
            //MessageBox.Show("Start function");
            if (File.Exists(name))
            {
                //MessageBox.Show("In open function");
                BinaryReader reader = new BinaryReader(File.Open(name, FileMode.Open));
                //MessageBox.Show("Read bin file");
                LenX = reader.ReadInt32();
                LenY = reader.ReadInt32();
                LenZ = reader.ReadInt32();
                //MessageBox.Show("Read 3 int values");
                int size = LenX * LenY * LenZ;
                array = new short[size];
                //for (int i = 0; i < size; i++)
                //    array[i] = 0;
                for (int i = 0; i < size; i++)
                    array[i] = reader.ReadInt16();  
            }
            //MessageBox.Show("file does not exist");
        }
    }
}
