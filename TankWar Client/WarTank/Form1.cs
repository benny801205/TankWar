using NetworkUtil;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using UnityClass;

namespace TankWars
{


    /// <summary>
    /// Form for drawpanel
    /// </summary>
    public partial class Form1 : Form
    {

        private DrawingPanel drawingPanel;

        private GameController ctl;

        /// <summary>
        /// Form constructor
        /// </summary>
        public Form1()
        {
            InitializeComponent();

            ctl = new GameController();

            ctl.ErrorReceived += PopupMessage;

            ctl.UpdateArrived += OnFrame;

            ctl.ConnectToServer += PopupNamePanel;

            ctl.MessageArrived += AppendToChatBox;

            ctl.WorldSizeReceived += ChangePanelSize;

            (GamePanel as Control).KeyUp +=new KeyEventHandler(GamePanel_KeyUp) ;
        }


        /// <summary>
        /// Popup Message 
        /// </summary>
        /// <param name="msg"></param>
        private void PopupMessage(string msg)
        {
            this.Invoke(new Action(() =>
            {
                MessageBox.Show(msg);

            }));
            
        }



        /// <summary>
        /// Refresh on Frame
        /// </summary>
        private void OnFrame()
        {
            // Don't try to redraw if the window doesn't exist yet.
            // This might happen if the controller sends an update
            // before the Form has started.
            if (!IsHandleCreated)
                return;

            // Invalidate this form and all its children
            // This will cause the form to redraw as soon as it can



            GamePanel.Invoke(new Action(() =>

            {
                GamePanel.Refresh();
                //this.Invalidate(true);
            }));


            Vector2D TankAbsoultLocation = new Vector2D(this.Location.X+544,this.Location.Y+470);

            Vector2D MouseLocation = new Vector2D(MousePosition.X, MousePosition.Y);


            ctl.SetTdir(Normalize(MouseLocation-TankAbsoultLocation));

            ctl.SendCommand();
        }



        /// <summary>
        /// Normalize view
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        private Vector2D Normalize(Vector2D v)
        {

            double L = Math.Pow(v.GetX() * v.GetX() + v.GetY() * v.GetY(),0.5);

            return new Vector2D(v.GetX() / L, v.GetY() / L);
        }




        /// <summary>
        /// Click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectButton_Click(object sender, EventArgs e)
        {
            ctl.StartConnect(AddressBox.Text);
            
        }



        /// <summary>
        /// Popup panel
        /// </summary>
        private void PopupNamePanel()
        {
            this.Invoke(new Action(() =>

            {
                AddressBox.Enabled = false;

                ConnectButton.Enabled = false;

                NamePanel.Visible = true;

                PlayernameBox.Enabled = true;

                SubmitButton.Enabled = true;

            }));

        }


        /// <summary>
        /// Change Panel size after world size is received
        /// </summary>
        /// <param name="worldSize"></param>
        private void ChangePanelSize(int worldSize)
        {
            this.Invoke(new Action(() =>

            {
                drawingPanel = new DrawingPanel(ctl.GetWorld());

                drawingPanel.Location = new Point(0, 0);

                drawingPanel.Size = new Size(worldSize,worldSize);

                drawingPanel.ResizeBackgroundImage(worldSize);
               

                BackgroundPanel.Visible = false;

                ChatBox.Visible = true;

                NamePanel.Visible = false;

                SubmitButton.Enabled = false;

                PlayernameBox.Enabled = false;

                ChatBox.BringToFront();

                GamePanel.Focus();

                GamePanel.Controls.Add(drawingPanel);


                foreach (Control c in GamePanel.Controls) { 
                    if (c is Panel)
                        c.MouseUp += p_Click; 
            }




            }));
        }

        private void p_Click(object sender, EventArgs e)
        {
            ctl.ShootBeam();
        }



        /// <summary>
        /// Add Message into Chat Box
        /// </summary>
        /// <param name="l"></param>
        private void AppendToChatBox(IEnumerable<string> l )
        {
            foreach(string str in l)
            {

                this.Invoke(new MethodInvoker(() => ChatBox.AppendText(str + Environment.NewLine)));
            }
        }

        

        /// <summary>
        /// Sumbit player name to server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmitButton_Click(object sender, EventArgs e)
        {
            //send player name
            ctl.Send(PlayernameBox.Text);

        }


        /// <summary>
        /// Begining form of the start of the game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GamePanel_Paint(object sender, PaintEventArgs e)
        {

        }
 

        /// <summary>
        /// Control for when each of the keys on the keyboard are pressed
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GamePanel_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            
            
            if (e.KeyCode == Keys.W)
            {

                ctl.MoveUp();
            }


            if (e.KeyCode == Keys.S)
            {

                ctl.MoveDown();
            }


            if (e.KeyCode == Keys.A)
            {

                ctl.MoveLeft();
            }


            if (e.KeyCode == Keys.D)
            {

                ctl.MoveRight();
            }


            if (e.KeyCode == Keys.Space)
            {
               
                ctl.ShootProjectile();
            }
        }


        /// <summary>
        /// Refresh Control Command
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GamePanel_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.W|| e.KeyCode == Keys.S|| e.KeyCode == Keys.A|| e.KeyCode == Keys.D)
            {

                ctl.RenewControlCommandMove();
            }

            if (e.KeyCode == Keys.Space)
            {

                ctl.RenewControlCommandFire();
            }
        }



        /// <summary>
        /// Focus panel when clicked the chat box. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChatBox_Enter(object sender, EventArgs e)
        {
            //this.Focus();
            GamePanel.Focus();
        }
        /// <summary>
        /// exit menu item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        /// <summary>
        /// help menu  item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HelpMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Press W,A,S,D to move up, left, right down, and right.\n"+ 

            "Pressing space will shoot a projectile\n"+

            "Left clicking will shoot a beam if have powerup");
        }
    }
}
