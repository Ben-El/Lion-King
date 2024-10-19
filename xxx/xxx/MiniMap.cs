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
    class MiniMap
    {
        float zoom = 0.09f;
        SpriteObject sbpo;

        /// <summary>
        /// Minimap's constructor
        /// </summary>
        /// <param name="tex"></param>
        /// <param name="scale"></param>
        public MiniMap(Texture2D tex, Vector2 scale)
        {
            this.sbpo = new SpriteObject(tex, new Vector2(7, 60), null, Color.White * 1f, 0f,
                Vector2.Zero, new Vector2(scale.X * zoom, scale.Y * zoom), SpriteEffects.None, 1f);
        }

        /// <summary>
        /// Drawing the minimap
        /// </summary>
        public void Draw()
        {
            this.sbpo.DrawObject();

            SpriteObject.draw_rect(new Vector2(Level.hero.Pos.X * zoom + 7,
              Level.hero.Pos.Y * zoom + 60 - 1), 8, Color.Red);

            for (int i = 0; i < Level.Characters.Count; i++)
            {
                SpriteObject.draw_rect(new Vector2(Level.Characters[i].Pos.X * zoom + 7,
                    Level.Characters[i].Pos.Y * zoom + 60 - 2), 8, Color.Blue);
            }
        }
    }
}