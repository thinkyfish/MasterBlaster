using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.Drawing;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Drawing.Text;
using StringTextureGL;

namespace MasterBlaster
{
	public enum WindowMode {Menu, Game, Settings, NewGame};
    
    class MBGameWindow : GameWindow
    {

		public bool resize = false;
		public Pane menuPane;
		public ScoreWriter scoreWriter;
		private Font labelfont;

		public WindowMode Mode;


		public MBGameWindow() : base(){
			this.scoreWriter = new ScoreWriter();
			this.labelfont = StringTexture.NewFont("Fonts/Simplex.ttf", "Simplex", 30, FontStyle.Bold);

		}

		public void paneSetup(){
			menuPane = new Pane (new RectangleF (-0.6f, -0.6f, 1.0f, 1.0f));
			//this.setupFonts ();
			menuPane.addLabel (new Label ("MasterBlaster", this.labelfont, GameToView(-0.5f, -1.0f)));//GameToView (0f,0.6f)));
			menuPane.addButton (new Button ("Start New Game", this.labelfont, GameToView (-0.5f, 0.4f)));
			//testlabel = new Label("Test", new Font(families[0], 30), new PointF(0,0));

		}
		public PointF GameToView(float x, float y){
			return new PointF ((Width / (2.0f * Engine.ViewX)) * (x + Engine.ViewX),
				(- Height / (2.0f * Engine.ViewY)) * ( y - Engine.ViewY));
		}
    }	
}
