

using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Windows.Forms;

//sources http://www.opentk.com/node/1492?page=1s
namespace CS177Project
{
    class Game : GameWindow
    {
        private Matrix4 cameraMatrix;
        private float mouseX, mouseY;
        private float sceenWidth = 1024, sceenHeight = 768;
        public Game()
            : base(1024, 768)
        {
            GL.Enable(EnableCap.DepthTest);
            //glfwSetMousePos(x,y); 
            VSync = VSyncMode.On;
        }

        /// <summary>Load resources here.</summary>
        /// <param name="e">Not used.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Cursor.Position = new Point(800, 450);
            mouseX = 0;
            mouseY = 0;
            GL.ClearColor(0f, 0f, 0f, 0f);
            GL.Enable(EnableCap.DepthTest);
            cameraMatrix *= Matrix4.CreateRotationX(6);
            cameraMatrix = Matrix4.CreateTranslation(0f, 0f, 40f);
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

            GL.Begin(BeginMode.TriangleFan);
            GL.Color3(Color.Red); GL.Vertex3(0f, 10f, -5f);
            GL.Color3(Color.Orange); GL.Vertex3(-10f, -10f, 0f);
            GL.Color3(Color.Yellow); GL.Vertex3(10f, -10f, 0f);
            GL.Color3(Color.Green); GL.Vertex3(10f, -10f, -10f);
            GL.Color3(Color.Blue); GL.Vertex3(-10f, -10f, -10f);
            GL.Color3(Color.Indigo); GL.Vertex3(-10f, -10f, 0f);
            GL.End();

            SwapBuffers();
        }

        /// <summary>
        /// Called when it is time to setup the next frame. Add you game logic here.
        /// </summary>
        /// <param name="e">Contains timing information for framerate independent logic.</param>
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            cameraMatrix *= Matrix4.CreateRotationX(mouseY);
            cameraMatrix *= Matrix4.CreateRotationY(mouseX);

            float time = 10f*(float)e.Time;
            float forwardZ = 0f, sideX = 0f;
            if (Keyboard[Key.W])
            {
                forwardZ = time;
            }
            else if (Keyboard[Key.S])
            {
                forwardZ = -time;
            }

            if (Keyboard[Key.A])
            {
                sideX = time;
            }

            else if (Keyboard[Key.D])
            {
                sideX = -time;
            }
            cameraMatrix *= Matrix4.CreateTranslation(sideX, 0f, forwardZ);

            mouseX = Mouse.XDelta / 150f;
            mouseY = Mouse.YDelta / 150f;
            float mouseZ = Mouse.WheelPrecise;

            
            
            //cameraMatrix *= Matrix4.Scale(mouseZ);
            
            //cameraMatrix = Matrix4.LookAt(, new Vector3(Mouse.X, Mouse.Y, Mouse.WheelPrecise), new Vector3(0, 0, 0));

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