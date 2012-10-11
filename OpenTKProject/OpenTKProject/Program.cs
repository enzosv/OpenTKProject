

using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Windows.Forms;

//sources 
//http://www.opentk.com/node/1492?page=1s fps camera
//http://www.opentk.com/node/2873 rotating cube
//http://www.opentk.com/node/1800 sphere
namespace CS177Project
{
    class Game : GameWindow
    {
        private Matrix4 cameraMatrix;
        private float speed, mouseX, forwardZ, sideX, rotation, crement, crement2, crement3, look, jump;
        private bool zoomed, direction;
        private int n, awesomesauce;
        #region Cubes

        float[] cubeColors = {
			1, 0, 0,
            Color.Orange.R, Color.Orange.G, Color.Orange.B,
            Color.Yellow.R, Color.Yellow.G, Color.Yellow.B,
            0, 1, 1,
            1, 0, 0,
            Color.Indigo.R, Color.Indigo.G, Color.Indigo.B,
            1, 0, 0,
            Color.Orange.R, Color.Orange.G, Color.Orange.B,
		};

        byte[] cubeTriangles =
		{
			1, 0, 2, // front
			3, 2, 0,
			6, 4, 5, // back
			4, 6, 7,
			4, 7, 0, // left
			7, 3, 0,
			1, 2, 5, //right
			2, 6, 5,
			0, 1, 5, // top
			0, 5, 4,
			2, 3, 6, // bottom
			3, 7, 6,
		};

        float[] cube = {
			-0.5f,  1f,  0.5f, // vertex[0]
			 0.5f,  1f,  0.5f, // vertex[1]
			 0.5f, -1f,  0.5f, // vertex[2]
			-0.5f, -1f,  0.5f, // vertex[3]
			-0.5f,  1f, -0.5f, // vertex[4]
			 0.5f,  1f, -0.5f, // vertex[5]
			 0.5f, -1f, -0.5f, // vertex[6]
			-0.5f, -1f, -0.5f, // vertex[7]
		};

