﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;

namespace Second_Lab
{
    public class View
    {
        //public Bitmap TextureImage;
        public int VBOtexture;
        public Bitmap textureImage;

        public View()
        {
            //TextureImage = new Bitmap(Bin.LenX, Bin.LenY);
            VBOtexture = 0;
        }

        public void SetupView(int wight, int height)
        {
            GL.ShadeModel(ShadingModel.Smooth);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, Bin.LenX, 0, Bin.LenY, -1, 1);
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

        public void Load2dTexture()
        {
            GL.BindTexture(TextureTarget.Texture2D, VBOtexture);
            BitmapData data = textureImage.LockBits(new System.Drawing.Rectangle(0, 0, textureImage.Width, textureImage.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            textureImage.UnlockBits(data);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            ErrorCode error = GL.GetError();
            string str = error.ToString();
        }

        public void generateTextureImage(int layerNomber)
        {
            textureImage = new Bitmap(Bin.LenX, Bin.LenY);
            for (int i = 0; i < Bin.LenX; i++)
                for (int j = 0; j < Bin.LenY; j++)
                {
                    int pixelNumber = i + j * Bin.LenX + layerNomber * Bin.LenX * Bin.LenY;
                    textureImage.SetPixel(i, j, TransferFunction(Bin.array[pixelNumber]));
                }
        }

        public void DrawTexture()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, VBOtexture);
            GL.Begin(BeginMode.Quads);
            GL.Color3(Color.White);
            GL.TexCoord2(0f, 0f);
            GL.Vertex2(0, 0);
            GL.TexCoord2(0f, 1f);
            GL.Vertex2(0, Bin.LenY);
            GL.TexCoord2(1f, 1f);
            GL.Vertex2(Bin.LenX, Bin.LenY);
            GL.TexCoord2(1f, 0f);
            GL.Vertex2(Bin.LenX, 0);
            GL.End();
            GL.Disable(EnableCap.Texture2D);
        }
    }
}
