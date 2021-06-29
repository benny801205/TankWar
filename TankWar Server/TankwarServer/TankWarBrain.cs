/// <summary>
/// Implemented by Ping Cheng,Chung and Michael Meadows for Cs 3500 Networking.cs
/// Date: 12/5/19 
/// Class: CS3500
/// 
/// 
/// Tank war server logic for world, timers, and random. 
/// </summary>




using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityItem;

namespace TankwarServer
{
    /// <summary>
    /// Tank war fields 
    /// the logic
    /// </summary>
    class TankWarBrain
    {
      
        private int projectileCounter;
        
        private int beamCounter;
        
        private World world;
        
        private int playerCounter;
        
        private Random random;
        
        private double UpdateTimer;
        
        private int round;
        
        private int PowerupCounter;
        
        private double SpawnPowerupTimer;
        
        private int MaximumPowerup = 2;
        
        private readonly double TankRespawnTime;

        private const string connectionString = "server=atr.eng.utah.edu;" +
        "database=cs3500_u0954730;" +
        "uid=cs3500_u0954730;" +
        "password=u0954730";




        /// <summary>
        /// Construction of TankWar 
        /// </summary>
        public TankWarBrain()
        {
           
            world = new World(1200);
            
            TankRespawnTime = 250;
            
            playerCounter = 0;
            
            projectileCounter = 0;
            
            beamCounter = 0;
            
            random = new Random();
            
            this.UpdateTimer = 0;
            
            this.round = 1;
            
            this.PowerupCounter = 0;
            
            this.SpawnPowerupTimer = 0;
            
            InitializeWorld();
        }




        /// <summary>
        /// Create world
        /// </summary>
        public void InitializeWorld()
        {

            
            world.GetWalls().Add(1, new Wall(1, new Vector2D(300, 200), new Vector2D(300, 400)));
            
            world.GetWalls().Add(2, new Wall(2, new Vector2D(-200, 100), new Vector2D(200, 100)));
            
            world.GetWalls().Add(3, new Wall(3, new Vector2D(50, -500), new Vector2D(-500, -500)));
            
            world.GetWalls().Add(4, new Wall(4, new Vector2D(-600, -600), new Vector2D(600, -600)));
            
            world.GetWalls().Add(5, new Wall(5, new Vector2D(-600, 600), new Vector2D(-600, -600)));
            
            world.GetWalls().Add(6, new Wall(6, new Vector2D(600, -600), new Vector2D(600, 600)));
            
            world.GetWalls().Add(7, new Wall(7, new Vector2D(-600, 600), new Vector2D(600, 600)));
            
            GenerateNewPowerup();
            
            GenerateNewPowerup();
            
            GenerateNewPowerup();
        }




        /// <summary>
        /// player counter const.
        /// </summary>
        public int PlayerCounter
        {

            get => playerCounter;

            set => playerCounter = value;
        }




        /// <summary>
        /// Return world const.
        /// </summary>
        /// <returns></returns>
        public World GetWorld()
        {

            return world;
        }




