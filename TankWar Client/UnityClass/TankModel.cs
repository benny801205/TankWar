///this class is for storing the Image for Tank
///

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityClass;




namespace UnityClass
{

    /// <summary>
    /// Class of the tank color then addes the top and bottom of each tank to make the correct color combination.
    /// </summary>
    public class TankModel
    {
        private string color;
        private object bottom;
        private object turret;
        private object projectile;


        /// <summary>
        /// Used to tell what type of tank model is being used
        /// </summary>
        /// <param name="color"></param>
        /// <param name="bottom"></param>
        /// <param name="turret"></param>
        public TankModel(string color,object bottom,object turret)
        {
            this.color = color;
            this.bottom = bottom;
            this.turret = turret;

        }


        /// <summary>
        /// returns the bottom of the set for the tank
        /// </summary>
        /// <returns></returns>
        public object getBottom()
        {
            return bottom;
        }


        /// <summary>
        /// Returns the projectile related to each tank
        /// 
        /// Used the same projectile for each tank without using different colors.
        /// </summary>
        /// <returns></returns>
        public object getProjectile()
        {
            return projectile;
        }



        /// <summary>
        /// Returns the top pair of the tank with matching color
        /// </summary>
        /// <returns></returns>
        public object getTurret()
        {
            return turret;
        }






    }
}
