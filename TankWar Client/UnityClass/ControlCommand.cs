using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnityClass
{
    /// <summary>
    /// Json..  Gives move, turret direction, and firing of tank
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ControlCommand
    {
        [JsonProperty]
        private string moving;
        [JsonProperty]
        private string fire;
        [JsonProperty]
        private Vector2D tdir;

        public string block = "";


        /// <summary>
        /// Gives the location of the mouse
        /// </summary>
        /// <param name="MouseLocation"></param>
        public ControlCommand(Vector2D MouseLocation)
        {
            this.moving = "none";
            this.fire = "none";
            //location of mouse
            this.tdir = MouseLocation;

        }

        /// <summary>
        /// Set the direction of tank to the moving direction
        /// </summary>
        /// <param name="dir"></param>
        public void SetMove(string dir)
        {
            this.moving = dir;
        }


        /// <summary>
        /// Set the fire as fired
        /// </summary>
        /// <param name="fire"></param>
        public void SetFire(string fire)
        {
            this.fire = fire;
        }



        /// <summary>
        /// Set the turret direction from the Vector2d class
        /// </summary>
        /// <param name="vector"></param>
        public void SetTurret(Vector2D vector)
        {
            this.tdir = vector;
        }


    }
}
