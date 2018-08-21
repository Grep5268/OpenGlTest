using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace GrepEngine.Game
{
    public class GameMain
    {
        public const double FRAMES_PER_SECOND = 1.0 / 60.0;

        GameWindow gameWindow;

        double theta = 0; //delete
        int texture;

        public GameMain(GameWindow gameWindow)
        {
            this.gameWindow = gameWindow;
            this.Start();
        }

        void Start()
        {
            this.gameWindow.Load += this.LoadGame;
            this.gameWindow.Resize += this.Resize;
            this.gameWindow.UpdateFrame += this.UpdateFrame;
            this.gameWindow.RenderFrame += this.RenderFrame;
            this.gameWindow.KeyPress += this.KeyPressed;
            this.gameWindow.KeyDown += this.KeyDown;
            this.gameWindow.KeyUp += this.KeyUp;
            this.gameWindow.Run(FRAMES_PER_SECOND);
        }

        void LoadGame(object sender, EventArgs e)
        {
            GL.ClearColor(0,0,0,0);
            GL.Enable(EnableCap.DepthTest);

            //lighting
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);

            //texturing
            GL.Enable(EnableCap.Texture2D);
            GL.GenTextures(1, out texture);
            GL.BindTexture(TextureTarget.Texture2D, texture); //sets next texture to use
            var texData = LoadImage(@"../../Resources/Textures/dev_water.jpg");
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, texData.Width, texData.Height, 0, 
                OpenTK.Graphics.OpenGL.PixelFormat.Bgr, PixelType.UnsignedByte, texData.Scan0);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        void RenderFrame(object sender, EventArgs e)
        {
            this.gameWindow.SwapBuffers(); //show buffered screen
        }

        void UpdateFrame(object sender, EventArgs e)
        {
            GL.LoadIdentity(); //always reset to identity
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Translate(0, 0, -150);
            GL.Scale(2, 2, 2);
            //use push and pop matrix to keep a standard state
            //abstract out into transforms and render each
            GL.Rotate(theta, 0, 1, 1);
            theta = (theta + 2) % 360;

            GL.Begin(PrimitiveType.Quads);

            GL.Normal3(-1.0, 0.0, 0.0);
            GL.TexCoord2(1, 1);
            GL.Vertex3(-10.0, 10.0, 10.0);
            GL.TexCoord2(0, 1);
            GL.Vertex3(-10.0, 10.0, -10.0);
            GL.TexCoord2(0, 0);
            GL.Vertex3(-10.0, -10.0, -10.0);
            GL.TexCoord2(1, 0);
            GL.Vertex3(-10.0, -10.0, 10.0);

            GL.Normal3(1.0, 0.0, 0.0);
            GL.TexCoord2(1, 1);
            GL.Vertex3(10.0, 10.0, 10.0);
            GL.TexCoord2(0, 1);
            GL.Vertex3(10.0, 10.0, -10.0);
            GL.TexCoord2(0, 0);
            GL.Vertex3(10.0, -10.0, -10.0);
            GL.TexCoord2(1, 0);
            GL.Vertex3(10.0, -10.0, 10.0);

            GL.Normal3(0.0, -1.0, 0.0);
            GL.TexCoord2(1, 1);
            GL.Vertex3(10.0, -10.0, 10.0);
            GL.TexCoord2(0, 1);
            GL.Vertex3(10.0, -10.0, -10.0);
            GL.TexCoord2(0, 0);
            GL.Vertex3(-10.0, -10.0, -10.0);
            GL.TexCoord2(1, 0);
            GL.Vertex3(-10.0, -10.0, 10.0);

            GL.Normal3(0.0, 1.0, 0.0);
            GL.TexCoord2(1, 1);
            GL.Vertex3(10.0, 10.0, 10.0);
            GL.TexCoord2(0, 1);
            GL.Vertex3(10.0, 10.0, -10.0);
            GL.TexCoord2(0, 0);
            GL.Vertex3(-10.0, 10.0, -10.0);
            GL.TexCoord2(1, 0);
            GL.Vertex3(-10.0, 10.0, 10.0);

            GL.Normal3(0.0, 0.0, -1.0);
            GL.TexCoord2(1, 1);
            GL.Vertex3(10.0, 10.0, -10.0);
            GL.TexCoord2(0, 1);
            GL.Vertex3(10.0, -10.0, -10.0);
            GL.TexCoord2(0, 0);
            GL.Vertex3(-10.0, -10.0, -10.0);
            GL.TexCoord2(1, 0);
            GL.Vertex3(-10.0, 10.0, -10.0);

            GL.Normal3(0.0, 0.0, 1.0);
            GL.TexCoord2(1, 1);
            GL.Vertex3(10.0, 10.0, 10.0);
            GL.TexCoord2(0, 1);
            GL.Vertex3(10.0, -10.0, 10.0);
            GL.TexCoord2(0, 0);
            GL.Vertex3(-10.0, -10.0, 10.0);
            GL.TexCoord2(1, 0);
            GL.Vertex3(-10.0, 10.0, 10.0);

            GL.End();
        }

        void Resize(object sender, EventArgs e)
        {
            GL.Viewport(0, 0, this.gameWindow.Width, this.gameWindow.Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            var matrix = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI * (45.0f / 180f), gameWindow.Width / gameWindow.Height, 1.0f, 512.0f); //field of view, aspect, start clip, end clip (view distance)
            GL.LoadMatrix(ref matrix);
            GL.MatrixMode(MatrixMode.Modelview); //return to modelview matrix for drawing
        }

        void KeyPressed(object sender, KeyPressEventArgs e)
        {
            Console.WriteLine(e.KeyChar);
            if (e.KeyChar == 'l')
            {
                if(GL.IsEnabled(EnableCap.Lighting))
                {
                    GL.Disable(EnableCap.Lighting);
                } else
                {
                    GL.Enable(EnableCap.Lighting);
                }
            }
        }

        void KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            Console.WriteLine("down");
            //(if e.Key = Key.R == 'r')
        }

        void KeyUp(object sender, KeyboardKeyEventArgs e)
        {
            Console.WriteLine("up");
        }

        BitmapData LoadImage(string filePath)
        {
            var bmp = new Bitmap(filePath);
            var rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            var bmpData = bmp.LockBits(rect, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            bmp.UnlockBits(bmpData);
            return bmpData;
        }
    }
}
