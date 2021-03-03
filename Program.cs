using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;

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
                Size = new Vector2i(1200, 800)
            };

            var game = new Game(gwSettings, nwSettings);
            game.Run();
        }
    }
}
