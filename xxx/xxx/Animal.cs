using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace xxx
{
    class Animal : Animation
    {
        #region Variables

        public AnimalType AnimalType;
        float NumOfSeconds;
        const float gravity = 9.8f;
        public float RightRunSpeed;
        public float LeftRunSpeed;
        public float jumpspeed = 0; // jumpspeed to see how fast it jumps
        public bool jumping;
        public bool PressedJump;
        public bool falling;
        public bool GeneralFalling;
        public bool Hanging;
        public bool Swinging;
        bool PressedRoar;
        bool PressedPunch;
        bool PressedDirectionToClimb;
        public bool LeftRunning;
        public bool RightRunning;
        public bool LeftStanding;
        public bool RightStanding;
        bool WasJumping;
        bool WasFalling;
        bool Crouching;
        public bool RunningJump;
        bool Rolling;
        public bool DoubleSlashing;
        public bool ThrowingAway;
        int DelayBetweenRoars;
        int DelayBetweenSlashes;
        public BaseKeys baseKeys;
        public SpriteEffects effect;
        float StartFalling_Y;
        float EndFalling_Y;
        public bool blocked;
        public bool IsDead;
        public int hp;
        public static Vector2 StaticHangingData;
        public bool HangingOnSmallRock;

        #endregion

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
        /// <param name="animalType"></param>  
        #region Ctor

        public Animal(Folders folder, States state, SpriteBatch sb, Vector2 position, Rectangle? sourceRectangle,
            Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects,
            float layerDepth, AnimalType animalType)
            : base(folder, state, sb, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth)
        {
            Game1.UPDATE_EVENT += this.Update;
            Game1.DRAW_EVENT += this.DrawAnimal;

            this.AnimalType = animalType;

            if (this.AnimalType == AnimalType.Adult_Simba || this.AnimalType == AnimalType.Cub_Simba)
            {
                if (Level.LevelNumber == 1)
                {
                    LeftStanding = false;
                    RightStanding = true;
                    this.effects = SpriteEffects.None;
                }

                if (Level.LevelNumber == 2)
                {
                    LeftStanding = true;
                    RightStanding = false;
                    this.effects = SpriteEffects.FlipHorizontally;
                }
            }

            else
            {
                LeftStanding = true;
                RightStanding = false;
                this.effects = SpriteEffects.FlipHorizontally;
            }

            blocked = false;
            jumping = false;
            falling = false;
            GeneralFalling = false;
            Hanging = false;
            Swinging = false;
            Crouching = false;
            DoubleSlashing = false;
            ThrowingAway = false;
            LeftRunning = false;
            RightRunning = false;
            PressedJump = false;
            PressedRoar= false;
            PressedPunch = false;
            PressedDirectionToClimb = false;

            if (this.AnimalType == AnimalType.Adult_Simba || this.AnimalType == AnimalType.Cub_Simba)
            {
                RightRunSpeed = 9f;
                LeftRunSpeed = -9f;
            }
            else if (this.AnimalType == AnimalType.lizard ||
                this.AnimalType == AnimalType.hedgehog ||
                this.AnimalType == AnimalType.beetle)
            {
                RightRunSpeed = 2f;
                LeftRunSpeed = -2f;
            }

            jumpspeed = 0;
            DelayBetweenRoars = 45;
            DelayBetweenSlashes = 20;

            StartFalling_Y = 0;
            EndFalling_Y = 0;

            WasJumping = false;
            WasFalling = false;
            RunningJump = false;
            Rolling = false;

            NumOfSeconds = 1;

            hp = 100;
            IsDead = false;

            HangingOnSmallRock = false;
        }

        #endregion

        /// <summary>
        /// Updating all the Characters' data
        /// </summary>
        public void Update()
        {
            if (this.AnimalType == AnimalType.Cub_Simba || this.AnimalType == AnimalType.Adult_Simba)
                Console.WriteLine("x: " + this.Pos.X + "  y: " + this.Pos.Y);

            if (!Game1.IsPaused)
            {
                PrevState = state;

                #region If some animation was over

                if (IsAnimationOver)
                {
                    this.state = States.Stand;
                }

                #endregion

                #region Borders determination

                if (Level.LevelNumber == 1)
                {
                    if (Pos.X < 300)
                    {
                        Pos = new Vector2(300, Pos.Y);
                    }

                    if (Pos.X > 3900)
                    {
                        Pos = new Vector2(3900, Pos.Y);
                    }

                    if (Pos.Y > 3170)
                    {
                        Pos = new Vector2(Pos.X, 3170);
                    }

                    if (Pos.Y <= 0)
                    {
                        Pos = new Vector2(Pos.X, 0);
                    }
                }

                if (Level.LevelNumber == 2)
                {
                    if (Pos.X < 200)
                    {
                        Pos = new Vector2(200, Pos.Y);
                    }

                    if (Pos.X > 3960)
                    {
                        Pos = new Vector2(3960, Pos.Y);
                    }

                    if (Pos.Y > 4000)
                    {
                        Pos = new Vector2(Pos.X, 4000);
                    }

                    if (Pos.Y <= 100)
                    {
                        Pos = new Vector2(Pos.X, 100);
                    }
                }

                #endregion

                #region If Simba Is Blocked

                if (!Hanging)
                {
                    if (Level.LevelNumber == 1 && this.AnimalType == AnimalType.Cub_Simba)
                    {
                        if (this.Pos.Y <= 100f)
                        {
                            this.Pos = new Vector2(this.Pos.X, 100f);
                        }
                    }

                    if ((Map.Locations[(int)(Pos.Y / S.MapsScale), (int)((Pos.X + 15f) / S.MapsScale)] == GroundType.Blocked ||
                        Map.Locations[(int)(Pos.Y / S.MapsScale), (int)((Pos.X - 15f) / S.MapsScale)] == GroundType.Blocked))
                    {
                        blocked = true;

                        if (baseKeys.LeftKey() && this.effects == SpriteEffects.None)
                        {
                            this.Pos += new Vector2(-9f, 0);
                        }

                        if (baseKeys.RightKey() && this.effects == SpriteEffects.FlipHorizontally)
                        {
                            this.Pos += new Vector2(9f, 0);
                        }
                    }

                    else if (Map.Locations[(int)((Pos.Y + 7f) / S.MapsScale), (int)(Pos.X / S.MapsScale)] == GroundType.Blocked)
                    {
                        blocked = true;

                        if (baseKeys.LeftKey() && this.effects == SpriteEffects.FlipHorizontally)
                        {
                            this.Pos = new Vector2(this.Pos.X + 7f, this.Pos.Y - 7f);
                        }

                        if (baseKeys.RightKey() && this.effects == SpriteEffects.None)
                        {
                            this.Pos = new Vector2(this.Pos.X - 7f, this.Pos.Y - 7f);
                        }
                    }

                    else if (Map.Locations[(int)((Pos.Y - 100f) / S.MapsScale), (int)(Pos.X / S.MapsScale)] == GroundType.Blocked)
                    {
                        blocked = true;
                        this.Pos += new Vector2(0, 15f);
                    }

                    else
                    {
                        blocked = false;
                    }
                }

                #endregion

                #region Movement

                if (!S.SimbaWon &&
                    (baseKeys.RightKey() || baseKeys.LeftKey()) && !baseKeys.RoarKey() &&
                    !Hanging && !Swinging && !Rolling && !Crouching && this.state != States.GettingHurt && this.state != States.AfterFalling &&
                    this.state != States.Slash && this.state != States.DoubleSlash && this.state != States.CrouchSlash &&
                    this.state != States.ThrowingEnemy && this.state != States.Roar && this.state != States.Pouncing &&
                    ((this.effects == SpriteEffects.None &&
                    Map.Locations[(int)(Pos.Y / S.MapsScale), (int)((Pos.X + 15f) / S.MapsScale)] != GroundType.Blocked) ||
                    (this.effects == SpriteEffects.FlipHorizontally &&
                    Map.Locations[(int)(Pos.Y / S.MapsScale), (int)((Pos.X - 15f) / S.MapsScale)] != GroundType.Blocked)))
                {
                    if (baseKeys.RightKey())
                    {
                        RightRunning = true;
                        LeftStanding = false;
                        this.effects = SpriteEffects.None;
                        Pos += new Vector2(RightRunSpeed, 0);
                    }

                    if (baseKeys.LeftKey())
                    {
                        LeftRunning = true;
                        RightStanding = false;
                        this.effects = SpriteEffects.FlipHorizontally;
                        Pos += new Vector2(LeftRunSpeed, 0);
                    }

                    if (jumping)
                    {
                        RunningJump = true;
                    }
                    if (this.AnimalType == AnimalType.Cub_Simba && baseKeys.DownKey()) // When cub
                    {
                        Rolling = true;
                    }
                    else
                    {
                        this.state = States.Running;
                    }
                }

                if (RunningJump)
                {
                    this.state = States.Running_Jump;
                    RunningJump = false;
                }

                // --------------------------------------------------------------------------------

                // This condition is in order to avoid the falling from platform while running jump
                if (this.state == States.Running_Jump && Map.Locations[(int)((Pos.Y + 5f) / S.MapsScale),
                    (int)(Pos.X / S.MapsScale)] == GroundType.Platform)
                {
                    this.Pos = new Vector2(this.Pos.X, this.Pos.Y + 5f);
                    this.state = States.Stand;
                    jumping = false;
                }
                // --------------------------------------------------------------------------------

                if (Rolling)
                {
                    this.state = States.Rolling;
                    Rolling = false;
                }

                if (!baseKeys.RightKey() && RightRunning && this.state != States.GettingHurt && !Swinging)
                {
                    RightRunning = false;
                    RightStanding = true;

                    if (!Hanging && Map.Locations[(int)(Pos.Y / S.MapsScale), (int)(Pos.X / S.MapsScale)] != GroundType.Air)
                    {
                        this.state = States.Stand;
                    }
                }

                if (!baseKeys.LeftKey() && LeftRunning && this.state != States.GettingHurt && !Swinging)
                {
                    LeftRunning = false;
                    LeftStanding = true;

                    if (!Hanging && Map.Locations[(int)(Pos.Y / S.MapsScale), (int)(Pos.X / S.MapsScale)] != GroundType.Air)
                    {
                        this.state = States.Stand;
                    }
                }

                #endregion

                #region Simaba's_Punches (When adult)

                if (this.AnimalType == AnimalType.Adult_Simba)
                {
                    if (DelayBetweenSlashes < 20)
                    {
                        DelayBetweenSlashes += 1;
                    }

                    if (!jumping && !PressedPunch && !Hanging && !Swinging && !Crouching && this.state != States.Roar &&
                        this.state != States.ThrowingEnemy && this.state != States.GettingHurt && baseKeys.PunchKey())
                    {
                        PressedPunch = true;

                        if (DelayBetweenSlashes == 20)
                        {
                            DelayBetweenSlashes = 0;
                            this.state = States.Slash;
                        }
                    }

                    if (baseKeys.ShiftKey() && PressedPunch && !DoubleSlashing && !jumping && !Swinging)
                    {
                        for (int i = 0; i < Level.Characters.Count; i++)
                        {
                            if (Level.Characters[i].AnimalType == AnimalType.lion && !ThrowingAway &&
                                ((this.Pos.X < Level.Characters[i].Pos.X && Level.Characters[i].Pos.X - this.Pos.X <= 200f &&
                                this.effects == SpriteEffects.None && Level.Characters[i].effects == SpriteEffects.FlipHorizontally) ||
                                (this.Pos.X > Level.Characters[i].Pos.X && this.Pos.X - Level.Characters[i].Pos.X <= 200f &&
                                this.effects == SpriteEffects.FlipHorizontally && Level.Characters[i].effects == SpriteEffects.None)))
                            {
                                ThrowingAway = true;

                                if (this.effects == SpriteEffects.None)
                                {
                                    this.Pos = Level.Characters[i].Pos + new Vector2(-200f, 0);
                                }

                                else if (this.effects == SpriteEffects.FlipHorizontally)
                                {
                                    this.Pos = Level.Characters[i].Pos + new Vector2(200f, 0);
                                }

                                break;
                            }

                            else
                            {
                                DoubleSlashing = true;
                                this.state = States.DoubleSlash;
                            }
                        }
                    }

                    if (!baseKeys.PunchKey() && PressedPunch)
                    {
                        PressedPunch = false;
                        DoubleSlashing = false;
                    }
                }

                #endregion

                #region Simba vs Hyena boss sound music (in the first level)

                if (this.AnimalType == AnimalType.Cub_Simba && Level.LevelNumber == 1 && !S.HyenaBossFight &&
                    this.Pos.X >= 3270f && this.Pos.Y < 850f && this.Pos.Y > 600f)
                {
                    S.HyenaBossFight = true;
                    LionSounds.firstlevelsonginstance.Stop();
                    LionSounds.hyenabosssoundinstance.Play();
                }

                #endregion

                #region Throwing_Away

                if (ThrowingAway)
                {
                    ThrowingAway = false;
                    DelayBetweenSlashes = 0;
                    S.SimbaPosAtThrowing = this.Pos;
                    this.state = States.ThrowingEnemy;
                }


                #endregion

                #region Crouch + CrouchSlash

                if ((baseKeys.DownKey() && !baseKeys.RightKey() && !baseKeys.LeftKey() && !Crouching && !falling && !jumping &&
                    this.state != States.Hanging && this.state != States.Climb && !Swinging &&
                    this.state != States.DoubleSlash && this.state != States.Slash
                    && this.state != States.GettingHurt && this.state != States.GettingSlashed
                    && this.state != States.AfterFalling && this.state != States.ThrowingEnemy && this.state != States.Roar &&
                    Map.Locations[(int)(Pos.Y / S.MapsScale), (int)(Pos.X / S.MapsScale)] != GroundType.Air)
                    || (IsAnimationOver && Crouching))
                {
                    Crouching = true;
                    this.state = States.Crouch;
                }

                if (baseKeys.PunchKey() && !PressedPunch && Crouching)
                {
                    PressedPunch = true;

                    if (DelayBetweenSlashes == 20)
                    {
                        DelayBetweenSlashes = 0;
                        this.state = States.CrouchSlash;
                    }
                }

                if (!baseKeys.DownKey() && Crouching && this.state != States.CrouchSlash)
                {
                    this.state = States.Stand;
                    Crouching = false;
                }

                #endregion

                #region Roaring

                //if (DelayBetweenRoars < 45)
                //{
                //    DelayBetweenRoars += 1;
                //}

                if (baseKeys.RoarKey() && !PressedRoar && !Hanging && !Swinging &&
                    !baseKeys.RightKey() && !baseKeys.LeftKey() && !Crouching && !ThrowingAway && !jumping &&
                    this.state != States.Slash && this.state != States.DoubleSlash && this.state != States.AfterFalling)
                {
                    PressedRoar = true;
                    this.state = States.Roar;
                }

                if (!baseKeys.RoarKey())
                {
                    PressedRoar = false;
                }

                #endregion

                #region Jump

                if (!S.SimbaWon && baseKeys.UpKey() && !baseKeys.RoarKey() && !jumping && !Hanging &&
                    this.state != States.DoubleSlash && this.state != States.Slash && this.state != States.GettingSlashed &&
                    this.state != States.GettingHurt && this.state != States.CrouchSlash && this.state != States.Climb &&
                    this.state != States.ThrowingEnemy && this.state != States.AfterFalling &&
                    Map.Locations[(int)(Pos.Y / S.MapsScale), (int)(Pos.X / S.MapsScale)] != GroundType.Air)
                {
                    jumping = true;
                    WasJumping = true;
                    StartFalling_Y = this.Pos.Y / S.MapsScale;
                    this.state = States.Jump;

                    if (Swinging)
                    {
                        jumpspeed = -10.5f; // נותן לו דחיפה כלפי מעלה
                    }
                    else
                    {
                        jumpspeed = -16f; // נותן לו דחיפה כלפי מעלה
                    }
                }

                if (jumping)
                {
                    Pos += new Vector2(0, jumpspeed);

                    if (jumpspeed <= 25f) // הגבלת מהירות
                    {
                        jumpspeed += 0.6f; // באיזה קצב תהיה הקפיצה
                    }

                    if (this.AnimalType == AnimalType.Cub_Simba || this.AnimalType == AnimalType.Adult_Simba)
                    {
                        foreach (Vector2 hangingData in Map.HangLocations)
                        {
                            StaticHangingData = hangingData;

                            if (!Hanging && jumpspeed >= 0 &&
                                Math.Abs((new Vector2(Pos.X / S.MapsScale, Pos.Y / S.MapsScale)
                                - new Vector2(hangingData.X, hangingData.Y)).Length()) <= 15f &&
                                ((this.effects == SpriteEffects.FlipHorizontally &&
                                (Map.Locations[(int)(hangingData.Y),
                                (int)((hangingData.X - 5f))] == GroundType.Platform ||
                                Map.Locations[(int)(hangingData.Y),
                                (int)((hangingData.X - 5f))] == GroundType.Blocked)) ||
                                (this.effects == SpriteEffects.None &&
                                (Map.Locations[(int)(hangingData.Y),
                                (int)((hangingData.X + 5f))] == GroundType.Platform ||
                                Map.Locations[(int)(hangingData.Y),
                                (int)((hangingData.X + 5f))] == GroundType.Blocked))))
                            {
                                if (Map.Locations[(int)(hangingData.Y),
                                    (int)((hangingData.X - 5f))] == GroundType.Blocked ||
                                    Map.Locations[(int)(hangingData.Y),
                                    (int)((hangingData.X + 5f))] == GroundType.Blocked)
                                {
                                    HangingOnSmallRock = true;
                                }

                                Hanging = true;
                                jumping = false;
                                this.state = States.Hanging;
                                this.Pos = new Vector2(
                                    hangingData.X * S.MapsScale,
                                    hangingData.Y * S.MapsScale);

                                break;
                            }
                        }

                        if (this.AnimalType == AnimalType.Adult_Simba)
                        {
                            foreach (Vector2 SwingData in Map.SwingLocations)
                            {
                                if (!Swinging && jumpspeed >= 0 &&
                                    Math.Abs((new Vector2(Pos.X / S.MapsScale, Pos.Y / S.MapsScale)
                                    - new Vector2(SwingData.X, SwingData.Y)).Length()) <= 25f &&
                                    ((this.effects == SpriteEffects.FlipHorizontally &&
                                    Map.Locations[(int)(SwingData.Y),
                                    (int)((SwingData.X - 5f))] == GroundType.Air) ||
                                    (this.effects == SpriteEffects.None &&
                                    Map.Locations[(int)(SwingData.Y),
                                    (int)((SwingData.X + 5f))] == GroundType.Air)))
                                {
                                    Swinging = true;
                                    jumping = false;
                                    this.state = States.Swinging;

                                    if (this.effects == SpriteEffects.None)
                                    {
                                        S.offset = -0.08f;
                                    }
                                    else
                                    {
                                        S.offset = 0.08f;
                                    }

                                    this.Pos = new Vector2(
                                        SwingData.X * S.MapsScale,
                                        SwingData.Y * S.MapsScale);

                                    break;
                                }
                            }
                        }
                    }
                }

                if (Map.Locations[(int)(Pos.Y / S.MapsScale), (int)(Pos.X / S.MapsScale)] == GroundType.Air || falling)
                {
                    for (int i = 0; i < Math.Abs(jumpspeed); i++)
                    {
                        if (Map.Locations[(int)((Pos.Y + i) / S.MapsScale), (int)(Pos.X / S.MapsScale)] == GroundType.Platform)
                        {
                            jumpspeed = i;
                            Pos += new Vector2(0, jumpspeed); // לגרום לדמות לעלות מעלה על משטח שטוח
                            jumping = false;
                            falling = false;
                            EndFalling_Y = this.Pos.Y / S.MapsScale;

                            if ((this.AnimalType == AnimalType.Cub_Simba || this.AnimalType == AnimalType.Adult_Simba) &&
                                (WasFalling || WasJumping) && EndFalling_Y - StartFalling_Y >= 200f)
                            {
                                this.state = States.AfterFalling;
                            }

                            else
                            {
                                this.state = States.Stand;
                            }

                            WasJumping = false;
                            WasFalling = false;

                            break;
                        }

                        else
                        {
                            for (int k = 0; k < 10; k++)
                            {
                                if (Map.Locations[(int)((Pos.Y - k) / S.MapsScale),
                                    (int)(Pos.X / S.MapsScale)] == GroundType.Platform
                                    && !jumping)
                                {
                                    this.Pos = new Vector2(Pos.X, Pos.Y - k);
                                }
                            }

                            if (falling) // נפילה מתלייה
                            {
                                falling = false;
                                WasFalling = true;
                                GeneralFalling = true;
                            }
                        }
                    }
                }


                #endregion

                #region Hanging + Climbing

                if (Hanging)
                {
                    if ((baseKeys.LeftKey() &&
                        this.effects == SpriteEffects.FlipHorizontally &&
                        Map.Locations[(int)(StaticHangingData.Y),
                        (int)((StaticHangingData.X - 5f))] == GroundType.Platform) ||
                        (baseKeys.RightKey() &&
                        this.effects == SpriteEffects.None &&
                        Map.Locations[(int)(StaticHangingData.Y),
                        (int)((StaticHangingData.X + 5f))] == GroundType.Platform))
                    {
                        PressedDirectionToClimb = true;
                    }

                    if (HangingOnSmallRock && baseKeys.UpKey() && (baseKeys.RightKey() || baseKeys.LeftKey()))
                    {
                        if ((this.effects == SpriteEffects.None && baseKeys.LeftKey()) ||
                            (this.effects == SpriteEffects.FlipHorizontally && baseKeys.RightKey()))
                        {
                            jumping = true;
                            jumpspeed = -12f;
                            this.state = States.Jump;
                        }
                    }
                }

                if (PressedDirectionToClimb)
                {
                    this.state = States.Climb;

                    PressedDirectionToClimb = false;
                }

                if ((jumping) || (!PressedDirectionToClimb && IsAnimationOver && Hanging))
                {
                    Hanging = false;
                    HangingOnSmallRock = false;
                }

                #endregion

                #region Swinging

                if (Swinging)
                {
                    S.timer++;

                    this.Rot += S.offset;

                    if (S.timer == 30)
                    {
                        S.timer = 0;
                        S.offset *= -1;
                    }

                    if (jumping)
                    {
                        this.state = States.Running_Jump;
                        Swinging = false;
                        S.timer = 0;
                        this.Rot = 0;
                    }
                }

                #endregion

                #region General falling & After hanging/swinging falling

                if (baseKeys.DownKey() && (this.state == States.Hanging || this.state == States.Swinging) &&
                    !falling && !Crouching && !Swinging)
                {
                    falling = true;
                    WasFalling = true;
                    Hanging = false;
                    Swinging = false;
                    jumpspeed = -1.1f;
                    StartFalling_Y = this.Pos.Y / S.MapsScale;
                    this.state = States.Falling;
                }

                if (this.state == States.Falling && EndFalling_Y - StartFalling_Y < 200f &&
                    Map.Locations[(int)(Pos.Y / S.MapsScale), (int)(Pos.X / S.MapsScale)] == GroundType.Platform)
                {
                    this.state = States.Stand;
                }

                if (Map.Locations[(int)(Pos.Y / S.MapsScale), (int)(Pos.X / S.MapsScale)] == GroundType.Air &&
                    !jumping && !GeneralFalling)
                {
                    GeneralFalling = true;
                }
                if (GeneralFalling)
                {
                    Fall();
                    GeneralFalling = false;
                }
                else
                {
                    NumOfSeconds = 0;
                }

                #endregion

                #region Big Animals speed determination

                if (this.AnimalType == AnimalType.lion || this.AnimalType == AnimalType.hyena)
                {
                    if (this.jumping)
                    {
                        RightRunSpeed = 6f;
                        LeftRunSpeed = -6f;
                    }
                    else
                    {
                        RightRunSpeed = 5f;
                        LeftRunSpeed = -5f;
                    }
                }

                #endregion

                #region Collision

                if (this.AnimalType == AnimalType.Adult_Simba || this.AnimalType == AnimalType.Cub_Simba) // Collision with simba
                {
                    for (int i = 0; i < Level.Characters.Count; i++)
                    {
                        Collision.CheckCollision(this, this.index, Level.Characters[i], Level.Characters[i].index);
                    }
                }

                #endregion
            }
        }

        /// <summary>
        /// Drawing the character
        /// </summary>
        public void DrawAnimal()
        {
            base.DrawObject();
        }

        /// <summary>
        /// Falling, according the physics rules
        /// </summary>
        public void Fall()
        {
            // זו הגבלת מהירות - משמעות התנאי היא שהמהירות המקסימלית מושגת כעבור שנייה וחצי
            if (NumOfSeconds / 60 <= 1.5f)
            {
                NumOfSeconds++;
            }

            // ה 11 הזה זה מהירות התחלתית
            Pos += new Vector2(0, 11f + 0.5f * gravity * ((NumOfSeconds * NumOfSeconds) / 3600f));
        }
    }
}