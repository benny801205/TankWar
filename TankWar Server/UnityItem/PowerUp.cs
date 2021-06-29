using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnityItem
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
       // private int respawntime = 1650;
        private int ProjectileRadius;

        private double respawntimer;


        /// <summary>
        /// powerup constructor
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="location"></param>
        public Powerup(int ID,Vector2D location)
        {
            this.ID = ID;
           
            this.location=location;
            
            this.direction = new Vector2D(0, -1);
            
            ProjectileRadius = 0;
            
            this.respawntimer = 0;
        }




        /// <summary>
        /// timer for respawn no reff
        /// </summary>
        public double RespawnTimer
        {

            get => respawntimer;
            
            set => respawntimer = value;
        }




        /// <summary>
        /// Get the location in the Vector2d world
        /// </summary>
        /// <returns></returns>
        public Vector2D GetLocation()
        {

            return location;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool isCollected(Object obj)
        {
            if (typeof(Wall).IsInstanceOfType(obj))
            {

                Wall wall = obj as Wall;

                if (wall.GetStartP().GetX().ToString().Equals(wall.GetEndP().GetX().ToString()))
                {
                    //the wall is vertical

                    if (wall.GetStartP().GetY() > wall.GetEndP().GetY())
                    {

                        double rightEdge = wall.GetStartP().GetX() + 25 + ProjectileRadius;

                        double leftEdge = wall.GetEndP().GetX() - 25 - ProjectileRadius; ;

                        double downEdge = wall.GetStartP().GetY() + 25 + ProjectileRadius;

                        double upEdge = wall.GetEndP().GetY() - 25 - ProjectileRadius;


                        if (this.location.GetX() < rightEdge && this.location.GetX() > leftEdge && this.location.GetY() < downEdge && this.location.GetY() > upEdge)
                        {

                            return true;
                        }

                        return false;
                    }


                    else
                    {

                        double rightEdge = wall.GetStartP().GetX() + 25 + ProjectileRadius;

                        double leftEdge = wall.GetEndP().GetX() - 25 - ProjectileRadius; ;

                        double downEdge = wall.GetEndP().GetY() + 25 + ProjectileRadius;

                        double upEdge = wall.GetStartP().GetY() - 25 - ProjectileRadius;


                        if (this.location.GetX() < rightEdge && this.location.GetX() > leftEdge && this.location.GetY() < downEdge && this.location.GetY() > upEdge)
                        {
                            return true;
                        }


                        return false;
                    }
                }



                else
                {//the wall is horizontal

                    if (wall.GetStartP().GetX() > wall.GetEndP().GetX())
                    {

                        double rightEdge = wall.GetStartP().GetX() + 25 + ProjectileRadius;

                        double leftEdge = wall.GetEndP().GetX() - 25 - ProjectileRadius; ;

                        double downEdge = wall.GetEndP().GetY() + 25 + ProjectileRadius;

                        double upEdge = wall.GetEndP().GetY() - 25 - ProjectileRadius;


                        if (this.location.GetX() < rightEdge && this.location.GetX() > leftEdge && this.location.GetY() < downEdge && this.location.GetY() > upEdge)
                        {

                            return true;
                        }


                        return false;
                    }


                    else
                    {

                        double rightEdge = wall.GetEndP().GetX() + 25 + ProjectileRadius;

                        double leftEdge = wall.GetStartP().GetX() - 25 - ProjectileRadius; ;

                        double downEdge = wall.GetEndP().GetY() + 25 + ProjectileRadius;

                        double upEdge = wall.GetEndP().GetY() - 25 - ProjectileRadius;


                        if (this.location.GetX() < rightEdge && this.location.GetX() > leftEdge && this.location.GetY() < downEdge && this.location.GetY() > upEdge)
                        {

                            return true;
                        }


                        return false;
                    }
                }
            }


            else
            {
               
                return false;
            }
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
        public bool isDied
        {

            get => died;
           
            set => died = value;
        }
    }
}
