/// Implimented by: Ping Cheng Chung and Michael Meadows
/// Date: 11/21/19      Class: CS3500
/// Function draws each object onto the panel to create the world that each object will be on. 
///
/// 
/// 
///




using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UnityClass;

namespace TankWars
{


    /// <summary>
    /// Drawing panel
    /// </summary>
    public class DrawingPanel : Panel
    {
        private World theWorld;

        private Image projectile_Image;

        private Image background_Image;

        private Image tombStone_Image;

        private Image powerup_Image;




        /// <summary>
        /// Drawing panel constructor
        /// </summary>
        /// <param name="w"></param>
        public DrawingPanel(World w)
        {
             
            DoubleBuffered = true;

            theWorld = w;
            
            //Read image png file 
            background_Image = Image.FromFile(@"..\..\..\Resource\Images\Background.png");

            projectile_Image = Image.FromFile(@"..\..\..\Resource\Images\projectile.png");

            tombStone_Image = Image.FromFile(@"..\..\..\Resource\Images\tombstone.png");

            tombStone_Image= (Image)(new Bitmap(tombStone_Image, new Size(64, 64)));

            powerup_Image= Image.FromFile(@"..\..\..\Resource\Images\powerup.png");

            powerup_Image = (Image)(new Bitmap(powerup_Image, new Size(32, 32)));
        }



        /// <summary>
        /// Resize the background image 
        /// </summary>
        /// <param name="size"></param>
        public void ResizeBackgroundImage(int size)
        {
            background_Image= (Image)(new Bitmap(background_Image, new Size(size,size)));
        }


        /// <summary>
        /// Helper method for DrawObjectWithTransform
        /// </summary>
        /// <param name="size">The world (and image) size</param>
        /// <param name="w">The worldspace coordinate</param>
        /// <returns></returns>
        private static int WorldSpaceToImageSpace(int size, double w)
        {
            return (int)w + size / 2;
        }

        // A delegate for DrawObjectWithTransform
        // Methods matching this delegate can draw whatever they want using e  
        public delegate void ObjectDrawer(object o, PaintEventArgs e);


        /// <summary>
        /// This method performs a translation and rotation to drawn an object in the world.
        /// </summary>
        /// <param name="e">PaintEventArgs to access the graphics (for drawing)</param>
        /// <param name="o">The object to draw</param>
        /// <param name="worldSize">The size of one edge of the world (assuming the world is square)</param>
        /// <param name="worldX">The X coordinate of the object in world space</param>
        /// <param name="worldY">The Y coordinate of the object in world space</param>
        /// <param name="angle">The orientation of the objec, measured in degrees clockwise from "up"</param>
        /// <param name="drawer">The drawer delegate. After the transformation is applied, the delegate is invoked to draw whatever it wants</param>
        private void DrawObjectWithTransform(PaintEventArgs e, object o, int worldSize, double worldX, double worldY, double angle, ObjectDrawer drawer)
        {
            // "push" the current transform
            System.Drawing.Drawing2D.Matrix oldMatrix = e.Graphics.Transform.Clone();

            int x = WorldSpaceToImageSpace(worldSize, worldX);
            int y = WorldSpaceToImageSpace(worldSize, worldY);
            e.Graphics.TranslateTransform(x, y);
            e.Graphics.RotateTransform((float)angle);
            drawer(o, e);

            // "pop" the transform
            e.Graphics.Transform = oldMatrix;
        }

        /// <summary>
        /// Acts as a drawing delegate for DrawObjectWithTransform
        /// After performing the necessary transformation (translate/rotate)
        /// DrawObjectWithTransform will invoke this method
        /// </summary>
        /// <param name="o">The object to draw</param>
        /// <param name="e">The PaintEventArgs to access the graphics</param>
        private void TankDrawer(object o, PaintEventArgs e)
        {

            Tank p = o as Tank;

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;


            if (p.GetHp()==0)
            {
                
                e.Graphics.DrawImage(tombStone_Image, new Point(-32, -32));
            }


            else
            {
                
                e.Graphics.DrawImage((Image)theWorld.GetTankmodels()[p.GetID()].getBottom(), new Point(-32, -32));
            }
        }



