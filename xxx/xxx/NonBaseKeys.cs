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
    class NonBaseKeys : BaseKeys
    {
        // NonBaseKeys's constructor
        // It's empty in order to be able create an instance of this class
        public NonBaseKeys()
        {

        }

        /// <summary>
        /// Returns the UpKey state
        /// </summary>
        /// <returns></returns>
        public override bool UpKey()
        {
            return false;
        }

        /// <summary>
        /// Returns the DownKey state
        /// </summary>
        /// <returns></returns>
        public override bool DownKey()
        {
            return false;
        }

        /// <summary>
        /// Returns the LeftKey state
        /// </summary>
        /// <returns></returns>
        public override bool LeftKey()
        {
            return false;
        }

        /// <summary>
        /// Returns the RightKey state
        /// </summary>
        /// <returns></returns>
        public override bool RightKey()
        {
            return false;
        }

        /// <summary>
        /// Returns the ShiftKey state
        /// </summary>
        /// <returns></returns>
        public override bool ShiftKey()
        {
            return false;
        }

        /// <summary>
        /// Returns the PunchKey state
        /// </summary>
        /// <returns></returns>
        public override bool PunchKey()
        {
            return false;
        }

        /// <summary>
        /// Returns the RoarKey state
        /// </summary>
        /// <returns></returns>
        public override bool RoarKey()
        {
            return false;
        }
    }
}