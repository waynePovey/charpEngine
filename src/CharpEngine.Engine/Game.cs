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
        private FPSCamera _camera;
        private const float _cameraSpeed = 1.5f;
        private const float _cameraSensitivity = 0.3f;
        private float _aspectRatio;
        private Vector3 _initCameraPos;
        private Chunk _testChunk;


        public Game(
            GameWindowSettings gameWindowSettings,
            NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            _aspectRatio = (float) Size.X / Size.Y;
            _initCameraPos = new Vector3(0.0f, 0.0f, 15.0f);
        }


        protected override void OnLoad()
        {
            GL.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);
            GL.Enable(EnableCap.DepthTest);

            /* CREATE SHADER */
            _shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
            _shader.Use();

            /* CREATE CAMERA */
            _camera = new FPSCamera(_initCameraPos, _aspectRatio, _cameraSpeed, _cameraSensitivity);
            CursorVisible = false;
            CursorGrabbed = true;

            _testChunk = new Chunk(16);
            _testChunk.Load(_shader);

            base.OnLoad();
        }


        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            var model = Matrix4.Identity;
            _shader.SetMatrix4(model, "model");
            _shader.SetMatrix4(_camera.GetViewMatrix(), "view");
            _shader.SetMatrix4(_camera.GetProjectionMatrix(), "projection");

            _testChunk.Render();

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
            _testChunk.Unload();

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
