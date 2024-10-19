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
    class SpriteObject : Ifocus
    {
        #region Data

        public Texture2D texture { get; set; }
        public Vector2 Pos { get; set; }
        public Rectangle? sourceRectangle;
        public Color color;
        public float Rot { get; set; }
        public Vector2 origin { get; set; }
        public Vector2 scale;
        public SpriteEffects effects;
        public float layerDepth;

        #endregion

        #region Ctor

        /// <summary>
        /// Initializing the parameters
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="position"></param>
        /// <param name="sourceRectangle"></param>
        /// <param name="color"></param>
        /// <param name="rotation"></param>
        /// <param name="origin"></param>
        /// <param name="scale"></param>
        /// <param name="effects"></param>
        /// <param name="layerDepth"></param>
        public SpriteObject(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            this.texture = texture;
            this.Pos = position;
            this.sourceRectangle = sourceRectangle;
            this.color = color;
            this.Rot = rotation;
            this.origin = origin;
            this.scale = scale;
            this.effects = effects;
            this.layerDepth = layerDepth;
        }

        #endregion

        #region public funcs

        /// <summary>
        /// Drawing the texture
        /// </summary>
        public virtual void DrawObject()
        {
            S.spb.Draw(texture, Pos, sourceRectangle, color * S.GameColorLevel, Rot, origin, scale, effects, layerDepth);
        }

        /// <summary>
        /// Drawing the ractangle that representing each character in the minimap
        /// </summary>
        /// <param name="p"></param>
        /// <param name="size"></param>
        /// <param name="color"></param>
        public static void draw_rect(Vector2 p, int size, Color color)
        {
            Texture2D pointTex = new Texture2D(S.gd, 1, 1);
            pointTex.SetData<Color>(new Color[] { Color.White });
            S.spb.Draw(pointTex, new Rectangle((int)p.X - size / 2, (int)p.Y - size / 2, size, size), color);
        }

        public static Texture2D createCircleText(int radius)
        {
            radius *= 2;

            Texture2D texture = new Texture2D(S.gd, radius, radius);
            Color[] colorData = new Color[radius * radius];

            float diam = radius / 2f;
            float diamsq = diam * diam;

            for (int x = 0; x < radius; x++)
            {
                for (int y = 0; y < radius; y++)
                {
                    int index = x * radius + y;
                    Vector2 pos = new Vector2(x - diam, y - diam);
                    if (pos.LengthSquared() <= diamsq)
                    {
                        colorData[index] = Color.Green;
                    }
                    else
                    {
                        colorData[index] = Color.Transparent;
                    }
                }
            }

            texture.SetData(colorData);

            return texture;
        }
        #endregion

    }
}