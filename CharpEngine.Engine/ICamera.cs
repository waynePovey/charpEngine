using OpenTK.Mathematics;

namespace CharpEngine.Engine
{
    public interface ICamera
    {
         public Matrix4 GetViewMatrix();
         public Matrix4 GetProjectionMatrix();
    }
}