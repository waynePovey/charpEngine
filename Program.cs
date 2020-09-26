using System;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK;

namespace CharpEngine
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var gwSettings = new GameWindowSettings
            {
                IsMultiThreaded = true,
                RenderFrequency = 120,
                UpdateFrequency = 120

            };

            var nwSettings = new NativeWindowSettings
            {
                Title = "CharpEngine",
                Size = new Vector2i(800, 600)
            };


            var game = new Game(gwSettings, nwSettings);
            game.Run();
        }
    }
}
