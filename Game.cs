using CharpEngine.Shaders;
using OpenTK.Graphics.ES30;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace CharpEngine
{
    public class Game : GameWindow
    {
        private Shader _shader;
        private int _vertexBuffer;
        private int _vertexArray;
        private int _indicesBuffer;
        private Camera _camera;
        private const float _cameraSpeed = 1.5f;
        private const float _cameraSensitivity = 0.1f;
        private Vector2 _lastPos;
        private float _aspectRatio;
        private Vector3 _initCameraPos;


        private readonly float[] _vertices = {
            -0.5f, 0.5f, 0.5f, 1f, 0f, 0f,      // top right front
            -0.5f, -0.5f, 0.5f, 0f, 1f, 0f,      // bottom right front
            0.5f, -0.5f, 0.5f, 0f, 0f, 1f,     // bottom left front
            0.5f, 0.5f, 0.5f, 1f, 1f, 0f,     // top left front
            -0.5f, 0.5f, -0.5f, 1f, 1f, 0f,     // top right back
            0.5f, 0.5f, -0.5f, 0f, 1f, 1f,     // bottom right back
            -0.5f, -0.5f, -0.5f, 1f, 0f, 1f,    // bottom left back
            0.5f, -0.5f, -0.5f, 1f, 1f, 1f     // top left back
        };

        private readonly uint[] _indices = {
            //Front
            0, 1, 3, 3, 1, 2,

            //Top
            4, 0, 5, 5, 0, 3,

            //Right
            3, 2, 5, 5, 2, 7,

            //Left
            4, 6, 0, 0, 6, 1,

            //Bottom
            1, 6, 2, 2, 6, 7,

            //Back
            6, 4, 7, 7, 4, 5
        };

        public Game(
            GameWindowSettings gameWindowSettings,
            NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            _aspectRatio = (float) Size.X / Size.Y;
            _initCameraPos = new Vector3(0.0f, 0.0f, 3.0f);
        }


        protected override void OnLoad()
        {
            GL.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);
            GL.Enable(EnableCap.DepthTest);

            // CREATE VBO
            _vertexBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
            GL.BufferData(
                BufferTarget.ArrayBuffer,
                _vertices.Length * sizeof(float),
                _vertices,
                BufferUsageHint.StaticDraw);


            // CREATE EBO
            _indicesBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _indicesBuffer);
            GL.BufferData(
                BufferTarget.ElementArrayBuffer,
                _indices.Length * sizeof(uint),
                _indices,
                BufferUsageHint.StaticDraw);


            // CREATE SHADER
            _shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
            _shader.Use();


            // CREATE AND BIND VAO
            _vertexArray = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArray);

            var positionLocation = _shader.GetAttribLocation("aPosition");
            GL.VertexAttribPointer(
                positionLocation,
                3,
                VertexAttribPointerType.Float,
                false,
                6 * sizeof(float),
                0);
            GL.EnableVertexAttribArray(positionLocation);

            var colorLocation = _shader.GetAttribLocation("aColor");
            GL.VertexAttribPointer(
                colorLocation,
                3,
                VertexAttribPointerType.Float,
                false,
                6 * sizeof(float),
                3 * sizeof(float)
                );
            GL.EnableVertexAttribArray(colorLocation);

            _camera = new Camera(_initCameraPos, _aspectRatio, _cameraSpeed, _cameraSensitivity);

            CursorVisible = false;
            CursorGrabbed = true;

            base.OnLoad();
        }


        protected override void OnRenderFrame(FrameEventArgs args)
        {

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.BindVertexArray(_vertexArray);
            _shader.Use();

            var model = Matrix4.Identity;

            _shader.SetMatrix4(model, "model");
            _shader.SetMatrix4(_camera.GetViewMatrix(), "view");
            _shader.SetMatrix4(_camera.GetProjectionMatrix(), "projection");


            // DRAW OBJECTS
            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, _indices);

            SwapBuffers();
            base.OnRenderFrame(args);
        }


        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            if (!IsFocused)
            {
                return;
            }

            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            if (KeyboardState.IsKeyDown(Keys.W))
            {
                _camera.Forward(args.Time);
            }

            if (KeyboardState.IsKeyDown(Keys.S))
            {
                _camera.Back(args.Time);
            }

            if (KeyboardState.IsKeyDown(Keys.A))
            {
                _camera.StrafeLeft(args.Time);
            }

            if (KeyboardState.IsKeyDown(Keys.D))
            {
                _camera.StrafeRight(args.Time);
            }

            if (KeyboardState.IsKeyDown(Keys.Space))
            {
                _camera.Up(args.Time);
            }

            if (KeyboardState.IsKeyDown(Keys.LeftShift))
            {
                _camera.Down(args.Time);
            }

            var mousePosition = new Vector2(MouseState.X, MouseState.Y);
            if (!_camera.HasMoved)
            {
                _camera.LastPosition = mousePosition;
                _camera.HasMoved = true;
            }
            else
            {
                _camera.MouseUpdate(mousePosition);
            }

            base.OnUpdateFrame(args);
        }


        protected override void OnUnload()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(_vertexBuffer);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.DeleteBuffer(_indicesBuffer);

            GL.BindVertexArray(0);
            GL.DeleteVertexArray(_vertexArray);
            _shader.Dispose();

            base.OnUnload();
        }


        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, Size.X, Size.Y);

            base.OnResize(e);
        }
    }
}
