﻿using System;
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

		public QFont scoreFont;
		public QFont menuFont;
		public Pane menuPane;
		public void paneSetup(){
			menuPane = new Pane (new RectangleF (100, 100, 300, 300));
			menuPane.addLabel (new Button ("testbutton", scoreFont, new Vector2 (200, 200), 10f));
		}
		public void fontSetup(){
			string fontRoot = "/usr/share/fonts/truetype/";
			//var builderConfig = new QFontBuilderConfiguration (true);
			//var strokeFont = new Font ("Fonts/Simplex.ttf", 20);
			//menuFont = new QFont(fontRoot + "anonymous-pro/Anonymous Pro.ttf", 25, FontStyle.Regular);
			scoreFont = new QFont("Fonts/Simplex.ttf", 20, FontStyle.Bold);
			//scoreFont = new QFont(strokeFont, builderConfig);
			scoreFont.Options.DropShadowActive = false;
			scoreFont.Options.Colour = new Color4(1.0f, 1.0f, 1.0f, 1.0f);
			//QFontRenderOptions fontRenderOptions = new QFontRenderOptions();
			//menuFont.Options.UseDefaultBlendFunction = true;
			//QFont.CreateTextureFontFiles (strokeFont, "Stroke20");
			//GL.ClearColor(1.0f, 1.0f, 1.0f, 0.0f);
			//GL.Disable(EnableCap.DepthTest);
		}
		public void printScore(string text, Vector2 location){
			scoreFont.Print (text, QFontAlignment.Left, location);

		}
		public void printMenu(string text, Vector2 location){
			menuFont.Print (text, QFontAlignment.Centre, location);
		}
    }	
}
