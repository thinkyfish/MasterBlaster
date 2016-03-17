using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickFont;
using OpenTK;
using System.Drawing;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace MasterBlaster
{
    //private Engine engine;
    class MBGameWindow : GameWindow
    {
		public QFont menuFont;
		public QFont scoreFont;
		public void fontSetup(){

			var builderConfig = new QFontBuilderConfiguration (true);

			menuFont = new QFont("Fonts/Simplex.ttf", 20, builderConfig);
			scoreFont = new QFont("Fonts/Simplex.ttf", 20, builderConfig);
			menuFont.Options.Colour = new Color4(1.0f, 1.0f, 1.0f, 1.0f);
			QFontRenderOptions fontRenderOptions = new QFontRenderOptions();
			menuFont.Options.UseDefaultBlendFunction = true;
			QFont.CreateTextureFontFiles ("Fonts/Simplex.ttf", 20.0f, "Stroke");
			//GL.ClearColor(1.0f, 1.0f, 1.0f, 0.0f);
			//GL.Disable(EnableCap.DepthTest);
		}
		public void printScore(string text, Vector2 location){
			scoreFont.Print (text, QFontAlignment.Right, location);
		}
		public void printMenu(string text, Vector2 location){
			menuFont.Print (text, QFontAlignment.Centre, location);
		}
    }	
}
