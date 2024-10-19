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
    enum OnlineState
    {
        AskingRole, // host or join
        Connecting,
        Playing,
        host,
        join
    }

    enum GameStates
    {
        Play, Pause, Exit, Online, MainMenu
    }


    static class S
    {
        #region DATA
        // public static OnlineGame onlineGame; // אם האונליין קורס, אולי צריך להוציא את זה מהערה
        public static ContentManager cm;
        public static SpriteBatch spb;
        public static GraphicsDevice gd;
        public static GraphicsDeviceManager gdm;
        public static GameStates gameState;
        public static SpriteFont GameFont;
        public static Dictionary<GameStates, Menu> AllMenues;
        public static float MapsScale;
        public static float GameColorLevel;
        public static bool HyenaBossFight;
        public static bool SimbaWon;
        public static bool AfterWinning;
        public static int CounterLives;
        public static int MinutesCounter;
        public static int SecondsCounter;
        public static Texture2D Pointer;
        public static Vector2 PointerPos;
        public static Texture2D PauseMessage;
        public static Vector2 PauseMessageLocation;
        public static Texture2D FirstLevelAreaName;
        public static Texture2D SecondLevelAreaName;
        public static int LevelAreaNameAppearanceTime;
        public static Texture2D WinningMessage;
        public static Vector2 WinningMessageLocation;
        public static Vector2 SimbaCopyLocation;
        public static Texture2D[] CubSimbaLives;
        public static Texture2D MutePic;

        public static Vector2 ScarPos; // Initialized in BotKeys class
        public static Vector2 SimbaPosAtThrowing;

        public static int timer;
        public static float offset;

        #endregion

        #region INIT

        /// <summary>
        /// Initializing the parameters
        /// </summary>
        /// <param name="gdm"></param>
        /// <param name="spb"></param>
        /// <param name="cm"></param>
        public static void Init(GraphicsDeviceManager gdm, SpriteBatch spb, ContentManager cm)
        {
            S.gdm = gdm;
            S.gd = gdm.GraphicsDevice;
            S.spb = spb;
            S.cm = cm;
            S.gameState = GameStates.MainMenu;
            S.GameFont = S.cm.Load<SpriteFont>("Font");

            S.AllMenues = new Dictionary<GameStates, Menu>();
            S.AllMenues.Add(GameStates.MainMenu, new mainMenu());
            S.AllMenues.Add(GameStates.Online, new OnlineMenu());

            S.MapsScale = 2.5f;
            S.GameColorLevel = 1f;
            S.HyenaBossFight = false;
            S.SimbaWon = false;
            S.Pointer = S.cm.Load<Texture2D>("Pointer");
            S.PointerPos = Vector2.Zero;
            S.PauseMessage = S.cm.Load<Texture2D>("PauseMessage");
            S.PauseMessageLocation = new Vector2((Game1.ScreenWidth / 2), (Game1.ScreenHeight / 2));
            S.WinningMessageLocation = new Vector2(50, 50);
            S.WinningMessage = S.cm.Load<Texture2D>("WinningMessage");

            S.FirstLevelAreaName = S.cm.Load<Texture2D>("ThePrideLands");
            S.SecondLevelAreaName = S.cm.Load<Texture2D>("PrideRock");

            S.MutePic = S.cm.Load<Texture2D>("Mute");
            S.CounterLives = 5;
            S.MinutesCounter = 0;
            S.SecondsCounter = 0;
            S.AfterWinning = false;

            S.timer = 0;
            S.offset = 0; // Fictive value

            CubSimbaLives = new Texture2D[6];

            CubSimbaLives[0] = S.cm.Load<Texture2D>("Lives/ZeroLives");
            CubSimbaLives[1] = S.cm.Load<Texture2D>("Lives/OneLives");
            CubSimbaLives[2] = S.cm.Load<Texture2D>("Lives/TwoLives");
            CubSimbaLives[3] = S.cm.Load<Texture2D>("Lives/ThreeLives");
            CubSimbaLives[4] = S.cm.Load<Texture2D>("Lives/FourLives");
            CubSimbaLives[5] = S.cm.Load<Texture2D>("Lives/FiveLives");
        }

        #endregion
    }
}