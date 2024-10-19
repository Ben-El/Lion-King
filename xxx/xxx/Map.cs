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
    enum GroundType { Platform, Air, Blocked, Can_Hang, Swing }

    static class Map
    {
        static Color[] col;
        public static GroundType[,] Locations;
        public static List<Vector2> HangLocations;
        public static List<Vector2> SwingLocations;
        public static Texture2D tex { get; private set; }

        /// <summary>
        /// Processing the map's texture data
        /// </summary>
        /// <param name="tex"></param>
        public static void check(Texture2D tex)
        {
            Locations = new GroundType[tex.Height, tex.Width];
            HangLocations = new List<Vector2>();
            SwingLocations = new List<Vector2>();
            col = new Color[tex.Width * tex.Height];
            tex.GetData<Color>(col);

            for (int i = 0; i < tex.Height; i++)
            {
                for (int j = 0; j < tex.Width; j++)
                {
                    if (col[tex.Width * i + j] == col[0])
                    {
                        Locations[i, j] = GroundType.Platform;
                    }
                    else if (col[tex.Width * i + j] == col[1])
                    {
                        Locations[i, j] = GroundType.Blocked;
                    }
                    else if (col[tex.Width * i + j] == col[2])
                    {
                        HangLocations.Add(new Vector2(j, i));
                        Locations[i, j] = GroundType.Can_Hang;
                    }
                    else if (col[tex.Width * i + j] == col[3])
                    {
                        SwingLocations.Add(new Vector2(j, i));
                        Locations[i, j] = GroundType.Swing;
                    }
                    else
                    {
                        Locations[i, j] = GroundType.Air;
                    }
                }
            }
        }
    }
}