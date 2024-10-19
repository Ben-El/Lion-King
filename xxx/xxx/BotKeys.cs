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
    class BotKeys : BaseKeys
    {
        #region Variables

        Animal animal;
        Animal Simba;
        bool up, down, right, left, punch, shift, roar;
        bool TurnRight, TurnLeft;
        float Start_X;
        bool SimbaIsAboveEnemy;
        bool SimbaAndEnemyIsInTheSameHight;
        bool SimbaIsUnderEnemy;
        bool FirstCheck = true;
        bool WasRight = false;
        bool WasLeft = false;
        bool stop = false;
        bool FirstFight = true;
        bool SecondFight = false;
        bool ThirdFight = false;
        bool StartFinalMusic = false;
        bool StartTossing = false;

        #endregion

        /// <summary>
        /// Initializing the parameters
        /// </summary>
        /// <param name="lion"></param>
        /// <param name="Animal_Type"></param>
        public BotKeys(Animal animal, AnimalType Animal_Type)
        {
            this.Simba = Level.hero;
            this.animal = animal;
            up = down = right = left = punch = shift = roar = false;
            TurnRight = true;
            TurnLeft = false;

            Start_X = this.animal.Pos.X;

            SimbaIsAboveEnemy = false;
            SimbaIsUnderEnemy = false;
            SimbaAndEnemyIsInTheSameHight = false;

            Game1.UPDATE_EVENT += this.Update;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool UpKey()
        {
            return up;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool DownKey()
        {
            return down;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool RightKey()
        {
            return right;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool LeftKey()
        {
            return left;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool ShiftKey()
        {
            return shift;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool PunchKey()
        {
            return punch;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool RoarKey()
        {
            return roar;
        }

        /// <summary>
        /// Updateing AI reaction regarding to the hero
        /// </summary>
        public void Update()
        {
            #region Lirzard + Hedgehog + Beetle Movement (In first level only)

            if (this.animal.AnimalType == AnimalType.lizard)
            {
                if ((this.animal.effects == SpriteEffects.FlipHorizontally
                     && this.animal.Pos.X > this.Simba.Pos.X && Math.Abs(this.animal.Pos.Y - this.Simba.Pos.Y) <= 15f &&
                     this.animal.Pos.X - this.Simba.Pos.X <= 125f) ||
                    (this.animal.effects == SpriteEffects.None
                    && this.animal.Pos.X < this.Simba.Pos.X && Math.Abs(this.animal.Pos.Y - this.Simba.Pos.Y) <= 15f
                    && this.Simba.Pos.X - this.animal.Pos.X <= 125f))
                {
                    this.animal.state = States.Slash;
                    left = false;
                    right = false;
                }

                if (this.animal.state != States.Slash)
                {
                    Walk();
                }
            }


            if (this.animal.AnimalType == AnimalType.hedgehog || this.animal.AnimalType == AnimalType.beetle)
            {
                Walk();
            }

            #endregion

            #region The hyena boss vs cub simba in the first level

            if (this.animal.AnimalType == AnimalType.lion && Level.LevelNumber == 1)
            {
                Follow();
            }

            if (this.animal.AnimalType == AnimalType.hyena && Level.LevelNumber == 1)
            {
                if (this.Simba.Pos.X >= 3270f && this.Simba.Pos.Y <= 870)
                {
                    Follow();
                }
            }

            #endregion

            #region Hyena + Scar Movement (In the second level)

            if ((Level.LevelNumber == 2) &&
                this.animal.AnimalType == AnimalType.lion || this.animal.AnimalType == AnimalType.hyena)
            {
                if (this.Simba.Pos.Y >= 3960f)
                {
                    if (S.CounterLives > 0)
                    {
                        S.CounterLives--;
                        this.Simba.Pos = new Vector2(3550, 3800);
                    }
                    else
                    {
                        Game1.UPDATE_EVENT -= this.Simba.Update;
                        Game1.DRAW_EVENT -= this.Simba.DrawAnimal;
                        Game1.IsPaused = true;
                    }
                }

                #region Simba VS Scar or hyena

                if (!this.animal.jumping &&
                    ((this.Simba.Pos.X > this.animal.Pos.X && this.Simba.Pos.X - this.animal.Pos.X <= 200f &&
                    this.Simba.effects == SpriteEffects.FlipHorizontally && this.animal.effects == SpriteEffects.None) ||
                    (this.Simba.Pos.X < this.animal.Pos.X && this.animal.Pos.X - this.Simba.Pos.X <= 200f &&
                    this.Simba.effects == SpriteEffects.None && this.animal.effects == SpriteEffects.FlipHorizontally)) &&
                    (this.Simba.state == States.Slash ||
                    this.Simba.state == States.DoubleSlash ||
                    this.Simba.state == States.CrouchSlash))
                {
                    right = false;
                    left = false;
                    up = false;
                    this.animal.hp -= 50; // 50
                    this.animal.state = States.GettingSlashed;

                    if (this.animal.hp == 0)
                    {
                        if (FirstFight)
                        {
                            FirstFight = false;
                            this.animal.Pos = new Vector2(1350, 1900);
                            this.animal.hp = 100;
                            this.animal.jumpspeed = -16f;
                            this.animal.jumping = true;
                        }

                        if (SecondFight)
                        {
                            SecondFight = false;
                            this.animal.Pos = new Vector2(1750, 335);
                            this.animal.jumpspeed = -16f;
                            this.animal.jumping = true;
                        }
                    }
                }

                else if (((this.Simba.Pos.X < this.animal.Pos.X && this.Simba.Pos.X - this.animal.Pos.X <= 200f &&
                    this.Simba.effects == SpriteEffects.None && this.animal.effects == SpriteEffects.FlipHorizontally) ||
                    (this.Simba.Pos.X > this.animal.Pos.X && this.animal.Pos.X - this.Simba.Pos.X <= 200f &&
                    this.Simba.effects == SpriteEffects.FlipHorizontally && this.animal.effects == SpriteEffects.None)) &&
                    this.Simba.state == States.ThrowingEnemy && !this.animal.jumping &&
                    this.animal.state != States.TossedBySimba)
                {
                    right = false;
                    left = false;
                    up = false;
                    this.animal.state = States.TossedBySimba;

                    if (this.animal.AnimalType == AnimalType.lion)
                    {
                        S.ScarPos = this.animal.Pos;
                    }
                }

                else
                {
                    if (this.Simba.state != States.ThrowingEnemy &&
                        this.Simba.state != States.Slash &&
                        this.Simba.state != States.DoubleSlash &&
                        this.animal.state != States.TossedBySimba &&
                        this.animal.state != States.GettingSlashed &&
                        this.animal.state != States.GettingHurt)
                    {
                        if (Level.LevelNumber == 1)
                        {
                            Follow();
                        }

                        if (Level.LevelNumber == 2)
                        {
                            if (FirstFight || SecondFight || ThirdFight)
                            {
                                Follow();
                            }
                        }
                    }
                }

                #endregion

                #region Simba VS Scar - First fight

                if (FirstFight && this.Simba.Pos.Y >= 3000f && this.animal.Pos.Y >= 3000f)
                {
                    if (this.Simba.Pos.X < 1900)
                    {
                        this.Simba.Pos = new Vector2(1900f, this.Simba.Pos.Y);
                    }

                    if (this.animal.Pos.X < 1900)
                    {
                        this.animal.Pos = new Vector2(1900f, this.animal.Pos.Y);
                        right = true;
                        left = false;
                        WasRight = true;
                        WasLeft = false;
                    }
                }

                #endregion

                #region Simba VS Scar - Second fight

                else if (this.Simba.Pos.Y >= 1700f && this.Simba.Pos.Y <= 1950f &&
                         this.animal.Pos.Y >= 1700f && this.animal.Pos.Y <= 1950f)
                {
                    SecondFight = true;

                    if (this.Simba.Pos.X < 350)
                    {
                        this.Simba.Pos = new Vector2(350f, this.Simba.Pos.Y);
                    }

                    if (this.animal.Pos.X < 350)
                    {
                        this.animal.Pos = new Vector2(350f, this.animal.Pos.Y);
                        right = true;
                        left = false;
                        WasRight = true;
                        WasLeft = false;
                    }

                    if (this.Simba.Pos.X > 1500)
                    {
                        this.Simba.Pos = new Vector2(1500f, this.Simba.Pos.Y);
                    }

                    if (this.animal.Pos.X > 1500)
                    {
                        this.animal.Pos = new Vector2(1500f, this.animal.Pos.Y);
                        left = true;
                        right = false;
                        WasLeft = true;
                        WasRight = false;
                    }
                }

                #endregion

                #region Simba VS Scar - Third fight

                else if (this.Simba.Pos.Y <= 340f && this.animal.Pos.Y <= 340)
                {
                    ThirdFight = true;

                    if (this.Simba.Pos.X < 1700)
                    {
                        this.Simba.Pos = new Vector2(1700f, this.Simba.Pos.Y);
                    }

                    if (this.animal.Pos.X < 1700 && this.animal.state != States.TossedBySimba)
                    {
                        this.animal.Pos = new Vector2(1700f, this.animal.Pos.Y);
                        right = true;
                        left = false;
                        WasRight = true;
                        WasLeft = false;
                    }

                    if (this.Simba.Pos.X > 3200)
                    {
                        this.Simba.Pos = new Vector2(3200f, this.Simba.Pos.Y);
                    }

                    if (this.animal.Pos.X > 3200 && this.animal.state != States.TossedBySimba)
                    {
                        this.animal.Pos = new Vector2(3200f, this.animal.Pos.Y);
                        left = true;
                        right = false;
                        WasLeft = true;
                        WasRight = false;
                    }
                }

                #endregion

                #region Winning

                if (ThirdFight && this.animal.Pos.Y > 1000 && !Game1.IsPaused)
                {
                    right = false;
                    left = false;
                    up = false;

                    if (this.Simba.Pos.X >= 1700f)
                    {
                        S.SimbaWon = true;
                        this.Simba.Pos += new Vector2(this.Simba.LeftRunSpeed, 0);
                        this.Simba.effects = SpriteEffects.FlipHorizontally;
                        this.Simba.state = States.Running;
                    }

                    else
                    {
                        S.AfterWinning = true;
                        this.Simba.state = States.Stand;
                    }
                }

                if (S.SimbaWon)
                {
                    if (!StartFinalMusic)
                    {
                        StartFinalMusic = true;
                        LionSounds.secondlevelsonginstance.Stop();
                        LionSounds.finalsoundinstance.Play();
                        S.SimbaCopyLocation = this.Simba.Pos;
                    }
                }

                #endregion
            }

            #endregion
        }

        public void Follow()
        {
            if (FirstCheck)
            {
                if (this.animal.Pos.X > this.Simba.Pos.X)
                {
                    left = true;
                    WasLeft = true;
                    WasRight = false;
                    right = false;
                }

                else
                {
                    right = true;
                    WasRight = true;
                    WasLeft = false;
                    left = false;
                }

                FirstCheck = false;
            }

            if (this.animal.Pos.Y < this.Simba.Pos.Y) // אם סקאר במקום יותר גבוה
            {
                SimbaIsUnderEnemy = true;
                SimbaIsAboveEnemy = false;
            }


            else if (this.animal.Pos.Y > this.Simba.Pos.Y) // אם סקאר במקום יותר נמוך
            {
                SimbaIsAboveEnemy = true;
                SimbaIsUnderEnemy = false;
            }

            else
            {
                SimbaAndEnemyIsInTheSameHight = true;
                SimbaIsAboveEnemy = false;
                SimbaIsUnderEnemy = false;
            }

            if (SimbaAndEnemyIsInTheSameHight)
            {
                if (this.animal.Pos.X < this.Simba.Pos.X && this.Simba.Pos.X - this.animal.Pos.X >= 100f &&
                    this.Simba.Pos.Y == this.animal.Pos.Y)
                {
                    right = true;
                    WasRight = true;
                    left = false;
                    WasLeft = false;
                }

                else if (this.animal.Pos.X > this.Simba.Pos.X && this.animal.Pos.X - this.Simba.Pos.X >= 100f &&
                    this.Simba.Pos.Y == this.animal.Pos.Y)
                {
                    left = true;
                    WasLeft = true;
                    right = false;
                    WasRight = false;
                }

                else
                {
                    stop = true;
                    right = false;
                    left = false;
                    up = false;
                }
            }

            if (Math.Abs(this.Simba.Pos.Y - this.animal.Pos.Y) <= 5f &&
                ((this.Simba.Pos.X > this.animal.Pos.X && this.Simba.Pos.X - this.animal.Pos.X <= 100f) ||
                (this.Simba.Pos.X < this.animal.Pos.X && this.animal.Pos.X - this.Simba.Pos.X <= 100f)))
            {
                stop = true;
            }

            if (SimbaIsUnderEnemy || SimbaIsAboveEnemy)
            {
                stop = false;

                if (WasRight && !stop)
                {
                    right = true;
                    left = false;
                    WasLeft = false;
                }

                else if (WasLeft && !stop)
                {
                    left = true;
                    right = false;
                    WasRight = false;
                }

                // התנאים עם ה 220 הם בשביל זיהוי יכולת קפיצה מעל איפה שיש אדום
                // 220 זה גובה הקפיצה המקסימלי

                if (SimbaIsAboveEnemy)
                {
                    if ((Map.Locations[(int)(this.animal.Pos.Y / S.MapsScale),
                           (int)((this.animal.Pos.X - 15f) / S.MapsScale)] == GroundType.Blocked &&
                        Map.Locations[(int)((this.animal.Pos.Y - 220f) / S.MapsScale),
                           (int)((this.animal.Pos.X - 15f) / S.MapsScale)] != GroundType.Blocked) ||
                        (Map.Locations[(int)(this.animal.Pos.Y / S.MapsScale),
                           (int)((this.animal.Pos.X + 15f) / S.MapsScale)] == GroundType.Blocked &&
                        Map.Locations[(int)((this.animal.Pos.Y - 220f) / S.MapsScale),
                           (int)((this.animal.Pos.X + 15f) / S.MapsScale)] != GroundType.Blocked))
                    {
                        up = true;
                    }
                    else if (Map.Locations[(int)(this.animal.Pos.Y / S.MapsScale),
                           (int)((this.animal.Pos.X - 15f) / S.MapsScale)] == GroundType.Air ||
                        Map.Locations[(int)(this.animal.Pos.Y / S.MapsScale),
                           (int)((this.animal.Pos.X + 15f) / S.MapsScale)] == GroundType.Air)
                    {
                        up = true;
                    }
                    else
                    {
                        up = false;
                    }
                }

                // שני התנאים שמתחת זה למקרה שאני לא מצליח לעבור מחסום אדום גבוה
                // לכן זה סימן לבינה המלאכותית ללכת לכיוון הנגדי ממה שהלכה עד עכשיו
                if ((Map.Locations[(int)(this.animal.Pos.Y / S.MapsScale),
                      (int)((this.animal.Pos.X + 15f) / S.MapsScale)] == GroundType.Blocked &&
                   Map.Locations[(int)((this.animal.Pos.Y - 220f) / S.MapsScale),
                      (int)((this.animal.Pos.X + 15f) / S.MapsScale)] == GroundType.Blocked))
                {
                    left = true;
                    right = false;
                    WasLeft = true;
                    WasRight = false;
                }

                if ((Map.Locations[(int)(this.animal.Pos.Y / S.MapsScale),
                   (int)((this.animal.Pos.X - 15f) / S.MapsScale)] == GroundType.Blocked &&
                Map.Locations[(int)((this.animal.Pos.Y - 220f) / S.MapsScale),
                   (int)((this.animal.Pos.X - 15f) / S.MapsScale)] == GroundType.Blocked))
                {
                    right = true;
                    left = false;
                    WasRight = true;
                    WasLeft = false;
                }

                if (SimbaIsUnderEnemy)
                {
                    if (this.Simba.Pos.X > this.animal.Pos.X && this.Simba.Pos.X - this.animal.Pos.X >= 100f)
                    {
                        right = true;
                        left = false;
                        WasRight = true;
                        WasLeft = false;
                    }


                    if (this.Simba.Pos.X < this.animal.Pos.X && this.animal.Pos.X - this.Simba.Pos.X >= 100f)
                    {
                        left = true;
                        right = false;
                        WasLeft = true;
                        WasRight = false;
                    }
                }
            }
        }

        /// <summary>
        /// Walking side to side after passing determined distance each time
        /// </summary>
        public void Walk()
        {
            if (Map.Locations[(int)(this.animal.Pos.Y / S.MapsScale),
                (int)((this.animal.Pos.X - 15f) / S.MapsScale)] == GroundType.Air ||
                Map.Locations[(int)(this.animal.Pos.Y / S.MapsScale),
                (int)((this.animal.Pos.X - 15f) / S.MapsScale)] == GroundType.Blocked)
            {
                TurnRight = true;
                TurnLeft = false;
            }

            if (Map.Locations[(int)(this.animal.Pos.Y / S.MapsScale),
                (int)((this.animal.Pos.X + 15f) / S.MapsScale)] == GroundType.Air ||
                Map.Locations[(int)(this.animal.Pos.Y / S.MapsScale),
                (int)((this.animal.Pos.X + 15f) / S.MapsScale)] == GroundType.Blocked)
            {
                TurnLeft = true;
                TurnRight = false;
            }

            if (this.animal.Pos.X < Start_X)
            {
                TurnRight = true;
                TurnLeft = false;
            }

            if (this.animal.Pos.X > Start_X + 300f)
            {
                TurnLeft = true;
                TurnRight = false;
            }

            if (TurnRight)
            {
                right = true;
                left = false;
            }

            if (TurnLeft)
            {
                left = true;
                right = false;
            }
        }
    }
}