using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace xxx
{
    public delegate void UpdateDelegate();
    public delegate void DrawDelegate();

    class Game1 : Microsoft.Xna.Framework.Game
    {
        #region Variables

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static UpdateDelegate UPDATE_EVENT;
        public static DrawDelegate DRAW_EVENT;

        public OnlineGame onlineGame;
        public static OnlineState Onlinestate;

        public static int ScreenHeight;
        public static int ScreenWidth;

        public static bool exitGame;
        public static bool IsPaused;
        public static bool PressedPaused;
        public static bool IsMute;
        public static bool PressedMute;

        public static Texture2D ItemPic;
        public static Text TimeText;
        public static string time;

        public static ParticlesGenerator rain;
        public static ParticlesGenerator fire;

        #endregion

        /// <summary>
        /// Game1 constructor
        /// </summary>
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;

            ScreenHeight = graphics.PreferredBackBufferHeight;
            ScreenWidth = graphics.PreferredBackBufferWidth;

            graphics.IsFullScreen = true;
        }

        /// <summary>
        /// Initalizing Game1 objects
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            TheDict.Init();
        }

        /// <summary>
        /// Loading the content to the graphics card from the pipeline
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            S.Init(graphics, spriteBatch, Content);

            Onlinestate = OnlineState.AskingRole;

            LionSounds.init();

            TimeText = new Text(" ", new Vector2(1.6f), Color.Red, new Vector2(5, 0));

            ItemPic = S.cm.Load<Texture2D>("PowerUp");

            rain = new ParticlesGenerator(Content.Load<Texture2D>("rainDrop"),
                graphics.GraphicsDevice.Viewport.Width, 100);

            fire = new ParticlesGenerator(Content.Load<Texture2D>("flame"),
                graphics.GraphicsDevice.Viewport.Width, 40);
        }

        protected override void UnloadContent()
        {

        }

        /// <summary>
        /// Updating all the game objects
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(GameTime gameTime)
        {
            #region Displaying menues options

            if (Keyboard.GetState().IsKeyDown(Keys.Back) &&
                S.gameState != GameStates.Play &&
                S.gameState != GameStates.Pause)
            {
                S.gameState = GameStates.MainMenu;
            }

            if (S.gameState == GameStates.MainMenu ||
                S.gameState == GameStates.Online)
            {
                S.AllMenues[S.gameState].Update();

                if (exitGame)
                {
                    Exit();
                }
            }

            #endregion

            if (S.gameState == GameStates.Play)
            {
                #region Displaying time passed

                if (!Game1.IsPaused && !S.SimbaWon)
                {
                    S.SecondsCounter++;
                    S.LevelAreaNameAppearanceTime++;
                }

                if ((S.SecondsCounter / 60) == 60)
                {
                    S.SecondsCounter = 0;
                    S.MinutesCounter++;
                }

                time = (S.MinutesCounter / 10) + "" + (S.MinutesCounter) + ":" +
                    ((S.SecondsCounter / 60) / 10) + "" + ((S.SecondsCounter / 60) % 10);
                TimeText.text = "Time passed: " + time;

                #endregion

                #region Game muting

                if (Keyboard.GetState().IsKeyDown(Keys.M) && !PressedMute)
                {
                    PressedMute = true;
                }

                if (!Keyboard.GetState().IsKeyDown(Keys.M) && PressedMute)
                {
                    IsMute = !IsMute;
                    PressedMute = false;

                    if (IsMute)
                    {
                        foreach (SoundEffectInstance SoundInstance in LionSounds.AllSoundEffectsInstances)
                        {
                            SoundInstance.Volume = 0;
                        }
                    }
                    else
                    {
                        foreach (SoundEffectInstance SoundInstance in LionSounds.AllSoundEffectsInstances)
                        {
                            
                            SoundInstance.Volume = 1f;
                        }
                    }
                }

                #endregion

                #region Game pausing

                if (Keyboard.GetState().IsKeyDown(Keys.P) && !PressedPaused)
                {
                    PressedPaused = true;
                }

                if (!Keyboard.GetState().IsKeyDown(Keys.P) && PressedPaused && !S.SimbaWon)
                {
                    IsPaused = !IsPaused;
                    PressedPaused = false;

                    if (IsPaused)
                    {
                        if (LionSounds.firstlevelsonginstance.State == SoundState.Playing)
                            LionSounds.firstlevelsonginstance.Pause();
                        if (LionSounds.hyenabosssoundinstance.State == SoundState.Playing)
                            LionSounds.hyenabosssoundinstance.Pause();
                        if (LionSounds.secondlevelsonginstance.State == SoundState.Playing)
                            LionSounds.secondlevelsonginstance.Pause();
                        if (LionSounds.finalsoundinstance.State == SoundState.Playing)
                            LionSounds.finalsoundinstance.Pause();

                        S.GameColorLevel = 0.75f;
                    }
                    else
                    {
                        if (LionSounds.firstlevelsonginstance.State == SoundState.Paused)
                            LionSounds.firstlevelsonginstance.Resume();
                        if (LionSounds.hyenabosssoundinstance.State == SoundState.Paused)
                            LionSounds.hyenabosssoundinstance.Resume();
                        if (LionSounds.secondlevelsonginstance.State == SoundState.Paused)
                            LionSounds.secondlevelsonginstance.Resume();
                        if (LionSounds.finalsoundinstance.State == SoundState.Paused)
                            LionSounds.finalsoundinstance.Resume();

                        S.GameColorLevel = 1f;
                    }
                }

                #endregion

                #region Exiting game

                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    Exit();
                }

                #endregion

                #region Updates the fire flames (In the bottom of the second stage)

                if (Level.LevelNumber == 2)
                {
                    if (ParticlesGenerator.DecreaseHeight > 20 && !IsPaused)
                    {
                        fire.Update(gameTime, graphics.GraphicsDevice, "Flame");
                    }
                }

                #endregion

                #region Updates the rain drops (After simba won)

                if (S.AfterWinning)
                {
                    rain.Update(gameTime, graphics.GraphicsDevice, "RainDrop");
                }

                #endregion

                if (UPDATE_EVENT != null)
                {
                    UPDATE_EVENT();
                }
            }

            #region Online Game

            if (S.gameState == GameStates.Online)
            {
                switch (Onlinestate)
                {
                    case OnlineState.AskingRole:
                        break;

                    case OnlineState.host:
                        Console.WriteLine("Switch host");
                        onlineGame = new HostOnlineGame(int.Parse(File.ReadAllText("port.txt")));
                        onlineGame.OnConnection += new OnConnectionHandler(onlineGame_OnConnection);
                        onlineGame.Init();
                        Onlinestate = OnlineState.Connecting;
                        break;

                    case OnlineState.join:
                        Console.WriteLine("Switch join");
                        onlineGame = new JoinOnlineGame(File.ReadAllText("ip.txt"), int.Parse(File.ReadAllText("port.txt")));
                        onlineGame.OnConnection += new OnConnectionHandler(onlineGame_OnConnection);
                        onlineGame.Init();
                        Onlinestate = OnlineState.Connecting;
                        break;

                    case OnlineState.Connecting:
                        break;

                    case OnlineState.Playing:
                        onlineGame.hostChar.Update();
                        onlineGame.joinChar.Update();
                        break;
                }
            }

            #endregion

            base.Update(gameTime);
        }

        /// <summary>
        /// Drawing all the game objects
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Transparent);

            #region Menues Drawing + Menues' Pointer

            if (S.gameState == GameStates.MainMenu ||
                S.gameState == GameStates.Online)
            {
                spriteBatch.Begin();

                S.AllMenues[S.gameState].DrawMenues();
                S.spb.Draw(S.Pointer, S.PointerPos, null, Color.White * S.GameColorLevel, 0f,
                           new Vector2(0, 0), 0.55f, SpriteEffects.None, 1f);

                spriteBatch.End();
            }

            #endregion

            #region Game Drawing

            else
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Level.cam.Mat);

                S.spb.Draw(Level.BackGroundImage, new Vector2(0, 0), null, Color.White * S.GameColorLevel, 0f,
                        new Vector2(0, 0), S.MapsScale, SpriteEffects.None, 1f);

                if (DRAW_EVENT != null)
                    DRAW_EVENT();

                spriteBatch.End();

                // ----------------------------------------------------------------------------------------------------------

                spriteBatch.Begin();

                if (Level.LevelNumber == 2)
                {
                    if (!IsPaused)
                        fire.Draw(spriteBatch, Color.White);
                    else
                        fire.Draw(spriteBatch, Color.White * 0.25f);
                }

                if ((S.LevelAreaNameAppearanceTime / 60) <= 1)
                {
                    if (Level.LevelNumber == 1)
                    {
                        spriteBatch.Draw(S.FirstLevelAreaName, new Vector2(ScreenWidth / 2, ScreenHeight / 2),
                        null, Color.White * S.GameColorLevel, 0f,
                        new Vector2(S.FirstLevelAreaName.Width / 2, S.FirstLevelAreaName.Height / 2),
                        2f, SpriteEffects.None, 1f);
                    }
                    else
                    {
                        spriteBatch.Draw(S.SecondLevelAreaName, new Vector2(ScreenWidth / 2, ScreenHeight / 2),
                        null, Color.White * S.GameColorLevel, 0f,
                        new Vector2(S.SecondLevelAreaName.Width / 2, S.SecondLevelAreaName.Height / 2),
                        0.8f, SpriteEffects.None, 1f);
                    }
                }

                if (Game1.IsPaused)
                    S.spb.Draw(S.PauseMessage, S.PauseMessageLocation, null, Color.White, 0f,
                        new Vector2(S.PauseMessage.Width / 2, S.PauseMessage.Height / 2), 1f, SpriteEffects.None, 1f);

                if (Keyboard.GetState().IsKeyDown(Keys.Tab) && !S.SimbaWon)
                    Level.minimap.Draw();

                if (!S.SimbaWon)
                    TimeText.Draw();
                else
                {
                    S.spb.Draw(S.WinningMessage, S.WinningMessageLocation, null, Color.White, 0f,
                      new Vector2(0, 0), 1f, SpriteEffects.None, 1f);

                    TimeText.text = "  You finished the game in\n         " +
                        DateTime.Now.Date.Day + "/" + DateTime.Now.Date.Month + "/" + DateTime.Now.Date.Year + "\n" +
                        " and played a total time of:\n           " + time + "\n    Hope you enjoyed !!!";

                    TimeText.position = new Vector2(-20, 140);

                    if (S.AfterWinning)
                    {
                        TimeText.Draw();
                        rain.Draw(spriteBatch, Color.White);
                    }
                }

                spriteBatch.Draw(Level.SimbaFace,
                    new Vector2(0, ScreenHeight - (1.55f * Level.SimbaFace.Height)),
                    null, Color.White * S.GameColorLevel, 0f, new Vector2(0, 0), 1.5f, SpriteEffects.None, 1f);

                spriteBatch.Draw(S.CubSimbaLives[S.CounterLives],
                    new Vector2(Level.SimbaFace.Width + 30, ScreenHeight - Level.SimbaFace.Height - 10),
                    null, Color.White * S.GameColorLevel, 0f, new Vector2(0, 0), 1.5f, SpriteEffects.None, 1f);

                spriteBatch.End();

                if (IsMute)
                {
                    spriteBatch.Begin();

                    spriteBatch.Draw(S.MutePic, new Vector2(ScreenWidth, 0), null, Color.White, 0f,
                        new Vector2(S.MutePic.Width, 0), 0.3f, SpriteEffects.None, 1f);

                    spriteBatch.End();
                }

                // ----------------------------------------------------------------------------------------------------------
            }

            #endregion

            base.Draw(gameTime);
        }

        /// <summary>
        /// Checking if there was a connection between host and join players
        /// </summary>
        void onlineGame_OnConnection()
        {
            Console.WriteLine("found a connection !!!");
            Onlinestate = OnlineState.Playing;
            S.gameState = GameStates.Play;
        }
    }
}