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
        bool needReload;
        public int currentLayer;
        int FrameCount;
        DateTime NextFPSUpdate;
        bool change;

        public Form1()
        {
            InitializeComponent();
            bin = new Bin();
            view = new View();
            loader = false;
            needReload = true;
            currentLayer = 0;
            trackBar1.Hide();
            FrameCount = 0;
            change = false;
            NextFPSUpdate = DateTime.Now.AddSeconds(1);
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
                trackBar1.Maximum = Bin.LenZ - 1;
                trackBar1.Show();
            }
        }

        private void GlControl1_Paint(object sender, PaintEventArgs e)
        {
            if (loader == true)
            {
                if (change == false)
                    view.DrawQuads(currentLayer);
                else
                {
                    if (needReload == true)
                    {
                        view.generateTextureImage(currentLayer);
                        view.Load2dTexture();
                        needReload = false;
                    }
                    view.DrawTexture(); 
                }
                glControl1.SwapBuffers();
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            currentLayer = trackBar1.Value;
            needReload = true;
            glControl1.Invalidate();
        }

        private void Application_Idle(Object sender, EventArgs e)
        {
            while (glControl1.IsIdle)
            {
                DisplayFPS();
                glControl1.Invalidate();
            }
        }

        private void DisplayFPS()
        {
            if (DateTime.Now > NextFPSUpdate)
            {
                this.Text = String.Format("CT Visualizer (fps{0})", FrameCount);
                NextFPSUpdate = DateTime.Now.AddSeconds(1);
                FrameCount = 0;
            }
            FrameCount++;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Application.Idle += Application_Idle;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            change = false;
            checkBox2.CheckState = CheckState.Unchecked;
            //checkBox1.CheckState = CheckState.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            change = true;
            checkBox1.CheckState = CheckState.Unchecked;
            //checkBox2.CheckState = CheckState.Checked;
        }
    }
}