        /// <summary>
        /// Draw the turret of the tank
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void TurretDrawer(object o, PaintEventArgs e)
        {

            Tank p = o as Tank;

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            e.Graphics.DrawImage((Image)theWorld.GetTankmodels()[p.GetID()].getTurret(), new Point(-35, -35));
        }




        /// <summary>
        /// Draw the tombstone of the tank apon if hp == 0
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void TombstonekDrawer(object o, PaintEventArgs e)
        {

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            e.Graphics.DrawImage(tombStone_Image, new Point(-32, -32));
        }




        /// <summary>
        /// Draw the score for each player that was defeted
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void ScoreDrawer(object o, PaintEventArgs e)
        {

            Tank p = o as Tank;

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 16);

            e.Graphics.DrawString(p.GetName() + "  " + p.GetScore()+"", drawFont, new System.Drawing.SolidBrush(System.Drawing.Color.Black), -30 ,-55);

        }




        /// <summary>
        /// Draw the projectile
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void ProjectileDrawer(object o, PaintEventArgs e)
        {

            Projectile p = o as Projectile;

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            e.Graphics.DrawImage(projectile_Image, new Point(-16, -16));
        }




        /// <summary>
        /// Draw the walls
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void WallDrawer(object o, PaintEventArgs e)
        {

            Wall p = o as Wall;

            double startX = p.GetStartP().GetX();

            double startY = p.GetStartP().GetY();

            double endX = p.GetEndP().GetX();

            double endY = p.GetEndP().GetY();

            int width = 50;

            Rectangle r;

            if ((startX + "").Equals((endX + "")))
            {
                //the wall is vertical
               r = new Rectangle(-25, -25, width, Math.Abs(Convert.ToInt32((startY-endY)))+50);
            }


            else
            {
                // the wall is horizontal
                r = new Rectangle(-25, -25, Math.Abs(Convert.ToInt32((startX - endX)))+50, width);
            }


            using (System.Drawing.SolidBrush WallBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Gray))
            {

                e.Graphics.FillRectangle(WallBrush, r);
            }

        }




        /// <summary>
        /// Draw the Hp bar
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void HpNameDrawer(object o, PaintEventArgs e)
        {
            Tank p = o as Tank;

            int width = 51;
            int height = 9;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                // Circles are drawn starting from the top-left corner.
                // So if we want the circle centered on the powerup's location, we have to offset it
                // by half its size to the left (-width/2) and up (-height/2)
                Rectangle outline = new Rectangle(-25, 35, width, height);
                
                e.Graphics.DrawRectangle(Pens.Black, outline);
                


                //Change color from different hp amounts
                if (p.GetHp() == 3)
                {

                    Rectangle colorBar = new Rectangle(-24, 36, width - 1, height - 1);

                    e.Graphics.FillRectangle(Brushes.Green, colorBar);
                }


                else if (p.GetHp() == 2)
                {

                    Rectangle colorBar = new Rectangle(-24, 36, width*2/3, height - 1);

                    e.Graphics.FillRectangle(Brushes.Yellow, colorBar);
                }


                else if (p.GetHp() == 1)
                {

                    Rectangle colorBar = new Rectangle(-24, 36, width/3, height - 1);

                    e.Graphics.FillRectangle(Brushes.Red, colorBar);
                }

                else
                {
                    //do not draw anything
                }
            
        }




        /// <summary>
        /// Acts as a drawing delegate for DrawObjectWithTransform
        /// After performing the necessary transformation (translate/rotate)
        /// DrawObjectWithTransform will invoke this method
        /// </summary>
        /// <param name="o">The object to draw</param>
        /// <param name="e">The PaintEventArgs to access the graphics</param>
        private void PowerupDrawer(object o, PaintEventArgs e)
        {

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            e.Graphics.DrawImage(powerup_Image, new Point(-16, -16));
        }



        /// <summary>
        /// Acts as a drawing delegate for DrawObjectWithTransform
        /// After performing the necessary transformation (translate/rotate)
        /// DrawObjectWithTransform will invoke this method
        /// </summary>
        /// <param name="o">The object to draw</param>
        /// <param name="e">The PaintEventArgs to access the graphics</param>
        private void BeamDrawer(object o, PaintEventArgs e)
        {

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            Rectangle colorBar = new Rectangle(-10, -10, 10, 1500);

            e.Graphics.FillRectangle(Brushes.Red, colorBar);
        }



        /// <summary>
        /// This method is invoked when the DrawingPanel needs to be re-drawn
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {

            int worldSize;

            int viewSize;

            double playerX;

            double playerY;

            Tank myTank;

            if (theWorld.GetTanks().ContainsKey(theWorld.MyTankID))
            {

                myTank = theWorld.GetTanks()[theWorld.MyTankID];

                playerX = myTank.GetLocation().GetX();

                playerY = myTank.GetLocation().GetY();
            }



            //acts if did not get location yet
            else
            {

                playerX = 0;

                playerY = 0;
            }


            worldSize = theWorld.worldsize;

            viewSize = theWorld.clientsize;

            double ratio = (double)viewSize / (double)worldSize;

            int halfSizeScaled = (int)(worldSize / 2.0 * ratio);

            double inverseTranslateX = -WorldSpaceToImageSpace(worldSize, playerX) + halfSizeScaled;

            double inverseTranslateY = -WorldSpaceToImageSpace(worldSize, playerY) + halfSizeScaled;

            e.Graphics.TranslateTransform((float)inverseTranslateX, (float)inverseTranslateY);



            //Draw Background
            e.Graphics.DrawImage(background_Image, new Point(0, 0));


            //Draw walls 
            foreach (Wall wall in theWorld.GetWalls().Values)
            {

                double startX = wall.GetStartP().GetX();

                double startY = wall.GetStartP().GetY();

                double endX = wall.GetEndP().GetX();

                double endY = wall.GetEndP().GetY();

                DrawObjectWithTransform(e, wall, theWorld.worldsize, Math.Min(startX,endX), Math.Min(startY,endY), 0, WallDrawer);

            }



            lock (theWorld.block)
            {
                // Draw the Tanks
                foreach (Tank tank in theWorld.GetTanks().Values)
                {
                    try
                    {
                        if (tank.GetHp() == 0)
                        {
                            //Draw tombstone if tank died
                            DrawObjectWithTransform(e, tank, theWorld.worldsize, tank.GetLocation().GetX(), tank.GetLocation().GetY(), 0, TombstonekDrawer);
                        }


                        else
                        {
                            //Draw the bottom of the tank
                            DrawObjectWithTransform(e, tank, theWorld.worldsize, tank.GetLocation().GetX(), tank.GetLocation().GetY(), tank.GetOrientation().ToAngle(), TankDrawer);

                            //Draw the top/turret of the tank
                            DrawObjectWithTransform(e, tank, theWorld.worldsize, tank.GetLocation().GetX(), tank.GetLocation().GetY(), tank.GetTdir().ToAngle(), TurretDrawer);

                        }

                        //Draw Score
                        DrawObjectWithTransform(e, tank, theWorld.worldsize, tank.GetLocation().GetX(), tank.GetLocation().GetY(), 0, ScoreDrawer);

                        //Draw the hp Bar and name
                        DrawObjectWithTransform(e, tank, theWorld.worldsize, tank.GetLocation().GetX(), tank.GetLocation().GetY(), 0, HpNameDrawer);
                    }
                    catch
                    {

                    }
                }
            }



            //Draw the projectiles
            foreach (Projectile proj in theWorld.GetProjectiles().Values)
            {

                if (!proj.isDied())
                {

                        DrawObjectWithTransform(e, proj, theWorld.worldsize, proj.GetLocation().GetX(), proj.GetLocation().GetY(), proj.GetDirection().ToAngle(), ProjectileDrawer);

                }
            }



            // Draw the powerups
            foreach (Powerup pow in theWorld.GetPowerups().Values)
            {
                if (!pow.isDied())
                {
                    
                    DrawObjectWithTransform(e, pow, theWorld.worldsize, pow.GetLocation().GetX(), pow.GetLocation().GetY(), 0, PowerupDrawer);
                }
            }



            // Draw the beams
            foreach (Beam beam in theWorld.GetBeamsList())
            {
              
                Vector2D ReverseDirection = new Vector2D(-beam.GetDirection().GetX(), -beam.GetDirection().GetY());

                DrawObjectWithTransform(e, beam, theWorld.worldsize, beam.GetLocation().GetX(), beam.GetLocation().GetY(),ReverseDirection.ToAngle(), BeamDrawer);

                theWorld.GetBeams().Remove(beam.GetID());
                
            }


            //Paint
            base.OnPaint(e);
        }
    }
}

