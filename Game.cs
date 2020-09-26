using System;
using OpenTK.Graphics.ES30;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Common.Input;
using OpenTK.Windowing.Desktop;

namespace CharpEngine
{
    public class Game : GameWindow
    {
        private Shader _shader;
        private int _vertexBufferObject;
        private int _vertexArrayObject;
        
        private readonly float[] _vertices = {
            -0.5f, -0.5f, 3.0f,
            0.5f, -0.5f, 3.0f,
            0.0f,  0.5f, 3.0f
        };

        public Game(
            GameWindowSettings gameWindowSettings,
            NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            if (KeyboardState.IsKeyDown(Key.Escape)) Close();

            base.OnUpdateFrame(args);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            
            MakeCurrent();

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BindVertexArray(_vertexArrayObject);
            
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            
            _shader.Use();
            SwapBuffers();

        }

        protected override void OnLoad()
        {
            // CREATE SHADER
            _shader = new Shader("../../../shader.vert", "../../../shader.frag");

            // CREATE AND BIND VBO
            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(
                BufferTarget.ArrayBuffer,
                _vertices.Length * sizeof(float),
                _vertices,
                BufferUsageHint.StaticDraw);

            var positionLocation = _shader.GetAttribLocation("aPosition");

            // CREATE AND BIND VAO
            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);
            GL.VertexAttribPointer(
                positionLocation,
                3,
                VertexAttribPointerType.Float,
                false,
                3 * sizeof(float),
                0);
            GL.EnableVertexAttribArray(positionLocation);

            base.OnLoad();
        }

        protected override void OnUnload()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);

            GL.DeleteBuffer(_vertexBufferObject);
            GL.DeleteVertexArray(_vertexArrayObject);
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
