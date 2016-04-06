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
using System.Drawing.Text;


namespace MasterBlaster
{
    //private Engine engine;
    class MBGameWindow : GameWindow
    {

		public bool resize = false;
		public Pane menuPane;
		public TextWriter scoreWriter;
		public TextWriter buttonWriter;
		private PrivateFontCollection pfc;
		private FontFamily[] families;
		public void textSetup(){
			pfc = new PrivateFontCollection ();
			pfc.AddFontFile ("Fonts/Anonymous Pro.ttf");
			families = pfc.Families;
			//this.setupButtonFont ();
			this.setupScoreFont ();

		}
		public void setupButtonFont(){
			buttonWriter = new TextWriter (new Font(families[0], 20),
				new Size (Width, Height), new Size(Width,Height), TextWriter.Alignment.Center);

		}
		public void setupScoreFont(){
			scoreWriter = new TextWriter (new Font(families[0], 20),
				new Size (Width, Height), new Size (Width,Height),  TextWriter.Alignment.Right);		
		}
		public void paneSetup(){
			menuPane = new Pane (new RectangleF (0.1f, 0.1f, 0.5f, 0.5f));
			//GameToView(0f, 0.5f)
			this.setupButtonFont ();
			menuPane.addLabel (new Button ("testbutton", buttonWriter,GameToView (0f,0.5f), 10f));
		}
		public Vector2 GameToView(float x, float y){
			return new Vector2 ((Width / (2.0f * Engine.ViewX)) * (x + Engine.ViewX),
				(- Height / (2.0f * Engine.ViewY)) * ( y - Engine.ViewY));
		}

    }	
}
