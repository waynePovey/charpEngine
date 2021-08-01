using System.Drawing;

namespace CharpEngine.Engine.Models
{
    public class VoxModel
    {
        public Color[] ColorPalette { get; set; }
        public VoxNode[] Voxels { get; set; }
        public int SizeX { get; set; }
        public int SizeY { get; set; }
        public int SizeZ { get; set; }

        public VoxModel()
        {
            ColorPalette = new Color[256];
        }

        public class VoxNode
        {
            public byte X { get; }
            public byte Y { get; }
            public byte Z { get; }
            public byte ColorPaletteIndex { get; }

            public VoxNode(byte x, byte y, byte z, byte color)
            {
                X = x;
                Y = y;
                Z = z;
                ColorPaletteIndex = color;
            }
        }
    }
}