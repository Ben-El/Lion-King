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
    class HostOnlineGame : OnlineGame
    {
        /// <summary>
        /// Initializing the port
        /// </summary>
        /// <param name="port"></param>
        public HostOnlineGame(int port)
        {
            this.port = port;
        }

        /// <summary>
        /// Initializing the host and join players
        /// </summary>
        protected override void InitChars()
        {
            hostChar = new Animal(Folders.Cub_Simba, States.Stand, S.spb, new Vector2(200, 3170), null,
                Color.White, 0f, new Vector2(0, 0), new Vector2(2.2f), SpriteEffects.None, 1f, AnimalType.Cub_Simba);
            hostChar.baseKeys = new UserBaseKeys(Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.LeftShift,
                Keys.Space, Keys.LeftControl);
            joinChar = new Animal(Folders.Cub_Simba, States.Stand, S.spb, new Vector2(400, 3170), null,
                Color.White, 0f, new Vector2(0, 0), new Vector2(2.2f), SpriteEffects.None, 1f, AnimalType.Cub_Simba);
            joinChar.baseKeys = new NonBaseKeys();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void SocketThread()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            client = listener.AcceptTcpClient();

            reader = new BinaryReader(client.GetStream());
            writer = new BinaryWriter(client.GetStream());
            Console.WriteLine("before RaiseOnConnectionEvent");
            base.RaiseOnConnectionEvent();
            Console.WriteLine("after RaiseOnConnectionEvent");


            while (true)
            {
                WriteCharacterData(hostChar);
                ReadAndUpdateCharacter(joinChar);

                Thread.Sleep(10);
            }
        }
    }
}