        /// <summary>
        /// Get the tank ID  from the playerID in the Database
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int SerachTankID(string name)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {

                try
                {
                    conn.Open();
                    
                    MySqlCommand command = conn.CreateCommand();
                   
                    command.CommandText = "select PlayerID from Players where PlayerName=\"" + name + "\"";
                    
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {

                        if (reader.Read())
                        {

                            return (int)reader["PlayerID"];
                        }


                        else
                        {

                            reader.Close();

                            command.CommandText = "INSERT INTO Players(PlayerName) VALUES('" + name + "')";

                            command.ExecuteNonQuery();
                            
                            return SerachTankID(name);
                        }
                    }
                }


                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    
                    return 404;
                }
            }
        }



        /// <summary>
        /// Status of item used TankServer
        /// </summary>
        public void CheckStatus()
        {

            RemoveDiedItem();
           
            UpdateTimerPlus();
           
            SpawnPowerup();
            
            RespawnTank();
        }



        /// <summary>
        /// Update Sql database to add stats of players and new players
        /// </summary>
        /// <param name="TankID"></param>
        public void UpdateDatabase(int TankID)
        {
            int score;

            string accuracy;

            lock (world.GetTanks())
            {

                score=world.GetTanks()[TankID].GetScore();

                accuracy= world.GetTanks()[TankID].GetAccuracy();
            }



            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {

                try
                {
                    // Open a connection
                    conn.Open();

                    MySqlCommand command = conn.CreateCommand();

                    command.CommandText = "INSERT INTO PlayerPerformance(Date,PlayerID,Score,Accuracy) VALUES(now(),"+ TankID +","+score+",'"+accuracy+ "')";
                   
                    command.ExecuteNonQuery();
                }

                
                catch (Exception e)
                {

                    Console.WriteLine(e.Message);
                }
            }
        }



        /// <summary>
        /// Resoawn Tank with HP timer till respawn
        /// </summary>
        public void RespawnTank()
        {

            lock (world.GetTanks())
            {

                foreach (Tank tank in world.GetTanks().Values)
                {

                    if (tank.GetHp() == 0)
                    {

                        if (UpdateTimer - tank.RespawnTimer > TankRespawnTime)
                        {

                            GenerateNewTank(tank.GetID(), null);

                            tank.RecoverHp();

                            tank.BeamNumber = 0;
                        }
                    }
                }
            }
        }




        /// <summary>
        /// generate powerup in random place on world
        /// </summary>
        public void SpawnPowerup()
        {

            int variable = random.Next(-200, 200);

            if (Math.Abs(UpdateTimer -SpawnPowerupTimer) > 1650 + variable)
            {

                GenerateNewPowerup();
            }
        }



        /// <summary>
        /// Generate new power ups 
        /// </summary>
        public void GenerateNewPowerup()
        {

            lock (world.GetPowerups())
            {

                if (world.GetPowerups().Count >= MaximumPowerup)
                {

                    return;
                }


                bool isCollectwithWall = true;


                while (isCollectwithWall)
                {

                    world.GetPowerups()[PowerupCounter] = new Powerup(PowerupCounter, new Vector2D(random.Next(-500, 500), random.Next(-500, 500)));
                    
                    isCollectwithWall = PowerupisColledwithWall(world.GetPowerups()[PowerupCounter]);
                }
            }


            PowerupCounter++;
            
            SpawnPowerupTimer = UpdateTimer;
        }




        /// <summary>
        /// Generate a new tank.
        /// </summary>
        public void GenerateNewTank(int ID,string name)
        {

            lock (world.GetTanks())
            {

                Dictionary<int, Tank> dictionary = world.GetTanks();

                //use old tank
                if (world.GetTanks().ContainsKey(ID))
                {
                    Tank tank = world.GetTanks()[ID];

                    tank.setTankLocation(new Vector2D(random.Next(-500,500),random.Next(-500,500)));
                }

                //new tank if player ID is not in database
                else
                {

                    world.GetTanks()[ID] = new Tank(ID,name, new Vector2D(random.Next(-500, 500), random.Next(-500, 500)));
                }

                //check collision with walls
                if (TankisColledwithWall(world.GetTanks()[ID]))
                {

                    GenerateNewTank(ID,null);
                }
            }
        }




        /// <summary>
        /// When an item is used then it is removed from the world
        /// </summary>
        public void RemoveDiedItem()
        {
            HashSet<int> DiedProjectiles = new HashSet<int>();

            HashSet<int> DiedPowerUp = new HashSet<int>();

            //remove died projectiles
            lock (world.GetProjectiles())
            {

                foreach(Projectile pj in world.GetProjectiles().Values)
                {

                    if (pj.isDied)
                    {

                        DiedProjectiles.Add(pj.GetID());
                    }
                }


                foreach(int id in DiedProjectiles)
                {

                    world.GetProjectiles().Remove(id);
                }
            }

            //remove died powerup
            lock (world.GetPowerups())
            {

                foreach (Powerup powerup in world.GetPowerups().Values)
                {

                    if (powerup.isDied)
                    {

                        DiedPowerUp.Add(powerup.GetID());
                    }
                }


                foreach (int id in DiedPowerUp)
                {

                    world.GetPowerups().Remove(id);
                }
            }


            lock (world.GetBeams())
            {

                world.GetBeams().Clear();
            }
        }




        /// <summary>
        /// Remove the tank from wolrd when a player disconnects
        /// </summary>
        public void RemoveDisconnectdTank()
        {
            HashSet<int> DisconnectedTank = new HashSet<int>();

            lock (world.GetTanks())
            {

                foreach (Tank tank in world.GetTanks().Values)
                {

                    if (tank.isDisconnected())
                    {

                        DisconnectedTank.Add(tank.GetID());
                    }


                    if (tank.isDied)
                    {

                        tank.isDied = false;
                    }
                }


                foreach (int id in DisconnectedTank)
                {

                    world.GetTanks().Remove(id);
                }
            }
        }




        /// <summary>
        /// detect collitions for proj, beam,powerup.
        /// </summary>
        public void DetectCollision()
        {

            lock (world) {

                foreach (Tank tank in world.GetTanks().Values)
                {
                    //detect projectil
                    foreach (Projectile pj in world.GetProjectiles().Values)
                    {

                        if(tank.isCollected(pj) && tank.GetHp()!=0 && !pj.isDied&& tank.GetID()!=pj.GetOwnerID())
                        {

                            tank.GetHurt();

                            pj.isDied = true;

                            world.GetTanks()[pj.GetOwnerID()].IncreaseHit();


                            if (tank.GetHp() == 0)
                            {

                                tank.RespawnTimer = UpdateTimer;

                                world.GetTanks()[pj.GetOwnerID()].Score++;
                            }
                        }
                    }


                //detect beam
                    foreach(Beam beam in world.GetBeams().Values)
                    {

                        if (tank.isCollected(beam) && tank.GetHp() != 0 && tank.GetID() != beam.GetOwnerID())
                        {
                            tank.GetHurt();

                            world.GetTanks()[beam.GetOwnerID()].IncreaseHit();

                            if (tank.GetHp() == 0)
                            {

                                tank.RespawnTimer = UpdateTimer;

                                world.GetTanks()[beam.GetOwnerID()].Score++;
                            }
                        }
                    }


                    foreach (Powerup powerup in world.GetPowerups().Values)
                    {

                        if ((tank.isCollected(powerup) && tank.GetHp() != 0 && !powerup.isDied))
                        {

                            tank.BeamNumber++;

                            powerup.isDied = true;
                        }
                    }

                }

                //detect projectile with wall
                foreach(Projectile pj in world.GetProjectiles().Values)
                {

                    foreach(Wall w in world.GetWalls().Values)
                    {

                        if (pj.isCollected(w))
                        {

                            pj.isDied = true;
                        }
                    }
                }
            }
        }



        /// <summary>
        /// Take commands then send them accoerdingly
        /// </summary>
        public void ProcessCommands()
        {

            lock (world.GetTanks())
            {

                foreach (Tank t in world.GetTanks().Values)
                {

                    lock (world.GetCommands())
                    {

                        if (world.GetCommands().ContainsKey(t.GetID()))
                        {

                            ControlCommand cc = world.GetCommands()[t.GetID()];

                            ControlTank(cc, t);
                        }
                    }
                }
            }
        }




        /// <summary>
        /// projectiles will move when fired
        /// </summary>
        public void MoveProjectiles()
        {

            lock (world.GetProjectiles())
            {

                foreach (Projectile pj in world.GetProjectiles().Values)
                {

                    if (!pj.isDied)
                    {

                        pj.MoveForEachFrame();
                    }
                }
            }
        }


        /// <summary>
        /// Updates timer increament
        /// </summary>
        public void UpdateTimerPlus()
        {

            if (UpdateTimer > 1000000)
            {

                UpdateTimer = 0;
            }


            else
            {

                this.UpdateTimer++; 
            }
        }




        /// <summary>
        /// Check if the tank has hit the wall
        /// </summary>
        /// <param name="tank"></param>
        /// <returns></returns>
        private bool TankisColledwithWall(Tank tank)
        {

            foreach(Wall w in world.GetWalls().Values)
            {

                if (tank.isCollected(w))
                {

                    return true;
                }
            }


            return false;
        }




        /// <summary>
        /// Check if proj hit wall
        /// </summary>
        /// <param name="powerup"></param>
        /// <returns></returns>
        private bool PowerupisColledwithWall(Powerup powerup)
        {

            foreach (Wall w in world.GetWalls().Values)
            {

                if (powerup.isCollected(w))
                {

                    return true;
                }
            }


            return false;
        }




        /// <summary>
        /// check if tank hit wall
        /// </summary>
        /// <param name="tank"></param>
        /// <returns></returns>
        private bool isCollectedwithWall(Tank tank)
        {

            foreach (Wall wall in world.GetWalls().Values)
            {

                if (tank.isCollected(wall))
                {

                    return true;
                }
            }


            return false;
        }





        /// <summary>
        /// Commands to control the tank from the player to the tank
        /// </summary>
        /// <param name="cc"></param>
        /// <param name="tank"></param>
        public void ControlTank(ControlCommand cc,Tank tank)
        {

            if (tank.GetHp() == 0)
            {

                return;
            }

            
            //Tank motion
            if (cc.GetMoving().Equals("up"))
            {

                tank.MoveUp();


                if (isCollectedwithWall(tank))
                {

                    tank.MoveDown();
                }
            }


            else if (cc.GetMoving().Equals("down"))
            {

                tank.MoveDown();


                if (isCollectedwithWall(tank))
                {

                    tank.MoveUp();
                }
            }


            else if (cc.GetMoving().Equals("right"))
            {

                tank.MoveRight();


                if (isCollectedwithWall(tank))
                {

                    tank.MoveLeft();
                }
            }
            
            
            else if (cc.GetMoving().Equals("left"))
            {

                tank.MoveLeft();
            

                if (isCollectedwithWall(tank))
                {
                
                    tank.MoveRight();
                }
            }


            else
            {
                //do not move the Tank
            }

            //Fire
            if (cc.GetFire().Equals("main"))
            {
                if (Math.Abs(UpdateTimer - tank.FireTimer) > 50)
                {

                    lock (world.GetProjectiles())
                    {
                        world.GetProjectiles()[projectileCounter] = new Projectile(projectileCounter, tank.GetLocation(), tank.GetTdir(), tank.GetID());
                        
                    }
                    

                    projectileCounter++;

                    tank.IncreaseFireCounter();

                    tank.FireTimer = UpdateTimer;
                }

            }


            else if (cc.GetFire().Equals("alt"))
            {

                if (tank.BeamNumber > 0)
                {

                    lock (world.GetBeams())
                    {
                        world.GetBeams()[beamCounter] = new Beam(beamCounter, tank.GetLocation(), tank.GetTdir(),tank.GetID());
                    }


                    beamCounter++;

                    tank.IncreaseFireCounter();

                    tank.BeamNumber--;
                }
            }


            else
            {
                //do nothing
            }

            //tank direction
            tank.Settdir(cc.Gettdir());
        }



        /// <summary>
        /// Disconnect player tank
        /// </summary>
        /// <param name="ID"></param>
        public void playerDisconnect(int ID)
        {

            if (world.GetTanks().ContainsKey(ID))
            {

                world.GetTanks()[ID].disconect();
            }
        }
    }
}
