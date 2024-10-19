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
    class ParticlesGenerator
    {
        Texture2D texture;

        float spawnWidth;
        float density;

        List<Particle> Particles = new List<Particle>();

        float timer;

        Random rand1, rand2;
      
        int DecreaseTime = 0;
        public static int DecreaseHeight = 125;

        bool flamesGone = false;

        public ParticlesGenerator(Texture2D texture, float newSpawnWidth, float newDensity)
        {
            this.texture = texture;
            this.spawnWidth = newSpawnWidth;
            this.density = newDensity;

            rand1 = new Random();
            rand2 = new Random();
        }

        public void createParticle(string ParticleType)
        {
            if (ParticleType == "RainDrop")
            {
                if (!flamesGone)
                {
                    Particles.Add(new Particle(texture, new Vector2(
                        20 + (float)rand1.NextDouble() * spawnWidth, 0),
                        new Vector2(-1, rand2.Next(8, 10))));
                }
            }

            if (ParticleType == "Flame")
            {
                Particles.Add(new Particle(texture, new Vector2(
                -100 + (float)rand1.NextDouble() * spawnWidth, Game1.ScreenHeight - DecreaseHeight),
                new Vector2(0, 1)));
            }
        }

        public void Update(GameTime gameTime, GraphicsDevice graphics, string ParticleType)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            while (timer > 0)
            {
                timer -= 1f / density;
                createParticle(ParticleType);
            }

            for (int i = 0; i < Particles.Count; i++)
            {
                Particles[i].Update();

                if (Particles[i].Position.Y > graphics.Viewport.Height)
                {
                    Particles.RemoveAt(i);
                    i--;
                }
            }

            #region The flames fading after simba won, rain drops stop raining after some time

            if (S.AfterWinning)
            {
                DecreaseTime++;

                if (DecreaseTime == 60)
                {
                    DecreaseHeight -= 2;

                    if (DecreaseHeight < 25)
                    {
                        flamesGone = true;
                    }

                    DecreaseTime = 0;
                }
            }

            #endregion
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            foreach (Particle particle in Particles)
            {
                particle.Draw(spriteBatch, color);
            }
        }
    }
}