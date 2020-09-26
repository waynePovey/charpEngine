using System;
using System.IO;
using System.Text;
using OpenTK.Graphics.ES30;

namespace CharpEngine
{
    public class Shader
    {
        private int _handle;
        private bool _isDisposed;

        public Shader(string vertexPath, string fragmentPath)
        {
            using var vertexReader = new StreamReader(vertexPath, Encoding.UTF8);
            var vertexShaderSrc = vertexReader.ReadToEnd();

            using var fragmentReader = new StreamReader(fragmentPath, Encoding.UTF8);
            var fragmentShaderSrc = fragmentReader.ReadToEnd();

            var vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexShaderSrc);

            var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentShaderSrc);

            GL.CompileShader(vertexShader);

            var infoLogVertex = GL.GetShaderInfoLog(vertexShader);
            if (infoLogVertex != string.Empty)
                Console.WriteLine(infoLogVertex);

            GL.CompileShader(fragmentShader);

            var infoLogFragment = GL.GetShaderInfoLog(fragmentShader);
            if (infoLogFragment != string.Empty)
                Console.WriteLine(infoLogFragment);

            _handle = GL.CreateProgram();

            GL.AttachShader(_handle, vertexShader);
            GL.AttachShader(_handle, fragmentShader);
            
            GL.LinkProgram(_handle);

            GL.DetachShader(_handle, vertexShader);
            GL.DetachShader(_handle, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
        }

        public void Use()
        {
            GL.UseProgram(_handle);
        }

        public int GetAttribLocation(string attribName)
        {
            return GL.GetAttribLocation(_handle, attribName);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed) return;
            GL.DeleteProgram(_handle);

            _isDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Shader()
        {
            GL.DeleteProgram(_handle);
        }
    }
}
