

using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

//sources http://www.opentk.com/node/1492?page=1s
namespace OpenTKCameraPort
{
    class Program : GameWindow
    {
        private Matrix4 cameraMatrix;

        public Program()
            : base(1024, 768)
        {
            GL.Enable(EnableCap.DepthTest);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            cameraMatrix = Matrix4.CreateTranslation(0f, -10f, 0f);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.LoadMatrix(ref cameraMatrix);

            // Display some planes
            for (int x = -10; x <= 10; x++)
            {
                for (int z = -10; z <= 10; z++)
                {
                    GL.PushMatrix();
                    GL.Translate((float)x * 5f, 0f, (float)z * 5f);
                    GL.Begin(BeginMode.Quads);
                    GL.Color3(Color.Red);
                    GL.Vertex3(1f, 4f, 0f);
                    GL.Color3(Color.Orange);
                    GL.Vertex3(-1f, 4f, 0f);
                    GL.Color3(Color.Brown);
                    GL.Vertex3(-1f, 0f, 0f);
                    GL.Color3(Color.Maroon);
                    GL.Vertex3(1f, 0f, 0f);
                    GL.End();
                    GL.PopMatrix();
                }
            }

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            float forwardZ = 0f, sideX = 0f;
            if (Keyboard[Key.W])
            {
                forwardZ = 10f* (float)e.Time;
            }
            else if (Keyboard[Key.S])
            {
                forwardZ = -10f * (float)e.Time;
            }

            if (Keyboard[Key.A])
            {
                sideX = 10f * (float)e.Time;
            }

            else if (Keyboard[Key.D])
            {
                sideX = -10f * (float)e.Time;
            }
            cameraMatrix *= Matrix4.CreateTranslation(sideX, 0f, forwardZ);

            float mouseX = Mouse.XDelta/150f;
            float mouseY = Mouse.YDelta/150f;

            cameraMatrix *= Matrix4.CreateRotationX(mouseY);
            cameraMatrix *= Matrix4.CreateRotationY(mouseX);

            if (Keyboard[Key.Escape])
                Exit();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);

            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, Width / (float)Height, 1.0f, 64.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
        }



        public static void Main(string[] args)
        {


            using (Program p = new Program())
            {
                p.Run();
            }
        }
    }
}