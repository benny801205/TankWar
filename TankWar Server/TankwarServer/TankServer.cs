/// <summary>
/// Implemented by Ping Cheng,Chung and Michael Meadows for Cs 3500 Networking.cs
/// Date: 12/5/19 
/// Class: CS3500
/// 
/// 
/// Tank war server to connect to clients for the game. 
/// </summary>



using System;
using System.Text;
using System.Collections.Generic;
using NetworkUtil;
using System.Text.RegularExpressions;
using UnityItem;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Threading;

namespace TankwarServer
{

    /// <summary>
    /// A simple server for receiving simple text messages from multiple clients
    /// </summary>
    class TankServer
    {
        // A set of clients that are connected.
        private Dictionary<long, SocketState> clients;
        private Dictionary<long, int> SocketID_TankID;
        private TankWarBrain TB;
        
        private readonly int frame = 50;


        /// <summary>
        /// Main to create a new tankserver called server then start
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            
            TankServer server = new TankServer();
            WebServer webserver = new WebServer();
            webserver.StartServer();
            server.StartServer();
            Console.Read();
        }


        /// <summary>
        /// client socket state and tb
        /// </summary>
        public TankServer()
        {

            clients = new Dictionary<long, SocketState>();

            SocketID_TankID = new Dictionary<long, int>();

            TB = new TankWarBrain();

        }




        /// <summary>
        /// Start accepting Tcp sockets connections from clients
        /// </summary>
        public void StartServer()
        {
            // This begins an "event loop"
            Networking.StartServer(NewClientConnected, 11000);
           
            Console.WriteLine("Server is running");
            //update timer


            while (true)
            {
             
                Stopwatch stopWatch = new Stopwatch();
             
                stopWatch.Start();

                while (stopWatch.ElapsedMilliseconds < 1000 / frame)
                { 

                    Thread.Sleep(1); 
                }


                TB.MoveProjectiles();
               
                TB.ProcessCommands();
               
                TB.DetectCollision();

                lock (TB.GetWorld().GetTanks())
                {
                    ClientWorldUpdate();

                    TB.CheckStatus();
                }
            }
        }


        /// <summary>
        /// Connect to a new client
        /// </summary>
        /// <param name="state"></param>
        private void NewClientConnected(SocketState state)
        {

            if (state.ErrorOccured)
                return;

            // Save the client state
            // Need to lock here because clients can disconnect at any time

            state.OnNetworkAction = ReceivePlayerName;
           
            Console.WriteLine("get a new client");
           
            Networking.GetData(state);
        }



        /// <summary>
        /// sends a message
        /// </summary>
        /// <param name="state"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        private bool SendMSG(SocketState state, string str)
        {

            return Networking.Send(state.TheSocket, str);
        }



        /// <summary>
        /// Client state then get data from the socket state
        /// </summary>
        /// <param name="state"></param>
        private void ReceivePlayerName(SocketState state)
        {

            if (state.ErrorOccured)
            {

                return;
            }

            int TankID=GenerateTankGetID(state);

            TB.PlayerCounter++;

            SendMSG(state, TankID + "\n" + TB.GetWorld().GetWorldSize());

            SendWallInfo(state);
           
            // Save the client state
            // Need to lock here because clients can disconnect at any time
            lock (clients)
            {

                clients[state.ID] = state;
            }

            state.OnNetworkAction = ReceiveCommand;
           
            Networking.GetData(state);
        }




        /// <summary>
        /// Wall information to send to client from server
        /// </summary>
        /// <param name="state"></param>
        private void SendWallInfo(SocketState state)
        {
            World world = TB.GetWorld();

            StringBuilder WallsInformation = new StringBuilder();

            lock (world.GetWalls())
            {

                foreach (Wall w in world.GetWalls().Values)
                {

                    WallsInformation.Append(JsonConvert.SerializeObject(w) + "\n");

                }
            }

            SendMSG(state, WallsInformation.ToString());
        }



