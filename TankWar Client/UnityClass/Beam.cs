using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnityClass
{

    /// <summary>
    /// Beam class to used for the tank using the Id orgin and driection
    /// </summary>
    public class Beam
    {
        [JsonProperty(PropertyName = "beam")]
        private int ID;
        [JsonProperty(PropertyName = "org")]
        private Vector2D origin;
        [JsonProperty(PropertyName = "dir")]
        private Vector2D direction;
        [JsonProperty(PropertyName = "owner")]
        private int OwnerID;

        /// <summary>
        /// Return the Id of the beams tank
        /// </summary>
        /// <returns></returns>
        public int GetID()
        {
            return ID;
        }
        /// <summary>
        /// get location
        /// </summary>
        /// <returns></returns>
        public Vector2D GetLocation()
        {
            return origin;
        }
        //get direction
        public Vector2D GetDirection()
        {
            return direction;
        }

    }




}
