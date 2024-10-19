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
    class Particle
    {
        Texture2D texture;
        Vector2 position;
        Vector2 velocity;

        public Vector2 Position
        {
            get { return position; }
        }

        public Particle(Texture2D newTexture, Vector2 newPosition, Vector2 newVelocity)
        {
            this.texture = newTexture;
            this.position = newPosition;
            this.velocity = newVelocity;
        }

        public void Update()
        {
            position += velocity;
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            spriteBatch.Draw(texture, position, color);
        }
    }
}