        /// <summary>
        /// Update client world 
        /// </summary>
        private void ClientWorldUpdate()
        {
           
            World world = TB.GetWorld();
           
            HashSet<long> disconnectedClients = new HashSet<long>();

            StringBuilder fullWorldMessage = new StringBuilder();

            //check for all tanks
            lock (world.GetTanks())
            {

                foreach (Tank tank in world.GetTanks().Values)
                {
                    fullWorldMessage.Append(JsonConvert.SerializeObject(tank) + "\n");
                }

                //remove the disconnected tank from tank list
                TB.RemoveDisconnectdTank();
            }


            lock (world.GetProjectiles())
            {

                foreach (Projectile pj in world.GetProjectiles().Values)
                {

                    fullWorldMessage.Append(JsonConvert.SerializeObject(pj) + "\n");
                }
            }


            lock (world.GetBeams())
            {

                foreach (Beam beam in world.GetBeams().Values)
                {

                    fullWorldMessage.Append(JsonConvert.SerializeObject(beam) + "\n");
                }
            }


            lock (world.GetPowerups())
            {

                foreach (Powerup powerup in world.GetPowerups().Values)
                {

                    fullWorldMessage.Append(JsonConvert.SerializeObject(powerup) + "\n");
                }
            }


            lock (clients)
            {
                foreach (SocketState ss in clients.Values)
                {

                    if (!SendMSG(ss, fullWorldMessage.ToString()))
                    {
                        disconnectedClients.Add(ss.ID);
                    }
                }
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private int GenerateTankGetID(SocketState state)
        {
            string totalData = state.GetData();

            string[] parts = Regex.Split(totalData, @"(?<=[\n])");
           
            Console.WriteLine("received Name from client " + state.ID + ": \"" + parts[0] + "\"");
           
            // Remove it from the SocketState's growable buffer
            state.RemoveData(0, totalData.Length);

            int TankID = TB.SerachTankID(parts[0]);

            SocketID_TankID.Add(state.ID,TankID);

            TB.GenerateNewTank(TankID, parts[0]);

            return TankID;

        }



        /// <summary>
        /// Recieve a command if there is no errors
        /// </summary>
        /// <param name="state"></param>
        private void ReceiveCommand(SocketState state)
        {

            if (state.ErrorOccured)
            {

                RemoveClient(state.ID);

                return;
            }

            ProcessCommand(state);

            // Continue the event loop that receives messages from this client
            Networking.GetData(state);
        }


        /// <summary>
        /// Given the data that has arrived so far, 
        /// potentially from multiple receive operations, 
        /// determine if we have enough to make a complete message,
        /// and process it (print it and broadcast it to other clients).
        /// </summary>
        /// <param name="sender">The SocketState that represents the client</param>
        private void ProcessCommand(SocketState state)
        {

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

                 HandleJson(state,p);
                

                // Remove it from the SocketState's growable buffer
                state.RemoveData(0, p.Length);



                /////////finsih reveive data

                // Broadcast the message to all clients
                // Lock here beccause we can't have new connections 
                // adding while looping through the clients list.
                // We also need to remove any disconnected clients.
             
            }
        }



        /// <summary>
        /// json file handling 
        /// </summary>
        /// <param name="state"></param>
        /// <param name="str"></param>
        private void HandleJson(SocketState state,string str)
        {

            JObject obj = JObject.Parse(str);

            int TankID;

            if (obj["moving"] != null)
            {

                ControlCommand rebuilt = JsonConvert.DeserializeObject<ControlCommand>(str);

                lock (TB.GetWorld().GetCommands())
                {

                    lock (SocketID_TankID)
                    {

                        TankID = SocketID_TankID[state.ID];
                    }


                    TB.GetWorld().GetCommands()[TankID] = rebuilt;
                }
            }
        }


        /// <summary>
        /// Update the database then remove the clients 
        /// </summary>
        /// <param name="SocketID"></param>
        private void RemoveClient(long SocketID)
        {
            Console.WriteLine("Client " + SocketID + " disconnected");

            lock (clients)
            {
                lock (SocketID_TankID)
                {

                    if (SocketID_TankID.ContainsKey(SocketID))
                    {
                        TB.UpdateDatabase(SocketID_TankID[SocketID]);

                        clients.Remove(SocketID);

                        TB.playerDisconnect(SocketID_TankID[SocketID]);

                        SocketID_TankID.Remove(SocketID);
                    }


                    else
                    {

                        clients.Remove(SocketID);
                    }
                }
            }
        }
    }
}

