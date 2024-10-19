using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace xxx
{
    class Animation : SpriteObject
    {
        public Folders folder { get; set; }
        public States state { get; set; }
        public States PrevState;
        public bool IsAnimationOver;
        public int index;
        int indexSlow;
        int slow;

        bool IsWinningTossedState = false;
        float adder = 3f;

        /// <summary>
        /// Initializing the parameters
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="state"></param>
        /// <param name="sb"></param>
        /// <param name="position"></param>
        /// <param name="sourceRectangle"></param>
        /// <param name="color"></param>
        /// <param name="rotation"></param>
        /// <param name="origin"></param>
        /// <param name="scale"></param>
        /// <param name="effects"></param>
        /// <param name="layerDepth"></param>
        #region Ctor

        public Animation(Folders folder, States state, SpriteBatch sb, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
            : base(null, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth)
        {
            IsAnimationOver = false;
            this.folder = folder;
            this.state = state;
            this.indexSlow = 0;
            this.index = 0;
            this.slow = 0;
        }

        #endregion

        /// <summary>
        /// Drawing a texture
        /// </summary>
        public override void DrawObject()
        {
            Page p = TheDict.dic[folder][state];
            base.texture = p.tex;
            base.sourceRectangle = p.rec[index % p.rec.Count];

            try
            {
                if (this.effects == SpriteEffects.None)
                {
                    base.origin = p.org[index % p.rec.Count];
                }

                if (this.effects == SpriteEffects.FlipHorizontally)
                {
                    base.origin = p.FlippedOrg[index % p.rec.Count];
                }
            }

            catch { }

            base.DrawObject();

            this.slow = Page.DefineStatesSlows(this.state);

            if (PrevState != state)
            {
                IsAnimationOver = false;
                indexSlow = 0;
                index = 0; // כדי שבמעבר מאנימציה אחת לאחרת, נתחיל מהפריים הראשון בכל סטריפ אנימציה
            }

            if (indexSlow == this.slow && !Game1.IsPaused)
            {
                index++;

                if (this.state == States.Running_Jump && index == 8 &&
                    Map.Locations[(int)(this.Pos.Y / S.MapsScale), (int)(this.Pos.X / S.MapsScale)] == GroundType.Air)
                {
                    index -= 1;
                }

                if (this.state == States.Crouch && index == 4)
                {
                    index -= 1;
                }

                if (this.state == States.Dying && index == 7)
                {
                    index -= 1;
                }

                if (this.state == States.ThrowingEnemy)
                {
                    if (this.effects == SpriteEffects.None &&
                        this.Pos.X < S.ScarPos.X - 100)
                    {
                        this.Pos += new Vector2(20f, 0);
                    }

                    if (this.effects == SpriteEffects.FlipHorizontally &&
                       this.Pos.X > S.ScarPos.X + 100)
                    {
                        this.Pos += new Vector2(-20f, 0);
                    }
                }

                if (this.state == States.TossedBySimba)
                {
                    if (!IsWinningTossedState && S.SimbaPosAtThrowing.Y <= 350 &&
                        ((S.SimbaPosAtThrowing.X >= 3030 && this.effects == SpriteEffects.None) ||
                        (S.SimbaPosAtThrowing.X <= 1800 && this.effects == SpriteEffects.FlipHorizontally)))
                    {
                        IsWinningTossedState = true;
                    }

                    if (IsWinningTossedState && index == 16)
                    {
                        index -= 1;
                        this.Pos += new Vector2(0, 0.5f);
                    }

                    if (this.effects == SpriteEffects.None && index >= 7)
                    {
                        if (!IsWinningTossedState)
                        {
                            this.Pos += new Vector2(20, 0);
                        }
                        else
                        {
                            this.Pos += new Vector2(20 + (adder += 1), 0);
                        }
                    }

                    else if (this.effects == SpriteEffects.FlipHorizontally && index >= 7)
                    {
                        if (!IsWinningTossedState)
                        {
                            this.Pos += new Vector2(-20f, 0);
                        }
                        else
                        {
                            this.Pos += new Vector2(-20 - (adder += 1), 0);
                        }
                    }
                }


                if (this.state == States.Climb)
                {
                    if (index >= 3 && index <= 7)
                    {
                        if (Level.hero.AnimalType == AnimalType.Cub_Simba)
                        {
                            if (this.effects == SpriteEffects.None)
                            {
                                this.Pos += new Vector2(13f, 0);
                            }

                            if (this.effects == SpriteEffects.FlipHorizontally)
                            {
                                this.Pos += new Vector2(-13f, 0);
                            }
                        }

                        if (Level.hero.AnimalType == AnimalType.Adult_Simba)
                        {
                            if (this.effects == SpriteEffects.None)
                            {
                                this.Pos += new Vector2(10f, 0);
                            }

                            if (this.effects == SpriteEffects.FlipHorizontally)
                            {
                                this.Pos += new Vector2(-10f, 0);
                            }
                        }
                    }
                }

                if (this.state == States.DoubleSlash || this.state == States.AfterFalling)
                {
                    if (index == p.org.Count)
                    {
                        IsAnimationOver = true;
                    }
                }

                else
                {
                    if (index + 1 == p.org.Count)
                    {
                        IsAnimationOver = true;
                    }
                }

                indexSlow = 0;
            }

            if (indexSlow > slow)
            {
                indexSlow = 0;
            }

            indexSlow++;
        }
    }
}