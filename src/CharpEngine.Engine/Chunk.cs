using System;
using System.Collections.Generic;
using System.Drawing;
using CharpEngine.Shaders;
using OpenTK.Graphics.ES30;
using OpenTK.Mathematics;

namespace CharpEngine
{
    public class Chunk
    {
        private Voxel[,,] _voxels;
        private List<float> _vertices;
        private List<uint> _indices;
        private int _vertexBuffer;
        private int _indicesBuffer;
        private int _vertexArray;


        public Chunk(int size)
        {
            Size = size;

            _vertices = new List<float>();
            _indices = new List<uint>();

            _voxels = new Voxel[Size, Size, Size];

            for(var x = 0; x < Size; x++)
            {
                for(var y = 0; y < Size; y++)
                {
                    for(var z = 0; z < Size; z++)
                    {
                        _voxels[x, y, z] = new Voxel()
                        {
                            IsActive = true
                        };
                    }
                }
            }
        }

        public int Size { get; set; }

        public void Load(Shader shader)
        {
            uint counter = 0;
            for(var x = 0; x < Size; x++)
            {
                for(var y = 0; y < Size; y++)
                {
                    for(var z = 0; z < Size; z++)
                    {
                        if(_voxels[x, y, z].IsActive)
                        {
                            CreateVoxel(new Vector3(x, y, z), counter);
                            counter++;
                        }
                    }
                }
            }


             /* VERTEX VBO */
            _vertexBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
            GL.BufferData(
                BufferTarget.ArrayBuffer,
                _vertices.Count * sizeof(float),
                _vertices.ToArray(),
                BufferUsageHint.StaticDraw
            );


            /* INDEX VBO */
            _indicesBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _indicesBuffer);
            GL.BufferData(
                BufferTarget.ElementArrayBuffer,
                _indices.Count * sizeof(uint),
                _indices.ToArray(),
                BufferUsageHint.StaticDraw
            );

            /* CREATE AND BIND VAO */
            _vertexArray = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArray);

            var positionLocation = shader.GetAttribLocation("aPosition");
            GL.VertexAttribPointer(
                positionLocation,
                3,
                VertexAttribPointerType.Float,
                false,
                6 * sizeof(float),
                0
            );
            GL.EnableVertexAttribArray(positionLocation);

            var colorLocation = shader.GetAttribLocation("aColor");
            GL.VertexAttribPointer(
                colorLocation,
                3,
                VertexAttribPointerType.Float,
                false,
                6 * sizeof(float),
                3 * sizeof(float)
            );
            GL.EnableVertexAttribArray(colorLocation);
        }

        public void Render()
        {
            GL.DrawElements(PrimitiveType.Triangles, _indices.Count, DrawElementsType.UnsignedInt, _indices.ToArray());
        }

        public void Unload()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(_vertexBuffer);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.DeleteBuffer(_indicesBuffer);

            GL.BindVertexArray(0);
            GL.DeleteVertexArray(_vertexArray);
        }

        private void CreateVoxel(Vector3 pos, uint counter)
        {
            Random rnd = new Random();
            var r = (float)1 / 256 * rnd.Next(0, 255);
            var g = (float)1 / 256 * rnd.Next(0, 255);
            var b = (float)1 / 256 * rnd.Next(0, 255);

            _vertices.AddRange(new List<float>
            {
                1f + pos.X, 0f + pos.Y, 0f - pos.Z, r, g, b,     // front bottom right
                0f + pos.X, 0f + pos.Y, 0f - pos.Z, r, g, b,     // front bottom left
                0f + pos.X, 1f + pos.Y, 0f - pos.Z, r, g, b,     // front top    left
                1f + pos.X, 1f + pos.Y, 0f - pos.Z, r, g, b,     // front top    right
                1f + pos.X, 0f + pos.Y, 1f - pos.Z, r, g, b,     // back  bottom right
                0f + pos.X, 0f + pos.Y, 1f - pos.Z, r, g, b,     // back  bottom left
                0f + pos.X, 1f + pos.Y, 1f - pos.Z, r, g, b,     // back  top    left
                1f + pos.X, 1f + pos.Y, 1f - pos.Z, r, g, b      // back  top    right
            });

            var step = counter * 8;
            _indices.AddRange(new List<uint>
            {
                step + 0, step + 1, step + 3, step + 2, step + 3, step + 1,           // front
                step + 3, step + 2, step + 7, step + 6, step + 7, step + 2,           // top
                step + 4, step + 0, step + 7, step + 3, step + 7, step + 0,           // right
                step + 1, step + 5, step + 2, step + 6, step + 2, step + 5,           // left
                step + 4, step + 5, step + 0, step + 1, step + 0, step + 5,           // bottom
                step + 5, step + 4, step + 6, step + 7, step + 6, step + 4            // back
            });
        }
    }
}
