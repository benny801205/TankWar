using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnityItem
{


    /// <summary>
    /// Wall class that uses Json. Having a starting point and and ending point 
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Wall
    {


        [JsonProperty(PropertyName = "wall")]
        private int ID;

        [JsonProperty(PropertyName = "p1")]
        private Vector2D StartPoint;

        [JsonProperty(PropertyName = "p2")]
        private Vector2D EndPoint;


        public Wall(int ID, Vector2D StartPoint, Vector2D EndPoint)
        {

            this.ID = ID;

            this.StartPoint = StartPoint;

            this.EndPoint = EndPoint;
        }





        /// <summary>
        /// Return Id of the wall
        /// </summary>
        /// <returns></returns>
        public int GetID()
        {

            return ID;
        }




        /// <summary>
        /// return the starting point of the wall
        /// </summary>
        /// <returns></returns>
        public Vector2D GetStartP()
        {

            return StartPoint;
        }




        /// <summary>
        /// Returns the ending point of the wall
        /// </summary>
        /// <returns></returns>
        public Vector2D GetEndP()
        {

            return EndPoint;
        }
    }
}
