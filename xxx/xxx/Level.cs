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
    static class Level
    {
        #region Variables

        public static int LevelNumber;
        public static Animal hero;
        public static Animal scar;
        public static Animal lizard;
        public static Animal hedgehog;
        public static Animal beetle;
        public static Animal hyena;
        public static List<Animal> Characters;

        public static Texture2D BackGroundImage;
        public static Texture2D LogicBackGround;
        public static Texture2D SimbaFace;
        public static MiniMap minimap;
        public static Camera cam;

        public static List<Vector2> LiazrdsLocatins;
        public static List<Vector2> HedgehogsLocatins;
        public static List<Vector2> BeetlesLocatins;

        public static float offset = 500;

        public static List<Item> FirstLevelItems;
        public static List<Item> SecondLevelItems;

        public static float c = 960;

        #endregion

        public static void InitLizardsLocations()
        {
            LiazrdsLocatins.Add(new Vector2(920, 3170));
            LiazrdsLocatins.Add(new Vector2(1650, 3170));
            LiazrdsLocatins.Add(new Vector2(2390, 2650));
            LiazrdsLocatins.Add(new Vector2(1600, 2620));
            LiazrdsLocatins.Add(new Vector2(520, 2400));
            LiazrdsLocatins.Add(new Vector2(1250, 1675));
            LiazrdsLocatins.Add(new Vector2(1440, 1722));
            LiazrdsLocatins.Add(new Vector2(1400, 2272));
            LiazrdsLocatins.Add(new Vector2(2940, 1945));
        }

        public static void InitHedgehogsLocations()
        {
            HedgehogsLocatins.Add(new Vector2(2100, 3170));
            HedgehogsLocatins.Add(new Vector2(1100, 2620));
        }

        public static void InitBeetlesLocatins()
        {
            BeetlesLocatins.Add(new Vector2(1000, 2350));
            BeetlesLocatins.Add(new Vector2(1778, 2220));
            BeetlesLocatins.Add(new Vector2(2222, 1917));
        }

        /// <summary>
        /// Initializing all the first level necessary data
        /// </summary>
        public static void InitFirstLevel()
        {
            LevelNumber = 1;

            LogicBackGround = S.cm.Load<Texture2D>("Stages/LogicFirststage");
            BackGroundImage = S.cm.Load<Texture2D>("Stages/Firststage");
            SimbaFace = S.cm.Load<Texture2D>("SimbaFaces/CubSimbaFace");
            LionSounds.mainmenusoundinstance.Stop();
            LionSounds.firstlevelsonginstance.Play();
            S.LevelAreaNameAppearanceTime = 0;

            // 500 3170
            // 3270 850
            hero = new Animal(Folders.Cub_Simba, States.Stand, S.spb, new Vector2(500, 3170), null, Color.White, 0f,
                new Vector2(0, 0), new Vector2(2.2f), SpriteEffects.None, 1f, AnimalType.Cub_Simba);
            hero.baseKeys = new UserBaseKeys(Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.LeftShift, Keys.Space, Keys.Z);

            cam = new Camera(hero, new Vector2(1f), new Viewport(0, 0, Game1.ScreenWidth, Game1.ScreenHeight));

            Map.check(LogicBackGround);
            minimap = new MiniMap(BackGroundImage, new Vector2(S.MapsScale, S.MapsScale));

            FirstLevelItems = new List<Item>();
            Characters = new List<Animal>();

            LiazrdsLocatins = new List<Vector2>();
            InitLizardsLocations();

            HedgehogsLocatins = new List<Vector2>();
            InitHedgehogsLocations();

            BeetlesLocatins = new List<Vector2>();
            InitBeetlesLocatins();

            for (int i = 0; i < LiazrdsLocatins.Count; i++)
            {
                lizard = new Animal(Folders.lizard, States.Jump, S.spb, LiazrdsLocatins[i], null, Color.White, 0f,
                    new Vector2(0, 0), new Vector2(2.2f), SpriteEffects.None, 1f, AnimalType.lizard);
                lizard.baseKeys = new BotKeys(lizard, AnimalType.lizard);
                Characters.Add(lizard);
                lizard.jumping = true;
            }

            for (int i = 0; i < HedgehogsLocatins.Count; i++)
            {
                hedgehog = new Animal(Folders.hedgehog, States.Jump, S.spb, HedgehogsLocatins[i], null, Color.White, 0f,
                    new Vector2(0, 0), new Vector2(2.2f), SpriteEffects.None, 1f, AnimalType.hedgehog);
                hedgehog.baseKeys = new BotKeys(hedgehog, AnimalType.hedgehog);
                Characters.Add(hedgehog);
                hedgehog.jumping = true;
            }

            for (int i = 0; i < BeetlesLocatins.Count; i++)
            {
                beetle = new Animal(Folders.beetle, States.Jump, S.spb, BeetlesLocatins[i], null, Color.White, 0f,
                    new Vector2(0, 0), new Vector2(2.2f), SpriteEffects.None, 1f, AnimalType.beetle);
                beetle.baseKeys = new BotKeys(beetle, AnimalType.beetle);
                Characters.Add(beetle);
                beetle.jumping = true;
            }

            hyena = new Animal(Folders.hyena, States.Stand, S.spb, new Vector2(3900, 626), null, Color.White, 0f,
                new Vector2(0, 0), new Vector2(2f), SpriteEffects.None, 1f, AnimalType.hyena);
            hyena.baseKeys = new BotKeys(hyena, AnimalType.hyena);
            Characters.Add(hyena);

            //scar = new Animal(Folders.scar, States.Stand, S.spb, new Vector2(800, 3170), null, Color.White, 0f,
            //    new Vector2(0, 0), new Vector2(2.2f), SpriteEffects.None, 1f, AnimalType.lion);
            //scar.baseKeys = new BotKeys(scar, AnimalType.lion);
            //Characters.Add(scar);

            for (int i = 1; i <= 3; i++)
            {
                FirstLevelItems.Add(new Item(Game1.ItemPic, new Vector2(c += 100, 2800f), 2.2f, Color.White));
            }
        }

        /// <summary>
        /// Initializing all the first level necessary data
        /// </summary>
        public static void InitSecondLevel()
        {
            #region Deleting the leftovers of the first level

            if (hero != null)
            {
                Game1.UPDATE_EVENT -= hero.Update;
                Game1.DRAW_EVENT -= hero.DrawAnimal;
            }

            if (Characters != null)
            {
                for (int i = 0; i < Characters.Count; i++)
                {
                    Game1.UPDATE_EVENT -= Characters[i].Update;
                    Game1.DRAW_EVENT -= Characters[i].DrawObject;
                    Characters[i].color = Color.Transparent;
                }
            }

            if (FirstLevelItems != null)
            {
                foreach (Item item in FirstLevelItems)
                {
                    Game1.UPDATE_EVENT -= item.UpdateItem;
                    Game1.DRAW_EVENT -= item.DrawItem;
                }
            }

            #endregion

            LevelNumber = 2;

            S.LevelAreaNameAppearanceTime = 0;
            LogicBackGround = S.cm.Load<Texture2D>("Stages/LogicSecondstage");
            BackGroundImage = S.cm.Load<Texture2D>("Stages/Secondstage");
            SimbaFace = S.cm.Load<Texture2D>("SimbaFaces/AdultSimbaFace");
            LionSounds.hyenabosssoundinstance.Stop();

            // In case I want to start immediately from the second level
            LionSounds.mainmenusoundinstance.Stop();

            LionSounds.secondlevelsonginstance.Play();

            hero = new Animal(Folders.Adult_Simba, States.Jump, S.spb, new Vector2(3550, 3800), null, Color.White, 0f,
                new Vector2(0, 0), new Vector2(2.4f), SpriteEffects.None, 1f, AnimalType.Adult_Simba);
            hero.baseKeys = new UserBaseKeys(Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.LeftShift, Keys.Space, Keys.Z);

            hero.jumpspeed = -16f;
            hero.jumping = true;

            cam = new Camera(hero, new Vector2(1f), new Viewport(0, 0, Game1.ScreenWidth, Game1.ScreenHeight));

            Map.check(LogicBackGround);
            minimap = new MiniMap(BackGroundImage, new Vector2(S.MapsScale, S.MapsScale));

            Characters = new List<Animal>();

            scar = new Animal(Folders.scar, States.Stand, S.spb, new Vector2(3000, 3800), null, Color.White, 0f,
                    new Vector2(0, 0), new Vector2(2.5f), SpriteEffects.None, 1f, AnimalType.lion);
            scar.baseKeys = new BotKeys(scar, AnimalType.lion);
            Characters.Add(scar);

            //hyena = new Animal(Folders.hyena, States.Stand, S.spb, new Vector2(3000, 3800), null, Color.White, 0f,
            //       new Vector2(0, 0), new Vector2(2f), SpriteEffects.None, 1f, AnimalType.lion);
            //hyena.baseKeys = new BotKeys(hyena, AnimalType.hyena);
            //Characters.Add(hyena);
        }
    }
}