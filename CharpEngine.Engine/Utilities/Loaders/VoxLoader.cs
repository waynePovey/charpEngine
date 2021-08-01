using System.Drawing;
using System.IO;
using System.Linq;
using CharpEngine.Engine.Models;

namespace CharpEngine.Engine.Utilities.Loaders
{
    public class VoxLoader
    {
        private const int VOX_FORMAT_VERSION = 150;
        private VoxModel _model;

        public VoxModel Read(string path)
        {
            _model = new VoxModel();

            using var reader = new BinaryReader(File.OpenRead(path));

            ProcessHeader(reader);

            while(!EndOfFile(reader))
            {
                ProcessChunk(reader);
            }

            return _model;
        }

        private void ProcessHeader(BinaryReader reader)
        {
            if(!reader.ReadChars(4).SequenceEqual("VOX ".ToCharArray()))
            {
                throw new InvalidDataException("Invalid: VOX header not found");
            }

            if(reader.ReadInt32() > VOX_FORMAT_VERSION)
            {
                throw new InvalidDataException("Invalid: Unsupported VOX version");
            }
        }

        private void ProcessChunk(BinaryReader reader)
        {
            var id = new string(reader.ReadChars(4));
            var contentByteSize = reader.ReadInt32();
            var childByteSize = reader.ReadInt32();

            switch(id)
            {
                case "MAIN":
                    if(contentByteSize > 0)
                    {
                        throw new InvalidDataException("Invalid: Main chunk size greater than 0");
                    }

                    break;
                case "SIZE":
                    _model.SizeX = reader.ReadInt32();
                    _model.SizeY = reader.ReadInt32();
                    _model.SizeZ = reader.ReadInt32();

                    break;
                case "XYZI":
                    var numberOfVoxels = reader.ReadInt32();
                    _model.Voxels = new VoxModel.VoxNode[numberOfVoxels];
                    for(var i = 0; i < numberOfVoxels; i++)
                    {
                        var x = reader.ReadByte();
                        var y = reader.ReadByte();
                        var z = reader.ReadByte();
                        var color = reader.ReadByte();

                        _model.Voxels[i] = new VoxModel.VoxNode(x, y , z, color);
                    }

                    break;
                case "RGBA":
                    for(var i = 0; i < 256; i++)
                    {
                        var R = reader.ReadByte();
                        var G = reader.ReadByte();
                        var B = reader.ReadByte();
                        var A = reader.ReadByte();
                        _model.ColorPalette[i] = Color.FromArgb(A, R, G, B);
                    }

                    break;
                case "MATT":
                    break;
                default:
                    reader.ReadBytes(contentByteSize);

                    break;
            }
        }

        private bool EndOfFile(BinaryReader reader) =>
            reader.BaseStream.Position == reader.BaseStream.Length;
    }
}