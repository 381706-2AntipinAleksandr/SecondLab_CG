using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using OpenTK;
using OpenTK.Graphics.OpenGL;

using System.Drawing;

namespace Second_Lab
{
    public class View
    {
        public void SetupView(int wight, int height)
        {
            GL.ShadeModel(ShadingModel.Smooth);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, Bin.LenX, 0, Bin.LenY, 1, 1);
            GL.Viewport(0, 0, wight, height);
        }

        protected int Clamp(int Val, int Min, int Max)
        {
            if (Val > Max)
                return Max;
            else if (Val < Min)
                return Min;
            else
                return Val;
        }

        protected Color TransferFunction(short value)
        {
            int min = 0;
            int max = 2000;
            int newVal = Clamp((value - min) * 255 / (max - min), 0, 255);
            return Color.FromArgb(255, newVal, newVal, newVal);
        }

        public void DrawQuads(int layerNumber)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Begin(BeginMode.Quads);
            for (int x_coord = 0; x_coord < Bin.LenX - 1; x_coord++)
                for (int y_coord = 0; y_coord < Bin.LenY - 1; y_coord++)
                {
                    short value;
                    value = Bin.array[x_coord + y_coord * Bin.LenX + layerNumber * Bin.LenX * Bin.LenY];
                    GL.Color3(TransferFunction(value));
                    GL.Vertex2(x_coord, y_coord);

                    value = Bin.array[x_coord + (y_coord + 1) * Bin.LenX + layerNumber * Bin.LenX * Bin.LenY];
                    GL.Color3(TransferFunction(value));
                    GL.Vertex2(x_coord, y_coord + 1);

                    value = Bin.array[x_coord + 1 + (y_coord + 1) * Bin.LenX + layerNumber * Bin.LenX * Bin.LenY];
                    GL.Color3(TransferFunction(value));
                    GL.Vertex2(x_coord + 1, y_coord + 1);

                    value = Bin.array[x_coord + 1 + y_coord * Bin.LenX + layerNumber * Bin.LenX * Bin.LenY];
                    GL.Color3(TransferFunction(value));
                    GL.Vertex2(x_coord + 1, y_coord);
                }
            GL.End();
        }
    }
}
