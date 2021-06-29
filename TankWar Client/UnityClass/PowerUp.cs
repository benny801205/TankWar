using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnityClass
{


    /// <summary>
    /// PowerUp Class that has ID, Location, And direction. The will know when the player has died to reset. 
    /// </summary>
    public class Powerup
    {
        [JsonProperty(PropertyName = "power")]
        private int ID;
        [JsonProperty(PropertyName = "loc")]
        private Vector2D location;
        [JsonProperty(PropertyName = "dir")]
        private Vector2D direction;
        [JsonProperty]
        private bool died;
        [JsonProperty(PropertyName = "owner")]
        private int OwnerID;



        /// <summary>
        /// Get the location in the Vector2d world
        /// </summary>
        /// <returns></returns>
        public Vector2D GetLocation()
        {

            return location;
        }

        /// <summary>
        /// return Id
        /// </summary>
        /// <returns></returns>
        public int GetID()
        {
            return ID;
        }

        /// <summary>
        /// Return if the tank has died.
        /// </summary>
        /// <returns></returns>
        public bool isDied()
        {
            return died;
        }

    }
}
