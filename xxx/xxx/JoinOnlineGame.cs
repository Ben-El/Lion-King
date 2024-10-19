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
    class JoinOnlineGame : OnlineGame
    {
        string hostip;

        /// <summary>
        /// Initializing the host ip  and the port
        /// </summary>
        /// <param name="hostip"></param>
        /// <param name="port"></param>
        public JoinOnlineGame(string hostip, int port)
        {
            this.port = port;
            this.hostip = hostip;
        }


        /// <summary>
        /// Initializing the host and join players
        /// </summary>
        protected override void InitChars()
        {
            hostChar = new Animal(Folders.Cub_Simba, States.Stand, S.spb, new Vector2(200, 3170), null,
                Color.White, 0f, new Vector2(0, 0), new Vector2(2.2f), SpriteEffects.None, 1f, AnimalType.Cub_Simba);
            hostChar.baseKeys = new NonBaseKeys();
            joinChar = new Animal(Folders.Cub_Simba, States.Stand, S.spb, new Vector2(400, 3170), null,
                Color.White, 0f, new Vector2(0, 0), new Vector2(2.2f), SpriteEffects.None, 1f, AnimalType.Cub_Simba);
            joinChar.baseKeys = new UserBaseKeys(Keys.W, Keys.S, Keys.A, Keys.D, Keys.LeftShift,
                Keys.Z, Keys.X);
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void SocketThread()
        {
            client = new TcpClient();
            client.Connect(hostip, port);

            reader = new BinaryReader(client.GetStream());
            writer = new BinaryWriter(client.GetStream());

            base.RaiseOnConnectionEvent();

            while (true)
            {
                ReadAndUpdateCharacter(hostChar);
                WriteCharacterData(joinChar);

                Thread.Sleep(10);
            }
        }
    }
}