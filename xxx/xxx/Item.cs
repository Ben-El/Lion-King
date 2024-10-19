using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace xxx
{
    class Item
    {
        public Texture2D tex;
        public Vector2 Pos;
        public Color color;
        public float scale;

        public List<Item> a;

        public Item(Texture2D tex, Vector2 Pos, float scale, Color color)
        {
            Game1.UPDATE_EVENT += this.UpdateItem;
            Game1.DRAW_EVENT += this.DrawItem;

            this.tex = tex;
            this.Pos = Pos;
            this.scale = scale;
            this.color = color;
        }

        public void UpdateItem()
        {
            if (color != Color.Transparent && Math.Abs((this.Pos - Level.hero.Pos).Length()) <= 50f)
            {
                Level.FirstLevelItems.Remove(this);
                Game1.DRAW_EVENT -= this.DrawItem;
            }
        }

        public void DrawItem()
        {
            S.spb.Draw(tex, Pos, null, Color.White * S.GameColorLevel, 0f,
                       new Vector2(tex.Height / 2, tex.Width / 2), scale, SpriteEffects.None, 1f);
        }

        public void Init()
        {
            a = new List<Item>();

            a.Add(new Item(Game1.ItemPic, new Vector2(1060, 2800), 2.2f, Color.White));
            a.Add(new Item(Game1.ItemPic, new Vector2(1060 + 960, 2800), 2.2f, Color.White));
        }
    }
}