using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Threading;

namespace UnityItem
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
        [JsonProperty(PropertyName = "loc")]
        private Vector2D location;
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
        [JsonProperty(PropertyName = "dc")]
        private bool disconnect;
        [JsonProperty]
        private bool join;
        public string block = "";

        private int TankRadius;
        private double Velocity;
        private bool isBuff;
        private int FireCounter;
        private int HitCounter;
       
        
        
        /// <summary>
        /// Tank object that passes in id and name.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public Tank(int id,string name,Vector2D loc)
        {
            this.ID = id;
            this.name = name;
            this.TankRadius =30;
            this.location = loc;
            this.bdir = new Vector2D(0, -1);
            this.tdir = new Vector2D(0, -1);
            this.score = 0;
            this.hp = 3;
            this.died = false;
            this.disconnect = false;
            this.join = false;
            this.Velocity = 2.9;
            this.IsFire = false;
            this.BeamNumber = 0;//
            this.FireTimer = 0;
            this.BuffTimer = 0;
            this.isBuff = false;
            this.RespawnTimer = 0;
            this.FireCounter = 0;
            this.HitCounter = 0;
        }


        /// <summary>
        /// gives health back to 3
        /// </summary>
        public void RecoverHp()
        {
           
            this.hp = 3;
        }


        /// <summary>
        /// add to the firecount
        /// </summary>
        public void IncreaseFireCounter()
        {

            this.FireCounter++;
        }

        /// <summary>
        /// increase hit counter
        /// </summary>
        public void IncreaseHit()
        {

            this.HitCounter++;
        }




        /// <summary>
        /// check if tank has died
        /// </summary>
        public void GetHurt()
        {
            this.hp--;
            if (this.hp == 0)
            {
                this.died = true;
            }

        }




        public int BeamNumber { get; set; }


        public double FireTimer { get; set; }


        public double BuffTimer { get; set; }


        public double RespawnTimer { get; set; }


        public bool IsFire { get; set; }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="vector"></param>
        public void setTankLocation(Vector2D vector)
        {

            this.location = vector;


        }



        /// <summary>
        /// 
        /// </summary>
        public void disconect()
        {
            disconnect = true;

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
                    
                        double rightEdge = wall.GetStartP().GetX() + 25 + TankRadius;
                        
                        double leftEdge = wall.GetEndP().GetX() - 25 - TankRadius; ;
                        
                        double downEdge = wall.GetStartP().GetY() + 25 + TankRadius;
                        
                        double upEdge = wall.GetEndP().GetY() - 25 - TankRadius;
                        
                        
                        if (this.location.GetX() < rightEdge && this.location.GetX() > leftEdge && this.location.GetY() < downEdge && this.location.GetY() > upEdge)
                        {
                        
                            return true;
                        }

                        return false;
                    }


                    else
                    {

                        double rightEdge = wall.GetStartP().GetX() + 25 + TankRadius;
                    
                        double leftEdge = wall.GetEndP().GetX() - 25 - TankRadius; ;
                        
                        double downEdge = wall.GetEndP().GetY() + 25 + TankRadius;
                        
                        double upEdge = wall.GetStartP().GetY() - 25 - TankRadius;
                        
                        
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
                      
                        double rightEdge = wall.GetStartP().GetX() + 25 + TankRadius;
                        
                        double leftEdge = wall.GetEndP().GetX() - 25 - TankRadius; ;
                        
                        double downEdge = wall.GetEndP().GetY() + 25 + TankRadius;
                        
                        double upEdge = wall.GetEndP().GetY() - 25 - TankRadius;


                        if(this.location.GetX()<rightEdge && this.location.GetX()>leftEdge&&this.location.GetY()<downEdge && this.location.GetY()>upEdge)
                        {
                       
                            return true;
                        }


                        return false;
                    }
                    
                    else
                    {
                     
                        double rightEdge = wall.GetEndP().GetX() + 25 + TankRadius;
                        
                        double leftEdge = wall.GetStartP().GetX() - 25 - TankRadius; ;
                        
                        double downEdge = wall.GetEndP().GetY() + 25 + TankRadius;
                        
                        double upEdge = wall.GetEndP().GetY() - 25 - TankRadius;


                        if (this.location.GetX() < rightEdge && this.location.GetX() > leftEdge && this.location.GetY() < downEdge && this.location.GetY() > upEdge)
                        {
                          
                            return true;
                        }

                        return false;
                    }
                }
            }


            else if (typeof(Projectile).IsInstanceOfType(obj))
            {
               
                Projectile proj = obj as Projectile;
               
                
                if((proj.GetLocation() - this.location).Length() < TankRadius)
                {
                 
                    return true;
                }

                return false;
            }
            
            
            else if (typeof(Powerup).IsInstanceOfType(obj))
            {
              
                Powerup powerup = obj as Powerup;
               
                
                if ((powerup.GetLocation() - this.location).Length() < TankRadius+10)
                {
                 
                    return true;
                }


                return false;
            }
           
            
            else if (typeof(Beam).IsInstanceOfType(obj))
            {
              
                Beam beam = obj as Beam;
               
                
                if (beam.GetOwnerID().ToString().Equals(this.ID.ToString()))
                {

                    return false;
                }


                double distance;

                Vector2D TanktoBeam = this.location - beam.GetLocation();

                double CosTheta = TanktoBeam.Dot(beam.GetDirection()) / (TanktoBeam.Length() * beam.GetDirection().Length());


                distance = Math.Sqrt((TanktoBeam.Length() * TanktoBeam.Length() * (1 - CosTheta * CosTheta)));

                return distance<this.TankRadius;
            }
           
            
            else
            {

                return false;
            }
        }




        /// <summary>
        /// Movement
        /// </summary>
        public void MoveUp()
        {

            location = location + new Vector2D(0, -1) * Velocity;
            bdir = new Vector2D(0, -1);
        }




        /// <summary>
        /// Movement
        /// </summary>
        public void MoveDown()
        {
            location = location + new Vector2D(0, 1) * Velocity;
            bdir = new Vector2D(0, 1);
        }




        /// <summary>
        /// Movement
        /// </summary>
        public void MoveLeft()
        {
            location = location + new Vector2D(-1, 0) * Velocity;
            bdir = new Vector2D(-1, 0);
        }




        /// <summary>
        /// Movement
        /// </summary>
        public void MoveRight()
        {
            location = location + new Vector2D(1, 0) * Velocity;
            bdir = new Vector2D(1, 0);
        }




        /// <summary>
        /// Return current health of tank
        /// </summary>
        /// <returns></returns>
        public int GetHp()
        {
            return hp;
        }



        public string GetAccuracy()
        {
            if (FireCounter == 0 || HitCounter == 0)
            {
                return "0.00%";
            }
            else
            {
                double a = 100*(double)HitCounter / (double)FireCounter;
                return a.ToString("0.00") + "%";

            }


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
        /// Score constructor
        /// </summary>
        public int Score
        {
            get => score;
            set => score = value;

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

            return location;
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
        /// Set the tank direction
        /// </summary>
        /// <param name="vector"></param>
        public void Settdir(Vector2D vector)
        {

            this.tdir = vector;
        }




        /// <summary>
        /// Returned if disconnected
        /// </summary>
        /// <returns></returns>
        public bool isDisconnected()
        {

            return disconnect;
        }



        /// <summary>
        /// Returned if Died
        /// </summary>
        /// <returns></returns>
        public bool isDied
        {
            get => died;
           
            set => died = value;
        }
    }



    /// <summary>
    /// 
    /// </summary>
    class LinearEquation
    {
      
        double constant;
       
        double slope;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="slope"></param>
        /// <param name="constant"></param>
        public LinearEquation(double slope, double constant)
        {
            this.constant = constant;
           
            this.slope = -slope;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="TankLocation"></param>
        /// <param name="TankRadius"></param>
        /// <returns></returns>
        public bool isHit(Vector2D TankLocation,int TankRadius)
        {

            for(int x = -600; x < 600; x++)
            {

                Vector2D BeamPoint = new Vector2D(x,slope*x+constant);
               
                if((BeamPoint-TankLocation).Length()< TankRadius)
                {

                    return true;
                }
            }

            return false;
        }
    }
}
