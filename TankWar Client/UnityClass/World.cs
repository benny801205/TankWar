using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityClass
{


    /// <summary>
    /// World Class that holds all of the objects that need to be placed into the world. 
    /// Including the tanks, power-ups, walls, beams, projectiles, and what tankmodel. 
    /// </summary>
    public class World
    {
        // In reality, these should not be public,
        // but for the purposes of this lab, the "World" 
        // class is just a wrapper around these two fields.
        private Dictionary<int, Tank> tanks;

        private Dictionary<int, Powerup> powerups;

        private Dictionary<int, Wall> walls;

        private Dictionary<int, Beam> beams;

        private Dictionary<int, Projectile> projectiles;

        private Dictionary<int, TankModel> tankmodels;

        private int WorldSize;

        private int ClientSize;

        private int MyID;

        public string block = "";

        /// <summary>
        /// world that shows all other objects to create the world 
        /// </summary>
        public World()
        {

            tankmodels = new Dictionary<int, TankModel>();

            tanks = new Dictionary<int, Tank>();

            powerups = new Dictionary<int, Powerup>();

            walls = new Dictionary<int, Wall>();

            beams = new Dictionary<int, Beam>();

            projectiles = new Dictionary<int, Projectile>();

            ClientSize = 800;
        }

        /// <summary>
        /// get Id of the tank and the player controling it
        /// </summary>
        public int MyTankID
        {

            get => MyID;

            set => MyID = value;
        }

        /// <summary>
        /// constructor of clientsize 
        /// </summary>
        public int clientsize
        {

            get => ClientSize;

            set => ClientSize = value;
        }

        /// <summary>
        /// Constructor of the world size
        /// </summary>
        public int worldsize
        {

            get => WorldSize;

            set => WorldSize = value;
        }


        /// <summary>
        /// Returns tankmodels in Dictionary.
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, TankModel> GetTankmodels()
        {

            return tankmodels;
        }


        /// <summary>
        /// Returns tanks in Dictionary.
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, Tank> GetTanks()
        {

            return tanks;
        }


        /// <summary>
        /// 
        /// Returns powerups froma a Dictionary.
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, Powerup> GetPowerups()
        {

            return powerups;
        }


        /// <summary>
        /// 
        /// Returns walls from a Dictionary.
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, Wall> GetWalls()
        {

            return walls;
        }


        /// <summary>
        /// return the beams list
        /// </summary>
        /// <returns></returns>
        public List<Beam> GetBeamsList()
        {

            return new List<Beam>(beams.Values);
        }
        /// <summary>
        /// Returns beams from a Dictionary.
        /// </summary>
        /// <returns></returns>
        public Dictionary<int,Beam> GetBeams()
        {
            return beams;
        }

        /// <summary>
        /// Returns projectiles from a Dictionary.
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, Projectile> GetProjectiles()
        {

            return projectiles;
        }

 
    }
}
