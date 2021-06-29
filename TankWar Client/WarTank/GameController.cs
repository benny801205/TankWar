/// Implimented by: Ping Cheng Chung and Michael Meadows
/// Date: 11/21/19      Class: CS3500
/// Receive the Json and add the objects from the server for all of the logic behind the game.  
///
/// 
/// 
///






using System;
using UnityClass;
using NetworkUtil;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Drawing;

namespace TankWars

{

    /// <summary>
    /// Game controller
    /// </summary>
    public class GameController
    {
        private static World world;

        private SocketState theServer;

        private ControlCommand cc;

        private List<TankModel> tankModelList;

        private int TankCounter;


        //pop up CreateName Panel
        public delegate void ConnectServerHandler();

        public event ConnectServerHandler ConnectToServer;


        //update GamePanel
        public delegate void ServerUpdateHandler();

        public event ServerUpdateHandler UpdateArrived;


        //update ChatBox
        public delegate void MessageArrivedHandler(IEnumerable<string> newMessages);

        public event MessageArrivedHandler MessageArrived;


        //pop up error msg
        public delegate void ErrorReceivedHandler(string msg);

        public event ErrorReceivedHandler ErrorReceived;


        //receive world size
        public delegate void WorldSizeReceivedHandler(int size);

        public event WorldSizeReceivedHandler WorldSizeReceived;




        /// <summary>
        /// Game controller Constructor
        /// </summary>
        public GameController()
        {
            cc = new ControlCommand(new Vector2D(0,0));

            world = new World();


            tankModelList = new List<TankModel>();

            TankCounter = 0;

            tankModelList.Add(new TankModel("Blue", Image.FromFile(@"..\..\..\Resource\Images\BlueTank.png"), Image.FromFile(@"..\..\..\Resource\Images\BlueTurretBig.png")));

            tankModelList.Add(new TankModel("Dark", Image.FromFile(@"..\..\..\Resource\Images\DarkTank.png"), Image.FromFile(@"..\..\..\Resource\Images\DarkTurret.png")));

            tankModelList.Add(new TankModel("Green", Image.FromFile(@"..\..\..\Resource\Images\GreenTank.png"), Image.FromFile(@"..\..\..\Resource\Images\GreenTurret.png")));

            tankModelList.Add(new TankModel("LightGreen", Image.FromFile(@"..\..\..\Resource\Images\LightGreenTank.png"), Image.FromFile(@"..\..\..\Resource\Images\LightGreenTurret.png")));

            tankModelList.Add(new TankModel("Orange", Image.FromFile(@"..\..\..\Resource\Images\OrangeTank.png"), Image.FromFile(@"..\..\..\Resource\Images\OrangeTurret.png")));

            tankModelList.Add(new TankModel("Purple", Image.FromFile(@"..\..\..\Resource\Images\PurpleTank.png"), Image.FromFile(@"..\..\..\Resource\Images\PurpleTurret.png")));

            tankModelList.Add(new TankModel("Red", Image.FromFile(@"..\..\..\Resource\Images\RedTank.png"), Image.FromFile(@"..\..\..\Resource\Images\RedTurret.png")));

            tankModelList.Add(new TankModel("Yellow", Image.FromFile(@"..\..\..\Resource\Images\YellowTank.png"), Image.FromFile(@"..\..\..\Resource\Images\YellowTurret.png")));
        }


        /// <summary>
        /// NewPort
        /// </summary>
        /// <param name="IPAddress"></param>
        public void StartConnect(string IPAddress)
        {
            int port = 11000;

            Networking.ConnectToServer(ConnectServer, IPAddress, port);
        }


        /// <summary>
        /// 
        /// return if error Occurred in socket state
        /// </summary>
        /// <param name="state"></param>
        private void ConnectServer(SocketState state)
        {
            if (state.ErrorOccured)
            {
                //form.PopupMessage("Error connecting to server. Please check the IPAddress.");
                ErrorReceived("Error connecting to server. Please check the IPAddress.");

                return;
            }


            // Save the SocketState so we can use it to send messages
            theServer = state;

            ConnectToServer();

            state.OnNetworkAction = ReceiveMapSize;

            Networking.GetData(state);
        }

        /// <summary>
        /// 
        /// Sends Json to server
        /// </summary>
        public void SendCommand()
        {
            string message = JsonConvert.SerializeObject(cc);

            Console.WriteLine(message);
            Send(message + "\n");
            cc.SetFire("none");

        }



        /// <summary>
        /// Receive the map size
        /// </summary>
        /// <param name="state"></param>
        private void ReceiveMapSize(SocketState state)
        {
            if (state.ErrorOccured)
            {
                
                ErrorReceived("Error while receiving. Please restart the client.");

                return;
            }

            string totalData = state.GetData();

            string[] parts = Regex.Split(totalData, @"(?<=[\n])");



            world.MyTankID = Int32.Parse(parts[0]);

            state.RemoveData(0, parts[0].Length);

            world.worldsize = Int32.Parse(parts[1]);

            state.RemoveData(0, parts[1].Length);


            //change size of client
            WorldSizeReceived(world.worldsize);
            

            //start receive data loop
            state.OnNetworkAction = ReceiveData;

            Networking.GetData(state);
        }



