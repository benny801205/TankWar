using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnityItem
{

    /// <summary>
    /// Projectile class using the Id, Location,and Direction of the projectile space.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Projectile
    {
        [JsonProperty(PropertyName = "proj")]
        private int ID;
        [JsonProperty(PropertyName = "loc")]
        private Vector2D location;
        [JsonProperty(PropertyName = "dir")]
        private Vector2D direction;
        [JsonProperty]
        private bool died;
        [JsonProperty(PropertyName = "owner")]
        private int OwnerID;

        private int ProjectileRadius;

        private double speed;

        public Projectile(int ID,Vector2D location,Vector2D direction,int OwnerID)
        {
            this.speed = 25;

            ProjectileRadius = 0;
            this.ID = ID;
            this.location = location;
            this.direction = direction;
            this.OwnerID = OwnerID;
            this.died = false;
        }

     



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
                {
                    //the wall is horizontal

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
        /// Movement in the frame
        /// </summary>
        public void MoveForEachFrame()
        {
            
            Vector2D vector =  direction*speed;
           
            location = location + vector;
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
        /// return if the tank has died
        /// </summary>
        /// <returns></returns>
        public bool isDied
        {
            
            get=> died;
           
            set => died = value;
        }




        /// <summary>
        /// return the location 
        /// </summary>
        /// <returns></returns>
        public Vector2D GetLocation()
        {
            return location;
        }



        /// <summary>
        /// return owner Id
        /// </summary>
        /// <returns></returns>
        public int GetOwnerID()
        {

            return OwnerID;
        }




        /// <summary>
        /// return the direction 
        /// </summary>
        /// <returns></returns>
        public Vector2D GetDirection()
        {
           
            return direction;
        }
    }
}
