using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnityClass
{

    /// <summary>
    /// Projectile class using the Id, Location,and Direction of the projectile space.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Projectile
    {
        [JsonProperty(PropertyName = "proj")]
        private int ID;
        [JsonProperty]
        private Vector2D loc;
        [JsonProperty]
        private Vector2D dir;
        [JsonProperty]
        private bool died;
        [JsonProperty(PropertyName = "owner")]
        private int OwnerID;


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetID()
        {
            return ID;
        }

        /// <summary>
        /// return if the tank has died
        /// </summary>
        /// <returns></returns>
        public bool isDied()
        {
            return died;
        }


        /// <summary>
        /// return the location 
        /// </summary>
        /// <returns></returns>
        public Vector2D GetLocation()
        {
            return loc;
        }


        /// <summary>
        /// return the direction 
        /// </summary>
        /// <returns></returns>
        public Vector2D GetDirection()
        {
            return dir;
        }

    }
}
