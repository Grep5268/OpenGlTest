using GrepEngine.Engine.Objects;
using GrepEngine.Engine.Objects.Generation;
using GrepEngine.Engine.Physics.MoveDirection;
using GrepEngine.Engine.Viewport;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace GrepEngine.Game
{
    public class GameMain
    {
        public const double FRAMES_PER_SECOND = 1.0 / 60.0;

        GameWindow gameWindow;
        Camera cam = new Camera();

        MapGenerator mapGenerator;

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
            this.gameWindow.FocusedChanged += this.FocusChanged;
            this.gameWindow.Run(FRAMES_PER_SECOND);
        }

        void LoadGame(object sender, EventArgs e)
        {
            GL.ClearColor(0,0,0,0);
            GL.Enable(EnableCap.DepthTest);

            //lighting
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);

            gameWindow.CursorVisible = false;

            //texturing
            GL.Enable(EnableCap.Texture2D);
            GL.GenTextures(1, out texture);
            GL.BindTexture(TextureTarget.Texture2D, texture); //sets next texture to use
            var texData = LoadImage(@"../../Resources/Textures/dev_water.jpg");
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, texData.Width, texData.Height, 0, 
                OpenTK.Graphics.OpenGL.PixelFormat.Bgr, PixelType.UnsignedByte, texData.Scan0);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            //map
            mapGenerator = new MapGenerator(@"../../Resources/Maps/test.xml");
            mapGenerator.LoadMap();
        }

        void RenderFrame(object sender, EventArgs e)
        {
            this.gameWindow.SwapBuffers(); //show buffered screen
        }

        void UpdateFrame(object sender, EventArgs e)
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            var matrix = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI * (45.0f / 180f), gameWindow.Width / gameWindow.Height, 1.0f, 512.0f); //field of view, aspect, start clip, end clip (view distance)
            GL.LoadMatrix(ref matrix);

            GL.MatrixMode(MatrixMode.Modelview); //return to modelview matrix for drawing
            GL.LoadIdentity();
            var viewMatrix = cam.GetViewMatrix();
            GL.LoadMatrix(ref viewMatrix);

            //camera
            if (gameWindow.Focused)
            {
                cam.AddRotation(gameWindow);
                cam.Move();
            }

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            mapGenerator.Render();

            GL.End();
        }

        void Resize(object sender, EventArgs e)
        {
            GL.Viewport(0, 0, this.gameWindow.Width, this.gameWindow.Height);
            
        }

        void KeyPressed(object sender, OpenTK.KeyPressEventArgs e)
        {  
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
            switch (e.Key)
            {
                case Key.W:
                    cam.AddMovementCommand(new MoveDirection(Direction.FORWARD, 1));
                    break;
                case Key.A:
                    cam.AddMovementCommand(new MoveDirection(Direction.LEFT, 1));
                    break;
                case Key.S:
                    cam.AddMovementCommand(new MoveDirection(Direction.BACKWARDS, 1));
                    break;
                case Key.D:
                    cam.AddMovementCommand(new MoveDirection(Direction.RIGHT, 1));
                    break;
            }

        }

        void KeyUp(object sender, KeyboardKeyEventArgs e)
        {
            //  Console.WriteLine("up");
            if (e.Key.Equals(Key.Escape))
            {
                Environment.Exit(0);
            }

            switch (e.Key)
            {
                case Key.W:
                    cam.RemoveMovementCommand(Direction.FORWARD);
                    break;
                case Key.A:
                    cam.RemoveMovementCommand(Direction.LEFT);
                    break;
                case Key.S:
                    cam.RemoveMovementCommand(Direction.BACKWARDS);
                    break;
                case Key.D:
                    cam.RemoveMovementCommand(Direction.RIGHT);
                    break;
            }
        }

        void FocusChanged(object sender, EventArgs e)
        {
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
