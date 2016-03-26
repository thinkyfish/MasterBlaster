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
using System.Drawing.Text;


namespace MasterBlaster
{
    //private Engine engine;
    class MBGameWindow : GameWindow
    {


		public Pane menuPane;
		public TextWriter scoreWriter;
		private PrivateFontCollection pfc;
		private FontFamily[] families;
		public void textSetup(){
			pfc = new PrivateFontCollection ();
			pfc.AddFontFile ("Fonts/Anonymous Pro.ttf");
			families = pfc.Families;
			scoreWriter = new TextWriter (new Font(families[0], 20),
				new Size (Width, Height), new Size (Width, 50), StringFormatFlags.DirectionRightToLeft |StringFormatFlags.NoFontFallback);
		}
		public void paneSetup(){
			menuPane = new Pane (new RectangleF (0.1f, 0.1f, 0.5f, 0.5f));
			menuPane.addLabel (new Button ("testbutton", scoreWriter, new Vector2 (200, 200), 10f));
		}

    }	
}
