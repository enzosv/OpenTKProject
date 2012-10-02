

using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Windows.Forms;

//sources http://www.opentk.com/node/1492?page=1s
//http://code.google.com/p/speedprogramming/
namespace CS177Project
{
    class Game : GameWindow
    {
        private Matrix4 cameraMatrix;
        private float mouseX;
        public Game()
            : base(Screen.PrimaryScreen.Bounds.Right, Screen.PrimaryScreen.Bounds.Bottom)
        {
            GL.Enable(EnableCap.DepthTest);
            VSync = VSyncMode.On;
        }

        /// <summary>Load resources here.</summary>
        /// <param name="e">Not used.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            mouseX = 0;
            GL.ClearColor(0f, 0f, 0f, 0f);
            GL.Enable(EnableCap.DepthTest);
            //cameraMatrix *= Matrix4.CreateRotationX(6);
            cameraMatrix = Matrix4.CreateTranslation(0f, 0f, 0f);
            Cursor.Hide();
            Cursor.Position = new Point(Screen.PrimaryScreen.Bounds.Right / 2, Screen.PrimaryScreen.Bounds.Bottom / 2);
        }

        /// <summary>
        /// Called when it is time to render the next frame. Add your rendering code here.
        /// </summary>
        /// <param name="e">Contains timing information.</param>
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.LoadMatrix(ref cameraMatrix);
            for (int x = 0; x <= 20; x++)
            {

                //int rn = 1;
                for (int z = 0; z <= 20; z++)
                {
                    GL.PushMatrix();
                    GL.Translate((float)x * 5f, 0f, (float)z * 5f);
                    GL.Begin(BeginMode.TriangleFan);
                    GL.Color3(Color.Red); GL.Vertex3(0f, 1f,0f); //0
                    GL.Color3(Color.Orange); GL.Vertex3(-1f, -1f, 1f);
                    GL.Color3(Color.Yellow); GL.Vertex3(1f, -1f, 1f);
                    GL.Color3(Color.Green); GL.Vertex3(1f, -1f, -1f);
                    GL.Color3(Color.Blue); GL.Vertex3(-1f, -1f, -1f);
                    GL.Color3(Color.Indigo); GL.Vertex3(-1f, -1f, 1f);

                    GL.End();
                    GL.PopMatrix();
                }
            }


            SwapBuffers();
        }

        /// <summary>
        /// Called when it is time to setup the next frame. Add you game logic here.
        /// </summary>
        /// <param name="e">Contains timing information for framerate independent logic.</param>
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            float speed = 15f * (float)e.Time;
            float forwardZ = 0f, sideX = 0f;
            if (Keyboard[Key.W])
            {
                forwardZ = speed;
            }
            else if (Keyboard[Key.S])
            {
                forwardZ = -speed;
            }

            if (Keyboard[Key.A])
            {
                sideX = speed;
            }

            else if (Keyboard[Key.D])
            {
                sideX = -speed;
            }
            cameraMatrix *= Matrix4.CreateTranslation(sideX, 0f, forwardZ);

            cameraMatrix *= Matrix4.CreateRotationY(mouseX);
            mouseX = Mouse.XDelta / 100f;
            //mouseY = Mouse.YDelta / 150f;
            if (Cursor.Position.X >= Screen.PrimaryScreen.Bounds.Right - 1)
            {
                Cursor.Position = new Point(Screen.PrimaryScreen.Bounds.Left, Cursor.Position.Y);
            }
            else if (Cursor.Position.X <= Screen.PrimaryScreen.Bounds.Left)
            {
                Cursor.Position = new Point(Screen.PrimaryScreen.Bounds.Right, Cursor.Position.Y);
            }

            if (Keyboard[Key.Escape])
                Exit();
        }

        /// <summary>
        /// Called when your window is resized. Set your viewport here. It is also
        /// a good place to set up your projection matrix (which probably changes
        /// along when the aspect ratio of your window).
        /// </summary>
        /// <param name="e">Not used.</param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);

            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, Width / (float)Height, 1.0f, 64.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
        }



        /// <summary>        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // The 'using' idiom guarantees proper resource cleanup.
            // We request 30 UpdateFrame events per second, and unlimited
            // RenderFrame events (as fast as the computer can handle).
            using (Game game = new Game())
            {
                game.Run(30.0);
            }
        }

    }
}