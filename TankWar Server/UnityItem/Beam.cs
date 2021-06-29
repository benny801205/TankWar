using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnityItem
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
        /// beam constructor
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="origin"></param>
        /// <param name="direction"></param>
        /// <param name="OwnerID"></param>
        public Beam(int ID,Vector2D origin,Vector2D direction,int OwnerID)
        {

            this.ID = ID;
            
            this.origin = origin;
            
            this.direction = direction;
            
            this.OwnerID = OwnerID;
        }




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



        /// <summary>
        /// return direction
        /// </summary>
        /// <returns></returns>
        public Vector2D GetDirection()
        {
           
            return direction;
        }

        /// <summary>
        /// return owner Id of beam
        /// </summary>
        /// <returns></returns>
        public int GetOwnerID()
        {
          
            return OwnerID;
        }
    }
}
