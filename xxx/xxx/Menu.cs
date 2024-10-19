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
    class Button
    {
        String text;
        public String name;
        public Vector2 Pos;
        public Color color;

        /// <summary>
        /// Button's constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="text"></param>
        /// <param name="pos"></param>
        public Button(String name, String text, Vector2 pos)
        {
            this.name = name;
            this.text = text;
            this.Pos = pos;
            this.color = Color.Red;
        }

        /// <summary>
        /// Reading the each button's name and changing the the game/online states respectively
        /// </summary>
        /// <param name="name"></param>
        public static void ReadButtonName(string name)
        {
            try
            {
                S.gameState = (GameStates)Enum.Parse(typeof(GameStates), name);

                if (name == "MainMenu")
                {
                    // S.ReloadGame();
                }
            }

            catch
            {
                if (name == "SinglePlayer")
                {
                    Level.InitFirstLevel();
                    // Level.InitSecondLevel();
                    S.gameState = GameStates.Play;
                }

                if (name == "exit")
                {
                    Game1.exitGame = true;
                }

                if (name == "Online")
                {
                    S.gameState = GameStates.Online;
                }
            }


            try
            {
                Game1.Onlinestate = (OnlineState)Enum.Parse(typeof(OnlineState), name);

                if (name == "host")
                {
                    Console.WriteLine("Pressed Host Button");
                    Game1.Onlinestate = OnlineState.host;
                }

                if (name == "join")
                {
                    Console.WriteLine("Pressed Join Button");
                    Game1.Onlinestate = OnlineState.join;
                }
            }

            catch { }
        }

        /// <summary>
        /// Changing the online game state to "playing"
        /// </summary>
        public static void onlineGame_OnConnection()
        {
            Game1.Onlinestate = OnlineState.Playing;
        }

        /// <summary>
        /// Drawing the button
        /// </summary>
        public void drawButton()
        {
            S.spb.DrawString(S.GameFont, this.text, this.Pos, this.color);
        }
    }

    class Text
    {
        public string text;
        public Vector2 scale;
        public Color color;
        public Vector2 position;

        public Text(string text, Vector2 scale, Color color, Vector2 position)
        {
            this.color = color;
            this.text = text;
            this.scale = scale;
            this.position = position;
        }

        public void Draw()
        {
            S.spb.DrawString(S.GameFont, text, position, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 1);
        }
    }

    class Menu
    {
        public List<Button> buttons = new List<Button>();
        public List<Text> texts = new List<Text>();
        public int select;
        public int numberButtons;
        KeyboardState lastState;
        KeyboardState curState;

        /// <summary>
        /// Updating each menu's data
        /// </summary>
        public void Update()
        {
            #region update selection

            lastState = curState;
            curState = Keyboard.GetState();

            if (curState == lastState && (curState.IsKeyDown(Keys.Up) || curState.IsKeyDown(Keys.Down) ||
                curState.IsKeyDown(Keys.Enter)))
                return;

            if (buttons.Count == 0)
                return;

            buttons[select].color = Color.Red;

            if (curState.IsKeyDown(Keys.Up))
            {
                select--;
            }

            if (curState.IsKeyDown(Keys.Down))
            {
                select++;
            }

            if (select < 0)
            {
                select = numberButtons;
            }

            if (select > numberButtons)
            {
                select = 0;
            }

            S.PointerPos = new Vector2(buttons[select].Pos.X - 30, buttons[select].Pos.Y + 4);

            #endregion

            #region The Selection

            if (curState.IsKeyUp(Keys.Enter) && lastState.IsKeyDown(Keys.Enter))
            {
                Button.ReadButtonName(buttons[select].name);

                if (buttons[select].name == "host" || buttons[select].name == "join")
                {
                    buttons = new List<Button>();
                }
            }

            #endregion
        }

        public void DrawMenues()
        {
            try
            {
                string menuName = S.gameState.ToString();

                WallPapers sp = (WallPapers)Enum.Parse(typeof(WallPapers), menuName);
                Texture2D wp = TheDict.WallPapersDic[sp];
                // Rectangle screenRectangle = new Rectangle(0, 0, Game1.ScreenWidth, Game1.ScreenHeight);
                // S.spb.Draw(wp, screenRectangle, Color.White);
                S.spb.Draw(wp, S.gd.Viewport.Bounds, Color.White);
            }
            catch { }

            foreach (Button button in buttons)
            {
                buttons[select].color = Color.Blue;
                button.drawButton();

                foreach (Text text in texts)
                {
                    text.Draw();
                }
            }
        }
    }

    class mainMenu : Menu
    {
        /// <summary>
        /// mainMenu's Constructor
        /// </summary>
        public mainMenu()
        {
            buttons = new List<Button>();

            buttons.Add(new Button("SinglePlayer", "Start Game", new Vector2(Game1.ScreenWidth / 2 - 75, Game1.ScreenHeight / 2 - 85)));
            buttons.Add(new Button("Online", "Play Online", new Vector2(Game1.ScreenWidth / 2 - 85, Game1.ScreenHeight / 2 - 35)));
            buttons.Add(new Button("exit", "Exit", new Vector2(Game1.ScreenWidth / 2 - 35, Game1.ScreenHeight / 2 + 15)));

            numberButtons = buttons.Count - 1;
        }
    }

    class OnlineMenu : Menu
    {
        /// <summary>
        /// OnlineMenu's Constructor
        /// </summary>
        public OnlineMenu()
        {
            buttons = new List<Button>();

            buttons.Add(new Button("host", "Host", new Vector2(620, 420)));
            buttons.Add(new Button("join", "Join", new Vector2(617, 470)));
            buttons.Add(new Button("exit", "Exit", new Vector2(620, 520)));
            numberButtons = buttons.Count - 1;
        }
    }
}