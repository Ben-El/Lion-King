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
    enum States
    {
        Stand, Running, Running_Jump, Roar, Jump, Falling, AfterFalling,
        Hanging, Swinging, Climb, Slash, Pouncing, Rolling, DoubleSlash, GettingHurt, GettingSlashed,
        Crouch, CrouchSlash, ThrowingEnemy, TossedBySimba, Dying
    }

    enum AnimalType { Cub_Simba, Adult_Simba, lion, lizard, hedgehog, beetle, hyena }

    enum Folders { Cub_Simba, Adult_Simba, scar, lizard, hedgehog, beetle, hyena }
    enum WallPapers { MainMenu, Online, Pause }

    class TheDict
    {
        #region DATA
        public static Dictionary<Folders, Dictionary<States, Page>> dic;
        public static Dictionary<WallPapers, Texture2D> WallPapersDic;
        #endregion

        /// <summary>
        /// Initializing the big dictionaries dictionary
        /// </summary>
        public static void Init()
        {
            dic = new Dictionary<Folders, Dictionary<States, Page>>();
            WallPapersDic = new Dictionary<WallPapers, Texture2D>();

            foreach (WallPapers wp in Enum.GetValues(typeof(WallPapers)))
            {
                WallPapersDic.Add(wp, S.cm.Load<Texture2D>(wp.ToString()));
            }

            foreach (Folders folder in Enum.GetValues(typeof(Folders)))
            {
                Dictionary<States, Page> temp = new Dictionary<States, Page>();

                foreach (States state in Enum.GetValues(typeof(States)))
                {
                    try
                    {
                        temp.Add(state, new Page(folder, state));
                    }

                    catch { }
                }

                dic.Add(folder, temp);
            }
        }
    }
}