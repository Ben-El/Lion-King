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
using System.Threading;

namespace xxx
{
    class Collision
    {
        /// <summary>
        /// Checking collision with a creature or with an attack ball
        /// </summary>
        /// <param name="hero"></param>
        /// <param name="StatesIndex1"></param>
        /// <param name="enemy"></param>
        /// <param name="StatesIndex2"></param>
        public static void CheckCollision(Animal hero, int StatesIndex1, Animal enemy, int StatesIndex2)
        {
            try
            {
                if ((TheDict.dic[hero.folder][hero.state].BigCircles[StatesIndex1 %
                     TheDict.dic[hero.folder][hero.state].rec.Count].radius +
                     TheDict.dic[enemy.folder][enemy.state].BigCircles[StatesIndex2 %
                     TheDict.dic[enemy.folder][enemy.state].rec.Count].radius) >
                     (hero.Pos - enemy.Pos).Length())
                {
                    foreach (Circle cir1 in TheDict.dic[hero.folder][hero.state].AllSmallCircles[StatesIndex1 %
                             TheDict.dic[hero.folder][hero.state].rec.Count])
                    {
                        foreach (Circle cir2 in TheDict.dic[enemy.folder][enemy.state].AllSmallCircles[StatesIndex2 %
                                 TheDict.dic[enemy.folder][enemy.state].rec.Count])
                        {
                            #region Collision with animals

                            #region Collision with lizard

                            if (enemy.AnimalType == AnimalType.lizard)
                            {
                                if (hero.AnimalType == AnimalType.Cub_Simba)
                                {
                                    if (((hero.jumping && hero.jumpspeed >= 0) || hero.falling) &&
                                        enemy.Pos.Y - hero.Pos.Y <= 2 * cir2.radius &&
                                        hero.Pos.X >= enemy.Pos.X - cir2.radius - 10f &&
                                        hero.Pos.X <= enemy.Pos.X + cir2.radius + 10f)
                                    {
                                        SmallAnimalsDeath(hero, enemy, cir2);
                                    }
                                    else
                                    {
                                        if (((!hero.jumping && hero.Pos.X >= enemy.Pos.X - cir2.radius - 23f &&
                                            hero.Pos.X <= enemy.Pos.X + cir2.radius + 23f)))
                                        {
                                            hero.state = States.GettingHurt;
                                        }
                                        else if (!hero.jumping && enemy.state == States.Slash)
                                        {
                                            hero.state = States.GettingHurt;
                                        }
                                    }
                                }
                            }

                            #endregion

                            #region Collision with hedgehog

                            if (enemy.AnimalType == AnimalType.hedgehog)
                            {
                                if (hero.AnimalType == AnimalType.Cub_Simba)
                                {
                                    if (hero.state != States.Rolling && Math.Abs((hero.Pos - enemy.Pos).Length()) <= 65f)
                                    {
                                        hero.state = States.GettingHurt;
                                    }

                                    if (hero.state == States.Rolling && Math.Abs((hero.Pos - enemy.Pos).Length()) <= 20f)
                                    {
                                        enemy.jumpspeed = -8f;
                                        enemy.jumping = true;
                                        enemy.state = States.Running;
                                        enemy.effects = SpriteEffects.FlipVertically;
                                    }

                                    if (enemy.effects == SpriteEffects.FlipVertically)
                                    {
                                        if (((hero.jumping && hero.jumpspeed >= 0) || hero.falling) &&
                                            enemy.Pos.Y - hero.Pos.Y <= 2 * cir2.radius &&
                                            hero.Pos.X >= enemy.Pos.X - cir2.radius - 20f &&
                                            hero.Pos.X <= enemy.Pos.X + cir2.radius + 20f)
                                        {
                                            SmallAnimalsDeath(hero, enemy, cir2);
                                        }
                                    }
                                }
                            }

                            #endregion

                            #region Collision with beetle

                            if (enemy.AnimalType == AnimalType.beetle)
                            {
                                if (hero.AnimalType == AnimalType.Cub_Simba)
                                {
                                    if (((hero.jumping && hero.jumpspeed >= 0) || hero.falling) &&
                                        enemy.Pos.Y - hero.Pos.Y <= 2 * cir2.radius &&
                                        hero.Pos.X >= enemy.Pos.X - cir2.radius - 40f &&
                                        hero.Pos.X <= enemy.Pos.X + cir2.radius + 40f)
                                    {
                                        SmallAnimalsDeath(hero, enemy, cir2);
                                    }
                                    else
                                    {
                                        if (!hero.jumping && Math.Abs((hero.Pos - enemy.Pos).Length()) <= 60f)
                                        {
                                            hero.state = States.GettingHurt;
                                        }
                                    }
                                }
                            }

                            #endregion

                            #region Collision with hyena

                            if (enemy.AnimalType == AnimalType.hyena)
                            {
                                if (hero.AnimalType == AnimalType.Cub_Simba)
                                {
                                    if ((hero.jumping && hero.jumpspeed >= 0) &&
                                        !enemy.RunningJump && !enemy.jumping &&
                                        enemy.state != States.GettingSlashed &&
                                        enemy.Pos.Y - hero.Pos.Y <= 2 * cir2.radius &&
                                        hero.Pos.X >= enemy.Pos.X - cir2.radius - 10f &&
                                        hero.Pos.X <= enemy.Pos.X + cir2.radius + 10f)
                                    {
                                        hero.Pos = new Vector2(hero.Pos.X, enemy.Pos.Y - 1.5f * cir2.radius);
                                        hero.jumpspeed = -10f;
                                        hero.jumping = true;
                                        enemy.hp -= 50;
                                        enemy.state = States.GettingSlashed;
                                        hero.state = States.Pouncing;

                                        if (enemy.hp == 0)
                                        {
                                            enemy.state = States.Dying;
                                            Level.Characters.Remove(enemy);
                                            enemy.color = Color.Transparent;
                                            Game1.UPDATE_EVENT -= enemy.Update;
                                            Game1.DRAW_EVENT -= enemy.DrawAnimal;
                                            enemy.IsDead = true;
                                        }
                                    }


                                    if (Math.Abs((hero.Pos - enemy.Pos).Length()) <= 95f)
                                    {
                                        hero.state = States.GettingHurt;
                                    }
                                }
                            }

                            #endregion

                            #endregion

                            #region When adult simba can be hurt

                            if (hero.AnimalType == AnimalType.Adult_Simba)
                            {
                                if (Math.Abs((hero.Pos - enemy.Pos).Length()) <= 140f && hero.state != States.ThrowingEnemy)
                                {
                                    hero.state = States.GettingHurt;
                                }
                            }

                            #endregion

                            #region Simba is getting hurt

                            if (hero.state == States.GettingHurt && !hero.blocked)
                            {
                                if (hero.Pos.X > enemy.Pos.X)
                                {
                                    hero.Pos += new Vector2(1f, 0);
                                }

                                else if (hero.Pos.X <= enemy.Pos.X)
                                {
                                    hero.Pos += new Vector2(-1f, 0);
                                }
                            }

                            #endregion

                            #region Animal death (In the first level)

                            if (enemy.IsDead && hero.AnimalType == AnimalType.Cub_Simba)
                            {
                                hero.state = States.Pouncing;

                                if (Level.LevelNumber == 1 &&
                                    enemy.AnimalType == AnimalType.hyena)
                                {
                                    Thread.Sleep(500);
                                    Level.InitSecondLevel();
                                }
                            }

                            #endregion
                        }
                    }
                }
            }
            catch { }
        }

        public static void SmallAnimalsDeath(Animal hero, Animal enemy, Circle cir)
        {
            hero.Pos = new Vector2(hero.Pos.X, enemy.Pos.Y - 1.5f * cir.radius);
            hero.jumpspeed = -10f;
            hero.jumping = true;
            Level.Characters.Remove(enemy);
            enemy.color = Color.Transparent;
            Game1.UPDATE_EVENT -= enemy.Update;
            Game1.DRAW_EVENT -= enemy.DrawAnimal;
            enemy.hp = 0;
            enemy.IsDead = true;
        }
    }
}