        #endregion
        public Game()
            : base(Screen.PrimaryScreen.Bounds.Right, Screen.PrimaryScreen.Bounds.Bottom)
        {
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
            GL.Enable(EnableCap.CullFace);
            GL.EnableClientState(EnableCap.VertexArray);
            GL.EnableClientState(EnableCap.ColorArray);
            //cameraMatrix = Matrix4.CreateTranslation(0f, 0f, 0f);
            cameraMatrix = Matrix4.CreateRotationY(10);
            Cursor.Hide();
            Cursor.Position = new Point(Screen.PrimaryScreen.Bounds.Right / 2, Screen.PrimaryScreen.Bounds.Bottom / 2);

            zoomed = false;
            rotation = 1f;
            crement = 0.03f;
            n = 0;
            awesomesauce = 1;
            crement3 = 0.1f;
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
            //rotation = (rotation > 0f) ? (rotation - 1 / 30) : 1f;

            for (int x = 0; x <= 20; x++)
            {
                for (int z = 0; z <= 20; z++)
                {
                    GL.PushMatrix();
                    GL.Translate((float)x * 5f, 0f, (float)z * 5f);
                    //rotation = (rotation < 360f) ? (rotation + (float)e.Time) : 0f;

                    if (x % 3 == 0)
                    {
                        if (z % 3 == 0)
                        {
                            generatePyramid();
                        }
                        else if (z % 3 == 1)
                        {
                            generateCube();
                        }
                        else if (z % 3 == 2)
                        {
                            generateDiamond();
                        }

                    }
                    else if (x % 3 == 1)
                    {
                        if (z % 3 == 0)
                        {
                            generateCube();
                        }
                        else if (z % 3 == 1)
                        {
                            generateDiamond();
                        }
                        else if (z % 3 == 2)
                        {
                            generatePyramid();
                        }
                    }
                    else if (x % 3 == 2)
                    {
                        if (z % 3 == 0)
                        {
                            generateDiamond();
                        }
                        else if (z % 3 == 1)
                        {
                            generatePyramid();
                        }
                        else if (z % 3 == 2)
                        {
                            generateCube();
                        }
                    }
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
            speed = 15f * (float)e.Time;
            look = 150f;
            forwardZ = 0f;
            sideX = 0f;

            #region controls
            #region zoom
            if (zoomed)
            {
                speed /= 10;
                look /= 2;
                if (OpenTK.Input.Mouse.GetState().IsButtonDown(MouseButton.Right))
                {
                    cameraMatrix *= Matrix4.CreateTranslation(0f, 0f, -3f);
                    zoomed = false;
                }
            }
            else
            {
                if (OpenTK.Input.Mouse.GetState().IsButtonDown(MouseButton.Left) && !zoomed)
                {
                    cameraMatrix *= Matrix4.CreateTranslation(0f, 0f, 3f);
                    zoomed = true;
                }
            }
            #endregion
            #region WASD
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
            #endregion
            #endregion

            cameraMatrix *= Matrix4.CreateTranslation(sideX, 0f, forwardZ);

            cameraMatrix *= Matrix4.CreateRotationY(mouseX);
            mouseX = Mouse.XDelta / look;
            //mouseY = Mouse.YDelta / 150f;
            if (Cursor.Position.X >= Screen.PrimaryScreen.Bounds.Right - 1)
            {
                Cursor.Position = new Point(Screen.PrimaryScreen.Bounds.Right - 1, Cursor.Position.Y);
            }
            else if (Cursor.Position.X <= Screen.PrimaryScreen.Bounds.Left)
            {
                Cursor.Position = new Point(Screen.PrimaryScreen.Bounds.Left, Cursor.Position.Y);
            }

            if (Keyboard[Key.Escape])
                Exit();

            
            #region pyramidMovement
            rotation -= crement;
            if (rotation <= -1f || rotation >= 0.99f)
            {
                crement *= -1;
                direction = !direction;
            }
            #endregion
            #region cubeMovement
            int y;
            if (direction) //horizontal
                y = 2;
            else
                y = 0;
            if (n == 10)
            {
                crement2 = -crement;
                awesomesauce = -1;
                //Console.WriteLine(crement2);
            }
            else if (n == 0)
            {
                crement2 = crement;
                awesomesauce = 1;
                //Console.WriteLine(crement2);
            }
            for (int x = y; x < 24; x += 3)
            {
                float orig = cube[x];
                cube[x] += crement2;
            }
            n += awesomesauce;
            #endregion
            #region diamondMovement
            jump += crement3;
            if (jump <= 0f || jump >= 0.99f)
            {
                crement3 *= -1;
            }	
            #endregion
            //Console.WriteLine(rotation);
            //negative positive positive negative dapat
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

        void generateCube()
        {
            GL.VertexPointer(3, VertexPointerType.Float, 0, cube);
            GL.ColorPointer(4, ColorPointerType.Float, 0, cubeColors);
            GL.DrawElements(BeginMode.Triangles, 36, DrawElementsType.UnsignedByte, cubeTriangles);
        }

        void generatePyramid()
        {
            GL.Begin(BeginMode.TriangleFan);
            GL.Color3(Color.Red);
            GL.Vertex3(0, 1f, 0); //0 //I want to make it point to where the camera is
            GL.Color3(Color.Orange);
            GL.Vertex3(-rotation, -1f, rotation);
            GL.Color3(Color.Yellow);
            GL.Vertex3(rotation, -1f, rotation);
            GL.Color3(Color.Green);
            GL.Vertex3(rotation, -1f, -rotation);
            GL.Color3(Color.Blue);
            GL.Vertex3(-rotation, -1f, -rotation);
            GL.Color3(Color.Indigo);
            GL.Vertex3(-rotation, -1f, rotation);
            //GL.VertexPointer(3, VertexPointerType.Float, 0, pyramid);
            //GL.ColorPointer(4, ColorPointerType.Float, 0, pyramidColors);
            //GL.DrawElements(BeginMode.TriangleFan, 6, DrawElementsType.UnsignedByte, pyramid);
        }

        void generateDiamond()
        {
            GL.Begin(BeginMode.TriangleFan);
            GL.Color3(Color.Red);
            GL.Vertex3(0, 1f+jump, 0);

            GL.Color3(Color.OrangeRed);
            GL.Vertex3(-1f, jump, 1f);

            GL.Color3(Color.Orange);
            GL.Vertex3(0f, jump, 1.5f);

            GL.Color3(Color.Yellow);
            GL.Vertex3(1f, jump, 1f);

            GL.Color3(Color.YellowGreen);
            GL.Vertex3(1.5f, jump, 0f);

            GL.Color3(Color.Green);
            GL.Vertex3(1f, jump, -1f);

            GL.Color3(Color.SeaGreen);
            GL.Vertex3(0f, jump, -1.5f);

            GL.Color3(Color.Blue);
            GL.Vertex3(-1f, jump, -1f);

            GL.Color3(Color.Violet);
            GL.Vertex3(-1.5f, jump, 0f);

            GL.Color3(Color.Indigo);
            GL.Vertex3(-1f, jump, 1f);
            GL.End();

            GL.Begin(BeginMode.TriangleFan);
            GL.Color3(Color.Red);
            GL.Vertex3(0, -1f+jump, 0);

            GL.Color3(Color.OrangeRed);
            GL.Vertex3(-1f, jump, 1f);

            GL.Color3(Color.Orange);
            GL.Vertex3(0f, jump, 1.5f);

            GL.Color3(Color.Yellow);
            GL.Vertex3(1f, jump, 1f);

            GL.Color3(Color.YellowGreen);
            GL.Vertex3(1.5f, jump, 0f);

            GL.Color3(Color.Green);
            GL.Vertex3(1f, jump, -1f);

            GL.Color3(Color.SeaGreen);
            GL.Vertex3(0f, jump, -1.5f);

            GL.Color3(Color.Blue);
            GL.Vertex3(-1f, jump, -1f);

            GL.Color3(Color.Violet);
            GL.Vertex3(-1.5f, jump, 0f);

            GL.Color3(Color.Indigo);
            GL.Vertex3(-1f, jump, 1f);
        }

    }
}