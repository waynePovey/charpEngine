using OpenTK.Mathematics;

namespace CharpEngine
{
    public interface ICamera
    {
         public Matrix4 GetViewMatrix();
         public Matrix4 GetProjectionMatrix();
    }
}