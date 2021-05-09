using QuickFont;
using QuickFont.Configuration;

namespace CharpEngine.GUI
{
    public class DebugGUI
    {
        private QFont _debugFont;

        public DebugGUI()
        {

        }

        public void Load()
        {
            _debugFont = new QFont("Font/cour.ttf", new QFontBuilderConfiguration());
        }

        public void Render()
        {

        }

        public void Unload()
        {

        }
    }
}