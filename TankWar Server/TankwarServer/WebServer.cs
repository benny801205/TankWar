/// <summary>
/// Implemented by Ping Cheng,Chung and Michael Meadows for Cs 3500 Networking.cs
/// Date: 12/5/19 
/// Class: CS3500
/// 
/// 
/// Tank war server to connect to clients for the game. 
/// </summary>

using MySql.Data.MySqlClient;
using NetworkUtil;
using System;
using System.Collections.Generic;
using TankWars;

namespace TankwarServer
{
    public class WebServer
    {
        private const string connectionString = "server=atr.eng.utah.edu;" +
    "database=cs3500_u0954730;" +
    "uid=cs3500_u0954730;" +
    "password=u0954730";


        public WebServer()
        {



        }


        /// <summary>
        /// 
        /// </summary>
        public void StartServer()
        {
            // This begins an "event loop"
            Networking.StartServer(HandleHttpConection, 80);

            Console.WriteLine("WebServer is running");
            //update timer
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        private void HandleHttpConection(SocketState state)
        {
            state.OnNetworkAction = ServerHttpRequest;
            Networking.GetData(state);
        }

        private void ServerHttpRequest(SocketState state)
        {
            List<Record> RecordList = new List<Record>();
            string request = state.GetData();
            // Console.WriteLine(request);

            if (request.Contains("GET /games?player="))
            {

                string[] array = request.Split('\n');

                array = array[0].Split('=');

                array = array[1].Split(' ');

                string name = array[0];

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    try
                    {
                        // Open a connection
                        conn.Open();

                        // Create a command
                        MySqlCommand command = conn.CreateCommand();

                        command.CommandText = "select Date, Score, PlayerName,Accuracy from PlayerPerformance join Players on PlayerPerformance.PlayerID = Players.PlayerID AND PlayerName=\"" + name + "\"";

                        // Execute the command and cycle through the DataReader object
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                RecordList.Add(new Record(reader["PlayerName"].ToString(), reader["Date"].ToString(), reader["Score"].ToString(), reader["Accuracy"].ToString()));
                            }
                        }
                    }

                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }







                Networking.Send(state.TheSocket, WebViews.GetRecord(RecordList, name));
                state.TheSocket.Close();


            }



            else if (request.Contains("GET /game"))
            {


                //get data from DB
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    try
                    {
                        // Open a connection
                        conn.Open();

                        // Create a command
                        MySqlCommand command = conn.CreateCommand();
                        //command.CommandText = "INSERT INTO Players(PlayerName) VALUES('Audi')";
                        //            command.ExecuteNonQuery();

                        command.CommandText = "select Date, Score, PlayerName,Accuracy from PlayerPerformance join Players on PlayerPerformance.PlayerID = Players.PlayerID";

                        // Execute the command and cycle through the DataReader object
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                RecordList.Add(new Record(reader["PlayerName"].ToString(), reader["Date"].ToString(), reader["Score"].ToString(), reader["Accuracy"].ToString()));
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }




                Networking.Send(state.TheSocket, WebViews.GetRecord(RecordList, "PS9"));
                state.TheSocket.Close();
            }


            else
            {

                Networking.Send(state.TheSocket, WebViews.GetHomePage(5));
                state.TheSocket.Close();


            }




        }






    }
}