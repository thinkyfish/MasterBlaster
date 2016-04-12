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
	public enum WindowMode {Menu, Game, Settings, NewGame};
    //private Engine engine;
    class MBGameWindow : GameWindow
    {

		public bool resize = false;
		public Pane menuPane;
		public TextWriter scoreWriter;
		public TextWriter buttonWriter;
		public TextWriter menuWriter;
		private PrivateFontCollection pfc;
		private FontFamily[] families;

		public WindowMode Mode;
		public void LoadFonts(){
			pfc = new PrivateFontCollection ();
			pfc.AddFontFile ("Fonts/Anonymous Pro.ttf");
			families = pfc.Families;
			//this.setupButtonFont ();
			this.setupFonts ();

		}
		public void setupFonts(){
			buttonWriter = new TextWriter (new Font(families[0], 20),
				new Size (Width, Height), new Size(Width,Height), TextWriter.Alignment.Center);
			scoreWriter = new TextWriter (new Font(families[0], 20),
				new Size (Width, Height), new Size (Width,Height),  TextWriter.Alignment.Right);
			menuWriter = new TextWriter (new Font(families[0], 30),
				new Size (Width, Height), new Size(Width,Height), TextWriter.Alignment.Center);
		}
		public void paneSetup(){
			menuPane = new Pane (new RectangleF (-0.75f, -0.75f, 1.5f, 1.5f));
			//GameToView(0f, 0.5f)
			this.setupFonts ();
			menuPane.addLabel (new Label ("MasterBlaster", menuWriter,GameToView (0f,0.6f)));
			menuPane.addButton (new Button ("Start New Game", buttonWriter, GameToView (0f, 0.4f)));
		}
		public Vector2 GameToView(float x, float y){
			return new Vector2 ((Width / (2.0f * Engine.ViewX)) * (x + Engine.ViewX),
				(- Height / (2.0f * Engine.ViewY)) * ( y - Engine.ViewY));
		}

    }	
}
