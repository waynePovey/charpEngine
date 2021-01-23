using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace CharpEngine
{
    public class Voxel
    {
        public bool IsActive { get; set; }
        public Vector3 Position { get; set; }


        public Voxel(Vector3 position)
        {

        }
    }
}