        /// <summary>
        /// Event loop to receive data from server
        /// </summary>
        /// <param name="state"></param>
        private void ReceiveData(SocketState state)
        {
            if (state.ErrorOccured)
            {

                ErrorReceived("Error while receiving. Please restart the client.");

                return;
            }

            ProcessData(state);

            // Continue the event loop
            Networking.GetData(state);
        }


        /// <summary>
        /// Handle the data from server
        /// </summary>
        /// <param name="state"></param>
        private void ProcessData(SocketState state)
        {
            //handle the data loop
            string totalData = state.GetData();

            string[] parts = Regex.Split(totalData, @"(?<=[\n])");

            // Loop until we have processed all messages.
            // We may have received more than one.

            foreach (string p in parts)
            {
                // Ignore empty strings added by the regex splitter
                if (p.Length == 0)
                {

                    continue;
                }

                // The regex splitter will include the last string even if it doesn't end with a '\n',
                // So we need to ignore it if this happens. 
                if (p[p.Length - 1] != '\n')
                {

                    break;
                }


                //handel the JSON
                lock (world.block)
                {

                    HandleJson(p);
                }
              //  Console.WriteLine(p);
                // Then remove it from the SocketState's growable buffer
                state.RemoveData(0, p.Length);
            }

            UpdateArrived();
        }



        /// <summary>
        /// Return world
        /// </summary>
        /// <returns></returns>
        public World GetWorld()
        {

            return world;
        }


        /// <summary>
        /// check the data type,then build one and add to WorldList
        /// </summary>
        /// <param name="str"></param>
        private void HandleJson(string str)
        {

            //Console.WriteLine(str);
            List<string> newMessage = new List<string>();

            JObject obj = JObject.Parse(str);

            if (obj["wall"] != null)
            {
                Wall rebuilt = JsonConvert.DeserializeObject<Wall>(str);

                var WallDictionary = world.GetWalls();

                WallDictionary[rebuilt.GetID()] = rebuilt;
            }


            else if (obj["tank"] != null)
            {

                Tank rebuilt = JsonConvert.DeserializeObject<Tank>(str);

                var TankDictionary = world.GetTanks();


                if (TankDictionary.ContainsKey(rebuilt.GetID()))
                {

                    if (rebuilt.isDisconnected())
                    {

                        TankDictionary.Remove(rebuilt.GetID());

                        //when other player disconnects from the game
                        newMessage.Add(rebuilt.GetName() + " left");
                    }


                    else 
                    {
                        
                         TankDictionary[rebuilt.GetID()] = rebuilt;
                    }
                }


                else
                {
                    //add a new Tank, and give a specific color model.
                    world.GetTankmodels()[rebuilt.GetID()] =tankModelList[TankCounter % 8];

                    TankDictionary[rebuilt.GetID()] = rebuilt;

                    TankCounter++;


                    //When new player joins the game
                    newMessage.Add(rebuilt.GetName() + " join");
                }


            }


            else if (obj["proj"] != null)
            {
                Projectile rebuilt = JsonConvert.DeserializeObject<Projectile>(str);

                var ProjectileDictionary = world.GetProjectiles();

                ProjectileDictionary[rebuilt.GetID()] = rebuilt;
            }


            else if(obj["beam"] != null)
            {

                Beam rebuilt = JsonConvert.DeserializeObject<Beam>(str);

                var BeamDictionary = world.GetBeams();

                BeamDictionary[rebuilt.GetID()] = rebuilt;
            }


            else if(obj["power"] != null)
            {

                Powerup rebuilt = JsonConvert.DeserializeObject<Powerup>(str);

                var PowerupsDictionary = world.GetPowerups();

                PowerupsDictionary[rebuilt.GetID()] = rebuilt;
            }


            else
            {
                //print on chatBox
                newMessage.Add(str);
            }


            if (newMessage.Count != 0)
            {

                MessageArrived(newMessage);
            }
        }

        /// <summary>
        /// 
        /// Send message to server
        /// </summary>
        /// <param name="str"></param>
        public void Send(string str)
        {
            Networking.Send(theServer.TheSocket, str);
        }


        /// <summary>
        /// Refresh Control Command Move
        /// </summary>
        public void RenewControlCommandMove()
        {
            cc.SetMove("none");

        }


        /// <summary>
        /// Refresh Control Command Fire
        /// </summary>
        public void RenewControlCommandFire()
        {
            cc.SetFire("none");

        }
        /// <summary>
        /// set tank direction
        /// </summary>
        /// <param name="vector"></param>
        public void SetTdir(Vector2D vector)
        {
            cc.SetTurret(vector);
        }



        /// <summary>
        /// move Directions
        /// </summary>
        public void MoveUp()
        {
            cc.SetMove("up");

        }

        /// <summary>
        /// Move down
        /// </summary>
        public void MoveDown()
        {
            cc.SetMove("down");

        }


        /// <summary>
        /// Move Left
        /// </summary>
        public void MoveLeft()
        {
            cc.SetMove("left");

        }


        /// <summary>
        /// move Right
        /// </summary>
        public void MoveRight()
        {

            cc.SetMove("right");
        }



        /// <summary>
        /// Shoot the projectile
        /// </summary>
        public void ShootProjectile()
        {
            cc.SetFire("main");

        }



        /// <summary>
        /// Shoot the beam
        /// </summary>
        public void ShootBeam()
        {
            cc.SetFire("alt");
        }
    }
}
