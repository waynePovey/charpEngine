using System;
using System.IO;
using System.Text;
using OpenTK.Graphics.ES30;
using OpenTK.Mathematics;

namespace CharpEngine.Shaders
{
    public class Shader
    {
        public int Handle { get; set; }
        private bool _isDisposed;

        public Shader(string vertexPath, string fragmentPath)
        {
            // Vertex shader
            using var vertexReader = new StreamReader(vertexPath, Encoding.UTF8);
            var vertexShaderSrc = vertexReader.ReadToEnd();
            var vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexShaderSrc);
            GL.CompileShader(vertexShader);

            var infoLogVertex = GL.GetShaderInfoLog(vertexShader);
            if (infoLogVertex != string.Empty)
                Console.WriteLine(infoLogVertex);
            

            // Fragment shader
            using var fragmentReader = new StreamReader(fragmentPath, Encoding.UTF8);
            var fragmentShaderSrc = fragmentReader.ReadToEnd();
            var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentShaderSrc);
            GL.CompileShader(fragmentShader);

            var infoLogFragment = GL.GetShaderInfoLog(fragmentShader);
            if (infoLogFragment != string.Empty)
                Console.WriteLine(infoLogFragment);


            // Create shader program
            Handle = GL.CreateProgram();

            GL.AttachShader(Handle, vertexShader);
            GL.AttachShader(Handle, fragmentShader);
            
            GL.LinkProgram(Handle);


            // Dispose of individual shaders
            GL.DetachShader(Handle, vertexShader);
            GL.DetachShader(Handle, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
        }

        public void Use()
        {
            GL.UseProgram(Handle);
        }

        public int GetAttribLocation(string attribName)
        {
            return GL.GetAttribLocation(Handle, attribName);
        }

        public void SetMatrix4(Matrix4 mat, string name)
        {
            var modelMatrix = GL.GetUniformLocation(Handle, name);
            GL.UniformMatrix4(modelMatrix, true, ref mat);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed) return;
            GL.DeleteProgram(Handle);

            _isDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Shader()
        {
            GL.DeleteProgram(Handle);
        }
    }
}
