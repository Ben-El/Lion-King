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
    class UserBaseKeys : BaseKeys
    {
        Keys Up;
        Keys Down;
        Keys Left;
        Keys Right;
        Keys Shift;
        Keys Punch;
        Keys Roar;

        /// <summary>
        /// Initializing the parameters
        /// </summary>
        /// <param name="UpKey"></param>
        /// <param name="DownKey"></param>
        /// <param name="LeftKey"></param>
        /// <param name="RightKey"></param>
        /// <param name="ShiftKey"></param>
        /// <param name="PunchKey"></param>
        /// <param name="RoarKey"></param>
        public UserBaseKeys(Keys UpKey, Keys DownKey, Keys LeftKey, Keys RightKey, Keys ShiftKey, Keys PunchKey,
            Keys RoarKey)
        {
            //Game1.UPDATE_EVENT += this.Update;

            this.Up = UpKey;
            this.Down = DownKey;
            this.Left = LeftKey;
            this.Right = RightKey;
            this.Shift = ShiftKey;
            this.Punch = PunchKey;
            this.Roar = RoarKey;
        }

        /// <summary>
        /// Returns the UpKey's state
        /// </summary>
        /// <returns></returns>
        public override bool UpKey()
        {
            return Keyboard.GetState().IsKeyDown(Up);
        }

        /// <summary>
        /// Returns the DownKey's state
        /// </summary>
        /// <returns></returns>
        public override bool DownKey()
        {
            return Keyboard.GetState().IsKeyDown(Down);
        }

        /// <summary>
        /// Returns the LeftKey's state
        /// </summary>
        /// <returns></returns>
        public override bool LeftKey()
        {
            return Keyboard.GetState().IsKeyDown(Left);
        }

        /// <summary>
        /// Returns the RightKey's state
        /// </summary>
        /// <returns></returns>
        public override bool RightKey()
        {
            return Keyboard.GetState().IsKeyDown(Right);
        }

        /// <summary>
        /// Returns the ShiftKey's state
        /// </summary>
        /// <returns></returns>
        public override bool ShiftKey()
        {
            return Keyboard.GetState().IsKeyDown(Shift);
        }

        /// <summary>
        /// Returns the PunchKey's state
        /// </summary>
        /// <returns></returns>
        public override bool PunchKey()
        {
            return Keyboard.GetState().IsKeyDown(Punch);
        }

        /// <summary>
        /// Returns the RoarKey's state
        /// </summary>
        /// <returns></returns>
        public override bool RoarKey()
        {
            return Keyboard.GetState().IsKeyDown(Roar);
        }
    }
}