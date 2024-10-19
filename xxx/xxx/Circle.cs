using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace xxx
{
    class Circle
    {
        public Vector2 center = Vector2.Zero; // The center of the Circle
        public float radius; // The radius of the Circle

        /// <summary>
        /// constructor which initiallize those paramaters.
        /// </summary>
        /// <param name="center">The center of the circle</param>
        /// <param name="tex">The texture of the car</param>
        /// <param name="scale">The scale of the car</param>
        /// <param name="org">The origin of the car</param>
        /// <param name="ifBig">Set if this is the Big Circle or regular Circle</param>
        public Circle(Vector2 center, Rectangle rec, Vector2 scale, bool ifBig)
        {
            this.center.X = center.X * scale.X;
            this.center.Y = center.Y * scale.Y;
            this.radius = find_radius(rec, scale, ifBig);
        }

        /// <summary>
        /// Find the center center of the car
        /// </summary>
        /// <param name="tex">The texture of the car</param>
        /// <returns>The center vector of the car</returns>
        public static Vector2 find_center(Rectangle rec)
        {
            Vector2 center;
            center = new Vector2((rec.Width) / 2, (rec.Height) / 2);

            return center;
        }

        /// <summary>
        /// Find the quarter center of the car
        /// </summary>
        /// <param name="tex">The texture of the car</param>
        /// <returns>The quarter vector of the car</returns>
        public static Vector2 find_reva(Rectangle rec)
        {
            Vector2 center;
            center = new Vector2((rec.Width) / 4, (rec.Height) / 2);

            return center;
        }

        /// <summary>
        /// Find the three quarter center of the car
        /// </summary>
        /// <param name="tex">The texture of the car</param>
        /// <returns>The three quarter vector of the car</returns>
        public static Vector2 find_shloshtreva(Rectangle rec)
        {
            Vector2 center;
            center = new Vector2(((rec.Width / 4) * 3), (rec.Height / 2));

            return center;
        }

        /// <summary>
        /// This function calculate the radius of the current circle.
        /// </summary>
        /// <param name="tex">The texture of the car</param>
        /// <param name="scale">The scale of the car</param>
        /// <param name="big">Set if we need the radius of the big circle or regular circle</param>
        /// <returns></returns>
        public float find_radius(Rectangle rec, Vector2 scale, bool big)
        {
            float w = (rec.Width) * scale.X;
            float h = (rec.Height) * scale.Y;

            if (!big)
            {
                float min1 = Math.Min(w - center.X, h - center.Y); // the smaller between height and width
                float min2 = Math.Min(min1, 0 + center.X);
                float min3 = Math.Min(min2, 0 + center.Y);

                return min3;
            }
            else
            {
                if (w - center.X > h - center.Y)
                {
                    return (w - center.X);
                }
                else
                {
                    return h - center.Y;
                }
            }
        }
    }
}