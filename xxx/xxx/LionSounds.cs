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
    static class LionSounds
    {
        public static SoundEffect mainmenusound;
        public static SoundEffectInstance mainmenusoundinstance;

        public static SoundEffect firstlevelsong;
        public static SoundEffectInstance firstlevelsonginstance;

        public static SoundEffect secondlevelsong;
        public static SoundEffectInstance secondlevelsonginstance;

        public static SoundEffect finalsound;
        public static SoundEffectInstance finalsoundinstance;

        public static SoundEffect hyenabosssound;
        public static SoundEffectInstance hyenabosssoundinstance;

        public static List<SoundEffectInstance> AllSoundEffectsInstances;

        /// <summary>
        /// Initializing game's audio data
        /// </summary>
        public static void init()
        {
            AllSoundEffectsInstances = new List<SoundEffectInstance>();

            mainmenusound = S.cm.Load<SoundEffect>("Audio/MainMenuSound");
            mainmenusoundinstance = mainmenusound.CreateInstance();
            mainmenusoundinstance.IsLooped = true;
            mainmenusoundinstance.Play();

            firstlevelsong = S.cm.Load<SoundEffect>("Audio/FirstLevelSong");
            firstlevelsonginstance = firstlevelsong.CreateInstance();
            firstlevelsonginstance.IsLooped = true;

            secondlevelsong = S.cm.Load<SoundEffect>("Audio/SecondLevelSong");
            secondlevelsonginstance = secondlevelsong.CreateInstance();
            secondlevelsonginstance.IsLooped = true;

            hyenabosssound = S.cm.Load<SoundEffect>("Audio/HyenaBosSound");
            hyenabosssoundinstance = hyenabosssound.CreateInstance();
            hyenabosssoundinstance.IsLooped = true;

            finalsound = S.cm.Load<SoundEffect>("Audio/Final");
            finalsoundinstance = finalsound.CreateInstance();
            finalsoundinstance.IsLooped = true;

            // Adding all sound instances int the list
            AllSoundEffectsInstances.Add(mainmenusoundinstance);
            AllSoundEffectsInstances.Add(firstlevelsonginstance);
            AllSoundEffectsInstances.Add(secondlevelsonginstance);
            AllSoundEffectsInstances.Add(hyenabosssoundinstance);
            AllSoundEffectsInstances.Add(finalsoundinstance);
        }
    }
}