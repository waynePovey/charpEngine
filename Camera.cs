using System;
using OpenTK.Mathematics;

namespace CharpEngine
{
    public class Camera
    {
        private Vector3 _position;
        private Vector3 _posX = Vector3.UnitX;
        private Vector3 _posY = Vector3.UnitY;
        private Vector3 _posZ = -Vector3.UnitZ;
        private float _pitch;
        private float _yaw = -MathHelper.PiOver2;
        private float _fov = MathHelper.PiOver4;
        private float _aspectRatio;
        private float _speed;
        private float _sensitivity;


        public Camera(Vector3 position, float aspectRatio, float speed, float sensitivity)
        {
            _position = position;
            _aspectRatio = aspectRatio;
            _speed = speed;
            _sensitivity = sensitivity;
        }

        public bool HasMoved { get; set; } = false;

        public Vector2 LastPosition { get; set; }

        public float Pitch
        {
            get => MathHelper.RadiansToDegrees(_pitch);
            set
            {
                var angle = MathHelper.Clamp(value, -89, 89f);
                _pitch = MathHelper.DegreesToRadians(angle);
                UpdateVectors();
            }
        }

        public float Yaw
        {
            get => MathHelper.RadiansToDegrees(_yaw);
            set
            {
                _yaw = MathHelper.DegreesToRadians(value);
                UpdateVectors();
            }
        }

        public float FOV
        {
            get => MathHelper.RadiansToDegrees(_fov);
            set
            {
                var angle = MathHelper.Clamp(value, 1f, 45f);
                _fov = MathHelper.DegreesToRadians(angle);
            }
        }

        public Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(_position, _position + _posZ, _posY);
        }

        public Matrix4 GetProjectionMatrix()
        {
            return Matrix4.CreatePerspectiveFieldOfView(_fov, _aspectRatio, 0.01f, 1000f);
        }

        public void UpdateVectors()
        {
            // Positive Z-axis
            _posZ.X = MathF.Cos(_pitch) * MathF.Cos(_yaw);
            _posZ.Y = MathF.Sin(_pitch);
            _posZ.Z = MathF.Cos(_pitch) * MathF.Sin(_yaw);
            _posZ = Vector3.Normalize(_posZ);

            // POsitive Y-axis
            _posX = Vector3.Normalize(Vector3.Cross(_posZ, Vector3.UnitY));

            // Positive X-axis
            _posY = Vector3.Normalize(Vector3.Cross(_posX, _posZ));
        }

        public void Forward(double time) => _position += _posZ * _speed * (float) time;

        public void Back(double time) => _position -= _posZ * _speed * (float) time;

        public void StrafeRight(double time) => _position += _posX * _speed * (float) time;

        public void StrafeLeft(double time) => _position -= _posX * _speed * (float) time;

        public void Up(double time) => _position += _posY * _speed * (float) time;

        public void Down(double time) => _position -= _posY * _speed * (float) time;

        public void MouseUpdate(Vector2 mousePosition)
        {
            Pitch -= (mousePosition.Y - LastPosition.Y) * _sensitivity;
            Yaw += (mousePosition.X - LastPosition.X) * _sensitivity;
            LastPosition = mousePosition;
        }
    }
}
