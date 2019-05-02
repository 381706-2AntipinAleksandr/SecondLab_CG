using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Second_Lab
{
    public partial class Form1 : Form
    {
        public Bin bin;
        public View view;
        public bool loader;
        public int currentLayer;

        public Form1()
        {
            InitializeComponent();
            loader = false;
            currentLayer = 0;
        }

        private void ОткрытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string filename = dialog.FileName;
                bin.ReadBin(filename);
                view.SetupView(glControl1.Width, glControl1.Height);
                loader = true;
                glControl1.Invalidate();
            }
        }

        private void GlControl1_Paint(object sender, PaintEventArgs e)
        {
            if (loader == true)
            {
                view.DrawQuads(currentLayer);
                glControl1.SwapBuffers();
            }
        }
    }
}
