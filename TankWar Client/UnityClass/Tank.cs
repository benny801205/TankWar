using Newtonsoft.Json;
using System;

namespace UnityClass
{
    /// <summary>
    /// Tank classfor drawing tanks to be used in the world. 
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Tank
    {
        [JsonProperty(PropertyName ="tank")]
        private int ID;
        [JsonProperty]
        private string name;
        [JsonProperty]
        private Vector2D loc;
        [JsonProperty]
        private Vector2D bdir;
        [JsonProperty]
        private Vector2D tdir;
        [JsonProperty]
        private int score;
        [JsonProperty]
        private int hp;
        [JsonProperty]
        private bool died;
        [JsonProperty]
        private bool dc;
        [JsonProperty]
        private bool join;

        private TankModel TM;
        public string block = "";



        /// <summary>
        /// Tank object that passes in id and name.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public Tank(int id,string name)
        {
            this.ID = id;
            this.name = name;

        }


        /// <summary>
        /// Return current health of tank
        /// </summary>
        /// <returns></returns>
        public int GetHp()
        {
            return hp;
        }


        /// <summary>
        /// Return the score (players defeted) to be shown.
        /// </summary>
        /// <returns></returns>
        public int GetScore()
        {
            return score;
        }

        /// <summary>
        /// Return the direction of the tanks turret. used to file projectiles.
        /// </summary>
        /// <returns></returns>
        public Vector2D GetTdir()
        {
            return tdir;
        }


        /// <summary>
        /// Return the Id of the tank
        /// </summary>
        /// <returns></returns>
        public int GetID()
        {
            return ID;
        }

        /// <summary>
        /// Return the name of the player of the tank. 
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return name;
        }


        /// <summary>
        /// Return the location of the tank on the world plane.
        /// </summary>
        /// <returns></returns>
        public Vector2D GetLocation()
        {

            return loc;
         }

        /// <summary>
        /// Return the orientation of the tank on the world plane
        /// </summary>
        /// <returns></returns>
        public Vector2D GetOrientation()
        {

            return bdir;
        }

        /// <summary>
        /// Returned if disconnected
        /// </summary>
        /// <returns></returns>
        public bool isDisconnected()
        {

            return dc;
        }



        /// <summary>
        /// Returned if Died
        /// </summary>
        /// <returns></returns>
        public bool isDied()
        {

            return died;
        }


        /// <summary>
        /// Set the color of the tank and the pair of images used together
        /// </summary>
        /// <param name="o"></param>
        public void setTankModel(TankModel o)
        {

            TM = o;
        }



        /// <summary>
        /// Get the image of the bottom half of the tank image
        /// </summary>
        /// <returns></returns>
        public object getBottomImag()
        {

            return TM.getBottom();
        }


        /// <summary>
        /// Get the turret image of the tank.
        /// </summary>
        /// <returns></returns>
        public object getTurretImag()
        {

            return TM.getTurret();
        }
    }
}
